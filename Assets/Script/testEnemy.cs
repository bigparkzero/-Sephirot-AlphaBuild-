using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class testEnemy : MonoBehaviour
{
    public float detectionRange;
    public float attackRange;
    public float attackRangeNear;
    public LayerMask playerLayer;
    public Vector3 detectplayer;
    public float rotationSmoothSpeed;

    public List<EnemySkill> skills = new List<EnemySkill>();
    public List<EnemySkill> availableSkills = new List<EnemySkill>();

    private Vector3 originPos;

    Animator an;
    NavMeshAgent navAgent;
    private void Start()
    {
        an = GetComponent<Animator>();
        navAgent = GetComponent<NavMeshAgent>();
        originPos = transform.position;
        for (int i = 0; i < skills.Count; i++)
        {
            skills[i].cooltimeTimer_debug = 0;
        }
    }
    private void Update()
    {
        for (int i = 0; i < skills.Count; i++)
        {
            skills[i].TimeUpdate();
        }

        if (detectRange())//���������� ������ ����
        {
            if (Vector3.Distance(transform.position, detectplayer) <= attackRange)//���ݹ����� ������ ���߱� and ����
            {
                navAgent.isStopped = true;

                //Quaternion targetRotation = Quaternion.LookRotation(detectplayer);
                //transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0, targetRotation.eulerAngles.y, 0), rotationSmoothSpeed);

                for (int i = 0; i < skills.Count; i++)
                {
                    if (skills[i].SkillAvailable())
                    {
                        availableSkills.Add(skills[i]);
                    }
                    
                }
                if (availableSkills.Count >= 1)
                {
                    availableSkills[Random.Range(0, availableSkills.Count)].UseSkill(this);
                    an.SetBool("HasTarget", true);
                    an.SetInteger("ActionIndex", 1);
                    availableSkills.Clear();
                }
            }
            else
            {
                navAgent.isStopped = false;
                navAgent.updateRotation = true;
                navAgent.SetDestination(detectplayer);
            }
        }
        else
        {
            navAgent.SetDestination(originPos);
        }
        if (Vector3.Distance(transform.position, detectplayer) <= attackRangeNear)//�ʹ� ������ �ڷ� ��������
        {
            navAgent.isStopped = false;
            navAgent.updateRotation = false;
            //Quaternion targetRotation = Quaternion.LookRotation(detectplayer);
            //transform.rotation = Quaternion.Slerp(transform.rotation,Quaternion.Euler(0,targetRotation.eulerAngles.y,0), rotationSmoothSpeed);
            navAgent.SetDestination(-detectplayer);
        }
        ani();
    }
    void ani()
    {
        Vector3 speed = navAgent.velocity;
        float speedMagnitude = speed.magnitude;
        an.SetBool("IsWandering", speedMagnitude > 0.5f ? true : false);
    }
    private bool detectRange()//���ڹ������� �÷��̾ �ֳ�?
    {
        Collider[] hitColliders = Physics.OverlapSphere(originPos, detectionRange, playerLayer);

        for (int i = 0; hitColliders.Length > i; i++)
        {
            detectplayer = hitColliders[i].transform.position;
            return true;
        }
        return false;
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue; // ���� ������ ����
        Gizmos.DrawWireSphere(originPos, detectionRange);
        Gizmos.color = Color.red; // �ִ� ���� ������ ����
        Gizmos.DrawWireSphere(transform.position, attackRange);
        Gizmos.color = Color.yellow; // �ּ� ���� ������ ����
        Gizmos.DrawWireSphere(transform.position, attackRangeNear);
    }
}
