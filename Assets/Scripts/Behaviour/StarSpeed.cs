using UnityEngine;

public class StarSpeed : MonoBehaviour
{
    [SerializeField] AsteroidGenerator generator;

    void Update() {
        var main = GetComponent<ParticleSystem>().main;
        main.simulationSpeed = generator._asteroidSpeed * 0.1f * (
            GameState.instance.isDead ? GameState.instance.slowDown : 1f
        );
    }
}