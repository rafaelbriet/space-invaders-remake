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

        private GameObject[] invasionRows;
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
