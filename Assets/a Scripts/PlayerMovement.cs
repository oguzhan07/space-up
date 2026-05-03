using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public CharacterController controller;
    public Transform cam; 
    public GameObject groundCheck; // İçinde Box Collider olan obje
    public float speed = 10f, jump = 2.5f, gravity = -25f;
    
    Vector3 velocity;

    void Update()
    {
        // 1. Zemin ve Tag Kontrolü
        bool isGrounded = false;
        BoxCollider bCol = groundCheck.GetComponent<BoxCollider>();
        // Kutunun içindeki tüm objeleri bulur
        Collider[] hitColliders = Physics.OverlapBox(groundCheck.transform.position, bCol.bounds.extents, groundCheck.transform.rotation);
        
        foreach (var hit in hitColliders)
        {
            if (hit.CompareTag("Ground")) { isGrounded = true; break; }
        }

        // 2. Hareket (Kameranın baktığı yöne göre)
        float x = Input.GetAxisRaw("Horizontal"), z = Input.GetAxisRaw("Vertical");
        Vector3 move = cam.right * x + cam.forward * z;
        move.y = 0; // Yere bakınca yere gömülmemesi için
        controller.Move(move.normalized * speed * Time.deltaTime);

        // 3. Zıplama ve Yerçekimi
        if (isGrounded && velocity.y < 0) velocity.y = -2f;
        if (Input.GetButtonDown("Jump") && isGrounded) velocity.y = Mathf.Sqrt(jump * -2f * gravity);
        
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }
}