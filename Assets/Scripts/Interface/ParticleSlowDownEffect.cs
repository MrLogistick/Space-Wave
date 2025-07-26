using UnityEngine;
using UnityEngine.SceneManagement;

public class ParticleSlowDownEffect : MonoBehaviour
{
    void Update() {
        string currentScene = SceneManager.GetActiveScene().name;
        var main = GetComponent<ParticleSystem>().main;

        if (currentScene == "Gameplay") {
            main.simulationSpeed = GameState.instance.slowDown;
        }
        else {
            main.simulationSpeed = 1f;
        }
    }
}