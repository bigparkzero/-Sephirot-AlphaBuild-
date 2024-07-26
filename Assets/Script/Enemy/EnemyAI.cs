using UnityEngine;
using System.Collections.Generic;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    public EnemyGeneralState GeneralState;

    private Vector3 detectplayer;
    private Vector3 originPos;
    private List<EnemySkill> AvailableSkills;

    private Collider playerCollider;
    private Node _behaviorTree;
    private NavMeshAgent navMeshAgent;
    private Stats enemystats;
    private Animator an;

   public List<EnemySkill> skills = new List<EnemySkill>();
    private void AnimatorSetUp()
    { 
        for (int i = 0; i < skills.Count; i++) 
        {
            
        }
        originPos = transform.position;
    }

    void Start()
    {
        _behaviorTree = InitializeBehaviorTree();
        navMeshAgent = GetComponent<NavMeshAgent>();
        enemystats= GetComponent<Stats>();
        an = GetComponent<Animator>();

    }
    
    void Update()
    {
        for (int i = 0; i < skills.Count; i++)
        {
            skills[i].TimeUpdate();
        }
        _behaviorTree.Tick();
    }

    private Node InitializeBehaviorTree()
    {
        return new Selector(new List<Node> {
            new Sequence(new List<Node> { // 사망 시퀀스
                new Condition(() => IsDead()),
                new ActionNode(() => DeathProcessing()),
            }),
            new Sequence(new List<Node> { // 경직 시퀀스
                new Condition(() => hitdetect()),
                new ActionNode(() => AttackCancellation()),
                new WaitNode(GeneralState.waitTimeBeforeAttack),
            }),
            new Condition(() => CheckAttacking()), // 공격중? 컨디션

            new Sequence
            (
                new List<Node> 
                { // 이동 시퀀스
                
                    new Condition(() => SkillAvailable()), 
                    new Selector(new List<Node>{
                        new Sequence(new List<Node>
                        {
                            new Condition(() => detectRange(GeneralState.attackRange)),
                            new ActionNode(() => CombatMove()),
                        }),
                        new Sequence(new List<Node>{
                            new Condition(() => detectRange(GeneralState.detectionRange)),
                            new ActionNode(() => SetTargetPos(detectplayer)),
                        }),
                    }),
                }
            ),
            new Sequence(new List<Node> { // 공격 시퀀스
                new Condition(() => SkillAvailable()),
                new ActionNode(() => SkillActivation())
            }),
            new Sequence(new List<Node> { // 감지 시퀀스
                new Condition(() => detectRange(GeneralState.detectionRange)),
                new ActionNode(() => SetTargetPos(detectplayer)),
                new Condition(() => !detectRange(GeneralState.detectionRange)),
                new ActionNode(() => SetTargetPos(originPos)),
            }),
        });
    }
    private bool IsDead() // 사망?
    {
        return enemystats.HP <= 0;
    }

    private void DeathProcessing() // 사망처리
    {

        Debug.Log("사망처리");
    }

    private bool hitdetect()
    {
        return false;
    }
    private bool detectRange(float range)
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, range, GeneralState.playerLayer);

        for (int i = 0; hitColliders.Length > i; i++) 
        {
            detectplayer = hitColliders[i].transform.position;
            return true;
        }
        detectplayer = originPos;
        
        return false;
    }
    private void AttackCancellation()
    { 
      // 공격 취소 피격 판정
    }
    private bool CheckAttacking()
    {
        return false;
    }
    private void SetTargetPos(Vector3 target)
    {
        navMeshAgent.SetDestination(target);
        Debug.Log("SetTargetPos " + target);
        navMeshAgent.updateRotation = true;
    }
    private void CombatMove()
    {
        navMeshAgent.updateRotation = false;

    }

    private bool SkillAvailable()
    {
        for (int i = 0; i < skills.Count; i++)
        {
            if (skills[i].cooltimeTimer_debug > skills[i].coolTime)
            {
                AvailableSkills.Add(skills[i]);
            }
        }
        if (AvailableSkills.Count <= 0)
        {
            AvailableSkills.Clear();
            return false;
        }
        else
        {
            AvailableSkills.Clear();
            return true;
        }
    }
    private void SkillActivation()
    {
        if (AvailableSkills.Count <= 1)
        {
            AvailableSkills[0].UseSkill();
        }
        else
        {
            AvailableSkills[0].UseSkill();
        }
        Debug.Log("wwa;iufw;uwaf");
    }
    private void BattleStart()
    {

    }

    /*
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
    */
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue; // 감지 범위의 색상
        Gizmos.DrawWireSphere(transform.position, GeneralState.detectionRange);
        Gizmos.color = Color.red; // 최대 공격 범위의 색상
        Gizmos.DrawWireSphere(transform.position, GeneralState.attackRange);
        Gizmos.color = Color.yellow; // 최소 공격 범위의 색상
        Gizmos.DrawWireSphere(transform.position, GeneralState.attackRangeNear);
    }
}
