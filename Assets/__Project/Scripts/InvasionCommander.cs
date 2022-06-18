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
        private float spaceBetweenAliens = 1f;

        private GameObject[] invasionRows;

        private void Awake()
        {
            CreateInvasion();
        }

        private void CreateInvasion()
        {
            invasionRows = new GameObject[invasionSize.y];

            float invasionBounds = invasionSize.x / 2;
            float invasionSpacingBounds = (invasionSize.x - 1) * spaceBetweenAliens / 2;
            float alienOffset = invasionBounds + invasionSpacingBounds;

            for (int y = 0; y < invasionSize.y; y++)
            {
                GameObject invasionRow = new GameObject($"InvasionRow_{y}");
                float invasionSpacing = y * spaceBetweenAliens;
                Vector3 invasionRowPosition = new Vector3(0, y + invasionSpacing);
                invasionRow.transform.position = invasionRowPosition;
                invasionRows[y] = invasionRow;

                for (int x = 0; x < invasionSize.x; x++)
                {
                    float alienSpacing = x * spaceBetweenAliens;
                    Vector3 alienPosition = new Vector3(x + alienSpacing - alienOffset, invasionRow.transform.position.y);
                    Instantiate(alienPrefab, alienPosition, Quaternion.identity, invasionRow.transform);
                }
            }
        }
    }
}
