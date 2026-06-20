using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ResultPanel : MonoBehaviour
{
    public TMP_Text resultText;
    public TMP_Text poinText;
    public Button homeButton;

    private void Start()
    {
        homeButton.onClick.AddListener(() => SceneManager.LoadSceneAsync("Menu"));
    }

    private void OnEnable()
    {
        int poin = GameController.Instance.GetPoin();
        poinText.text = $"Poin = {poin}";
        resultText.text = poin > 10 ? "Menang" : "Kalah";
    }
}
