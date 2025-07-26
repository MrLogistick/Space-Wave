using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class WeaponController : MonoBehaviour
{
    [HideInInspector] public GameObject bombPrefab;
    [HideInInspector] public float bombCapacity;
    [HideInInspector] public int initialBombs;

    [HideInInspector] public Image bombMeter;
    [HideInInspector] public bool secondMeterEnabled;
    [HideInInspector] public Image secondMeter;
    [HideInInspector] public float bombCount;

    void Start() {
        bombCount = initialBombs;
    }

    void Update() {
        Mathf.Clamp(bombCount, 0, bombCapacity);
        bombMeter.transform.parent.gameObject.SetActive(true);

        if (secondMeterEnabled) {
            bombMeter.fillAmount = bombCount / (bombCapacity / 2f);
            secondMeter.fillAmount = (bombCount - 10f) / (bombCapacity / 2f);
        } else {
            bombMeter.fillAmount = bombCount / bombCapacity;
        }
    }

    public void ShootInput(InputAction.CallbackContext context) {
        if (context.performed) {
            Shoot();
        }
    }

    void Shoot() {
        if (GameState.instance.isDead || bombCount <= 0) return;

        bombCount--;

        if (ShipManager.instance.currentShip == "Round") {
            Instantiate(bombPrefab, transform.position, transform.rotation);
        }
        else {
            Instantiate(bombPrefab, transform.position, Quaternion.Euler(0, 0, -45f));
        }
    }
}