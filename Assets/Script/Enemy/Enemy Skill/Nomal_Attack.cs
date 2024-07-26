using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Nomal Skill", menuName = "Scriptable Object/Enemy/Enemy Skill/Nomal Skill")]
public class Nomal_Attack : EnemySkill
{

    public override void UseSkill()
    {
        Debug.Log("Nomal_Attack");
    }
   
}
