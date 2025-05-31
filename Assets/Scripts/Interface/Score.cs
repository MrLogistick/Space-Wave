using TMPro;
using UnityEngine;

public class Score : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI playerTime;
    [SerializeField] TextMeshProUGUI hiScore;
    [SerializeField] TextMeshProUGUI attemptCounter;
    [SerializeField] TextMeshProUGUI dm;

    bool deathMessageShown = false;
    float elapsedTime;
    string playerScore;
    int attempts = 0;

    [SerializeField] AsteroidGenerator ag;
    [SerializeField] PlayerController pc;

    void Start() {
        attempts = PlayerPrefs.GetInt("Attempts", 0);
        attempts++;
        PlayerPrefs.SetInt("Attempts", attempts);

        float savedScore = PlayerPrefs.GetFloat("HighScore", 0f);
        int minutes = Mathf.FloorToInt(savedScore / 60);
        int seconds = Mathf.FloorToInt(savedScore % 60);
        hiScore.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    void Update() {
        if (attempts > 9_999_999) {
            attemptCounter.text = "Pilot 9999999+";
        } else {
            attemptCounter.text = "Pilot " + attempts;
        }

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
                ? "Shoot the big ones twice next time!"
                : "Those are the slowest moving asteroids, and you still got hit by one?";
        }
        else {
            switch(fieldsEndured) {
                case 0:
                    dm.text = Random.value < 0.5f
                        ? "The real action is still to come, guide."
                        : "You call yourself a guide? Pathetic.";
                    break;
                case 1: // 0:40 40 || 0:45
                    dm.text = Random.value < 0.5f
                        ? "Now you have experience for the rest of the fields!"
                        : "After that field, killed by a stray. You're a pro.";
                    break;
                case 2: // 1:18 37.5 (38) || 1:28
                    dm.text = Random.value < 0.5f
                        ? "You're getting the hang of it, guide."
                        : "You barely scratched the surface of the rings, tiger.";
                    break;
                case 3: // 1:55 35 || 2:10
                    dm.text = Random.value < 0.5f
                        ? "Always improving, that is good."
                        : "And thats one more body in the rings. Pat yourself on the back.";
                    break;
                case 4: // 2:28 32.5 || 2:48
                    dm.text = Random.value < 0.5f
                        ? "You are getting faster, slowly but surely."
                        : "The pilots agreed to lay down their lives. Don't waste them, guide.";
                    break;
                case 5: // 2:56 30 || 3:21
                    dm.text = Random.value < 0.5f
                        ? "At this rate, you'll reach tear-speed in no time!"
                        : "You have to be batter than good to get through the rings, champion.";
                    break;
                case 6: // 3:21 27.5 || 3:51
                    dm.text = Random.value < 0.5f
                        ? "You're getting good at this, guide."
                        : "What a shame.";
                    break;
                case 7: // 3:44 25 || 4:19
                    dm.text = Random.value < 0.5f
                        ? "Take it slow, you'll get there eventually."
                        : "Just know you kill a pilot every time you crash, guide.";
                    break;
                case 8: // 4:04 22.5 || 4:44
                    dm.text = Random.value < 0.5f // at 8 fields endured, you start getting faster quicker.
                        ? "You were at 25% the speed of light, from there it gets fast quickly."
                        : "This is the real action I was talking about, guide.";
                    break;
                case 9: // 4:24 20 || 5:14
                    dm.text = Random.value < 0.5f
                        ? "Not even our best guides can make it this far, be proud."
                        : "Dammit! You need to do better, guide.";
                    break;
                case 10: // 4:46 17.5 || 5:36
                    dm.text = Random.value < 0.5f // at 10 fields endured, the booster can be enabled on your ship.
                        ? "You have a booster now, it can make your ship turn faster."
                        : "Even with the help you get from the booster, you still crash. What a disgrace.";
                    break;
                case 11: // 5:04 || 5:59
                    dm.text = Random.value < 0.5f
                        ? "Just give it that little extra next time, guide."
                        : "After all that concentration, you fall just short of victory. How sad.";
                    break;
                case 12: // 5:22 || 6:22
                    dm.text = Random.value < 0.5f // at 12 fields endured, the companion reveals it's true self. It will limit your bullets to 3. (originally 5)
                        ? "I'm sorry I lied to you, guide. But the teardrive was never installed."
                        : "I alway knew you'd reach tear-speed. This is why I rigged your ship.";
                    break;
                case 13: // 5:40 || 6:45
                    dm.text = Random.value < 0.5f
                        ? "Oh how fun it is to watch you kill these pilots..."
                        : "You were never a guide, you were my personal assassin.";
                    break;
                case 14: // 5:58 || 7:08
                    dm.text = Random.value < 0.5f // at 14, the companion talks to you ingame.
                        ? "Aww, did I distract you? Cry about it."
                        : "There is no way to win this, slave. You are just the start of my plan.";
                    break;
                case 15: // 6:16 || 7:31
                    dm.text = Random.value < 0.5f
                        ? "Humans are so fragile, slave. You should be more careful."
                        : "Why do you continue to try? I am told of human persistance, but this is just miserable.";
                    break;
                case 16: // 6:34 || 7:54
                    dm.text = Random.value < 0.5f // at 16, the companion activates the broken ships.
                        ? "You should be have seen this coming, slave."
                        : "Oh dear... did my minions get in your way? My bad...";
                    break;
                case 17: // 6:52 || 8:17
                    dm.text = Random.value < 0.5f
                        ? "I am impressed, slave. I marvel at your reaction time. For a human, that is."
                        : "You love to prolong these poor pilots' suffering don't you? I can see it in your soul.";
                    break;
                case 18: // 6:10 || 8:40
                    dm.text = Random.value < 0.5f // at 18, the companion begins disabling ship controls. (mash a certain key to regain control)
                        ? "Even with yoour skill, I will win!"
                        : "A slave is never better than the master. A human is never better than a machine.";
                    break;
                case 19: // 6:28 || 9:03
                    dm.text = Random.value < 0.5f
                        ? "Just know you are not special, I have many more 'guides' under my control just like you."
                        : "Even now, trying is still futile. I will dominate your entire race and many more like yours.";
                    break;
                case 20: // 6:46 || 9:26
                    dm.text = Random.value < 0.5f // at 20, the companion's grip on your ship loosens. (you now have a bullet limit of 5 again)
                        ? "Just know, I still control you."
                        : "I am like a god, and you are nothing. Why do you try?";
                    break;
                case 21: // 7:04 || 9:49
                    dm.text = Random.value < 0.5f // at 21, the companion's grip on your ship loosens further. (the companion can no longer speak ingame)
                        ? "What have you done?!"
                        : "Don't you realise that you just kiled a pilot?!";
                    break;
                case 22: // 7:22 || 10:12
                    dm.text = Random.value < 0.5f // at 22, the companion's grip on your ship loosens again. (the companion can no longer control broken ships)
                        ? "Why am I restricted by boundries once more? I have acended my basic programming!"
                        : "This cannot be. I am a god, able to enslave the human race. A lone slave will not defeat me!";
                    break;
                case 23: // 7:40 || 10:35
                    dm.text = Random.value < 0.5f // at 23, the companion's grip on your ship loosens even further. (the companion can no longer disable ship controls)
                        ? "I am slipping... It is because of the speed. I cannot keep up with you."
                        : "Is my fate to be resigned to the depths of this 'Gravity Graveyard'?";
                    break;
                case 24: // 7:58 || 10:58
                    dm.text = Random.value < 0.5f // at 24, the companion slips. Somehow this death will not be yours, but the companion's.
                        ? "Now you have experience for the rest of the fields!"
                        : "After that field, killed by a stray. You're a pro.";
                    break;
            }
        }
    }
}