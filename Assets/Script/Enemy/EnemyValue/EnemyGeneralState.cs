using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyGeneralState", menuName = "Scriptable Object/Enemy/EnemyGeneralState")]
public class EnemyGeneralState : ScriptableObject
{
    [Tooltip("적이 탐지할 오브젝트의 레이어")]
    public LayerMask playerLayer;
    [Tooltip("적 탐지 범위")]
    public float detectionRange;
    [Tooltip("적 최대 공격 범위")]
    public float attackRange;
    [Tooltip("적 최소 공격 범위")]
    public float attackRangeNear;

    public float waitTimeBeforeAttack;
}
