using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using System.Collections.Generic;

public class PlayerController : MonoBehaviour {
    [SerializeField] float borderPos;
    [HideInInspector] public int gravity;
    [HideInInspector] public int currentBombs;

    [SerializeField] List<Image> bombMeters;
    Dictionary<int, Image> bombMeterLookup = new Dictionary<int, Image>();

    Image bombMeter;
    Image secondBombMeter;
    float currentSpeed;
    float currentRot;
    bool hasBegun;
    bool canControl;

    ShipData data;
    Animator anim;

    void Awake() {
        data = PersistantManager.instance.currentShip;
        Instantiate(data.colliderObject, transform, false);
        GetComponent<SpriteRenderer>().sprite = data.shipSprite;

        foreach (var meter in bombMeters) {
            if (meter.name.Contains("D")) continue;
            int capacity = int.Parse(meter.name.Replace("Bullets", ""));
            bombMeterLookup[capacity] = meter;
        }
    }

    void Start() {
        anim = GetComponent<Animator>();
        anim.applyRootMotion = false;
        hasBegun = PlayerPrefs.GetInt("StaticStart", 1) == 0 ? true : false;

        currentBombs = data.initialBombs;
        if (data.shipType != ShipData.ShipType.Zeus) {
            bombMeter = bombMeterLookup[data.bombCapacity];
        } else {
            bombMeter = bombMeters[2];
            secondBombMeter = bombMeters[3];
        }
    }

    void Update() {
        GetComponentInChildren<PolygonCollider2D>().enabled = canControl;
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
        Mathf.Clamp(currentSpeed, -data.moveSpeed, data.moveSpeed);
        Mathf.Clamp(currentRot, -90f, 0f);

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
        if (context.performed) {
            currentBombs--;
            Transform sonar = Instantiate(data.weaponPrefab, transform.position, Quaternion.Euler(0f, 0f, -45f)).transform;
            if (data.shipType == ShipData.ShipType.Artemis) {
                sonar.rotation = transform.rotation;
            }
        }
    }

    void EndCutscene() {
        anim.applyRootMotion = true;
        canControl = true;
    }

    void Die() {
        GetComponentInChildren<ParticleSystem>().Stop();
        canControl = false;
    }
}