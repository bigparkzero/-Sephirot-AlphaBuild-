using System.Collections;
using System.Collections.Generic;
using UnityEngine;

 public abstract class EnemySkill : ScriptableObject
{
    public float coolTime;
    public float cooltimeTimer_debug;
    public float damage;

    public GameObject attackEffect;
    public void TimeUpdate()
    {
        cooltimeTimer_debug += Time.deltaTime;
    }
    public abstract void UseSkill();
    public abstract Animation GetAnimation();
}
