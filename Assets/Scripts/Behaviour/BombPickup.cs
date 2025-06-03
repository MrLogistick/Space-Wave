using UnityEngine;

public class BombPickup : ObstacleBehaviour
{
    bool isCollected = false;

    void Update() {
        if (isCollected) return;

        Vector2 target = new Vector2(-30, transform.position.y);
        transform.position = Vector2.MoveTowards(transform.position, target, speed * GameState.instance.slowDown * Time.deltaTime);

        if (transform.position.x <= -30)
        {
            DestroyPickup();
        }
    }

    public void DestroyPickup() {
        DisableObject(prefabs[objID], gameObject);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player")) {
            other.transform.parent.GetComponent<WeaponController>().bombCount++;
            GetComponent<BoxCollider2D>().enabled = false;
            GetComponent<Animator>().enabled = true;
            isCollected = true;
        }
    }
}