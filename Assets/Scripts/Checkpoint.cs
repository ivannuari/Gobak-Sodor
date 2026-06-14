using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    public Type type;

    private void OnTriggerEnter(Collider other)
    {
        //Check if player is on checkpoint
        if (other.CompareTag("Player") && type == Type.Checkpoint)
        {
            other.GetComponent<PelariController>().isCheckpoint = true;
        }

        //Check if player is on finish
        if (other.CompareTag("Player") && type == Type.Finish)
        {
            if (other.GetComponent<PelariController>().isCheckpoint)
            {
                GameManager.instance.PlaySFX(GameManager.instance.winSFX);
                other.GetComponent<PelariController>().isFinish = true;
                other.gameObject.SetActive(false);
                GameManager.instance.AddScore(10);
            }
        }
    }
}

[System.Serializable]
public enum Type
{
    Checkpoint,
    Finish
}