using System.Collections;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TitlePanel : MonoBehaviour
{
    [Header("Panels & Text")]
    [SerializeField] GameObject titlePanel;
    [SerializeField] GameObject settingsPanel;
    [SerializeField] GameObject resetMessage;

    [Header("Pin Objects")]
    [SerializeField] Image[] pinObjects = new Image[3];
    [SerializeField] Sprite[] pinSprite = new Sprite[2];

    [Header("Audio")]
    [SerializeField] AudioMixer audioMixer;
    AudioSource click;

    void Start() {
        click = GetComponent<AudioSource>();

        PlayerPrefs.GetInt("StaticStart", 1);
        PlayerPrefs.GetInt("AsteroidCollisions", 1);
    }

    void Update() {
        pinObjects[0].sprite = pinSprite[PlayerPrefs.GetInt("StaticStart", 1)];
        pinObjects[1].sprite = pinSprite[PlayerPrefs.GetInt("AsteroidCollisions", 1)];
        pinObjects[2].sprite = Screen.fullScreen ? pinSprite[1] : pinSprite[0];
    }

    public void Begin() {
        SceneManager.LoadScene("Gameplay");
    }

    public void SettingsPanel() {
        click.Play();
        settingsPanel.SetActive(true);
        titlePanel.SetActive(false);
    }

    public void OnMusicSliderChange(float value) {
        audioMixer.SetFloat("MusicVol", Mathf.Log10(Mathf.Clamp(value, 0.0001f, 1f)) * 20);
    }

    public void OnSFXSliderChange(float value) {
        audioMixer.SetFloat("SFXVol", Mathf.Log10(Mathf.Clamp(value, 0.0001f, 1f)) * 20);
        click.Play();
    }

    public void ResetData() {
        click.Play();
        PlayerPrefs.DeleteKey("HighScore");
        PlayerPrefs.DeleteKey("HasPlayed");
        PlayerPrefs.DeleteKey("ExtremeScore");
        StartCoroutine(ResetMessage());
    }

    IEnumerator ResetMessage() {
        resetMessage.SetActive(true);
        yield return new WaitForSeconds(2f);
        resetMessage.SetActive(false);
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
    }

    public void StaticStart() {
        click.Play();
        if (PlayerPrefs.GetInt("StaticStart", 1) == 1) {
            PlayerPrefs.SetInt("StaticStart", 0);
        }
        else {
            PlayerPrefs.SetInt("StaticStart", 1);
        }
    }

    public void ToggleFullscreen() {
        click.Play();
        if (Screen.fullScreen) {
            Screen.fullScreen = false;
        }
        else {
            Screen.fullScreen = true;
        }
    }

    public void Back() {
        click.Play();
        settingsPanel.SetActive(false);
        titlePanel.SetActive(true);
    }

    public void Quit() {
        click.Play();
        Application.Quit();
    }
}