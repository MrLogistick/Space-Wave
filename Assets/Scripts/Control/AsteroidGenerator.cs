using System.Collections;
using UnityEngine;

public class AsteroidGenerator : MonoBehaviour
{
    [Header("Asteroid Spawn")]
    [SerializeField] float asteroidRate;
    [SerializeField] float maxAsteroidRate;
    [SerializeField] float asteroidRateSpread;
    [SerializeField] float asteroidRateJump;
    float currentRate;
    [SerializeField] float asteroidDensity;
    float currentDensity;

    [Header("Asteroid Speed")]
    [SerializeField] float asteroidSpeed;
    [field: SerializeField] public float asteroidSpeedSpread { get; private set; }
    [SerializeField] float asteroidSpeedJump;
    [SerializeField] float newSpeedJump;
    float speedJump;

    [Header("Field Spawn")]
    [SerializeField] float fieldRate;
    [SerializeField] float maxFieldRate;
    [SerializeField] float fieldRateSpread;
    [SerializeField] float fieldRateJump;
    [SerializeField] float fieldDensity;
    float fieldAsteroidRate;

    [Header("Field Warning")]
    [SerializeField] float fieldPrewarning;
    [SerializeField] GameObject fieldWarning;
    public int fieldsEndured {get; private set;} = 0;

    [Header("Field Life")]
    [SerializeField] float minFieldLife, maxFieldLife;
    [SerializeField] int fieldsToWin;

    [Header("Megaroid & Other Settings")]
    public GameObject[] targetObjects;
    [SerializeField] float megaroidPositioning;
    public bool megaroidActive = true;

    IEnumerator Start() {
        targetObjects = GetComponent<MultiObjectPool>().prefabs;
        currentDensity = 1f;

        yield return new WaitForSeconds(0.1f);

        StartCoroutine(MegaroidSpawnDelay());
        StartCoroutine(AsteroidSpawner());
        StartCoroutine(AsteroidFieldSpawner());
    }

    void Update() {
        fieldAsteroidRate = asteroidRate - 0.1f;

        if (fieldsEndured >= 16) {
            speedJump = 0; // current speed is 60
        } else if (fieldsEndured >= 8) {
            speedJump = newSpeedJump; // speed 36.4
        } else {
            speedJump = asteroidSpeedJump; // speed 22
        }
    }

    IEnumerator MegaroidSpawnDelay() {
        yield return new WaitForSeconds(3);
        megaroidActive = false;
    }

    IEnumerator AsteroidSpawner() {
        while (fieldsEndured < fieldsToWin && GameState.instance.slowDown > 0) {
            int randomInt = Random.Range(0, 100);

            if (randomInt < 10 && !megaroidActive) { // 10% chance to spawn
                Spawn(0, Random.Range(0, 360));
                megaroidActive = true;
                // Spawning a Megaroid
            }
            else if (randomInt < 12) { // 2% chance to spawn
                Spawn(1, 0);
                // Spawning a Bomb
            }
            else { // 88% chance to spawn
                for (int i = 0; i < currentDensity; i++)
                {
                    int objID = Random.Range(2, targetObjects.Length);
                    Spawn(objID, Random.Range(0, 360));
                }
                //Spawning a normal asteroid
            }

            float asteroidRandomRate = Random.Range(currentRate - asteroidRateSpread, currentRate + asteroidRateSpread);
            yield return new WaitForSeconds(asteroidRandomRate);
        }

        yield break;
    }

    void Spawn(int objID, float rot)
    {
        float yPos = (targetObjects[objID] == targetObjects[0])
            ? megaroidPositioning * (Random.value < 0.5f ? -1 : 1)
            : Random.Range(-Camera.main.orthographicSize, Camera.main.orthographicSize);

        transform.position = new Vector2(transform.position.x, yPos);

        var spawnedRoid = GetComponent<MultiObjectPool>().GetFromPool(targetObjects[objID], transform.position, Quaternion.Euler(0, 0, rot));
        spawnedRoid.GetComponent<ObstacleBehaviour>().speed = Random.Range(asteroidSpeed - asteroidSpeedSpread, asteroidSpeed + asteroidSpeedSpread);
        spawnedRoid.GetComponent<ObstacleBehaviour>().objID = objID;
    }

    IEnumerator AsteroidFieldSpawner() {
        while (!GameState.instance.isDead && fieldsEndured < fieldsToWin) {
            currentRate = asteroidRate;
            float fieldRandomRate = Random.Range(fieldRate - fieldRateSpread, fieldRate + fieldRateSpread);
            yield return new WaitForSeconds(fieldRandomRate - fieldPrewarning);
    
            fieldWarning.SetActive(true);
            yield return new WaitForSeconds(fieldPrewarning);
    
            currentRate = fieldAsteroidRate;
            currentDensity = fieldDensity;
            asteroidSpeed += speedJump;
    
            if (asteroidRate > maxAsteroidRate) {
                asteroidRate -= asteroidRateJump;
            }
            if (fieldRate > maxFieldRate) {
                fieldRate -= fieldRateJump;
            }
    
            float fieldLength = Random.Range(minFieldLife, maxFieldLife);
            yield return new WaitForSeconds(fieldLength);
            
            fieldsEndured++;
            currentDensity = asteroidDensity;
            fieldWarning.SetActive(false);
        }

        yield break;
    }
}