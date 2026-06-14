using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public GameObject selectedPlayer;
    public float speed;

    [Header("Ground Settings")]
    public LayerMask groundLayer;
    public Vector3 targetPosition;

    [Header("Player Settings")]
    public LayerMask playerLayer;
    private bool isIdle;
    private bool isRun;
    private bool isRunning;

    private void Awake()
    {
        isIdle = true;
        isRun = false;
    }

    private void Update()
    {
        OnPlayerMove();
    }

    //Player movement
    private void OnPlayerMove()
    {
        if (selectedPlayer != null)
        {
            Vector3 moveTo = new Vector3(targetPosition.x, selectedPlayer.transform.position.y, targetPosition.z);
            selectedPlayer.transform.position = Vector3.MoveTowards(selectedPlayer.transform.position, moveTo, speed * Time.deltaTime);

            Animator animator = selectedPlayer.transform.GetChild(0).GetComponent<Animator>();
            if(Vector3.Distance(selectedPlayer.transform.position, moveTo) > 0.1f)
            {
                if (isRun)
                {
                    animator.Play("Run");
                    isRun = false;
                    isIdle = true;
                }
                if(selectedPlayer != null) isRunning = true;
                else isRunning = false;
            }
            else
            {
                if (isIdle)
                {
                    if (gameObject.activeInHierarchy)
                    { animator.Play("Idle"); }

                    isIdle = false;
                    isRun = true;
                }
                isRunning = false;
            }

            selectedPlayer.transform.LookAt(moveTo);
        }
        else
        {
            isRunning = false;
        }

        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit, Mathf.Infinity, playerLayer) && !isRunning)
            {
                if(selectedPlayer != null)
                {
                    selectedPlayer.GetComponent<Outline>().enabled = false;
                }
                selectedPlayer = hit.collider.gameObject;
                selectedPlayer.GetComponent<Outline>().enabled = true;
                targetPosition = hit.point;
                return;
            }
            
            if (Physics.Raycast(ray, out hit, Mathf.Infinity, groundLayer) && selectedPlayer != null)
            {
                targetPosition = hit.point;
            }
        }
    }
}
