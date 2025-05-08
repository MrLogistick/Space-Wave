using System.Collections;
using UnityEngine;

public class AsteroidGenerator : MonoBehaviour
{
    [Header("Asteroid Settings")]
    [SerializeField] float asteroidRate;
    [SerializeField] float maxAsteroidRate;
    [SerializeField] float asteroidRateSpread;
    [SerializeField] float asteroidRateJump;
    float asteroidDensity = 1;

    [field: SerializeField] public float asteroidSpeed {get; private set;}
    [field: SerializeField] public float asteroidSpeedSpread { get; private set; }
    [SerializeField] float asteroidSpeedJump;

    [Header("Field Settings")]
    [SerializeField] float fieldRate;
    [SerializeField] float maxFieldRate;
    [SerializeField] float fieldRateSpread;
    [SerializeField] float fieldRateJump;
    [SerializeField] float fieldDensity;

    [SerializeField] float fieldPrewarning;
    [SerializeField] float minFieldLife, maxFieldLife;
    [SerializeField] GameObject fieldWarning;
    public int fieldsEndured {get; private set;} = 0;

    [Header("Megaroid & Other Settings")]
    public GameObject[] targetObjects;
    [SerializeField] float megaroidPositioning;
    public bool megaroidActive = true;

    void Start() {
        StartCoroutine(MegaroidSpawnDelay());
        StartCoroutine(AsteroidSpawner());
        StartCoroutine(AsteroidFieldSpawner());
    }

    IEnumerator MegaroidSpawnDelay() {
        yield return new WaitForSeconds(3);
        megaroidActive = false;
    }

    IEnumerator AsteroidSpawner()
    {
        while (!GameState.instance.isDead) {

            int randomInt = Random.Range(0, 100);
            if (randomInt < 10 && !megaroidActive) { // 10% chance to spawn
                Spawn(targetObjects[0], Random.Range(0, 360));
                megaroidActive = true;
                // Spawning a Megaroid
            }
            else if (randomInt < 12) { // 2% chance to spawn
                Spawn(targetObjects[1], 0);
                // Spawning a Bomb
            }
            else { // 88% chance to spawn
                for (int i = 0; i < asteroidDensity; i++)
                {
                    Spawn(targetObjects[Random.Range(2, targetObjects.Length)], Random.Range(0, 360));
                }
                //Spawning a normal asteroid
            }

            float asteroidRandomRate = Random.Range(asteroidRate - asteroidRateSpread, asteroidRate + asteroidRateSpread);
            yield return new WaitForSeconds(asteroidRandomRate);
        }

        yield break;
    }

    void Spawn(GameObject target, float rot)
    {
        float yPos = (target == targetObjects[0])
            ? megaroidPositioning * (Random.value < 0.5f ? -1 : 1)
            : Random.Range(-Camera.main.orthographicSize, Camera.main.orthographicSize);

        transform.position = new Vector2(transform.position.x, yPos);

        GetComponent<MultiObjectPool>().GetFromPool(target, transform.position, Quaternion.Euler(0, 0, rot));
    }

    IEnumerator AsteroidFieldSpawner() {
        while (!GameState.instance.isDead) {

            asteroidDensity = 1;
            fieldWarning.SetActive(false);
            float fieldRandomRate = Random.Range(fieldRate - fieldRateSpread, fieldRate + fieldRateSpread);
            yield return new WaitForSeconds(fieldRandomRate - fieldPrewarning);

            fieldWarning.SetActive(true);
            yield return new WaitForSeconds(fieldPrewarning);

            asteroidDensity = fieldDensity;
            asteroidSpeed += asteroidSpeedJump;

            if (asteroidRate > maxAsteroidRate)
            {
                asteroidRate -= asteroidRateJump;
            }
            if (fieldRate > maxFieldRate)
            {
                fieldRate -= fieldRateJump;
            }

            float fieldLength = Random.Range(minFieldLife, maxFieldLife);
            yield return new WaitForSeconds(fieldLength);

            fieldsEndured++;
        }

        yield break;
    }
}