using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SpaceInvadersRemake
{
    public class InvasionCommander : MonoBehaviour
    {
        [SerializeField]
        private GameObject alienPrefab;
        [SerializeField]
        private int invasionWidth = 11;
        [SerializeField]
        private GameObject[] invasionHeight;
        [SerializeField]
        private float spaceBetweenAliens = 1f;
        [SerializeField]
        private float waveVerticalOffset = 1f;
        [SerializeField]
        private float firingRate = 1f;
        [SerializeField]
        private AnimationCurve invasionSpeedCurve;
        [SerializeField]
        private GameObject alienSpecialPrefab;
        [SerializeField]
        private float minTimeBetweenSpecialAlienSpawn = 3f;
        [SerializeField]
        private float maxTimeBetweenSpecialAlienSpawn = 6f;

        private GameObject[] invasionRows;
        private GameObject invasionSpecialRow;
        private List<Invader> invaders;
        private float screenBounds;
        private bool canFire = true;
        private int totalInvadersAmount;
        private float yOffset = 0;

        public event EventHandler<InvaderKilledEventArgs> InvaderKilled;
        public event EventHandler InvasionEnded;

        private void Awake()
        {
            totalInvadersAmount = invasionWidth * invasionHeight.Length;

            CalculateScreenBounds();
        }

        private void Update()
        {
            for (int i = 0; i < invasionRows.Length; i++)
            {
                GameObject row = invasionRows[i];

                if (row.transform.position.x > screenBounds)
                {
                    row.GetComponent<InvasionRow>().MoveDirection = -1f;
                    row.transform.position = new Vector3(row.transform.position.x, row.transform.position.y - 0.5f, row.transform.position.z);
                }
                else if (row.transform.position.x < -screenBounds)
                {
                    row.GetComponent<InvasionRow>().MoveDirection = 1f;
                    row.transform.position = new Vector3(row.transform.position.x, row.transform.position.y - 0.5f, row.transform.position.z);
                }

                float percentInvasionLeft = (float)invaders.Count / (float)totalInvadersAmount;
                float moveSpeed = invasionSpeedCurve.Evaluate(percentInvasionLeft);
                float movePosition = row.transform.position.x + (row.GetComponent<InvasionRow>().MoveDirection * moveSpeed * Time.deltaTime);
                row.transform.position = new Vector3(movePosition, row.transform.position.y, row.transform.position.z);
            }

            Fire();
            MoveSpecialRow();
        }

        private void MoveSpecialRow()
        {
            if (invasionSpecialRow == null)
            {
                return;
            }

            Debug.Log("MoveSpecialRow");

            float moveSpeed = invasionSpeedCurve.Evaluate(1f);
            float movePosition = invasionSpecialRow.transform.position.x + (invasionSpecialRow.GetComponent<InvasionRow>().MoveDirection * moveSpeed * Time.deltaTime);
            invasionSpecialRow.transform.position = new Vector3(movePosition, invasionSpecialRow.transform.position.y, invasionSpecialRow.transform.position.z);

            float screenWidth = Camera.main.orthographicSize * Camera.main.aspect;

            if (Mathf.Abs(invasionSpecialRow.transform.position.x) > screenWidth)
            {
                Destroy(invasionSpecialRow);
                StartCoroutine(SpawnSpecialInvaderCoroutine());
            }
        }

        private void SpawnSpecialInvader()
        {
            int diceRoll = UnityEngine.Random.Range(1, 7);
            float screenWidth = Camera.main.orthographicSize * Camera.main.aspect;
            float xPosition;
            float moveDirection;

            if (diceRoll <= 3)
            {
                xPosition = screenWidth;
                moveDirection = -1;
            }
            else
            {
                xPosition = -screenWidth;
                moveDirection = 1;
            }

            invasionSpecialRow = new GameObject("InvasionRow_Special");
            invasionSpecialRow.AddComponent<InvasionRow>().MoveDirection = moveDirection;
            Vector3 specialInvaderRowPosition = new Vector3(xPosition, Camera.main.orthographicSize - 2.5f);
            invasionSpecialRow.transform.position = specialInvaderRowPosition;
            Instantiate(alienSpecialPrefab, invasionSpecialRow.transform);
        }

        private IEnumerator SpawnSpecialInvaderCoroutine()
        {
            float timeToNextSpecialAlienSpawn = UnityEngine.Random.Range(minTimeBetweenSpecialAlienSpawn, maxTimeBetweenSpecialAlienSpawn);

            yield return new WaitForSeconds(timeToNextSpecialAlienSpawn);

            SpawnSpecialInvader();
        }

        private void Fire()
        {
            if (!canFire)
            {
                return;
            }

            if (invaders.Count > 0)
            {
                Invader randomInvander = invaders[UnityEngine.Random.Range(0, invaders.Count)];
                randomInvander.Weapon.Fire(Vector2.down);
                StartCoroutine(FiringCooldownCoroutine());
            }
        }

        private IEnumerator FiringCooldownCoroutine()
        {
            canFire = false;

            yield return new WaitForSeconds(firingRate);

            canFire = true;
        }

        public void CreateInvasion()
        {
            invaders = new List<Invader>();
            invasionRows = new GameObject[invasionHeight.Length];

            float invasionBounds = invasionWidth / 2;
            float invasionSpacingBounds = (invasionWidth - 1) * spaceBetweenAliens / 2;
            float alienOffset = invasionBounds + invasionSpacingBounds;

            for (int y = 0; y < invasionHeight.Length; y++)
            {
                GameObject invasionRow = new GameObject($"InvasionRow_{y}");
                invasionRow.AddComponent<InvasionRow>().MoveDirection = 1;
                float invasionSpacing = y * spaceBetweenAliens;
                Vector3 invasionRowPosition = new Vector3(0, y + invasionSpacing - yOffset);
                invasionRow.transform.position = invasionRowPosition;
                invasionRows[y] = invasionRow;

                for (int x = 0; x < invasionWidth; x++)
                {
                    float alienSpacing = x * spaceBetweenAliens;
                    Vector3 alienPosition = new Vector3(x + alienSpacing - alienOffset, invasionRow.transform.position.y);
                    Invader invader = Instantiate(invasionHeight[y], alienPosition, Quaternion.identity, invasionRow.transform).GetComponent<Invader>();
                    invader.InvaderKilled += OnInvaderKilled;
                    invaders.Add(invader);
                }
            }

            StartCoroutine(SpawnSpecialInvaderCoroutine());

            yOffset += waveVerticalOffset;
        }

        private void OnInvaderKilled(object sender, EventArgs e)
        {
            Invader invader = (Invader)sender;
            InvaderKilledEventArgs eventArgs = new InvaderKilledEventArgs(invader);
            InvaderKilled?.Invoke(this, eventArgs);
            invader.InvaderKilled -= OnInvaderKilled;
            invaders.Remove(invader);

            if (invaders.Count == 0)
            {
                InvasionEnded?.Invoke(this, EventArgs.Empty);
            }
        }

        private void CalculateScreenBounds()
        {
            float invasionBounds = invasionWidth / 2;
            float invasionSpacingBounds = (invasionWidth - 1) * spaceBetweenAliens / 2;
            float screenWidth = Camera.main.orthographicSize * Camera.main.aspect;
            screenBounds = screenWidth - invasionBounds - invasionSpacingBounds - 0.5f;
        }
    }
}
