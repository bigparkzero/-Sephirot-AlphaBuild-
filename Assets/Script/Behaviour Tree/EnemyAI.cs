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
            // 플레이어와 적의 위치를 기반으로 뒤로 물러나는 방향 계산
            Vector3 directionToPlayer = (transform.position - playerCollider.transform.position).normalized;
            Vector3 retreatPosition = transform.position + directionToPlayer * 0.5f;

            // 뒤로 이동할 위치로 NavMeshAgent의 목적지 설정
            navMeshAgent.SetDestination(retreatPosition);
            navMeshAgent.updateRotation = false;
        }
    }
    private bool PlayerInAttackRangeNear()
    {
        if (playerCollider == null)
        {
            return false; // 플레이어가 없는 경우
        }

        // 플레이어가 공격 범위 근처에 있는지 확인
        float distanceToPlayer = Vector3.Distance(transform.position, playerCollider.transform.position);
        return distanceToPlayer <= attackRangeNear;
    }
    private bool PlayerInDetectionRange()
    {
        // 실제 플레이어 감지 범위 확인 로직 구현
        // 감지 범위의 중심 위치 (적의 위치)

        // 감지 범위 내에 플레이어가 있는지 확인
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, detectionRange, playerLayer);

        // 감지된 플레이어가 있을 경우 첫 번째 플레이어의 Collider를 저장
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
            // 플레이어의 위치로 NavMeshAgent의 목적지 설정
            navMeshAgent.SetDestination(playerCollider.transform.position);
            navMeshAgent.updateRotation = true;
        }
    }
   
    private bool PlayerInAttackRange()
    {
        if (playerCollider == null)
        {
            return false; // 플레이어가 없는 경우
        }

        // 현재 적의 위치와 플레이어의 위치를 기반으로 감지 범위 내에 있는지 확인
        float distanceToPlayer = Vector3.Distance(transform.position, playerCollider.transform.position);
        return distanceToPlayer <= attackRange;
    }
    
    private void AttackPlayer()
    {
        // 플레이어 공격 로직 (예: 애니메이션 재생, 피해 입히기 등)
        Debug.Log("Attacking Player!");
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue; // 감지 범위의 색상
        Gizmos.DrawWireSphere(transform.position, detectionRange);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, attackRangeNear);
    }
}
