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
    }

    private void ChangeSfx(float val)
    {

    }

    private void ChangeBgm(float val)
    {

    }
}
