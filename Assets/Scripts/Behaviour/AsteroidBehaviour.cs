using UnityEngine;

public class AsteroidBehaviour : MonoBehaviour
{
    [SerializeField] float minRotationSpeed, maxRotationSpeed;
    [SerializeField] float maxSpeed;

    float asteroidSpeed;
    float rotationSpeed;

    [SerializeField] ParticleSystem ps;
    AsteroidGenerator ag;

    void Start() {
        ag = GameObject.Find("AsteroidSpawn").GetComponent<AsteroidGenerator>();

        asteroidSpeed = Random.Range(ag.asteroidSpeed - ag.asteroidSpeedSpread, ag.asteroidSpeed + ag.asteroidSpeedSpread);
        if (asteroidSpeed > maxSpeed) {
            asteroidSpeed -= ag.asteroidSpeed - maxSpeed;
        }

        rotationSpeed = Random.Range(minRotationSpeed, maxRotationSpeed) * (Random.value < 0.5f ? -1 : 1);
    }


    void Update() {
        Vector2 target = new Vector2(-30, transform.position.y);
        transform.position = Vector2.MoveTowards(transform.position, target, asteroidSpeed * GameState.instance.slowDown * Time.deltaTime);
        transform.Rotate(Vector3.forward * rotationSpeed * GameState.instance.slowDown * Time.deltaTime);

        if (transform.position.x <= -30) {
            if (gameObject.name.Contains("Gargantuan")) {
                ag.mgOnScreen = false;
            }

            Destroy(gameObject);
        }
    }

    void OnCollisionEnter2D(Collision2D other) {
        if (gameObject.name.Contains("Gargantuan")) return;

        if (PlayerPrefs.GetInt("AsteroidCollisions", 1) == 0) return;

        if (other.gameObject.CompareTag("Asteroid")) {
            DestroyAsteroid();
        }
    }

    void OnTriggerEnter2D(Collider2D other) {
        if (gameObject.name.Contains("Gargantuan")) return;

        if (other.gameObject.CompareTag("Sonar")) {
            DestroyAsteroid();
        }
    }

    void DestroyAsteroid()
    {
        Instantiate(ps, transform.position, Quaternion.Euler(0, 0, 0));
        Destroy(gameObject);
    }
}