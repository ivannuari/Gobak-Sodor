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
    private Vector3 _centerPosition; // posisi awal/tengah
    private CharacterHandler _charaHandler;

    private void Start()
    {
        _anim = GetComponentInChildren<Animator>();
        _charaHandler = GetComponentInChildren<CharacterHandler>();

        _anim.SetBool("Penjaga", true);

        bool isPlayer = GameBehaviour.Instance.characterType == CharacterType.Penjaga;
        _charaHandler.isPlayer = isPlayer;

        // Simpan posisi awal sebagai titik tengah
        _centerPosition = transform.position;
    }

    public void Activate()
    {
        _outline = GetComponent<Outline>();
        _outline.enabled = true;
    }

    private void Update()
    {
        bool _isPlayer = GameBehaviour.Instance.characterType == CharacterType.Penjaga;
        bool _isAi = GameBehaviour.Instance.characterType == CharacterType.Penyerang;

        if (_isPlayer) { HandlePlayerMovement(); }
        if (_isAi) { AiMovement(); }
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
                    // Diam di tempat
                    _anim.SetFloat("Geser", 0f);
                    break;

                case Difficulties.medium:
                    // Bergerak sedikit ke arah tengah (setengah speed)
                    MoveToCenter(speed * 0.5f, fullReturn: false);
                    break;

                case Difficulties.hard:
                    // Kembali sepenuhnya ke tengah (full speed)
                    MoveToCenter(speed, fullReturn: true);
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

        float diffX = target.position.x - transform.position.x;
        if (diffX > 0)
            transform.rotation = Quaternion.Euler(0f, 90f, 0f);
        else if (diffX < 0)
            transform.rotation = Quaternion.Euler(0f, -90f, 0f);

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

    private void MoveToCenter(float moveSpeed, bool fullReturn)
    {
        float diffZ = _centerPosition.z - transform.position.z;

        // Jika sudah di tengah atau sangat dekat, diam
        if (Mathf.Abs(diffZ) <= stopDistance)
        {
            _anim.SetFloat("Geser", 0f);

            // Snap ke tengah hanya jika hard (full return)
            if (fullReturn)
            {
                transform.position = new Vector3(
                    transform.position.x,
                    transform.position.y,
                    _centerPosition.z
                );
            }
            return;
        }

        // Medium: hanya bergerak jika masih jauh dari tengah (> 1 unit)
        if (!fullReturn && Mathf.Abs(diffZ) < 1f)
        {
            _anim.SetFloat("Geser", 0f);
            return;
        }

        float dirZ = Mathf.Sign(diffZ);
        transform.position = new Vector3(
            transform.position.x,
            transform.position.y,
            transform.position.z + dirZ * moveSpeed * Time.deltaTime
        );

        _anim.SetFloat("Geser", dirZ);
    }

    private Transform GetClosestTarget(Collider[] colliders)
    {
        Transform closest = null;
        float minDist = Mathf.Infinity;

        foreach (var col in colliders)
        {
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

    private Vector3? _moveTarget; // null jika tidak ada tujuan

    public void MoveTo(Vector3 point)
    {
        // Simpan hanya nilai Z tujuan, X dan Y tidak berubah
        _moveTarget = new Vector3(transform.position.x, transform.position.y, point.z);
    }

    private void HandlePlayerMovement()
    {
        if (_moveTarget == null) return;

        float diffZ = _moveTarget.Value.z - transform.position.z;

        Collider[] _catch = Physics.OverlapSphere(transform.position, catchArea, penyerangLayer);
        if (_catch.Length > 0)
        {
            GameController.Instance.CatchOpponentPenyerang();
            return;
        }

        // Sudah sampai, berhenti
        if (Mathf.Abs(diffZ) <= stopDistance)
        {
            _moveTarget = null;
            _anim.SetFloat("Geser", 0f);
            return;
        }

        float dirZ = Mathf.Sign(diffZ);

        transform.position = new Vector3(
            transform.position.x,
            transform.position.y,
            transform.position.z + dirZ * speed * Time.deltaTime
        );

        // Rotasi berpatokan X target vs posisi sekarang
        float diffX = _moveTarget.Value.x - transform.position.x;
        if (diffX > 0)
            transform.rotation = Quaternion.Euler(0f, 90f, 0f);
        else if (diffX < 0)
            transform.rotation = Quaternion.Euler(0f, -90f, 0f);

        _anim.SetFloat("Geser", dirZ);
    }

   
}