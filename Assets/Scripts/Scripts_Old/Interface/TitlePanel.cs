using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TitlePanel : MonoBehaviour
{
    [Header("Panels & Text")]
    [SerializeField] GameObject titlePanel;
    [SerializeField] GameObject shipsPanel;
    [SerializeField] GameObject settingsPanel;
    [SerializeField] GameObject resetMessage;

    [Header("Sprites")]
    [SerializeField] Image[] pinObjects = new Image[4];
    [SerializeField] Sprite[] pinSprite = new Sprite[2];
    [SerializeField] Image shipIcon;
    [SerializeField] Sprite[] ships = new Sprite[4];
    [SerializeField] TextMeshProUGUI shipDisplayName;

    [Header("Audio")]
    [SerializeField] AudioMixer audioMixer;
    AudioSource click;
    [SerializeField] Slider musicSlider;
    [SerializeField] Slider sfxSlider;
    [SerializeField] public bool sfxChanging;
    [SerializeField] float soundInterval;
    float timeElapsed;

    int currentShipIndex;
    ShipManager manager;

    void Start() {
        click = GetComponent<AudioSource>();
        manager = ShipManager.instance;

        PlayerPrefs.GetInt("StaticStart", 1);
        PlayerPrefs.GetInt("AsteroidCollisions", 1);

        currentShipIndex = manager.unlockedShips.IndexOf(manager.currentShip);

        musicSlider.value = PlayerPrefs.GetFloat("MusicVol");
        sfxSlider.value = PlayerPrefs.GetFloat("SFXVol");
    }

    void Update() {
        pinObjects[0].sprite = pinSprite[PlayerPrefs.GetInt("AsteroidCollisions", 1)];
        pinObjects[1].sprite = pinSprite[PlayerPrefs.GetInt("StaticStart", 1)];
        pinObjects[2].sprite = pinSprite[PlayerPrefs.GetInt("TutorialEnabled", 1)];
        pinObjects[3].sprite = Screen.fullScreen ? pinSprite[1] : pinSprite[0];

        audioMixer.SetFloat("MusicVol", PlayerPrefs.GetFloat("MusicVol"));
        audioMixer.SetFloat("SFXVol", PlayerPrefs.GetFloat("SFXVol"));

        shipIcon.sprite = ships[currentShipIndex];
        RectTransform rt = shipIcon.rectTransform;

        if (currentShipIndex == 3) {
            rt.sizeDelta = new Vector2(200f, 200f);
        } else {
            rt.sizeDelta = new Vector2(100f, 100f);
        }

        if (sfxChanging) {
            if (timeElapsed > 0f) {
                timeElapsed -= Time.deltaTime;
            } else {
                timeElapsed = soundInterval;
                click.Play();
            }
        }

        switch (manager.unlockedShips[currentShipIndex]) {
            case "Default":
                shipDisplayName.text = "Ares";
                break;
            case "Winged":
                shipDisplayName.text = "Hermes";
                break;
            case "Round":
                shipDisplayName.text = "Artemis";
                break;
            case "Giant":
                shipDisplayName.text = "Zeus";
                break;
            case "Generator":
                shipDisplayName.text = "Hera";
                break;
            case "Armoured":
                shipDisplayName.text = "Posiden";
                break;
            case "Pacifist":
                shipDisplayName.text = "Aphrodite";
                break;
            case "Twin":
                shipDisplayName.text = "Dionysis";
                break;
            case "Consumer":
                shipDisplayName.text = "Hephaestus";
                break;
        }
    }

    public void ShipsPanel() {
        click.Play();
        shipsPanel.SetActive(true);
        titlePanel.SetActive(false);
    }

    public void SettingsPanel() {
        click.Play();
        settingsPanel.SetActive(true);
        titlePanel.SetActive(false);
    }

    public void Back() {
        click.Play();
        shipsPanel.SetActive(false);
        settingsPanel.SetActive(false);
        titlePanel.SetActive(true);
    }

    public void Begin() {
        SceneManager.LoadScene("Gameplay");
    }

    public void Previous() {
        if (currentShipIndex > 0) {
            currentShipIndex--;
        } else {
            currentShipIndex = manager.unlockedShips.Count - 1;
        }
        manager.currentShip = manager.unlockedShips[currentShipIndex];
    }

    public void Next() {
        if (currentShipIndex < manager.unlockedShips.Count - 1) {
            currentShipIndex++;
        } else {
            currentShipIndex = 0;
        }
        manager.currentShip = manager.unlockedShips[currentShipIndex];
    }

    public void OnMusicSliderChange(float value) {
        PlayerPrefs.SetFloat("MusicVol", Mathf.Log10(Mathf.Clamp(value, 0.0001f, 1f)) * 20);
        PlayerPrefs.Save();
    }

    public void OnSFXSliderChange(float value) {
        PlayerPrefs.SetFloat("SFXVol", Mathf.Log10(Mathf.Clamp(value, 0.0001f, 1f)) * 20);
        PlayerPrefs.Save();
        sfxChanging = true;
    }

    public void ResetData() {
        click.Play();
        PlayerPrefs.DeleteKey("HighScore");
        PlayerPrefs.DeleteKey("Attempts");
        PlayerPrefs.DeleteKey("UnlockedShips");
        PlayerPrefs.DeleteKey("CurrentShip");
        StartCoroutine(ResetMessage());
    }

    IEnumerator ResetMessage() {
        resetMessage.SetActive(true);
        yield return new WaitForSeconds(2f);
        resetMessage.SetActive(false);
    }

    public void ToggleTutorial() {
        click.Play();
        if (PlayerPrefs.GetInt("TutorialEnabled", 1) == 1) {
            PlayerPrefs.SetInt("TutorialEnabled", 0);
        }
        else {
            PlayerPrefs.SetInt("TutorialEnabled", 1);
        }
        PlayerPrefs.Save();
    }

    public void ToggleAsteroidCollisions()
    {
        click.Play();
        if (PlayerPrefs.GetInt("AsteroidCollisions", 1) == 1) {
            PlayerPrefs.SetInt("AsteroidCollisions", 0);
        }
        else {
            PlayerPrefs.SetInt("AsteroidCollisions", 1);
        }
        PlayerPrefs.Save();
    }

    public void StaticStart() {
        click.Play();
        if (PlayerPrefs.GetInt("StaticStart", 1) == 1) {
            PlayerPrefs.SetInt("StaticStart", 0);
        }
        else {
            PlayerPrefs.SetInt("StaticStart", 1);
        }
        PlayerPrefs.Save();
    }

    public void ToggleFullscreen() {
        click.Play();
        if (Screen.fullScreenMode == FullScreenMode.FullScreenWindow) {
            Screen.fullScreenMode = FullScreenMode.Windowed;
        }
        else {
            Screen.fullScreenMode = FullScreenMode.FullScreenWindow;
        }
    }

    public void Quit() {
        click.Play();
        Application.Quit();
    }
}