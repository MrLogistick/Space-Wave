using UnityEngine;

public class Player_Sonar : MonoBehaviour {
    [SerializeField] float startSpeed;
    [SerializeField] float startSize;
    [SerializeField] float speedRate;
    [SerializeField] float sizeRate;
    float currentSpeed;
    float currentSizeRate;

    public int damage;

    void Start() {
        transform.localScale = Vector3.one * startSize;
        currentSizeRate = sizeRate;
        currentSpeed = startSpeed;
    }

    void Update() {
        transform.position += (transform.up + transform.right) * currentSpeed * GameState.instance.slowDown * Time.deltaTime;
        transform.localScale += Vector3.one * currentSizeRate * GameState.instance.slowDown * Time.deltaTime;

        if (currentSpeed > 0) currentSpeed += speedRate;
        else currentSpeed = 0.1f;

        if (transform.position.x > 50) {
            Destroy(gameObject);
        }
    }

    void ArtemisLogic() {
        if (transform.position.y > 11f) {
            transform.rotation = Quaternion.Euler(0f, 0f, -90f);
        } else if (transform.position.y < -11f) {
            transform.rotation = Quaternion.Euler(0f, 0f, 0f);
        }
    }
}