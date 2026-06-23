using System;
using UnityEngine;

public class CanvasManager : MonoBehaviour
{
    public GameObject[] allPanels;

    private void Start()
    {
        SetMenuPanel();
        GameBehaviour.Instance.PlaySound("BGM");
    }

    public void SetMenuPanel()
    {
        DisableAllPanel();
        allPanels[0].SetActive(true);
    }

    public void SetSetupPanel()
    {
        DisableAllPanel();
        allPanels[1].SetActive(true);
    }

    public void SetDifficultiesPanel()
    {
        DisableAllPanel();
        allPanels[2].SetActive(true);
    }

    public void SetOptionPanel()
    {
        DisableAllPanel();
        allPanels[3].SetActive(true);
    }

    private void DisableAllPanel()
    {
        foreach (var panel in allPanels) 
        {
            panel.SetActive(false);
        }
    }
}
