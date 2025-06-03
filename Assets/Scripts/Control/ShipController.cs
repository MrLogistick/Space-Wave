using UnityEngine;

public class ShipController : MonoBehaviour
{
    [SerializeField] float moveSpeed;
    [SerializeField] float boostSpeed;
    [SerializeField] float acceleration;
    [SerializeField] int fieldsToBoost;
    [SerializeField] float turnSpeed;

    [SerializeField] GameObject bombPrefab;
    [SerializeField] int bombCapacity;
    [SerializeField] int initialBombs;

    MovementController movement;
    WeaponController weapon;

    void Awake() {
        movement = transform.parent.GetComponent<MovementController>();
        weapon = transform.parent.GetComponent<WeaponController>();

        movement.moveSpeed = moveSpeed;
        movement.boostSpeed = boostSpeed;
        movement.acceleration = acceleration;
        movement.fieldsToBoost = fieldsToBoost;
        movement.turnSpeed = turnSpeed;

        weapon.bombPrefab = bombPrefab;
        weapon.bombCapacity = bombCapacity;
        weapon.initialBombs = initialBombs;
    }

    void OnCollisionEnter2D(Collision2D other) {
        if (other.gameObject.CompareTag("Asteroid")) {
            movement.deathBy = other.gameObject.name;
            transform.parent.GetComponent<Animator>().SetTrigger("Explode");
            GetComponent<SpriteRenderer>().enabled = false;
            movement.Die();
        }
    }
}