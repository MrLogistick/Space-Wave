using UnityEngine;

public class DestroyOnFinish : MonoBehaviour
{
    void Update() {
        if (GetComponent<ParticleSystem>().isStopped) {
            Destroy(gameObject);
        }
    }
}
