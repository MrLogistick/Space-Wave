using UnityEngine;

public class ArtemisSonar : MonoBehaviour
{
    [SerializeField] float startSpeed;
    [SerializeField] float startSize;
    [SerializeField] float startSizeRate;
    [SerializeField] float speedInterval;
    [SerializeField] float sizeInterval;
    float currentSpeed;
    float currentSizeRate;

    public int damage;

    void Start() {
        transform.localScale = Vector3.one * startSize;
        currentSizeRate = startSizeRate;
        currentSpeed = startSpeed;
    }

    void Update() {
        transform.position += (transform.up + transform.right) * currentSpeed * GameState.instance.slowDown * Time.deltaTime;
        transform.localScale += Vector3.one * currentSizeRate * GameState.instance.slowDown * Time.deltaTime;

        if (currentSpeed > 0) currentSpeed += speedInterval;
        else currentSpeed = 0.1f;
        if (currentSizeRate > 0) currentSizeRate += sizeInterval;
        else currentSizeRate = 0f;

        if (transform.position.y > 11f) {
            transform.rotation = Quaternion.Euler(0f, 0f, -90f);
        } else if (transform.position.y < -11f) {
            transform.rotation = Quaternion.Euler(0f, 0f, 0f);
        }

        if (transform.position.x > 30) {
            Destroy(gameObject);
        }
    }
}
