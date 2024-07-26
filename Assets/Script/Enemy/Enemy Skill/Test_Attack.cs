using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Test Skill", menuName = "Scriptable Object/Enemy/Enemy Skill/Test Skill")]
public class Test_Attack : EnemySkill
{

    public override void UseSkill()
    {
        Debug.Log("Test_Attack");
    }

}
