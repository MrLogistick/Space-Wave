using System.Collections;
using UnityEngine;

public class ControlsHelper : MonoBehaviour
{
    void Start() {
        if (PlayerPrefs.GetInt("TutorialEnabled", 0) == 0) {
            gameObject.SetActive(false);
        }
    }

    void Update() {
        if (GameState.instance.isDead) {
            gameObject.SetActive(false);
        }
    }

    public void Hide() {
        PlayerPrefs.SetInt("TutorialEnabled", 0);
        gameObject.SetActive(false);
    }
}