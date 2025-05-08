using System.Collections;
using UnityEngine;

public class AsteroidBehaviour : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] float minRotationSpeed;
    [SerializeField] float maxRotationSpeed;
    [SerializeField] float maxSpeed;
    [SerializeField] int spawnedMiniroids;
    [SerializeField] float miniroidSpawnRange;
    float asteroidSpeed;
    float rotationSpeed;

    [Header("Health")]
    [SerializeField] Color flashColor;
    int asteroidHealth;
    int asteroidInitialHealth = 1;
    int megaroidInitialHealth = 2;

    [SerializeField] ParticleSystem ps;
    AsteroidGenerator ag;

    void Start() {
        ag = GameObject.Find("AsteroidSpawn").GetComponent<AsteroidGenerator>();
        asteroidHealth = gameObject.name.Contains("Gargantuan") ? megaroidInitialHealth : asteroidInitialHealth;

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

        HealthCheck();
        if (transform.position.x <= -30) {
            if (gameObject.name.Contains("Gargantuan")) {
                ag.megaroidActive = false;
            }

            Destroy(gameObject);
        }
    }

    void HealthCheck() {
        if (asteroidHealth <= 0) {
            if (gameObject.name.Contains("Gargantuan")) {
                ag.megaroidActive = false;

                SpawnMiniroids();
            }

            DestroyAsteroid();
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
        if (other.gameObject.CompareTag("Sonar")) {
            asteroidHealth--;
            StartCoroutine(DamageFlash());
        }
    }

    IEnumerator DamageFlash() {
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        Color originalColor = sr.color;

        sr.color = flashColor;
        yield return new WaitForSeconds(0.1f);
        sr.color = originalColor;
    }

    void SpawnMiniroids() {
        for (int i = 0; i < spawnedMiniroids; i++) {
            Vector2 pos = transform.position;
            Vector2 spawnPos = pos + Random.insideUnitCircle * miniroidSpawnRange;

            ag.gameObject.GetComponent<MultiObjectPool>().GetFromPool(ag.targetObjects[5], spawnPos, Quaternion.Euler(0, 0, Random.Range(0, 360)));
        }
    }

    void DestroyAsteroid()
    {
        Instantiate(ps, transform.position, Quaternion.Euler(0, 0, 0));
        gameObject.SetActive(false);
    }
}