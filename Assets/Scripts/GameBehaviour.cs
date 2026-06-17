using UnityEngine;

public class GameBehaviour : MonoBehaviour
{
    public static GameBehaviour Instance;

    public Difficulties currentDifficulties = Difficulties.easy;

    private string difficultiesKey = "difficulty";
    private string unlockLevelKey = "unlockLevel";

    private void Awake()
    {
        if (Instance == null) 
        {
            Instance = this;
        }

        DontDestroyOnLoad(gameObject);
    }

    public void SetDifficulties(Difficulties newDifficulties)
    {
        switch (newDifficulties)
        {
            case Difficulties.easy:
                PlayerPrefs.SetInt(difficultiesKey, 0);
                break;
            case Difficulties.medium:
                PlayerPrefs.SetInt(difficultiesKey, 1);
                break;
            case Difficulties.hard:
                PlayerPrefs.SetInt(difficultiesKey, 2);
                break;
        }
    }

    public void UnlockLevel(int level)
    {
        int currentLevel = GetCurrentLevel();
        Debug.LogWarning($"CHECK Level: {level}");
        if(level > currentLevel)
        {
            currentLevel = level;
            PlayerPrefs.SetInt(unlockLevelKey, currentLevel);
        }
    }

    public int GetCurrentLevel()
    {
        return PlayerPrefs.GetInt(unlockLevelKey, 0);
    }

    private void OnApplicationQuit()
    {
        PlayerPrefs.DeleteAll();
    }
}

public enum Difficulties
{
    easy,
    medium,
    hard
}
