using System;
using UnityEngine;
using UnityEngine.AI;

public class Penyerang : MonoBehaviour
{
    public float speed;

    public bool checkpoint = false;
    
    private Outline _outline;
    private NavMeshAgent _agent;
    private Animator _anim;

    private void Start()
    {
        _outline = GetComponent<Outline>();
        _agent = GetComponent<NavMeshAgent>();
        _anim = GetComponentInChildren<Animator>();

        _anim.SetBool("Penyerang", true);
        _agent.speed = speed;
    }

    public void Activate()
    {
        _outline.enabled = true;
    }

    public void MoveTo(Vector3 targetPosition)
    {
        _agent.SetDestination(targetPosition);
    }

    private void Update()
    {
        _anim.SetFloat("Movement",_agent.velocity.magnitude);
    }
}