using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MusicManager : MonoBehaviour
{
    float speedUp = 1f;
    AudioSource music;
    [SerializeField] AudioClip[] backgroundMusic;

    IEnumerator Start() {
        music = GetComponent<AudioSource>();

        while (true) {
            for (int i = 0; i < backgroundMusic.Length; i++) {
                music.Play();
                yield return new WaitWhile(() => music.isPlaying);
                yield return new WaitForSeconds(1f);
                music.clip = backgroundMusic[i];
            }
        }
    }

    void Update() {
        if (SceneManager.GetActiveScene().name == "Gameplay") {
            if (GameState.instance.isDead) {
                music.pitch = GameState.instance.slowDown;
                speedUp = 0;
            } else {
                music.pitch = speedUp;
                SpeedUpMusic();
            }
        } else {
            SpeedUpMusic();
            music.pitch = speedUp;
        }
    }

    void SpeedUpMusic() {
        if (speedUp < 1) {
            speedUp += Time.deltaTime;
        } else {
            speedUp = 1;
        }
    }
}