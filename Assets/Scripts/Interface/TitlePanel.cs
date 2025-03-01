using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TitlePanel : MonoBehaviour
{
    [SerializeField] GameObject titlePanel;
    [SerializeField] GameObject settingsPanel;
    [SerializeField] Slider volumeSlider;
    [SerializeField] GameObject resetMessage;
    [SerializeField] Image ssPin;
    [SerializeField] Image acPin;
    [SerializeField] Sprite[] PinSprite = new Sprite[2];
    [SerializeField] AudioSource click;

    void Start() {
        PlayerPrefs.GetInt("StaticStart", 1);
        volumeSlider.value = PlayerPrefs.GetFloat("Volume", 1f);
    }

    void Update() {
        PlayerPrefs.SetFloat("Volume", volumeSlider.value);
        GameObject.Find("PersistantObjects").GetComponent<AudioSource>().volume = volumeSlider.value;
        
        ssPin.sprite = PinSprite[PlayerPrefs.GetInt("StaticStart", 1)];
        acPin.sprite = PinSprite[PlayerPrefs.GetInt("AsteroidCollisions", 1)];
    }

    public void Begin() {
        SceneManager.LoadScene("Gameplay");
    }

    public void SettingsPanel() {
        click.Play();
        settingsPanel.SetActive(true);
        titlePanel.SetActive(false);
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
        if (PlayerPrefs.GetInt("AsteroidCollisions", 1) == 1)
        {
            PlayerPrefs.SetInt("AsteroidCollisions", 0);
        }
        else
        {
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