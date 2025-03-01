using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ControlsHelper : MonoBehaviour
{
    [SerializeField] Image mouse;

    void Start() {
        if (PlayerPrefs.GetInt("HasPlayed", 0) == 0)
        {
            StartCoroutine(HideDelay());
        }
        else
        {
            gameObject.SetActive(false);
        }
    }

    void Update() {
        if (GameState.instance.isDead) {
            gameObject.SetActive(false);
        }
    }

    IEnumerator HideDelay() {
        yield return new WaitForSeconds(6f);
        PlayerPrefs.SetInt("HasPlayed", 1);
        gameObject.SetActive(false);
    }
}