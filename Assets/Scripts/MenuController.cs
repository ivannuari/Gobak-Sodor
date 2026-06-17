using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuController : MonoBehaviour
{
    public Button playButton;
    public Button exitButton;

    public Button easyButton;
    public Button mediumButton;
    public Button hardButton;

    // panel yang berisi tombol EASY / MEDIUM / HARD
    public GameObject levelPanel;

    private void Awake()
    {
        // pastikan panel level disembunyikan di awal
        if (levelPanel != null)
        {
            levelPanel.SetActive(false);
        }

        // PLAY: sekarang buka panel level, BUKAN langsung load gameplay
        playButton.onClick.RemoveAllListeners();
        playButton.onClick.AddListener(() =>
        {
            if (levelPanel != null)
            {
                CheckLevelUnlocked();
                levelPanel.SetActive(true);
            }
        });

        // EXIT: tetap sama
        exitButton.onClick.RemoveAllListeners();
        exitButton.onClick.AddListener(() =>
        {
            PlayerPrefs.DeleteAll();
            Application.Quit();
        });

        // EASY: simpan difficulty dan langsung ke Gameplay
        easyButton.onClick.RemoveAllListeners();
        easyButton.onClick.AddListener(() =>
        {
            GameBehaviour.Instance.SetDifficulties(Difficulties.easy);
            SceneManager.LoadScene("Gameplay");
        });

        // MEDIUM
        mediumButton.onClick.RemoveAllListeners();
        mediumButton.onClick.AddListener(() =>
        {
            GameBehaviour.Instance.SetDifficulties(Difficulties.medium);
            SceneManager.LoadScene("Gameplay");
        });

        // HARD
        hardButton.onClick.RemoveAllListeners();
        hardButton.onClick.AddListener(() =>
        {
            GameBehaviour.Instance.SetDifficulties(Difficulties.hard);
            SceneManager.LoadScene("Gameplay");
        });

        CheckLevelUnlocked();
    }

    private void CheckLevelUnlocked()
    {
        int currentLevel = GameBehaviour.Instance.GetCurrentLevel();
        Debug.Log(currentLevel);

        if(currentLevel == 0)
        {
            easyButton.interactable = true;
            mediumButton.interactable = false;
            hardButton.interactable = false;
        }

        if (currentLevel == 1)
        {
            easyButton.interactable = true;
            mediumButton.interactable = true;
            hardButton.interactable = false;
        }

        if (currentLevel == 2)
        {
            easyButton.interactable = true;
            mediumButton.interactable = true;
            hardButton.interactable = true;
        }
    }
}
