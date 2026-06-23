using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DifficultiesPanel : MonoBehaviour
{
    public Button easyButton;
    public Button mediumButton;
    public Button hardButton;
    public Button backButton;

    private CanvasManager canvasManager;

    private void Start()
    {
        canvasManager = GetComponentInParent<CanvasManager>();

        easyButton.onClick.AddListener(() =>
        {
            GameBehaviour.Instance.SetDifficulties(Difficulties.easy);
            SceneManager.LoadSceneAsync("Gameplay");
        });
        mediumButton.onClick.AddListener(() =>
        {
            GameBehaviour.Instance.SetDifficulties(Difficulties.medium);
            SceneManager.LoadSceneAsync("Gameplay");
        });
        hardButton.onClick.AddListener(() =>
        {
            GameBehaviour.Instance.SetDifficulties(Difficulties.hard);
            SceneManager.LoadSceneAsync("Gameplay");
        });

        backButton.onClick.AddListener(() => canvasManager.SetSetupPanel());
        SetupButton();

        LockLevel(GameBehaviour.Instance.GetCurrentLevel());
    }

    private void LockLevel(int level)
    {
        if(level == 0)
        {
            easyButton.interactable = true;
            mediumButton.interactable = false;
            hardButton.interactable = false;
        }

        if (level == 1)
        {
            easyButton.interactable = true;
            mediumButton.interactable = true;
            hardButton.interactable = false;
        }

        if (level > 1)
        {
            easyButton.interactable = true;
            mediumButton.interactable = true;
            hardButton.interactable = true;
        }
    }

    private void SetupButton()
    {
        Button[] allButtons = GetComponentsInChildren<Button>();
        foreach (var item in allButtons)
        {
            item.onClick.AddListener(() => GameBehaviour.Instance.PlaySound("Button"));
        }
    }
}
