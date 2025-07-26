using System.Collections.Generic;
using UnityEngine;

public class ShipManager : MonoBehaviour
{
    public string currentShip;
    public List<string> unlockedShips;

    public static ShipManager instance;

    void Awake() {
        if (instance != null && instance != this) {
            Destroy(gameObject);
            return;
        }

        instance = this;
        currentShip = PlayerPrefs.GetString("CurrentShip", "Default");
        unlockedShips = LoadPrefs();
    }

    void Update() {
        if (Input.GetKeyDown(KeyCode.Alpha2)) {
            string ship = "Winged";
            if (!unlockedShips.Contains(ship))
            {
                unlockedShips.Add(ship);
            }
        }
        if (Input.GetKeyDown(KeyCode.Alpha3)) {
            string ship = "Round";
            if (!unlockedShips.Contains(ship))
            {
                unlockedShips.Add(ship);
            }
        }
        if (Input.GetKeyDown(KeyCode.Alpha4)) {
            string ship = "Giant";
            if (!unlockedShips.Contains(ship))
            {
                unlockedShips.Add(ship);
            }
        }

        PlayerPrefs.SetString("CurrentShip", currentShip);
        SavePrefs(unlockedShips);
    }

    void SavePrefs(List<string> list) {
        string joined = string.Join(",", list);
        PlayerPrefs.SetString("UnlockedShips", joined);
        PlayerPrefs.Save();
    }

    List<string> LoadPrefs() {
        string data = PlayerPrefs.GetString("UnlockedShips", "Default");
        List<string> result = new List<string>(data.Split(","));
        return result;
    }
}