using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class PlayerSoundManager : MonoBehaviour
{
    [Header("Arka Plan Müziği")]
    public AudioClip bgmSound;
    [Range(0f, 1f)] public float bgmVolume = 0.5f; // Müziğin ses seviyesi

    [Header("Zıplama Sesi")]
    public AudioClip spaceSound;
    [Header("Zıplama (Space) Ayarları")]
    public float spaceCooldown = 1f; 
    private float nextSpaceTime = 0f; 

    [Header("Kitap (Zaman Durma) Sesi")]
    public AudioClip timeStopSound; 
    [Range(0f, 1f)] public float timeStopVolume = 0.3f; 

    private AudioSource sfxSource; // Efektler (zıplama, kitap) için
    private AudioSource bgmSource; // Sadece müzik için (kodla oluşturulacak)
    private float lastTimeScale;

    void Start()
    {
        // Karakterin üzerindeki var olan AudioSource'u efektler için alıyoruz
        sfxSource = GetComponent<AudioSource>();
        lastTimeScale = Time.timeScale;

        // Arka plan müziği tanımlanmışsa, ona özel bir AudioSource oluştur ve başlat
        if (bgmSound != null)
        {
            bgmSource = gameObject.AddComponent<AudioSource>();
            bgmSource.clip = bgmSound;
            bgmSource.loop = true; // Sürekli tekrarlasın
            bgmSource.volume = bgmVolume; // Inspector'daki ses seviyesini uygula
            bgmSource.Play();
        }
    }

    void Update()
    {
        // Arka plan müziğinin sesini oyun çalışırken canlı canlı ayarlayabilmek için:
        if (bgmSource != null && bgmSource.volume != bgmVolume)
        {
            bgmSource.volume = bgmVolume;
        }

        // 1. SPACE TUŞU KONTROLÜ
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (Time.unscaledTime >= nextSpaceTime)
            {
                PlaySound(spaceSound);
                nextSpaceTime = Time.unscaledTime + spaceCooldown;
            }
        }

        // 2. ZAMAN DURMA KONTROLÜ
        if (Time.timeScale == 0f && lastTimeScale > 0f)
        {
            PlaySound(timeStopSound, timeStopVolume);
        }
        lastTimeScale = Time.timeScale; 
    }

    void PlaySound(AudioClip clip, float volume = 1f)
    {
        if (clip != null)
        {
            sfxSource.PlayOneShot(clip, volume);
        }
    }
}