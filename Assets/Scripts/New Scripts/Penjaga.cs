using System;
using UnityEngine;

public class Penjaga : MonoBehaviour
{
    public float speed;
    public float defendArea;
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
            _anim.SetFloat("Geser", 0f); // idle
            return;
        }

        Collider[] hits = Physics.OverlapSphere(transform.position, defendArea, penyerangLayer);
        if (hits.Length == 0)
        {
            _anim.SetFloat("Geser", 0f); // idle
            return;
        }

        Transform target = GetClosestTarget(hits);
        if (target == null) return;

        float diffZ = target.position.z - transform.position.z;

        if (Mathf.Abs(diffZ) <= stopDistance)
        {
            _anim.SetFloat("Geser", 0f); // sudah di posisi target, idle
            return;
        }

        float dirZ = Mathf.Sign(diffZ); // otomatis -1, 0, atau 1

        transform.position = new Vector3(
            transform.position.x,
            transform.position.y,
            transform.position.z + dirZ * speed * Time.deltaTime
        );

        _anim.SetFloat("Geser", dirZ); // -1 = kiri, 1 = kanan
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
        Gizmos.DrawWireSphere(transform.position, defendArea);
    }
}