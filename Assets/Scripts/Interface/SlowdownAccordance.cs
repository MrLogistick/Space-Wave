using UnityEngine;

public class SlowdownAccordance : MonoBehaviour
{
    void Update() {
        var anim = GetComponent<Animator>();
        if (anim.speed > 0) {
            anim.speed = GameState.instance.slowDown;
            Mathf.Clamp(anim.speed, 0f, 1f);
        } else {
            anim.speed = 0f;
        }
    }
}