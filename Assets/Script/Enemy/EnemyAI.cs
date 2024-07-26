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
            new Sequence(new List<Node> { // ��� ������
                new Condition(() => IsDead()),
                new ActionNode(() => DeathProcessing()),
            }),
            new Sequence(new List<Node> { // ���� ������
                new Condition(() => hitdetect()),
                new ActionNode(() => AttackCancellation()),
                new WaitNode(GeneralState.waitTimeBeforeAttack),
            }),
            new Condition(() => CheckAttacking()), // ������? �����

            new Sequence
            (
                new List<Node> 
                { // �̵� ������
                
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
            new Sequence(new List<Node> { // ���� ������
                new Condition(() => SkillAvailable()),
                new ActionNode(() => SkillActivation())
            }),
            new Sequence(new List<Node> { // ���� ������
                new Condition(() => detectRange(GeneralState.detectionRange)),
                new ActionNode(() => SetTargetPos(detectplayer)),
                new Condition(() => !detectRange(GeneralState.detectionRange)),
                new ActionNode(() => SetTargetPos(originPos)),
            }),
        });
    }
    private bool IsDead() // ���?
    {
        return enemystats.HP <= 0;
    }

    private void DeathProcessing() // ���ó��
    {

        Debug.Log("���ó��");
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
      // ���� ��� �ǰ� ����
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
        Gizmos.DrawWireSphere(transform.position, GeneralState.detectionRange);
        Gizmos.color = Color.red; // �ִ� ���� ������ ����
        Gizmos.DrawWireSphere(transform.position, GeneralState.attackRange);
        Gizmos.color = Color.yellow; // �ּ� ���� ������ ����
        Gizmos.DrawWireSphere(transform.position, GeneralState.attackRangeNear);
    }
}
