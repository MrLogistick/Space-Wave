using UnityEngine;

public class GravityLightBehaviour : MonoBehaviour
{
    [SerializeField] PlayerController player;

    void Update() {
        if (player.gravity == 1) {
            transform.GetChild(0).gameObject.SetActive(true);
            transform.GetChild(1).gameObject.SetActive(false);

        }
        else {
            transform.GetChild(0).gameObject.SetActive(false);
            transform.GetChild(1).gameObject.SetActive(true);
        }
    }
}