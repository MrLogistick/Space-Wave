using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PostgamePanel : MonoBehaviour
{
    [SerializeField] GameObject newScore;
    bool blinkComplete = true;

    void Update() {
        if (GameState.instance.isDead && GameState.instance.timeSurvived >= PlayerPrefs.GetFloat("HighScore", 0f)) {
            StartCoroutine(NewHighscore());
        }
    }

    public void Restart() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void Menu() {
        SceneManager.LoadScene("Title");
    }

    IEnumerator NewHighscore()
    {
        if (blinkComplete == false) yield break;
        blinkComplete = false;
        
        newScore.SetActive(true);
        yield return new WaitForSeconds(1f);

        newScore.SetActive(false);
        yield return new WaitForSeconds(1f);

        blinkComplete = true;
    }
}