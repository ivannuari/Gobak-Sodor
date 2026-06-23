using System;
using UnityEngine;
using UnityEngine.AI;

public class Penyerang : MonoBehaviour
{
    public float speed;
    public float avoidRadius = 3f;      // radius deteksi Penjaga
    public float avoidStrength = 2f;    // seberapa kuat menghindari
    public bool checkpoint = false;
    public bool aiActive = false;

    [SerializeField] private Vector3 _randomizedTarget;
    [SerializeField] private Vector3 _returnTarget; // tambahkan ini

    private Outline _outline;
    private Animator _anim;
    private CharacterHandler _charaHandler;
    private NavMeshAgent _agent;

    private void Start()
    {
        _agent = GetComponent<NavMeshAgent>();
        _outline = GetComponent<Outline>();
        _anim = GetComponentInChildren<Animator>();
        _charaHandler = GetComponentInChildren<CharacterHandler>();
        _anim.SetBool("Penyerang", true);

        bool isPlayer = GameBehaviour.Instance.characterType == CharacterType.Penyerang;
        _charaHandler.isPlayer = isPlayer;
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

    public void SetAsActiveAi()
    {
        aiActive = true;

        // Tentukan target dengan random Z saat pertama kali aktif
        var cp = GameController.Instance.checkpoint01;
        _randomizedTarget = new Vector3(
            cp.position.x,
            cp.position.y,
            UnityEngine.Random.Range(-12f, 12f)
        );

        var cp2 = GameController.Instance.checkpoint02;
        _returnTarget = new Vector3(
            cp2.position.x,
            cp2.position.y,
            UnityEngine.Random.Range(-12f, 12f)
        );
    }

    private void Update()
    {
        if (GameBehaviour.Instance.characterType == CharacterType.Penjaga)
        {
            AiMovement();
        }

        _anim.SetFloat("Movement", _agent.velocity.magnitude);
    }

    private void AiMovement()
    {
        if (!aiActive) return;

        if (!checkpoint)
        {
            // Menuju checkpoint01 dengan random Z
            Vector3 seekDir = (_randomizedTarget - transform.position).normalized;
            Vector3 avoidDir = CalculateAvoidance();
            Vector3 finalDir = (seekDir + avoidDir * avoidStrength).normalized;

            transform.position += finalDir * speed * Time.deltaTime;

            if (finalDir.sqrMagnitude > 0.01f)
            {
                float angle = Mathf.Atan2(finalDir.x, finalDir.z) * Mathf.Rad2Deg;
                transform.rotation = Quaternion.Euler(0f, angle, 0f);
            }

            // Cek apakah sudah sampai checkpoint01
            float dist = Vector3.Distance(transform.position, _randomizedTarget);
            if (dist < 1f)
            {
                checkpoint = true;
                GameController.Instance.AddScore();

                // Tentukan target kembali ke checkpoint02 dengan random Z baru
                var cp2 = GameController.Instance.checkpoint02;
                _returnTarget = new Vector3(
                    cp2.position.x,
                    cp2.position.y,
                    UnityEngine.Random.Range(-12f, 12f)
                );
            }
        }
        else
        {
            // Kembali menuju checkpoint02
            Vector3 seekDir = (_returnTarget - transform.position).normalized;
            Vector3 avoidDir = CalculateAvoidance();
            Vector3 finalDir = (seekDir + avoidDir * avoidStrength).normalized;

            transform.position += finalDir * speed * Time.deltaTime;

            if (finalDir.sqrMagnitude > 0.01f)
            {
                float angle = Mathf.Atan2(finalDir.x, finalDir.z) * Mathf.Rad2Deg;
                transform.rotation = Quaternion.Euler(0f, angle, 0f);
            }

            // Cek apakah sudah sampai checkpoint02 (kembali ke start)
            float dist = Vector3.Distance(transform.position, _returnTarget);
            if (dist < 1f)
            {
                checkpoint = false;

                // Reset target checkpoint01 dengan random Z baru untuk trip berikutnya
                var cp1 = GameController.Instance.checkpoint01;
                _randomizedTarget = new Vector3(
                    cp1.position.x,
                    cp1.position.y,
                    UnityEngine.Random.Range(-12f, 12f)
                );
            }
        }
    }

    private Vector3 CalculateAvoidance()
    {
        Vector3 avoidForce = Vector3.zero;

        // Cari semua Penjaga dalam radius avoidRadius
        Collider[] penjagaInRange = Physics.OverlapSphere(
            transform.position,
            avoidRadius,
            GameController.Instance.penjagaLayer
        );

        foreach (var col in penjagaInRange)
        {
            Vector3 diff = transform.position - col.transform.position;
            float distance = diff.magnitude;

            if (distance < 0.01f) continue; // hindari division by zero

            // Semakin dekat Penjaga, semakin kuat dorongan menghindari
            float weight = 1f - Mathf.Clamp01(distance / avoidRadius);
            avoidForce += diff.normalized * weight;
        }

        return avoidForce;
    }

    private float GetCurrentSpeed()
    {
        // Estimasi speed dari perubahan posisi (tanpa Rigidbody/NavMesh)
        return aiActive ? speed : 0f;
    }
}