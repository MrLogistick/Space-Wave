using System.Collections;
using UnityEngine;

public class AsteroidBehaviour : ObstacleBehaviour
{
    [Header("Movement")]
    [SerializeField] int spawnedMiniroids;
    [SerializeField] float miniroidSpawnRange;
    float rotationSpeed;

    [Header("Health")]
    [SerializeField] Color flashColor;
    int asteroidHealth;
    int asteroidInitialHealth = 1;
    int megaroidInitialHealth = 2;

    [SerializeField] ParticleSystem ps;

    void Start() {
        asteroidHealth = gameObject.name.Contains("Gargantuan") ? megaroidInitialHealth : asteroidInitialHealth;
        rotationSpeed = Random.Range(rotationSpeedRange.x, rotationSpeedRange.y) * (Random.value < 0.5f ? -1 : 1);
    }


    void Update() {
        Vector2 target = new Vector2(-30, transform.position.y);
        transform.position = Vector2.MoveTowards(transform.position, target, speed * GameState.instance.slowDown * Time.deltaTime);
        transform.Rotate(Vector3.forward * rotationSpeed * GameState.instance.slowDown * Time.deltaTime);

        HealthCheck();
        if (transform.position.x <= -30) {
            if (gameObject.name.Contains("Gargantuan")) {
                generator.megaroidActive = false;
            }

            DisableObject(prefabs[objID], gameObject);
        }
    }

    void HealthCheck() {
        if (asteroidHealth <= 0) {
            if (gameObject.name.Contains("Gargantuan")) {
                generator.megaroidActive = false;

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
            asteroidHealth -= other.GetComponent<SonarBlast>().damage;
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

            var spawnedRoid = generator.gameObject.GetComponent<MultiObjectPool>().GetFromPool(generator.targetObjects[5], spawnPos, Quaternion.Euler(0, 0, Random.Range(0, 360)));
            spawnedRoid.GetComponent<AsteroidBehaviour>().speed = Random.Range(speed - generator.asteroidSpeedSpread, speed + generator.asteroidSpeedSpread);
        }
    }

    void DestroyAsteroid()
    {
        ParticleSystem explosion = Instantiate(ps, transform.position, Quaternion.Euler(0, 0, 0));
        var particleVelocity = explosion.velocityOverLifetime;
        particleVelocity.x = -speed;

        DisableObject(prefabs[objID], gameObject);
    }
}