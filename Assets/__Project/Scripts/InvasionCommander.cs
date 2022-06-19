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
        private Vector2Int invasionSize = new Vector2Int(11, 5);
        [SerializeField]
        private float invasionStartingSpeed = 5f;
        [SerializeField]
        private float spaceBetweenAliens = 1f;
        [SerializeField]
        private float firingRate = 1f;

        private GameObject[] invasionRows;
        private List<Invader> invaders;
        private float screenBounds;
        private bool canFire = true;

        public event EventHandler<InvaderKilledEventArgs> InvaderKilled;

        private void Awake()
        {
            CalculateScreenBounds();
            CreateInvasion();
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

                float moveSpeed = invasionStartingSpeed;
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

            invaders.RemoveAll(item => item == null);

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

        private void CreateInvasion()
        {
            invaders = new List<Invader>();
            invasionRows = new GameObject[invasionSize.y];

            float invasionBounds = invasionSize.x / 2;
            float invasionSpacingBounds = (invasionSize.x - 1) * spaceBetweenAliens / 2;
            float alienOffset = invasionBounds + invasionSpacingBounds;

            for (int y = 0; y < invasionSize.y; y++)
            {
                GameObject invasionRow = new GameObject($"InvasionRow_{y}");
                invasionRow.AddComponent<InvasionRow>().MoveDirection = 1;
                float invasionSpacing = y * spaceBetweenAliens;
                Vector3 invasionRowPosition = new Vector3(0, y + invasionSpacing);
                invasionRow.transform.position = invasionRowPosition;
                invasionRows[y] = invasionRow;

                for (int x = 0; x < invasionSize.x; x++)
                {
                    float alienSpacing = x * spaceBetweenAliens;
                    Vector3 alienPosition = new Vector3(x + alienSpacing - alienOffset, invasionRow.transform.position.y);
                    Invader invader = Instantiate(alienPrefab, alienPosition, Quaternion.identity, invasionRow.transform).GetComponent<Invader>();
                    invader.InvaderKilled += OnInvaderKilled;
                    invaders.Add(invader);
                }
            }
        }

        private void OnInvaderKilled(object sender, EventArgs e)
        {
            Invader invader = (Invader)sender;
            InvaderKilledEventArgs eventArgs = new InvaderKilledEventArgs(invader);
            InvaderKilled?.Invoke(this, eventArgs);
            invader.InvaderKilled -= OnInvaderKilled;
        }

        private void CalculateScreenBounds()
        {
            float invasionBounds = invasionSize.x / 2;
            float invasionSpacingBounds = (invasionSize.x - 1) * spaceBetweenAliens / 2;
            float screenWidth = Camera.main.orthographicSize * Camera.main.aspect;
            screenBounds = screenWidth - invasionBounds - invasionSpacingBounds - 0.5f;
        }
    }
}
