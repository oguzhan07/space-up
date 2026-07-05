using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SanityManager : MonoBehaviour
{
    public static SanityManager instance;

    [Header("Bar Ayarları")]
    public Image sanityBarFill;
    public float maxSanity = 100f;
    public float currentSanity;
    public float restoreAmount = 30f;

    [Header("Ekran Kararma Ayarları")]
    public Image darknessOverlay;
    public Image karanlikEkranObjesi;
    [Tooltip("Ekranın kararmaya başlayacağı bar yüzdesi (0.5 = %50)")]
    [Range(0f, 1f)] public float startDarkenThreshold = 0.5f;
    [Range(0f, 1f)] public float maxDarkenThreshold = 0.2f;
    [Range(0f, 1f)] public float maxDarknessAlpha = 0.8f; 

    [Header("Şekil Değiştirme (F Tuşu) Ayarları")]
    public KeyCode switchKey = KeyCode.F;
    public float switchCooldown = 2f;
    [Tooltip("F'ye basıldığında harcanacak anlık enerji")]
    public float switchCost = 5f;
    
    [Header("Karakter Enerji Tüketim Hızları")]
    public float miniDrainRate = 2f;
    public float bigDrainRate = 8f;

    [Header("Karakter Görselleri/Objeleri")]
    public GameObject miniCharacter;
    public GameObject bigCharacter;

    [Header("Oyun Sonu Ayarları")]
    [Tooltip("Enerji bittiğinde yüklenecek sahnenin adını buraya girin")]
    public string gameOverSceneName; // --- EKLENEN DEĞİŞKEN ---

    // Arka plan takip değişkenleri
    private bool isMiniActive = true;
    private float currentDecreaseRate;
    private float nextSwitchTime = 0f;
    private bool isTimerStarted = false; 
    private bool isGameOver = false;

    void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);
    }

    void Start()
    {
        currentSanity = maxSanity;
        currentDecreaseRate = isMiniActive ? miniDrainRate : bigDrainRate;
        UpdateUI(); 

        if (karanlikEkranObjesi != null)
        {
            karanlikEkranObjesi.gameObject.SetActive(true);

            Color tempColor = karanlikEkranObjesi.color;
            tempColor.a = 1f;
            karanlikEkranObjesi.color = tempColor;

            Invoke("ExecuteFadeIn", 0.05f);
        }
    }

    void ExecuteFadeIn()
    {
        karanlikEkranObjesi.CrossFadeAlpha(0.0f, 2.0f, false);
        Invoke("DisableFadeObject", 2.1f); 
    }

    void DisableFadeObject()
    {
        karanlikEkranObjesi.gameObject.SetActive(false);
    }

    void Update()
    {
        if (isGameOver) return;

        if (Input.GetKeyDown(switchKey) && Time.time >= nextSwitchTime)
        {
            if (currentSanity >= switchCost) 
            {
                PerformSwitch();
                nextSwitchTime = Time.time + switchCooldown;
            }
            else
            {
                Debug.Log("Dönüşüm için yeterli enerji yok!");
            }
        }

        if (isTimerStarted)
        {
            currentSanity -= currentDecreaseRate * Time.deltaTime;
            currentSanity = Mathf.Clamp(currentSanity, 0, maxSanity);

            UpdateUI();

            if (currentSanity <= 0) GameOver();
        }
    }

    void PerformSwitch()
    {
        currentSanity -= switchCost;
        currentSanity = Mathf.Clamp(currentSanity, 0, maxSanity);
        
        isMiniActive = !isMiniActive;

        if (miniCharacter) miniCharacter.SetActive(isMiniActive);
        if (bigCharacter) bigCharacter.SetActive(!isMiniActive);

        currentDecreaseRate = isMiniActive ? miniDrainRate : bigDrainRate;

        UpdateUI();
        
        Debug.Log("Karakter değişti. Aktif olan: " + (isMiniActive ? "Mini" : "Büyük") + " | Yeni Tüketim Hızı: " + currentDecreaseRate);

        if (currentSanity <= 0 && isTimerStarted)
        {
            GameOver();
        }
    }

    void UpdateUI()
    {
        float currentPercent = currentSanity / maxSanity;

        if (sanityBarFill != null) sanityBarFill.fillAmount = currentPercent;

        if (darknessOverlay != null)
        {
            float t = Mathf.InverseLerp(startDarkenThreshold, maxDarkenThreshold, currentPercent);
            float finalAlpha = t * maxDarknessAlpha;

            Color c = darknessOverlay.color;
            c.a = finalAlpha;
            darknessOverlay.color = c;
        }
    }

    public void StartTimer()
    {
        if (!isTimerStarted)
        {
            isTimerStarted = true;
            Debug.Log("İlk kağıt kapatıldı, süre azalmaları başladı!");
        
            if (karanlikEkranObjesi != null && !karanlikEkranObjesi.gameObject.activeSelf)
            {
                karanlikEkranObjesi.gameObject.SetActive(true);
            }
        }   
    }

    public void RestoreSanity()
    {
        if (isGameOver) return;
        currentSanity += restoreAmount;
        currentSanity = Mathf.Clamp(currentSanity, 0, maxSanity);
        UpdateUI();
    }

    void GameOver()
    {
        if(isGameOver) return; 
        isGameOver = true;
        Debug.Log("GAME OVER! Bar bitti.");

        // --- EKLENEN KISIM: Sahne Yükleme İşlemi ---
        if (!string.IsNullOrEmpty(gameOverSceneName))
        {
            SceneManager.LoadScene(gameOverSceneName);
        }
        else
        {
            Debug.LogWarning("Uyarı: Yüklenecek sahne ismi girilmedi! Lütfen Inspector'dan 'Game Over Scene Name' kısmını doldurun.");
        }
        // -------------------------------------------
    }
}