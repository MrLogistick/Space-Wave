using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour {
    [SerializeField] float borderPos;

    [SerializeField] List<Image> bombMeters;
    Dictionary<int, Image> bombMeterLookup = new Dictionary<int, Image>();

    Image bombMeter;
    Image secondBombMeter;
    public int gravity = -1;
    float currentSpeed;
    float currentRot = -45f;
    float currentBombs;
    bool hasBegun;
    bool canControl;

    [SerializeField] SpriteRenderer sr;
    PolygonCollider2D coll;
    ShipData data;
    Animator anim;

    IEnumerator Start() {
        yield return new WaitUntil(() => PersistantManager.instance.currentShip != null);
        data = PersistantManager.instance.currentShip;

        SetupShip();
    }

    void SetupShip() {
        coll = Instantiate(data.colliderObject, transform, false).GetComponent<PolygonCollider2D>();
        sr.enabled = true;
        sr.sprite = data.shipSprite;

        foreach (var meter in bombMeters) {
            if (meter.name.Contains("D")) continue;
            int capacity = int.Parse(meter.name.Replace("Bullets", ""));
            bombMeterLookup[capacity] = meter;
        }

        anim = GetComponent<Animator>();
        anim.applyRootMotion = false;
        hasBegun = PlayerPrefs.GetInt("StaticStart", 1) == 0 ? true : false;

        currentBombs = data.initialBombs;
        if (data.shipType != ShipData.ShipType.Zeus) {
            bombMeter = bombMeterLookup[data.bombCapacity];
        } else {
            bombMeter = bombMeters[2];
            secondBombMeter = bombMeters[3];
            secondBombMeter.transform.parent.gameObject.SetActive(true);
        }

        bombMeter.transform.parent.gameObject.SetActive(true);
    }

    void Update() {
        if (data == null) return;

        coll.enabled = canControl;
        GetComponent<PlayerInput>().enabled = canControl;

        UpdateBombMeter();

        if (Mathf.Abs(transform.position.y) > borderPos) {
            Die();
        }
    }

    void FixedUpdate() {
        if (!hasBegun) return;

        currentSpeed += data.acceleration * gravity * Time.deltaTime;
        currentRot += data.turnSpeed * gravity * Time.deltaTime;
        currentSpeed = Mathf.Clamp(currentSpeed, -data.moveSpeed, data.moveSpeed);
        currentRot = Mathf.Clamp(currentRot, -90f, 0f);

        Vector2 newPos = transform.position;
        newPos.y += currentSpeed;
        transform.position = newPos;

        transform.rotation = Quaternion.Euler(0f, 0f, currentRot);
    }

    void UpdateBombMeter() {
        if (data.shipType != ShipData.ShipType.Zeus) {
            bombMeter.fillAmount = currentBombs / data.bombCapacity;
        } else {
            bombMeter.fillAmount = currentBombs / (data.bombCapacity / 2);
            secondBombMeter.fillAmount = (currentBombs - 10) / (data.bombCapacity / 2);
        }
    }

    public void FlipInput(InputAction.CallbackContext context) {
        if (context.performed && canControl) {
            hasBegun = true;
            gravity *= -1;
        }
    }

    public void ShootInput(InputAction.CallbackContext context) {
        if (context.performed && currentBombs > 0) {
            currentBombs--;
            Transform sonar = Instantiate(data.weaponPrefab, transform.position, Quaternion.Euler(0f, 0f, -45f)).transform;
            if (data.shipType == ShipData.ShipType.Artemis) {
                sonar.rotation = transform.rotation;
            }
        }
    }

    public void AddBomb(int count) {
        currentBombs += count;
    }

    public void EndCutscene() {
        anim.applyRootMotion = true;
        canControl = true;
    }

    void OnCollisionEnter2D(Collision2D other) {
        if (other.gameObject.CompareTag("Asteroid")) {
            anim.SetTrigger("Explode");
            Die();
        }
    }

    void Die() {
        GetComponentInChildren<ParticleSystem>().Stop();
        sr.enabled = false;
        GameState.instance.isDead = true;
        canControl = false;
    }
}