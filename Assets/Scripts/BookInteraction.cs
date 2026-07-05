using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class BookInteraction : MonoBehaviour
{
    [Header("Mesafe")]
    public float interactDistance = 3f;
    public Transform player;

    [Header("Dönme")]
    public float rotateSpeed = 90f;

    [Header("Yanıp Sönme")]
    public Color pulseColor = Color.cyan;
    public float pulseSpeed = 2f;

    [Header("Yakın Işık")]
    public Color nearColor = Color.white;

    [Header("Sayfa")]
    public GameObject pageUI;
    public KeyCode interactKey = KeyCode.E;

    [Header("UI Animasyon Ayarları")]
    public float animDuration = 0.5f;
    public float hiddenYOffset = -1000f;

    [Header("Etkileşim Ayarları (YENİ)")]
    [Tooltip("E tuşuna art arda basmayı engellemek için bekleme süresi (saniye)")]
    public float interactionCooldown = 1f; 
    private float nextInteractTime = 0f;

    [Header("UI")]
    public GameObject eKeyUI;

    private Renderer[] renderers;
    private Material outlineMaterial;
    private bool isNear = false;
    private bool pageOpen = false;
    private float pulseTimer = 0f;

    private RectTransform pageRect;
    private Coroutine uiCoroutine;

    // --- YENİ EKLENEN KISIM: STATIC DEĞİŞKENLER ---
    // static değişkenler sahnedeki TÜM kitaplar için ortak tek bir hafızayı temsil eder.
    private static bool isUIInitialized = false;
    private static Vector2 globalShownPos;
    private static Vector2 globalHiddenPos;

    void Start()
    {
        renderers = GetComponentsInChildren<Renderer>();

        outlineMaterial = new Material(Shader.Find("Universal Render Pipeline/Lit"));
        outlineMaterial.color = nearColor;

        if (pageUI)
        {
            pageRect = pageUI.GetComponent<RectTransform>();
            if (pageRect != null)
            {
                // UI pozisyonları sadece İLK kitap yüklendiğinde 1 kere hesaplanır
                if (!isUIInitialized)
                {
                    globalShownPos = pageRect.anchoredPosition;
                    globalHiddenPos = globalShownPos + new Vector2(0, hiddenYOffset);
                    isUIInitialized = true;
                }
                
                // Sahnedeki tüm kitaplar bu ortak pozisyonları kullanır
                pageRect.anchoredPosition = globalHiddenPos;
            }
            pageUI.SetActive(false);
        }

        if (eKeyUI) eKeyUI.SetActive(false);
    }

    void Update()
    {
        if (player == null) return;

        float dist = Vector3.Distance(transform.position, player.position);
        isNear = dist <= interactDistance;

        transform.Rotate(0, rotateSpeed * Time.deltaTime, 0);

        pulseTimer += Time.deltaTime * pulseSpeed;
        float pulse = (Mathf.Sin(pulseTimer) + 1f) / 2f;

        foreach (Renderer r in renderers)
        {
            if (isNear)
            {
                r.material.SetColor("_EmissionColor", nearColor * pulse * 2f);
                r.material.EnableKeyword("_EMISSION");
            }
            else
            {
                r.material.SetColor("_EmissionColor", pulseColor * pulse);
                r.material.EnableKeyword("_EMISSION");
            }
        }

        if (eKeyUI) eKeyUI.SetActive(isNear && !pageOpen);

        if (isNear && Input.GetKeyDown(interactKey))
        {
            if (Time.unscaledTime >= nextInteractTime)
            {
                TogglePage(!pageOpen);
                nextInteractTime = Time.unscaledTime + interactionCooldown; 
            }
        }

        if (pageOpen && !isNear)
        {
            TogglePage(false);
            nextInteractTime = Time.unscaledTime + interactionCooldown; 
        }
    }

    void TogglePage(bool show)
    {
        if (pageOpen == show) return;

        pageOpen = show;

        if (uiCoroutine != null) StopCoroutine(uiCoroutine);

        if (pageRect != null)
        {
            uiCoroutine = StartCoroutine(AnimateUI(show));
        }
        else
        {
            if (pageUI) pageUI.SetActive(show);
        }

        if (show)
        {
            Time.timeScale = 0f;
            
            if (SanityManager.instance != null)
            {
                SanityManager.instance.RestoreSanity();
            }
        }
        else
        {
            Time.timeScale = 1f;

            if (SanityManager.instance != null)
            {
                SanityManager.instance.StartTimer();
            }
        }
    }

    IEnumerator AnimateUI(bool show)
    {
        if (show) pageUI.SetActive(true);

        float elapsedTime = 0f;
        Vector2 startPos = pageRect.anchoredPosition;
        // Artık bireysel değil, ortak pozisyonları kullanıyoruz
        Vector2 endPos = show ? globalShownPos : globalHiddenPos; 

        while (elapsedTime < animDuration)
        {
            elapsedTime += Time.unscaledDeltaTime; 
            
            float t = Mathf.Clamp01(elapsedTime / animDuration); 
            float curveT = 1f - Mathf.Pow(1f - t, 3f);
            
            pageRect.anchoredPosition = Vector2.Lerp(startPos, endPos, curveT);
            yield return null;
        }

        pageRect.anchoredPosition = endPos;
        if (!show) pageUI.SetActive(false);
    }
}