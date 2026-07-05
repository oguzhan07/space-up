using UnityEngine;

public class MenuManager : MonoBehaviour
{
    [Header("Paneller")]
    public GameObject tusPanel; // Tüm butonları tutan ana panelin

    [Header("Referanslar")]
    public CameraArcMovement kameraHareketKodu; // Kameradaki script

    // PLAY Butonuna basıldığında bu çalışacak
    public void PlayOyun()
    {
        // 1. Önce Tuşlar panelini komple kapatıyoruz
        if (tusPanel != null)
        {
            tusPanel.SetActive(false);
        }

        // 2. Kameranın hareketini başlatıyoruz
        if (kameraHareketKodu != null)
        {
            kameraHareketKodu.HareketiBaslat();
        }
    }

    // QUIT Butonuna basıldığında bu çalışacak
    public void QuitOyun()
    {
        Debug.Log("Oyundan Çıkılıyor...");
        
        // Bu kod oyun derlendiğinde (Build alındığında) çalışır
        Application.Quit();
    }
}