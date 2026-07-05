using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public static MusicManager instance;

    void Awake()
    {
        // Tekil (singleton) yap, sahneler arası yok olmasın
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject); // Yeni sahnede ikinci bir tane olursa sil
        }
    }
}