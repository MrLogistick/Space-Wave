using UnityEngine;

public class GameState : MonoBehaviour
{
    public bool isDead = false;
    public float timeSurvived;
    public bool newHiscore = false;
    public float slowDown;

    [SerializeField] GameObject postGamePanel;

    public static GameState instance;

    void Awake() {
        if (instance == null) {
            instance = this;
        }
    }

    void Update() {
        if (!isDead) return;

        postGamePanel.SetActive(true);
        
        if (slowDown > 0f) {
            slowDown -= 0.5f * Time.deltaTime;
        }
        else {
            slowDown = 0f;
        }
    }
}