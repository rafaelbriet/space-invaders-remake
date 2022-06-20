using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SpaceInvadersRemake
{
    public class BunkerPart : MonoBehaviour, IDamageable
    {
        [SerializeField]
        private Color[] maxLives;
        [SerializeField]
        private SpriteRenderer spriteRenderer;

        private int currentLives;

        private void Awake()
        {
            currentLives = maxLives.Length;

            UpdateSpriteRendererColor();
        }

        public void Damage()
        {
            currentLives--;
            UpdateSpriteRendererColor();

            if (currentLives <= 0)
            {
                Destroy(gameObject);
            }
        }

        private void UpdateSpriteRendererColor()
        {
            int indexFromCurrentLives = currentLives - 1;

            if (indexFromCurrentLives >= 0)
            {
                spriteRenderer.color = maxLives[indexFromCurrentLives];
            }
        }
    }
}
