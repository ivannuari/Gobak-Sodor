using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PelariController : MonoBehaviour
{
    [Header("Progession")]
    public bool isCheckpoint;
    public bool isFinish;

    private void OnCollisionEnter(Collision collision)
    {
        //Check if player is catched
        if (collision.gameObject.CompareTag("Enemy"))
        {
            GameManager.instance.PlaySFX(GameManager.instance.loseSFX);
            Destroy(gameObject, 0.1f);
        }
    }
}
