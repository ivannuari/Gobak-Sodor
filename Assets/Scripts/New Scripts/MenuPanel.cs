using System;
using UnityEngine;
using UnityEngine.UI;

public class MenuPanel : MonoBehaviour
{
    public Button playButton;
    public Button optionButton;
    public Button quitButton;

    private CanvasManager canvasManager;

    private void Start()
    {
        SetupButton();
        canvasManager = GetComponentInParent<CanvasManager>();

        playButton.onClick.AddListener(() => canvasManager.SetSetupPanel());
        optionButton.onClick.AddListener(() => canvasManager.SetOptionPanel());

        quitButton.onClick.AddListener(() =>
        {
            PlayerPrefs.DeleteAll();
            Application.Quit();
        });
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
