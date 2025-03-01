using UnityEngine;

public class SonarBullet : MonoBehaviour
{
    [SerializeField] float bulletSpeed;
    [SerializeField] GameObject sonarBlast;

    void Update() {
        Vector2 target = new Vector2(0, transform.position.y);
        transform.position = Vector2.MoveTowards(transform.position, target, bulletSpeed * GameState.instance.slowDown * Time.deltaTime);

        if (transform.position.x >= 0) {
            Instantiate(sonarBlast, transform.position, transform.rotation);
            Destroy(gameObject);
        }
    }
}