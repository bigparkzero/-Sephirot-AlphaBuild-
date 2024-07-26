using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyGeneralState", menuName = "Scriptable Object/Enemy/EnemyGeneralState")]
public class EnemyGeneralState : ScriptableObject
{
    [Tooltip("���� Ž���� ������Ʈ�� ���̾�")]
    public LayerMask playerLayer;
    [Tooltip("�� Ž�� ����")]
    public float detectionRange;
    [Tooltip("�� �ִ� ���� ����")]
    public float attackRange;
    [Tooltip("�� �ּ� ���� ����")]
    public float attackRangeNear;

    public float waitTimeBeforeAttack;
}
