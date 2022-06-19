using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SpaceInvadersRemake
{
    public class Bullet : MonoBehaviour
    {
        [SerializeReference]
        private float speed = 5f;

        private void Update()
        {
            transform.Translate(new Vector3(0, speed * Time.deltaTime, 0));

            if (transform.position.y > Camera.main.orthographicSize)
            {
                Destroy(gameObject);
            }
        }
    }
}
