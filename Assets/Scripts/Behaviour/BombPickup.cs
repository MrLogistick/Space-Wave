using UnityEngine;

public class BombPickup : MonoBehaviour
{
    [SerializeField] float maxSpeed;
    float speed;
    bool isCollected = false;

    AsteroidGenerator ag;

    void Start()
    {
        ag = GameObject.Find("AsteroidSpawn").GetComponent<AsteroidGenerator>();

        speed = Random.Range(ag.asteroidSpeed - ag.asteroidSpeedSpread, ag.asteroidSpeed + ag.asteroidSpeedSpread);
        if (speed > maxSpeed)
        {
            speed -= ag.asteroidSpeed - maxSpeed;
        }
    }

    void Update() {
        if (isCollected) return;

        Vector2 target = new Vector2(-30, transform.position.y);
        transform.position = Vector2.MoveTowards(transform.position, target, speed * GameState.instance.slowDown * Time.deltaTime);

        if (transform.position.x <= -30)
        {
            gameObject.SetActive(false);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player")) {
            other.GetComponent<BombController>().bombCount++;
            GetComponent<BoxCollider2D>().enabled = false;
            GetComponent<Animator>().enabled = true;
            isCollected = true;
        }
    }

    public void DestroyPickup() {
        gameObject.SetActive(false);
    }
}