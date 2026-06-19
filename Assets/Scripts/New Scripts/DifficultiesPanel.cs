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
    }
}
