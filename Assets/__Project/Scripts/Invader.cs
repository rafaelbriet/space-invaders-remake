using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SpaceInvadersRemake
{
    public class Invader : MonoBehaviour, IDamageable
    {
        public void Damage()
        {
            Destroy(gameObject);
        }
    }
}
