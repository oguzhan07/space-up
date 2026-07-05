using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;

    [Header("Ses Efektleri")]
    public AudioClip switchSound;      // F tuşu dönüşüm sesi
    public AudioClip paperSound;       // Kağıt kapatma sesi
    public AudioClip gameOverSound;    // Game over sesi
    // İstediğin kadar ekleyebilirsin

    private AudioSource audioSource;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            audioSource = GetComponent<AudioSource>();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Tek seferlik ses çalmak için
    public void PlaySound(AudioClip clip)
    {
        if (clip != null)
        {
            audioSource.PlayOneShot(clip);
        }
    }
}