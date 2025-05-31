using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [HideInInspector]
    public bool oppositeGravity = false;
    bool hasStarted = false;

    [SerializeField] float multiplier;
    [SerializeField] float playerSpeed;
    float currentSpeed;
    float currentRotation = -45f;

    bool canControl = false;
    public string deathBy {get; private set;}

    CircleCollider2D coll;
    PlayerInput playerInput;
    Animator anim;

    void Start() {
        coll = GetComponent<CircleCollider2D>();
        playerInput = GetComponent<PlayerInput>();
        anim = GetComponent<Animator>();

        anim.applyRootMotion = false;

        if (PlayerPrefs.GetInt("StaticStart", 1) == 0) {
            anim.SetBool("StaticStart", false);
            hasStarted = true;
        }
    }

    void Update() {
        if (Mathf.Abs(transform.position.y) > 11 && !GameState.instance.isDead) {
            deathBy = "Planet";
            Die();
        }

        coll.enabled = canControl;
        playerInput.enabled = canControl;
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
                currentRotation += 350 * Time.deltaTime;
            }
        }
        else {
            if (currentSpeed > -playerSpeed) {
                currentSpeed -= multiplier * Time.deltaTime;
            }
            if (currentRotation > -90) {
                currentRotation -= 350 * Time.deltaTime;
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
        if (context.performed) {
            GetComponent<BombController>().ShootBomb();
        }
    }

    public void EndCutscene() {
        anim.applyRootMotion = true;
        canControl = true;
    }

    void Die() {
        GameState.instance.isDead = true;
        GetComponentInChildren<ParticleSystem>().Stop();

        canControl = false;
    }

    private void OnCollisionEnter2D(Collision2D other) {
        if (other.gameObject.CompareTag("Asteroid")) {
            deathBy = other.gameObject.name;

            Die();
            anim.SetTrigger("Explode");
        }
    }
}