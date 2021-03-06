using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace SpaceInvadersRemake
{
    [RequireComponent(typeof(PlayerInput), typeof(BoxCollider2D))]
    public class Player : MonoBehaviour, IDamageable
    {
        [SerializeField]
        private float moveSpeed = 5f;
        [SerializeField]
        private Weapon weapon;
        [SerializeField]
        private int maxLives = 3;
        [Header("Audio")]
        [SerializeField]
        private AudioSource audioSource;
        [SerializeField]
        private AudioClip damageAudioClip;
        [SerializeField]
        private AudioClip fireAudioClip;

        private float horizontal;
        private float screenBounds;
        private new BoxCollider2D collider;
        private PlayerInput playerInput;
        private InputAction horizontalAction;
        private InputAction fireAction;

        public int CurrentLives { get; private set; }

        public event EventHandler PlayerDied;
        public event EventHandler PlayerDamaged;

        private void Awake()
        {
            CurrentLives = maxLives;

            collider = GetComponent<BoxCollider2D>();
            playerInput = GetComponent<PlayerInput>();

            horizontalAction = playerInput.actions["Horizontal"];
            fireAction = playerInput.actions["Fire"];
            
            CalculateScreenBounds();
        }

        private void OnEnable()
        {
            horizontalAction.started += OnHorizontalAction;
            horizontalAction.performed += OnHorizontalAction;
            horizontalAction.canceled += OnHorizontalAction;

            fireAction.performed += OnFireAction;
        }

        private void Update()
        {
            Move();
        }

        private void OnDisable()
        {
            horizontalAction.started -= OnHorizontalAction;
            horizontalAction.performed -= OnHorizontalAction;
            horizontalAction.canceled -= OnHorizontalAction;

            fireAction.performed -= OnFireAction;
        }

        private void OnHorizontalAction(InputAction.CallbackContext context)
        {
            horizontal = context.ReadValue<float>();
        }

        private void OnFireAction(InputAction.CallbackContext obj)
        {
            audioSource.PlayOneShot(fireAudioClip);
            weapon.Fire(Vector2.up);
        }

        private void CalculateScreenBounds()
        {
            float playerSize = collider.bounds.size.x * 0.5f;
            float screenWidth = Camera.main.orthographicSize * Camera.main.aspect;
            screenBounds = screenWidth - playerSize;
        }

        private void Move()
        {
            float xPosition = transform.position.x + (horizontal * Time.deltaTime * moveSpeed);
            xPosition = Mathf.Clamp(xPosition, -screenBounds, screenBounds);
            transform.position = new Vector3(xPosition, transform.position.y, transform.position.z);
        }

        public void Damage()
        {
            CurrentLives--;

            audioSource.PlayOneShot(damageAudioClip);

            PlayerDamaged?.Invoke(this, EventArgs.Empty);

            if (CurrentLives <= 0)
            {
                PlayerDied?.Invoke(this, EventArgs.Empty);
                Destroy(gameObject);
            }
        }
    }
}
