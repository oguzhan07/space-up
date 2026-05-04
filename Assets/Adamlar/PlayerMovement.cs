using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 5f;
    public float turnSpeed = 200f;
    public float gravity = 20f;
    public float jumpForce = 8f;

    private CharacterController controller;
    private Animator anim;
    private Vector3 moveDirection;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        float v = Input.GetAxis("Vertical");
        float h = Input.GetAxis("Horizontal");

        if (controller.isGrounded)
        {
            float tempY = moveDirection.y;
            moveDirection = transform.forward * v * speed;
            moveDirection.y = tempY;

            if (Input.GetKeyDown(KeyCode.Space))
            {
                moveDirection.y = jumpForce;
                if (anim != null)
                    anim.SetTrigger("Jump");
            }
        }

        transform.Rotate(0, h * turnSpeed * Time.deltaTime, 0);
        moveDirection.y -= gravity * Time.deltaTime;
        controller.Move(moveDirection * Time.deltaTime);

        if (anim != null)
            anim.SetFloat("Speed", Mathf.Abs(v));
    }
}