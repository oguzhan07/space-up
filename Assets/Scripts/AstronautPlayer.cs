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

        public LayerMask groundMask; 
        private Vector3 moveDirection = Vector3.zero;

        void Start()
        {
            controller = GetComponent<CharacterController>();
            anim = GetComponentInChildren<Animator>();
        }

        void Update()
        {
            // 1. Zemin Kontrolü (En sade haliyle)
            // Karakterin altındaki 0.3 birimlik alanda zemin var mı?
            bool isGrounded = Physics.CheckSphere(transform.position, 0.3f, groundMask);

            // 2. Hareket Girdileri
            float v = Input.GetAxis("Vertical");
            float h = Input.GetAxis("Horizontal");

            // 3. Momentum Çözümü: 
            // Yerdeyken hızı her karede yeni girişe göre ayarla (Kaymayı önler)
            // Havadayken ise mevcut x ve z hızını koru (Momentumu tutar)
            if (isGrounded)
            {
                float tempY = moveDirection.y; // Zıplama/Düşme hızını yedekle
                moveDirection = transform.forward * v * speed; // Yatay hızı tazele
                moveDirection.y = tempY; // Yedeklediğin dikey hızı geri ver

                if (Input.GetKeyDown(KeyCode.Space))
                    moveDirection.y = jumpForce;
                else if (moveDirection.y < 0)
                    moveDirection.y = -2f; // Yere yapıştır
            }

            // 4. Dönme ve Animasyon
            transform.Rotate(0, h * turnSpeed * Time.deltaTime, 0);
            anim.SetInteger("AnimationPar", v != 0 ? 1 : 0);

            // 5. Fizik Uygulama
            moveDirection.y -= gravity * Time.deltaTime;
            controller.Move(moveDirection * Time.deltaTime);
        }

        // Küreyi sahnede görebilmen için (Zıplamazsa bu küre yere değiyor mu bak)
        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, 0.5f);
        }
    }
}