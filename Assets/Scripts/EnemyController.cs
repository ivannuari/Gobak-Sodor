using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public float radiusTarget = 10f;
    
    public Transform selectedTarget;
    public Transform[] targets;
    public float speed;
    public bool isVertical;

    private bool isIdle;
    private bool isRun;
    private Vector3 moveTo;
    private Vector3 moveFrom;

    public float minX;
    public float maxX;

    private void Awake()
    {
        isIdle = true;
        isRun = false;

        var diff = GameBehaviour.Instance.currentDifficulties;

        switch (diff)
        {
            case Difficulties.easy:
                speed = 2.5f;
                radiusTarget = 7f;
                break;
            case Difficulties.medium:
                speed = 5f;
                radiusTarget = 10f;
                break;
            case Difficulties.hard:
                speed = 7f;
                radiusTarget = 12.5f;
                break;
        }

        //Init start position
        moveFrom = transform.position;
    }

    private void Update()
    {
        selectedTarget = OnSelectTarget();

        OnMoveToTarget();

        //Check if enemy is out of bounds
        if (selectedTarget != null)
        {
            float distance = Vector3.Distance(transform.position, selectedTarget.position);
            if (distance > radiusTarget)
            {
                Animator animator = transform.GetChild(0).GetComponent<Animator>();
                animator.Play("Idle");
                isIdle = false;
                isRun = true;

                selectedTarget = null;
            }
        }
    }

    //Move to target
    private void OnMoveToTarget()
    {
        if (selectedTarget != null)
        {
            if (isVertical)
            {
                moveTo = new Vector3(transform.position.x, transform.position.y, selectedTarget.position.z);
            }
            else
            {
                moveTo = new Vector3(selectedTarget.position.x, transform.position.y, transform.position.z);
            }
            if (transform.position.x > minX && transform.position.x < maxX) transform.position = Vector3.MoveTowards(transform.position, moveTo, speed * Time.deltaTime);

            if (Vector3.Distance(transform.position, moveTo) > 0.01f)
            {
                PlayRun();
            }
            else
            {
                PlayIdle();
            }

            transform.LookAt(selectedTarget);
        }
        else
        {
            PlayIdle();
        }
    }

    //Select target
    private Transform OnSelectTarget()
    {
        Transform target = null;
        float minDistance = Mathf.Infinity;
        foreach (Transform t in targets)
        {
            if (t != null)
            {
                float distance = Vector3.Distance(transform.position, t.position);
                if (distance < 10f)
                {
                    if (distance < minDistance)
                    {
                        minDistance = distance;
                        target = t;
                    }
                }
            }   
        }
        return target;
    }

    //Play idle animation
    private void PlayIdle()
    {
        if (isIdle)
        {
            Animator animator = transform.GetChild(0).GetComponent<Animator>();
            animator.Play("Idle");
            isIdle = false;
            isRun = true;
        }
    }

    //Play run animation
    private void PlayRun()
    {
        if (isRun)
        {
            Animator animator = transform.GetChild(0).GetComponent<Animator>();
            animator.Play("Run");
            isRun = false;
            isIdle = true;
        }
    }
}
