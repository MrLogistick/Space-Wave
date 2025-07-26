using UnityEngine;

public class SlowdownAccordance : MonoBehaviour
{
    void Update() {
        var anim = GetComponent<Animator>();
        if (anim.speed > 0) {
            anim.speed = GameState.instance.slowDown;
        } else {
            anim.speed = 0f;
        }
    }
}