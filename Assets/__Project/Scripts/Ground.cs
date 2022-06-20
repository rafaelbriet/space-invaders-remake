using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SpaceInvadersRemake
{
    public class Ground : MonoBehaviour
    {
        private bool hasInvaderReachedGround;

        public event EventHandler InvadersReachedGround;

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (!hasInvaderReachedGround && collision.CompareTag("Invader"))
            {
                hasInvaderReachedGround = true;
                InvadersReachedGround?.Invoke(this, EventArgs.Empty);
            }
        }
    }
}
