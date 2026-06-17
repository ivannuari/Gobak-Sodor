using UnityEngine;

public class EnvironmentHandler : MonoBehaviour
{
    [SerializeField] private GameObject[] allLevels;

    private void Start()
    {
        var diff = GameBehaviour.Instance.currentDifficulties;

        foreach (var item in allLevels)
        {
            item.SetActive(false);
        }

        switch (diff)
        {
            case Difficulties.easy:
                allLevels[0].SetActive(true);
                break;
            case Difficulties.medium:
                allLevels[1].SetActive(true);
                break;
            case Difficulties.hard:
                allLevels[2].SetActive(true);
                break;
            default:
                allLevels[0].SetActive(true);
                break;
        }
    }
}
