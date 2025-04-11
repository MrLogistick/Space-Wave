using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    [HideInInspector]
    public bool oppositeGravity = false;
    bool hasStarted = false;

    [HideInInspector]
    public float bombs = 0;
    [SerializeField] GameObject bullet;
    [SerializeField] Image bombMeter;

    [SerializeField] float multiplier;
    [SerializeField] float playerSpeed;
    float currentSpeed;
    public float currentRotation;

    [SerializeField] ParticleSystem ps;
    public string deathBy {get; private set;}

    IEnumerator Start() {
        if (PlayerPrefs.GetInt("StaticStart", 1) == 0) {
            GetComponent<Animator>().SetBool("StaticStart", false);
            hasStarted = true;
        }

        yield return new WaitForSeconds(1);

        GetComponent<PlayerInput>().enabled = true;
        GetComponent<Animator>().enabled = false;
    }

    void Update() {
        if (Mathf.Abs(transform.position.y) > 13 && !GameState.instance.isDead) {
            deathBy = "Planet";
            Die();
        }

        float bombAmount = bombs / 5;
        bombMeter.fillAmount = bombAmount;
    }

    void FixedUpdate() {
        if (GameState.instance.isDead) return;
        if (!hasStarted) return;

        Vector2 newPosition = transform.position;

        if (oppositeGravity) {
            if (currentSpeed < playerSpeed) {
                currentSpeed += multiplier * Time.deltaTime;
            }
            if (currentRotation < 0) {
                currentRotation += 900 * Time.deltaTime;
            }
        }
        else {
            if (currentSpeed > -playerSpeed) {
                currentSpeed -= multiplier * Time.deltaTime;
            }
            if (currentRotation > -90) {
                currentRotation -= 900 * Time.deltaTime;
            }
        }

        transform.rotation = Quaternion.Euler(0, 0, currentRotation);
        newPosition.y += currentSpeed;
        transform.position = newPosition;
    }
    
    public void FlipInput(InputAction.CallbackContext context) {
        if (context.performed) {
            hasStarted = true;
            oppositeGravity = !oppositeGravity;
        }
    }

    public void ShootInput(InputAction.CallbackContext context) {
        if (context.performed && bombs > 0) {
            Instantiate(bullet, transform.position, Quaternion.Euler(0, 0, 0));
            bombs--;
        }
    }

    void Die() {
        GameState.instance.isDead = true;
        transform.localScale = Vector3.zero;
        GetComponent<PlayerInput>().enabled = false;
    }

    private void OnCollisionEnter2D(Collision2D other) {
        if (other.gameObject.CompareTag("Asteroid")) {
            Instantiate(ps, transform.position, transform.rotation);
            deathBy = other.gameObject.name;
            Die();
        }
    }
}