using UnityEngine;
using UnityEngine.InputSystem;

public class MovementController : MonoBehaviour
{
    [HideInInspector] public float moveSpeed;
    [HideInInspector] public float boostSpeed;
    float topSpeed;
    [HideInInspector] public float acceleration;
    [HideInInspector] public int fieldsToBoost;
    float currentSpeed;

    [HideInInspector] public float turnSpeed;
    float currentRot = -45f;

    [SerializeField] float borderPos;
    [HideInInspector] public string deathBy;
    [HideInInspector] public int gravity = -1;
    bool hasStarted;
    bool canControl;
    Animator anim;

    void Start() {
        anim = GetComponent<Animator>();
        anim.applyRootMotion = false;

        if (PlayerPrefs.GetInt("StaticStart", 1) == 0) {
            anim.SetBool("StaticStart", false);
            hasStarted = true;
        }
    }

    void Update() {
        if (Mathf.Abs(transform.position.y) > borderPos) {
            deathBy = "Planet";
            Die();
        }

        if (GameState.instance.fieldsEndured < fieldsToBoost) {
            topSpeed = moveSpeed;
        } else {
            topSpeed = boostSpeed;
        }

        GetComponentInChildren<Collider2D>().enabled = canControl;
        GetComponent<PlayerInput>().enabled = canControl;

        if (GameState.instance.isDead) {
            enabled = false;
        }
    }

    void FixedUpdate() {
        if (!hasStarted) return;

        currentSpeed += acceleration * gravity * Time.fixedDeltaTime;
        currentSpeed = Mathf.Clamp(currentSpeed, -topSpeed, topSpeed);

        if (currentRot < 0f && gravity == 1) {
            currentRot += turnSpeed * Time.fixedDeltaTime;
        }
        if (currentRot > -90f && gravity == -1) {
            currentRot -= turnSpeed * Time.fixedDeltaTime;
        }

        Vector2 newPos = transform.position;
        newPos.y += currentSpeed;
        transform.position = newPos;

        transform.rotation = Quaternion.Euler(0, 0, currentRot);
    }

    public void FlipInput(InputAction.CallbackContext context) {
        if (context.performed) {
            hasStarted = true;
            gravity *= -1;
        }
    }

    public void EndCutscene() {
        anim.applyRootMotion = true;
        canControl = true;
    }

    public void Die() {
        transform.parent.GetComponentInChildren<ParticleSystem>().Stop();
        GameState.instance.isDead = true;
        canControl = false;
    }
}