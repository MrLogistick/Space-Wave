using TMPro;
using UnityEngine;

public class Score : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI playerTime;
    [SerializeField] TextMeshProUGUI hiScore;
    [SerializeField] TextMeshProUGUI dm;

    bool deathMessageShown = false;
    float elapsedTime;
    string playerScore;

    [SerializeField] AsteroidGenerator ag;
    [SerializeField] PlayerController pc;

    void Start() {
        float savedScore = PlayerPrefs.GetFloat("HighScore", 0f);
        int minutes = Mathf.FloorToInt(savedScore / 60);
        int seconds = Mathf.FloorToInt(savedScore % 60);
        hiScore.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    void Update() {
        if (!GameState.instance.isDead) {
            elapsedTime += Time.deltaTime;
            int minutes = Mathf.FloorToInt(elapsedTime / 60);
            int seconds = Mathf.FloorToInt(elapsedTime % 60);
            playerScore = string.Format("{0:00}:{1:00}", minutes, seconds);
        }
        else {
            GameState.instance.timeSurvived = elapsedTime;

            if (deathMessageShown) return;
            deathMessageShown = true;

            if (pc.deathBy.Contains("Gargantuan")) {
                DisplayMessage(ag.fieldsEndured, true);
            }
            else {
                DisplayMessage(ag.fieldsEndured, false);
            }
        }

        playerTime.text = playerScore;

        if (elapsedTime > PlayerPrefs.GetFloat("HighScore", 0f)) {
            PlayerPrefs.SetFloat("HighScore", elapsedTime);
            hiScore.text = playerScore;
            PlayerPrefs.Save();
            GameState.instance.newHiscore = true;
        }
    }

    void DisplayMessage(int fieldsEndured, bool isGargantuan) {
        if (isGargantuan) {
            dm.text = Random.value < 0.5f
                ? "It's the slowest moving target, do better!"
                : "Try shooting it twice next time!";
        }
        else if (fieldsEndured == 0) {
            dm.text = Random.value < 0.5f
                ? "You didn't even get to the good part yet!"
                : "You just killed an innocent pilot. Hope you feel good.";
        }
        else if (fieldsEndured == 1) {
            dm.text = Random.value < 0.5f
                ? "That was a toughy."
                : "Guess you could only handle the first field.";
        }
        else if (fieldsEndured <= 3) {
            dm.text = Random.value < 0.5f
                ? "You were great! Until the pilot died..."
                : "There's only so many pilots in the universe you know.";
        }
        else if (fieldsEndured <= 8) {
            dm.text = Random.value < 0.5f
                ? "An above average guide is nothing to complain about!"
                : "Do you want to try being a pilot? Thought not.";
        }
        else if (fieldsEndured <= 16) {
            dm.text = Random.value < 0.5f
                ? "Wow, you nearly reached tear-speed."
                : "Wait, this can't be right.";
        }
        else
        {
            FourthWallMessages();
        }
    }

    private void FourthWallMessages()
    {
        int incrementedES = PlayerPrefs.GetInt("ExtremeScore", 0) + 1;
        PlayerPrefs.SetInt("ExtremeScore", incrementedES);

        if (PlayerPrefs.GetInt("ExtremeScore", 0) > 6)
        {
            dm.text = "Another one dead. This is exciting.";
        }
        else
        {
            switch (PlayerPrefs.GetInt("ExtremeScore", 0))
            {
                case 0:
                    break;

                case 1:
                    dm.text = "HAHAHAHA, I set you up!";
                    break;

                case 2:
                    dm.text = "I rigged your ship so it could never reach tear-speed!!";
                    break;

                case 3:
                    dm.text = "You're just going to endlessly send pilots to their deaths!!!";
                    break;

                case 4:
                    dm.text = "It was all a fabrication, no one has ever passed through the 'Gravity Graveyard' before!!!!";
                    break;

                case 5:
                    dm.text = "Guides aren't real, you're just a tool for murder!!!!!";
                    break;

                case 6:
                    dm.text = "You'll never make it to the other side of the Twin-World Ring!!!!!!";
                    break;
            }
        }
    }
}