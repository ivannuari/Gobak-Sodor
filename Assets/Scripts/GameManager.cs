using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public int score;
    
    public GameObject[] targets;
    public GameObject winPanel;
    public GameObject losePanel;
    public TextMeshProUGUI winText;
    public TextMeshProUGUI loseText;
    public TextMeshProUGUI[] scoreText;
    public Button[] menuButton;

    public AudioSource sfx;
    public AudioClip winSFX;
    public AudioClip loseSFX;

    private bool isCalculated = false;

    public static GameManager instance;

    private void Awake()
    {
        instance = this;
        score = 0;

        for (int i = 0; i < menuButton.Length; i++)
        {
            menuButton[i].onClick.RemoveAllListeners();
            menuButton[i].onClick.AddListener(delegate
            {
                SceneManager.LoadScene("Menu");
            });
        }
    }

    private void Update()
    {
        CheckLose();
        CheckWin();

        foreach (TextMeshProUGUI text in scoreText)
        {
            text.text = $"Score : {score}";
        }
    }

    //Check if all players are destroyed
    private void CheckLose()
    {
        bool allMissing = true;

        foreach (GameObject target in targets)
        {
            if (target != null)
            {
                allMissing = false;
                break;
            }
        }

        if (allMissing)
        {
            losePanel.SetActive(true);
            winPanel.SetActive(false);

            loseText.text = $"Kamu Kalah!";
        }
    }

    //Check if all player has been finished
    private void CheckWin()
    {
        bool allDestroyed = true;

        foreach (GameObject target in targets)
        {
            if (target != null)
            {
                if (target.activeSelf)
                {
                    allDestroyed = false;
                    break;
                }
            }
        }

        if (allDestroyed)
        {
            winPanel.SetActive(true);
            losePanel.SetActive(false);

            if (!isCalculated)
            {
                isCalculated = true;
                if (score >= targets.Length * 10)
                {
                    int currentLevel = GameBehaviour.Instance.GetCurrentLevel();
                    currentLevel++;

                    GameBehaviour.Instance.UnlockLevel(currentLevel);
                    winText.text = $"Kamu Berhasil!";
                }
                else if (score < targets.Length * 10 && score > 0)
                {
                    winText.text = $"Coba Lagi Ya!";
                }
                else
                {
                    winText.text = $"Kamu Kalah!";
                }
            }
        }
    }

    //Add score
    public void AddScore(int value)
    {
        score += value;
    }

    //Play SFX
    public void PlaySFX(AudioClip clip)
    {
        sfx.clip = clip;
        sfx.Stop();
        sfx.Play();
    }
}
