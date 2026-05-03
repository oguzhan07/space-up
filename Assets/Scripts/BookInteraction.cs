using UnityEngine;
using UnityEngine.UI;

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

    [Header("UI")]
    public GameObject eKeyUI;

    private Renderer[] renderers;
    private Material[] originalMaterials;
    private bool isNear = false;
    private bool pageOpen = false;
    private float pulseTimer = 0f;

    // Outline için
    private Material outlineMaterial;

    void Start()
    {
        renderers = GetComponentsInChildren<Renderer>();

        // Outline materyali oluştur
        outlineMaterial = new Material(Shader.Find("Universal Render Pipeline/Lit"));
        outlineMaterial.color = nearColor;

        if (pageUI) pageUI.SetActive(false);
        if (eKeyUI) eKeyUI.SetActive(false);
    }

    void Update()
    {
        if (player == null) return;

        float dist = Vector3.Distance(transform.position, player.position);
        isNear = dist <= interactDistance;

        // Dönme
        transform.Rotate(0, rotateSpeed * Time.deltaTime, 0);

        // Yanıp sönme
        pulseTimer += Time.deltaTime * pulseSpeed;
        float pulse = (Mathf.Sin(pulseTimer) + 1f) / 2f;

        foreach (Renderer r in renderers)
        {
            if (isNear)
            {
                // Yakında: beyaz outline efekti
                r.material.SetColor("_EmissionColor", nearColor * pulse * 2f);
                r.material.EnableKeyword("_EMISSION");
            }
            else
            {
                // Uzakta: farklı renk yanıp sönsün
                r.material.SetColor("_EmissionColor", pulseColor * pulse);
                r.material.EnableKeyword("_EMISSION");
            }
        }

        // E tuşu UI
        if (eKeyUI) eKeyUI.SetActive(isNear && !pageOpen);

        // Etkileşim
        if (isNear && Input.GetKeyDown(interactKey))
        {
            pageOpen = !pageOpen;
            if (pageUI) pageUI.SetActive(pageOpen);
            if (eKeyUI) eKeyUI.SetActive(!pageOpen);
        }
    }
}