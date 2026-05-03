using UnityEngine;
using System.Collections;

public class CameraArcMovement : MonoBehaviour
{
    [Header("Hedefler")]
    [Tooltip("Kameranın etrafında döneceği ve odaklanacağı merkez obje")]
    public Transform centerPoint; 
    public Transform startPos;
    public Transform endPos;

    [Header("Ayarlar")]
    public float movementDuration = 2f;

    void Start()
    {
        // Test için başlatıyoruz. İstersen bunu bir butona veya tetikleyiciye bağlayabilirsin.
        StartCoroutine(MoveCameraInArc());
    }

    IEnumerator MoveCameraInArc()
    {
        float elapsedTime = 0f;

        // Başlangıç ve bitiş pozisyonlarının merkeze göre yön (vektör) farklarını alıyoruz.
        // Bu sayede Slerp işlemi dünya merkezine göre değil, bizim merkez objemize göre yay çizer.
        Vector3 relStart = startPos.position - centerPoint.position;
        Vector3 relEnd = endPos.position - centerPoint.position;

        while (elapsedTime < movementDuration)
        {
            elapsedTime += Time.deltaTime;
            
            // 0 ile 1 arasında zamanın ne kadarının geçtiğini hesaplıyoruz.
            float t = elapsedTime / movementDuration;

            // Merkez etrafında yay üzerindeki o anki pozisyonu hesaplıyoruz.
            Vector3 currentRelPos = Vector3.Slerp(relStart, relEnd, t);

            // Kameranın pozisyonunu güncelliyoruz (Merkez + Yaydaki Konum).
            transform.position = centerPoint.position + currentRelPos;

            // Kameranın rotasyonunu her karede merkeze bakacak şekilde kilitliyoruz.
            transform.LookAt(centerPoint);

            yield return null;
        }

        // Animasyon bittiğinde kameranın tam bitiş noktasında olduğundan emin oluyoruz.
        transform.position = endPos.position;
        transform.LookAt(centerPoint);
    }
}