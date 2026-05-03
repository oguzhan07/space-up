using UnityEngine;

namespace AstronautPlayer
{
    public class AstronautPlayer : MonoBehaviour
    {
        private Animator anim;
        private CharacterController controller;

        public float speed = 6.0f;
        public float turnSpeed = 400.0f;
        public float jumpForce = 8.0f;
        public float gravity = 20.0f;

        private Vector3 moveDirection = Vector3.zero;
        private bool isJumping = false;

        void Start()
        {
            controller = GetComponent<CharacterController>();
            anim = gameObject.GetComponentInChildren<Animator>();
        }

        void Update()
        {
            float vertical = Input.GetAxis("Vertical");
            float horizontal = Input.GetAxis("Horizontal");

            // Animasyon
            if (vertical != 0)
                anim.SetInteger("AnimationPar", 1); // yürü
            else
                anim.SetInteger("AnimationPar", 0); // idle

            // Hareket
            if (controller.isGrounded)
            {
                moveDirection = transform.forward * vertical * speed;

                // Zıplama
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    moveDirection.y = jumpForce;
                }
            }

            // Dönme
            transform.Rotate(0, horizontal * turnSpeed * Time.deltaTime, 0);

            // Gravity
            moveDirection.y -= gravity * Time.deltaTime;
            controller.Move(moveDirection * Time.deltaTime);
        }
    }
}