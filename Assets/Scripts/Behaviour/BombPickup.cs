using UnityEngine;

public class BombPickup : MonoBehaviour
{
    [SerializeField] float maxSpeed;
    float speed;
    AsteroidGenerator ag;

    void Start()
    {
        ag = GameObject.Find("AsteroidSpawn").GetComponent<AsteroidGenerator>();

        speed = Random.Range(ag.asteroidSpeed - ag.asteroidSpeedSpread, ag.asteroidSpeed + ag.asteroidSpeedSpread);
        if (speed > maxSpeed) {
            speed -= ag.asteroidSpeed - maxSpeed;
        }
    }

    void Update()
    {
        Vector2 target = new Vector2(-30, transform.position.y);
        transform.position = Vector2.MoveTowards(transform.position, target, speed * GameState.instance.slowDown * Time.deltaTime);

        if (transform.position.x <= -30) {
            Destroy(gameObject);
        }
    }

    void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.CompareTag("Player")) {
            other.GetComponent<BombController>().bombCount++;
            gameObject.SetActive(false);
        }
    }
}