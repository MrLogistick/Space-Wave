using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;

public class BombController : MonoBehaviour
{
    [SerializeField] int bombCapacity = 5;
    [SerializeField] int startingBombs = 1;
    public int bombCount;
    [SerializeField] GameObject bombPrefab;
    [SerializeField] Image bombMeter;

    void Start() {
        bombCount = startingBombs;
    }

    void Update() {
        if (bombCount > bombCapacity) {
            bombCount = bombCapacity;
        }
        bombMeter.fillAmount = (float)bombCount / 5;
    }

    public void ShootBomb() {
        if (bombCount < 1) return;

        bombCount--;
        Instantiate(bombPrefab, transform.position, Quaternion.identity);
    }
}