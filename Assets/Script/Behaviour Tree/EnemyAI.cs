// EnemyAI.cs
using UnityEngine;
using System.Collections.Generic;
using System;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    public LayerMask playerLayer;
    public float detectionRange;
    public float attackRange;
    public float attackRangeNear;

    private Collider playerCollider;
    private Node _behaviorTree;
    private NavMeshAgent navMeshAgent;

    void Start()
    {
        _behaviorTree = InitializeBehaviorTree();
        navMeshAgent = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        _behaviorTree.Tick();
    }

    private Node InitializeBehaviorTree()
    {
        return new Selector(new List<Node> {
            new Sequence(new List<Node> {
                new Condition(() => PlayerInAttackRangeNear()),
                new ActionNode(() => Retreat()),
            }),
            new Sequence(new List<Node> {
                new Condition(() => PlayerInAttackRange()),
                new ActionNode(() => AttackPlayer()),
            }),
            new Sequence(new List<Node> {
                new Condition(() => PlayerInDetectionRange()),
                new ActionNode(() => ChasePlayer()),
            }),
        });
    }
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
