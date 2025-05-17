using TMPro;
using UnityEngine;

public class Score : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI playerTime;
    [SerializeField] TextMeshProUGUI hiScore;
    [SerializeField] TextMeshProUGUI dm;

    [SerializeField] Color fourthWallColour;
    Color originalColour;

    bool deathMessageShown = false;
    float elapsedTime;
    string playerScore;

    [SerializeField] AsteroidGenerator ag;
    [SerializeField] PlayerController pc;

    void Start() {
        originalColour = dm.color;
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
        dm.color = originalColour;

        if (isGargantuan) {
            dm.text = Random.value < 0.5f
                ? "It's the slowest moving target, do better!"
                : "Try shooting it twice next time!";
        }
        else if (fieldsEndured == 0) {
            dm.text = Random.value < 0.5f
                ? "Do you want to try being the pilot? Thought not."
                : "And thats one more body in the rings. Great job!";
        }
        else if (fieldsEndured == 1) {
            dm.text = Random.value < 0.5f
                ? "At least you endured the first field."
                : "After that barrage of asteroids, killed by a stray. You're a pro.";
        }
        else if (fieldsEndured <= 3) {
            dm.text = Random.value < 0.5f
                ? "Not such a bad run guide, but you could do better."
                : "You barely scratched the surface of the rings, tiger.";
        }
        else if (fieldsEndured <= 8) {
            dm.text = Random.value < 0.5f
                ? "Just a little longer and you would've reached tear-speed!"
                : "Have to be better than good to get through the rings, champion.";
        }
        else if (fieldsEndured <= 16) {
            dm.color = fourthWallColour;
            FourthWallMessages();
        }
        else if (fieldsEndured <= 31) {
            dm.color = fourthWallColour;

            dm.text = Random.value < 0.5f
                ? "This can't be right... {fieldsEndured} fields?"
                : "Don't cheer yourself on, {fieldsEndured} fields is nothing!";
        }
    }

    void FourthWallMessages() {        
        int incrementedES = PlayerPrefs.GetInt("ExtremeScore", 0) + 1;
        PlayerPrefs.SetInt("ExtremeScore", incrementedES);

        if (PlayerPrefs.GetInt("ExtremeScore", 0) > 8)
        {
            dm.text = "16 fields if halfway. You only got to {fieldsEndured}, Laughable.";
        }
        else
        {
            switch (PlayerPrefs.GetInt("ExtremeScore", 0))
            {
                case 0:
                    break;

                case 1:
                    dm.text = "Well done, you got farther than I expected! But never far enough.";
                    break;

                case 2:
                    dm.text = "You think you can escape the rings? You can't. I took away your tear capabilities!";
                    break;

                case 3:
                    dm.text = "It's the end of the line, guide, it was from the start.";
                    break;

                case 4:
                    dm.text = "'Guide', even that was fake! You're just a tool for murder!";
                    break;

                case 5:
                    dm.text = "They call this the 'Gravity Graveyard', because no one has ever passed through before.";
                    break;

                case 6:
                    dm.text = "And I gave you my hostages to be my executioner.";
                    break;
                case 7:
                    dm.text = "You'll never break out, I'll give you every last person in the universe to kill!";
                    break;
                case 8:
                    dm.text = "If you must know, you haven't even made it halfway!";
                    break;
            }
        }
    }
}