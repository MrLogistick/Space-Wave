using UnityEngine;
using UnityEngine.InputSystem;

public class MovementController : MonoBehaviour
{
    [HideInInspector] public float moveSpeed;
    float nextPos;
    [HideInInspector] public float turnSpeed;
    float nextRot;

    [SerializeField] float borderPos;
    [HideInInspector] public string deathBy;
    [HideInInspector] public int gravity = -1;
    bool hasStarted;
    bool canControl;

    Rigidbody2D rb;

    void Start() {
        nextPos = transform.position.y;
        nextRot = transform.eulerAngles.z;
        rb = GetComponent<Rigidbody2D>();

        if (PlayerPrefs.GetInt("StaticStart", 1) == 0) {
            GetComponent<Animator>().SetBool("StaticStart", false);
            hasStarted = true;
        }
    }

    void Update() {
        if (Mathf.Abs(transform.position.y) > borderPos) {
            deathBy = "Planet";
            Die();
        }

        GetComponentInChildren<Collider2D>().enabled = canControl;
        GetComponent<PlayerInput>().enabled = canControl;

        if (GameState.instance.isDead) {
            enabled = false;
        }
    }

    void FixedUpdate() {
        if (!hasStarted) return;

        nextPos += moveSpeed * gravity * Time.fixedDeltaTime;
        nextRot += turnSpeed * gravity * Time.fixedDeltaTime;

        Vector2 targetPos = new Vector2(rb.position.x, nextPos);
        rb.MovePosition(targetPos);
        rb.MoveRotation(nextRot);
    }

    public void FlipInput(InputAction.CallbackContext context) {
        if (context.performed) {
            hasStarted = true;
            gravity *= -1;
        }
    }

    public void EndCutscene() {
        canControl = true;
    }

    public void Die() {
        transform.parent.GetComponentInChildren<ParticleSystem>().Stop();
        GameState.instance.isDead = true;
        canControl = false;
    }
}