using UnityEngine;

public class RotateZ : MonoBehaviour
{
    public float rotateSpeed = 90f;

    void Update()
    {
        transform.Rotate(0, 0, rotateSpeed * Time.deltaTime);
    }
}