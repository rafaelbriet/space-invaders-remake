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
        [Header("Audio")]
        [SerializeField]
        private AudioClip damageAudioClip;

        public Weapon Weapon => weapon;
        public int PointsWhenKilled => pointsWhenKilled;
        public AudioSource AudioSource { get; set; }

        public event EventHandler InvaderKilled;

        public void Damage()
        {
            if (AudioSource != null && damageAudioClip != null)
            {
                AudioSource.PlayOneShot(damageAudioClip);
            }
            
            InvaderKilled?.Invoke(this, EventArgs.Empty);
            Destroy(gameObject);
        }
    }
}
