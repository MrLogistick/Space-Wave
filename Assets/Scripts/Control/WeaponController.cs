using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class WeaponController : MonoBehaviour
{
    [HideInInspector] public GameObject bombPrefab;
    [HideInInspector] public int bombCapacity;
    [HideInInspector] public int initialBombs;

    [SerializeField] Image bombMeter;
    [HideInInspector] public int bombCount;

    void Start() {
        bombCount = initialBombs;
    }

    void Update() {
        Mathf.Clamp(bombCount, 0, bombCapacity);
        bombMeter.fillAmount = ((float)bombCount) / bombCapacity;
    }

    public void ShootInput(InputAction.CallbackContext context) {
        if (context.performed) {
            Shoot();
        }
    }

    void Shoot() {
        if (bombCount < 1) return;

        bombCount--;
        Instantiate(bombPrefab, transform.position, Quaternion.identity);
    }
}