using System.Collections.Generic;
using UnityEngine;

public class PersistantManager : MonoBehaviour {
    public List<ShipData> allShips = new List<ShipData>();
    public Dictionary<ShipData, bool> unlockedShips = new Dictionary<ShipData, bool>();
    public ShipData currentShip;

    public static PersistantManager instance;

    void Awake() {
        if (instance != null && instance != this) {
            Destroy(gameObject);
            return;
        }

        instance = this;
    }

    void Start() {
        foreach (var ship in allShips) {
            unlockedShips.Add(ship, false);
        }

        unlockedShips[allShips[0]] = true;
        currentShip = allShips[0];
    }

    void UnlockShip(ShipData ship) {
        if (unlockedShips.ContainsKey(ship)) {
            unlockedShips[ship] = true;
        }
    }
}