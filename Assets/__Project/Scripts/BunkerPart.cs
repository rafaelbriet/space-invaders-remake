using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SpaceInvadersRemake
{
    public class BunkerPart : MonoBehaviour, IDamageable
    {
        [SerializeField]
        private Sprite[] maxLives;
        [SerializeField]
        private SpriteRenderer spriteRenderer;

        private int currentLives;

        private void Awake()
        {
            currentLives = maxLives.Length;
        }

        public void Damage()
        {
            currentLives--;
            ChangeSprite();

            if (currentLives <= 0)
            {
                Destroy(gameObject);
            }
        }

        private void ChangeSprite()
        {
            int indexFromCurrentLives = currentLives - 1;

            if (indexFromCurrentLives >= 0)
            {
                spriteRenderer.sprite = maxLives[indexFromCurrentLives];
            }
        }
    }
}
