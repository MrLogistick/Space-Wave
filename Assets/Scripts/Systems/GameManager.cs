using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] GameObject postGamePanel;
    [HideInInspector] public bool isDead;
    float gameSpeed;

    public static GameManager instance;

    void Update() {
        if (!isDead) return;

        postGamePanel.SetActive(true);
        if (gameSpeed > 0) {
            gameSpeed -= Time.deltaTime;
        } else {
            gameSpeed = 0;
        }
    }
}