using UnityEngine;
using UnityEngine.UI; 
using System.Collections;
using UnityEngine.SceneManagement;

public class CameraArcMovement : MonoBehaviour
{
    [Header("Hedefler")]
    public Transform centerPoint; 
    public Transform startPos;
    public Transform endPos;

    [Header("UI & Fade Ayarları")]
    public Image fadeImage; 

    [Header("Ayarlar")]
    public float movementDuration = 2f;
    
    [Tooltip("Dönüşün (rotasyonun) ne kadar yumuşak olacağı. Yüksek değer = Daha hızlı dönüş")]
    public float rotationSmoothness = 5f;

    [Header("Sahne Geçiş Ayarları")]
    [Tooltip("Ekran siyah olduktan sonra sahne yüklenmeden önceki bekleme süresi")]
    public float delayBeforeSceneLoad = 0.3f;

    private bool hasStarted = false; 

    void Start()
    {
        if (fadeImage != null)
        {
            Color c = fadeImage.color;
            c.a = 0f;
            fadeImage.color = c;
        }
    }

    public void HareketiBaslat() 
    {
        if (!hasStarted)
        {
            hasStarted = true;
            StartCoroutine(MoveCameraInArcWithFade());
        }
    }

    IEnumerator MoveCameraInArcWithFade()
    {
        // Siyah ekranın objesi kapalıysa önce onu aç
        if (fadeImage != null && !fadeImage.gameObject.activeSelf)
        {
            Color startColor = fadeImage.color;
            startColor.a = 0f;
            fadeImage.color = startColor;

            fadeImage.gameObject.SetActive(true);
        }

        float elapsedTime = 0f;

        Vector3 relStart = startPos.position - centerPoint.position;
        Vector3 relEnd = endPos.position - centerPoint.position;

        while (elapsedTime < movementDuration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / movementDuration;

            // --- POZİSYON ---
            Vector3 currentRelPos = Vector3.Slerp(relStart, relEnd, t);
            transform.position = centerPoint.position + currentRelPos;

            // --- ROTASYON ---
            Vector3 targetDirection = centerPoint.position - transform.position;
            if (targetDirection != Vector3.zero)
            {
                Quaternion targetRotation = Quaternion.LookRotation(targetDirection);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * rotationSmoothness);
            }

            // --- FADE ---
            if (fadeImage != null)
            {
                Color c = fadeImage.color;
                c.a = Mathf.Lerp(0f, 1f, t);
                fadeImage.color = c;
            }

            yield return null;
        }

        // --- BİTİŞ ---
        transform.position = endPos.position;
        
        Vector3 finalDirection = centerPoint.position - transform.position;
        if (finalDirection != Vector3.zero)
        {
            transform.rotation = Quaternion.LookRotation(finalDirection);
        }

        if (fadeImage != null)
        {
            Color c = fadeImage.color;
            c.a = 1f;
            fadeImage.color = c;
        }

        Debug.Log("Hareket bitti, ekran siyah. Sahne geçişi yapılıyor...");

        // Ekran tam siyah olunca kısa bir bekleme yap, sonra sahneyi yükle
        yield return new WaitForSeconds(delayBeforeSceneLoad);

        SceneManager.LoadScene(1);
    }
}