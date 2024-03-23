using System;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent), typeof(EnemyManager))]
public class EnemyBase : MonoBehaviour
{
    protected enum EnemyState
    {
        Chase, Patrol
    }

    [SerializeField] protected float baseSpeed = 2.0f;
    protected float speed;
    [SerializeField] float turnSpeed = 120;
    [SerializeField] float widthOfView = 0.5f;
    [SerializeField] protected float sightDist = 1.5f;
    [SerializeField] protected float attackDist = 0.2f;
    [SerializeField] protected GameObject patrolPath;
    [SerializeField] protected float outOfSightThreshold = 3;

    public Transform target;
    protected NavMeshAgent agent;
    protected Animator anim;
    protected Transform player;
    protected event Action OnPlayerSpotted;
    protected event Action OnPlayerOutOfRange;
    public bool isAttacking;
    protected float moveInputModifier = 1;
    protected EnemyState currentState;

    protected Transform[] patrolNodes;
    protected int nodeIndex = 0;
    protected float distThreshol = 0.3f;

    private float outOfSightTimer = 0;


    // Start is called before the first frame update
    protected virtual void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponentInChildren<Animator>();
        player = GameObject.FindGameObjectWithTag("Player").transform;

        if (!agent) Debug.Log("No NavMeshAgent attached");
        if (!anim) Debug.Log("No Animator on enemy model");
        if (!player) Debug.Log("No Player in Scene (or untagged)");

        patrolNodes = GetNodes(patrolPath);
        target = patrolNodes[0];
        //target = player.transform;
        currentState = EnemyState.Patrol;
        agent.SetDestination(target.position);
        moveInputModifier = 0.3f;
        anim.SetFloat("Speed", moveInputModifier);

        speed = moveInputModifier * baseSpeed;
        agent.speed = speed;
        agent.angularSpeed = turnSpeed;
        //agent.stoppingDistance = attackDist;
    }

    protected virtual void FixedUpdate()
    {
        if (currentState == EnemyState.Patrol)
            SetPatrolNode();
        if (currentState == EnemyState.Chase)
            SetPlayerTarget();

        agent.SetDestination(target.position);

        PlayerCheck();
        PlayerOutOfRangeCheck();
    }

    private void PlayerCheck()
    {
        RaycastHit hitInfo;
        Vector3 pos = transform.position;
        Vector3 halfExtents = transform.localScale * widthOfView;
        Vector3 fwd = transform.forward;
        Quaternion rot = transform.rotation;

        if (Physics.BoxCast(pos, halfExtents, fwd, out hitInfo, rot, sightDist))
        {
            if (hitInfo.collider.CompareTag("Player"))
                OnPlayerSpotted?.Invoke();
        }
    }

    private void PlayerOutOfRangeCheck()
    {
        if (currentState == EnemyState.Chase)
        {
            if (agent.remainingDistance >= sightDist)
            {
                outOfSightTimer += Time.deltaTime;
                if (outOfSightTimer >= outOfSightThreshold)
                {
                    target = patrolNodes[nodeIndex];
                    ChangeToPatrol();
                }   
            }
        }
    }

    protected virtual void FacePlayer()
    {
        Vector3 targetPos = new Vector3(player.position.x, 0, player.position.z);
        Vector3 targetDir = (targetPos - new Vector3(transform.position.x, 0, transform.position.z)).normalized;

        float singleStep = turnSpeed * Time.deltaTime;
        Vector3 turnDir = Vector3.RotateTowards(transform.forward, targetDir, singleStep, 0.0f);

        transform.rotation = Quaternion.LookRotation(turnDir);
    }

    protected virtual void ChangeToChase()
    {
        if (currentState != EnemyState.Chase)
        {
            Debug.Log("Chasing Player");
            currentState = EnemyState.Chase;
            moveInputModifier = 1;
            anim.SetFloat("Speed", moveInputModifier);
            speed = moveInputModifier * baseSpeed;
            agent.speed = speed;
        }
    }

    protected virtual void ChangeToPatrol()
    {
        if (currentState != EnemyState.Patrol)
        {
            currentState = EnemyState.Patrol;
            moveInputModifier = 0.3f;
            anim.SetFloat("Speed", moveInputModifier);
            speed = moveInputModifier * baseSpeed;
            agent.speed = speed;
        }
    }

    protected virtual void Attack()
    {
        Debug.Log("Attack not Implemented");
    }

    protected virtual void StopAttack()
    {
        Debug.Log("StopAttack not Implemented");
    }

    protected void SetPatrolNode()
    {
        if (target)
            Debug.DrawLine(transform.position, target.position, Color.red);
        if (agent.remainingDistance <= distThreshol)
        {
            nodeIndex++;
            nodeIndex %= patrolNodes.Length;
            target = patrolNodes[nodeIndex];
        }
    }

    protected void SetPlayerTarget()
    {
        if (target.CompareTag("PatrolNode"))
        {
            target = player;
        }
    }

    private Transform[] GetNodes(GameObject go)
    {
        return go.GetComponentsInChildren<Transform>();
    }
}
