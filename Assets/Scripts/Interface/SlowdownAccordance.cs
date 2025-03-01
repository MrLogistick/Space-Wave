using UnityEngine;

public class SlowdownAccordance : MonoBehaviour
{
    void Update() {
        GetComponent<Animator>().speed = GameState.instance.slowDown;
    }
}