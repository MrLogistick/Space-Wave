using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MusicManager : MonoBehaviour
{
    float speedUp = 1;
    AudioSource music;

    [SerializeField] AudioClip[] backgroundMusic;

    IEnumerator Start() {
        music = GetComponent<AudioSource>();

        while (true) {
            for (int i = 0; i < backgroundMusic.Length; i++)
            {
                music.clip = backgroundMusic[i];
                music.Play();
                yield return new WaitForSeconds(music.clip.length + 1f);
            }
        }
    }

    void Update()
    {
        if (SceneManager.GetActiveScene().name == "Gameplay")
        {
            if (GameState.instance.isDead)
            {
                music.pitch = GameState.instance.slowDown;
                speedUp = 0;
            }
            else
            {
                music.pitch = speedUp;
                SpeedUpMusic();
            }
        }
        else
        {
            SpeedUpMusic();
            music.pitch = speedUp;
        }
    }

    private void SpeedUpMusic()
    {
        if (speedUp < 1)
        {
            speedUp += Time.deltaTime;
        }
        else
        {
            speedUp = 1;
        }
    }
}