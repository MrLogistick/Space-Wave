using UnityEngine;

public class BrokenShipRandomizer : MonoBehaviour
{
    [SerializeField] private Sprite[] brokenShipPrefabs;

    void Start() {
        GetComponent<SpriteRenderer>().sprite = brokenShipPrefabs[Random.Range(0, brokenShipPrefabs.Length)];
    }
}