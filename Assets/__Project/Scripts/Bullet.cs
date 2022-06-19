using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SpaceInvadersRemake
{
    [RequireComponent(typeof(Rigidbody2D), typeof(BoxCollider2D))]
    public class Bullet : MonoBehaviour, IDamageable
    {
        [SerializeReference]
        private float speed = 5f;

        private new Rigidbody2D rigidbody;

        public Vector2 MoveDirection { get; set; }

        private void Awake()
        {
            rigidbody = GetComponent<Rigidbody2D>();
        }

        private void FixedUpdate()
        {
            Vector2 movePosition = (speed * Time.fixedDeltaTime * MoveDirection) + rigidbody.position;
            rigidbody.MovePosition(movePosition);

            if (Mathf.Abs(transform.position.y) > Camera.main.orthographicSize)
            {
                Destroy(gameObject);
            }
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.TryGetComponent(out IDamageable damageable))
            {
                damageable.Damage();
                Destroy(gameObject);
            }
        }

        public void Damage()
        {
            Destroy(gameObject);
        }
    }
}
