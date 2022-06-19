using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SpaceInvadersRemake
{
    public class Invader : MonoBehaviour, IDamageable
    {
        [SerializeField]
        private Weapon weapon;

        public Weapon Weapon => weapon;

        public void Damage()
        {
            Destroy(gameObject);
        }
    }
}
