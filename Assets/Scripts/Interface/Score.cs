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
        if (fieldsEndured == 0) {
            dm.text = Random.value < 0.5f
                ? (isGargantuan ? "Did you see that one coming?" : "You didn't even get to the good part yet!")
                : (isGargantuan ? "No way you didn't see that MEGAroid coming." : "You just killed an innocent pilot. Hope you feel good.");
        }
        else if (fieldsEndured == 1) {
            dm.text = Random.value < 0.5f
                ? (isGargantuan ? "Sometimes you can't help but hit it I guess." : "That was a toughy.")
                : (isGargantuan ? "By now you should expect it." : "Guess you could only handle the first field.");
        }
        else if (fieldsEndured <= 3) {
            dm.text = Random.value < 0.5f
                ? (isGargantuan ? "most casualties come from the megaroids, don't feel bad." : "You were great! Until the pilot died...")
                : (isGargantuan ? "Another Megaroid casualty, great." : "There's only so many pilots in the universe you know.");
        }
        else if (fieldsEndured <= 8) {
            dm.text = Random.value < 0.5f
                ? (isGargantuan ? "If only this wasn't the only path to get through this system." : "An above average guide is nothing to complain about!")
                : (isGargantuan ? "They don't call this the 'Gravity Graveyard' for nothing." : "Do you want to try being a pilot? Thought not.");
        }
        else if (fieldsEndured <= 16) {
            dm.text = Random.value < 0.5f
                ? (isGargantuan ? "Woah, if only those pesky Megaroids weren't there." : "Wow, you nearly reached tear-speed.")
                : (isGargantuan ? "Lucky the Megaroid came. Wait, you weren't meant to hear that..." : "Wait, this can't be right.");
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