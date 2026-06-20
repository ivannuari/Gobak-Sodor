using System;
using UnityEngine;

public class Penjaga : MonoBehaviour
{
    public float speed;
    public float defendArea;
    public float catchArea;

    public float stopDistance = 0.1f;
    public bool isDefending = false;

    public LayerMask penyerangLayer;

    private Outline _outline;
    private Animator _anim;

    private void Start()
    {
        _outline = GetComponent<Outline>();
        _anim = GetComponentInChildren<Animator>();
        _anim.SetBool("Penjaga", true);
    }

    private void Update()
    {
        bool _isPlayer = GameBehaviour.Instance.characterType == CharacterType.Penjaga;
        bool _isAi = GameBehaviour.Instance.characterType == CharacterType.Penyerang;

        if (_isPlayer) { }

        if (_isAi)
        {
            AiMovement();
        }
    }

    private void AiMovement()
    {
        if (!isDefending)
        {
            _anim.SetFloat("Geser", 0f);
            return;
        }

        Collider[] hits = Physics.OverlapSphere(transform.position, defendArea, penyerangLayer);
        if (hits.Length == 0)
        {
            switch (GameBehaviour.Instance.currentDifficulties)
            {
                case Difficulties.easy:
                    _anim.SetFloat("Geser", 0f);
                    break;
                case Difficulties.medium:
                    break;
                case Difficulties.hard:
                    break;
            }
            return;
        }

        Collider[] _catch = Physics.OverlapSphere(transform.position, catchArea, penyerangLayer);
        if (_catch.Length > 0)
        {
            GameController.Instance.CatchPenyerang();
            return;
        }

        Transform target = GetClosestTarget(hits);
        if (target == null) return;

        // Rotasi berpatokan sumbu X (kanan/kiri)
        float diffX = target.position.x - transform.position.x;
        if (diffX > 0)
        {
            transform.rotation = Quaternion.Euler(0f, 90f, 0f);   // hadap kanan
        }
        else if (diffX < 0)
        {
            transform.rotation = Quaternion.Euler(0f, -90f, 0f); // hadap kiri
        }

        // Pergerakan berpatokan sumbu Z
        float diffZ = target.position.z - transform.position.z;

        if (Mathf.Abs(diffZ) <= stopDistance)
        {
            _anim.SetFloat("Geser", 0f);
            return;
        }

        float dirZ = Mathf.Sign(diffZ);

        transform.position = new Vector3(
            transform.position.x,
            transform.position.y,
            transform.position.z + dirZ * speed * Time.deltaTime
        );

        _anim.SetFloat("Geser", dirZ);
    }


    private Transform GetClosestTarget(Collider[] colliders)
    {
        Transform closest = null;
        float minDist = Mathf.Infinity;

        foreach (var col in colliders)
        {
            // Jarak dihitung hanya dari selisih Z
            float dist = Mathf.Abs(col.transform.position.z - transform.position.z);
            if (dist < minDist)
            {
                minDist = dist;
                closest = col.transform;
            }
        }

        return closest;
    }

    private void FixedUpdate()
    {
        isDefending = Physics.CheckSphere(transform.position, defendArea, penyerangLayer);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, defendArea);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, catchArea);
    }
}