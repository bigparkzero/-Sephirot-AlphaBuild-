// EnemyAI.cs
using UnityEngine;
using System.Collections.Generic;
using System;
using UnityEngine.AI;
using UnityEditor.Experimental.GraphView;
using System.Collections;

public class EnemyAI : MonoBehaviour
{
    public LayerMask playerLayer;
    public float detectionRange;
    public float attackRange;
    public float attackRangeNear;
    public float waitTimeBeforeAttack;

    private Collider playerCollider;
    private Node _behaviorTree;
    private NavMeshAgent navMeshAgent;
    private Stats enemystats;

   public List<EnemySkill> skills = new List<EnemySkill>();


    void Start()
    {
        _behaviorTree = InitializeBehaviorTree();
        navMeshAgent = GetComponent<NavMeshAgent>();
        enemystats= GetComponent<Stats>();
    }

    void Update()
    {
        _behaviorTree.Tick();
    }

    private Node InitializeBehaviorTree()
    {
        return new Selector(new List<Node> {
            new Sequence(new List<Node> { // ��� ������
                new Condition(() => IsDead()),
                new ActionNode(() => DeathProcessing()),
            }),
            new Sequence(new List<Node> { // ���� ������
                new Condition(() => PlayerInAttackRange()),
                new ActionNode(() => AttackCancellation()),
                new WaitNode(waitTimeBeforeAttack),
            }),
            new Condition(() => CheckAttacking()), // ������? �����

            new Sequence(new List<Node> { // �̵� ������
                new Condition(() => AttackRangeDetection() && SkillAvailable()), 
                new Selector(new List<Node>{
                    new Sequence(new List<Node>
                    {
                        new Condition(() => !AttackRangeDetection()),
                        new ActionNode(() => Chasing()),
                    }),
                    new Sequence(new List<Node>
                    {
                        new Condition(() => AttackRangeDetection()),
                        new ActionNode(() => CombatMove()),
                    })
                }),
            }),
            new Sequence(new List<Node> { // ���� ������
                new Condition(() => SkillAvailable()),
                new ActionNode(() => SkillActivation())
            }),
            new Sequence(new List<Node> { // ���� ������
                new Condition(() => BattleTrigger()),
                new ActionNode(() => BattleStart()),
            }),
        });
    }
    private bool IsDead() // ���?
    {
        return enemystats.HP <= 0;
    }

    private void DeathProcessing() // ���ó��
    { 
    //��� ���� ����
    }
    private bool PlayerInAttackRange()
    { 
    return true;
    }
    private void AttackCancellation()
    { 
      // ���� ��� �ǰ� ����
    }
    private bool CheckAttacking()
    {
        return true;
    }

    private void Chasing()
    { 
    }
    private void CombatMove()
    { 
    }

    private bool SkillAvailable()
    {
        return true;
    }
    private void SkillActivation()
    { 
    }
    private bool AttackRangeDetection()
    {
        return true;
    }

    private bool BattleTrigger()
    {
        return true;
    }
    private void BattleStart()
    { 
    }

    /*
    private void Retreat()
    {
        if (playerCollider != null)
        {
            // �÷��̾�� ���� ��ġ�� ������� �ڷ� �������� ���� ���
            Vector3 directionToPlayer = (transform.position - playerCollider.transform.position).normalized;
            Vector3 retreatPosition = transform.position + directionToPlayer * 0.5f;

            // �ڷ� �̵��� ��ġ�� NavMeshAgent�� ������ ����
            navMeshAgent.SetDestination(retreatPosition);
            navMeshAgent.updateRotation = false;
        }
    }
    
    private bool PlayerInAttackRangeNear()
    {
        if (playerCollider == null)
        {
            return false; // �÷��̾ ���� ���
        }

        // �÷��̾ ���� ���� ��ó�� �ִ��� Ȯ��
        float distanceToPlayer = Vector3.Distance(transform.position, playerCollider.transform.position);
        return distanceToPlayer <= attackRangeNear;
    }
    private bool PlayerInDetectionRange()
    {
        // ���� �÷��̾� ���� ���� Ȯ�� ���� ����
        // ���� ������ �߽� ��ġ (���� ��ġ)

        // ���� ���� ���� �÷��̾ �ִ��� Ȯ��
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, detectionRange, playerLayer);

        // ������ �÷��̾ ���� ��� ù ��° �÷��̾��� Collider�� ����
        if (hitColliders.Length > 0)
        {
            playerCollider = hitColliders[0];
            return true;
        }
        else
        {
            playerCollider = null;
            return false;
        }
    }
    private void ChasePlayer()
    {
        if (playerCollider != null)
        {
            // �÷��̾��� ��ġ�� NavMeshAgent�� ������ ����
            navMeshAgent.SetDestination(playerCollider.transform.position);
            navMeshAgent.updateRotation = true;
        }
    }
    private bool PlayerInAttackRange()
    {
        if (playerCollider == null)
        {
            return false; // �÷��̾ ���� ���
        }

        // ���� ���� ��ġ�� �÷��̾��� ��ġ�� ������� ���� ���� ���� �ִ��� Ȯ��
        float distanceToPlayer = Vector3.Distance(transform.position, playerCollider.transform.position);
        return distanceToPlayer <= attackRange;
    }
    private void AttackPlayer()
    {
        // �÷��̾� ���� ���� (��: �ִϸ��̼� ���, ���� ������ ��)
        Debug.Log("Attacking Player!");
    }
    */
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue; // ���� ������ ����
        Gizmos.DrawWireSphere(transform.position, detectionRange);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, attackRangeNear);
    }
}
