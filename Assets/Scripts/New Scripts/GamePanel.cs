using System.Threading;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GamePanel : MonoBehaviour
{
    public Button menuButton;

    public TMP_Text poinText;

    public ResultPanel resultPanel;
    public GameObject losePanel;

    public GameObject notifierPanel;
    public TMP_Text notifierText;

    private void Start()
    {
        GameController.Instance.OnNotifierShowed += Instance_OnNotifierShowed;
        GameController.Instance.OnScoreUpdated += Instance_OnScoreUpdated;
        GameController.Instance.OnGameOver += Instance_OnGameOver;

        menuButton.onClick.AddListener(() =>
        {
            SceneManager.LoadSceneAsync("Menu");
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

    private void OnDestroy()
    {
        GameController.Instance.OnNotifierShowed -= Instance_OnNotifierShowed;
        GameController.Instance.OnScoreUpdated -= Instance_OnScoreUpdated;
        GameController.Instance.OnGameOver -= Instance_OnGameOver;
    }

    private void Instance_OnGameOver()
    {
        resultPanel.gameObject.SetActive(true);
    }

    private async void Instance_OnNotifierShowed(string message)
    {
        notifierText.text = message;
        notifierPanel.SetActive(true);

        await Awaitable.WaitForSecondsAsync(2f, CancellationToken.None);
        notifierPanel.SetActive(false);
    }

    private void Instance_OnScoreUpdated(int poin)
    {
        poinText.text = $"Poin : {poin}";
    }
}