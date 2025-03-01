using UnityEngine;

public class PersistantObjects : MonoBehaviour
{
    private static PersistantObjects instance;

    void Awake() {
        if (instance != null && instance != this) {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);
    }
}