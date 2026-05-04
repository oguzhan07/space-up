using UnityEngine;
using Unity.Cinemachine;

public class CharacterSwitch : MonoBehaviour
{
    [Header("Karakterler")]
    public GameObject astronot;
    public GameObject x;

    [Header("Kamera")]
    public CinemachineCamera cinemachineCamera;

    [Header("Tuş")]
    public KeyCode switchKey = KeyCode.F;

    private bool astronotActive = true;

    void Start()
    {
        astronot.SetActive(true);
        x.SetActive(false);

        cinemachineCamera.Follow = astronot.transform;
        cinemachineCamera.LookAt = astronot.transform;
    }

    void Update()
    {
        if (Input.GetKeyDown(switchKey))
            Switch();
    }

    void Switch()
    {
        if (astronotActive)
        {
            // Astronot -> X
            x.transform.position = astronot.transform.position;
            x.transform.rotation = astronot.transform.rotation;

            astronot.SetActive(false);
            x.SetActive(true);

            cinemachineCamera.Follow = x.transform;
            cinemachineCamera.LookAt = x.transform;
        }
        else
        {
            // X -> Astronot
            astronot.transform.position = x.transform.position;
            astronot.transform.rotation = x.transform.rotation;

            x.SetActive(false);
            astronot.SetActive(true);

            cinemachineCamera.Follow = astronot.transform;
            cinemachineCamera.LookAt = astronot.transform;
        }

        astronotActive = !astronotActive;
    }
}