using System.Collections;
using System.Collections.Generic;
using UnityEngine;

 public abstract class EnemySkill : ScriptableObject
{
    public float coolTime;
    public float cooltimeTimer_debug;
    public float damage;
    public Animation attackAnimation;

    public GameObject attackEffect;

    public bool skillUsage;
    public float testskillusagetime;
    public void TimeUpdate()
    {
        cooltimeTimer_debug += Time.deltaTime;
    }
    public bool SkillAvailable()
    {
        if (cooltimeTimer_debug >= coolTime)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    public abstract void UseSkill(MonoBehaviour monoBehaviour);
}
