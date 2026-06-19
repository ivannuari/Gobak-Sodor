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
        canvasManager = GetComponentInParent<CanvasManager>();

        playButton.onClick.AddListener(() => canvasManager.SetSetupPanel());
        optionButton.onClick.AddListener(() => canvasManager.SetOptionPanel());

        quitButton.onClick.AddListener(() =>
        {
            PlayerPrefs.DeleteAll();
            Application.Quit();
        });
    }
}
