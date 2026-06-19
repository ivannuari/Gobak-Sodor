using System.Threading;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GamePanel : MonoBehaviour
{
    public Button menuButton;

    public TMP_Text poinText;

    public GameObject winPanel;
    public GameObject losePanel;

    public GameObject notifierPanel;
    public TMP_Text notifierText;

    private void Start()
    {
        GameController.Instance.OnNotifierShowed += Instance_OnNotifierShowed;
        GameController.Instance.OnScoreUpdated += Instance_OnScoreUpdated;

        menuButton.onClick.AddListener(() =>
        {
            SceneManager.LoadSceneAsync("Menu");
        });
    }

    private void OnDestroy()
    {
        GameController.Instance.OnNotifierShowed -= Instance_OnNotifierShowed;
        GameController.Instance.OnScoreUpdated -= Instance_OnScoreUpdated;
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