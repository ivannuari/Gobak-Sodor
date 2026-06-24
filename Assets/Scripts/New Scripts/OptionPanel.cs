using System;
using UnityEngine;
using UnityEngine.UI;

public class OptionPanel : MonoBehaviour
{
    public Slider bgmSlider;
    public Slider sfxSlider;

    public Button backButton;

    private CanvasManager canvasManager;

    private void Start()
    {
        canvasManager = GetComponentInParent<CanvasManager>();

        backButton.onClick.AddListener(() => canvasManager.SetMenuPanel());
        bgmSlider.onValueChanged.AddListener(ChangeBgm);
        sfxSlider.onValueChanged.AddListener(ChangeSfx);

        SetupButton();
    }

    private void SetupButton()
    {
        Button[] allButtons = GetComponentsInChildren<Button>();
        foreach (var item in allButtons)
        {
            item.onClick.AddListener(() => GameBehaviour.Instance.PlaySound("Button"));
        }
    }

    private void ChangeSfx(float val)
    {
        GameBehaviour.Instance.ChangeSfxVolume(val);
    }

    private void ChangeBgm(float val)
    {
        GameBehaviour.Instance.ChangeBgmVolume(val);
    }
}
