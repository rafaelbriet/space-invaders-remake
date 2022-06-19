using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SpaceInvadersRemake
{
    public class Weapon : MonoBehaviour
    {
        [SerializeField]
        private GameObject bulletPrefab;
        [SerializeField]
        private Transform firingPoint;
        [SerializeField]
        private float firingRate = 0.5f;

        private bool canFire = true;

        public void Fire(Vector2 direction)
        {
            if (canFire)
            {
                Instantiate(bulletPrefab, firingPoint.position, Quaternion.identity).GetComponent<Bullet>().MoveDirection = direction;
                StartCoroutine(FireActionCooldownCoroutine());
            }
        }

        private IEnumerator FireActionCooldownCoroutine()
        {
            canFire = false;

            yield return new WaitForSeconds(firingRate);

            canFire = true;
        }
    }
}
