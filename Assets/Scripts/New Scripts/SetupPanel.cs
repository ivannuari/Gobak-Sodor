using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SetupPanel : MonoBehaviour
{
    public TMP_Text characterTypeText;
    public TMP_Text characterColorText;

    public Button changeCharacterTypeButton;
    public Button changeCharacterColorButton;
    public Button playButton;
    public Button backButton;

    private bool isPenyerang = true;

    private CanvasManager canvasManager;

    private void Start()
    {
        canvasManager = GetComponentInParent<CanvasManager>();

        changeCharacterTypeButton.onClick.AddListener(ChangeCharacterType);
        changeCharacterColorButton.onClick.AddListener(ChangeCharacterColor);

        playButton.onClick.AddListener(()=> canvasManager.SetDifficultiesPanel());
        backButton.onClick.AddListener(()=> canvasManager.SetMenuPanel());

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

    private void ChangeCharacterType()
    {
        isPenyerang = !isPenyerang;
        if (isPenyerang) 
        {
            characterTypeText.text = "PENYERANG";
            GameBehaviour.Instance.characterType = CharacterType.Penyerang;
        }
        else
        {
            characterTypeText.text = "PENJAGA";
            GameBehaviour.Instance.characterType = CharacterType.Penjaga;
        }
    }

    private void ChangeCharacterColor()
    {
        Color randColor = UnityEngine.Random.ColorHSV();

        GameBehaviour.Instance.ChangeJerseyColor(randColor);
    }
}
