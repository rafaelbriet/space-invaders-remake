using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace SpaceInvadersRemake
{
    [RequireComponent(typeof(PlayerInput), typeof(BoxCollider2D))]
    public class Player : MonoBehaviour
    {
        [SerializeField]
        private float moveSpeed = 5f;

        private float horizontal;
        private float screenBounds;
        private new BoxCollider2D collider;
        private PlayerInput playerInput;
        private InputAction horizontalAction;

        private void Awake()
        {
            collider = GetComponent<BoxCollider2D>();
            playerInput = GetComponent<PlayerInput>();

            horizontalAction = playerInput.actions["Horizontal"];
            
            CalculateScreenBounds();
        }

        private void OnEnable()
        {
            horizontalAction.started += OnHorizontalAction;
            horizontalAction.performed += OnHorizontalAction;
            horizontalAction.canceled += OnHorizontalAction;
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
        }

        private void OnHorizontalAction(InputAction.CallbackContext context)
        {
            horizontal = context.ReadValue<float>();
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
    }
}
