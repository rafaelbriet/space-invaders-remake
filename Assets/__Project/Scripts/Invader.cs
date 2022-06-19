using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SpaceInvadersRemake
{
    public class Invader : MonoBehaviour, IDamageable
    {
        [SerializeField]
        private Weapon weapon;
        [SerializeField]
        private int pointsWhenKilled;

        public Weapon Weapon => weapon;
        public int PointsWhenKilled => pointsWhenKilled;

        public event EventHandler InvaderKilled; 

        public void Damage()
        {
            InvaderKilled?.Invoke(this, EventArgs.Empty);
            Destroy(gameObject);
        }
    }
}
