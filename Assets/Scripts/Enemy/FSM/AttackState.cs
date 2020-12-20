using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackState : EnemyBaseSate
{
    public override void EnterState(Enemy enemy)
    {
        //Debug.Log("发现敌人");
        enemy.animState = 2;//state=2进入攻击动画  详细解释Enemy45行
        enemy.targetPoint = enemy.attackList[0];//检测范围内
    }

    public override void OnUpdate(Enemy enemy)
    {
        if (enemy.attackList.Count==0)
        {
            enemy.TransitionToState(enemy.pratelState);
        }
        if (enemy.attackList.Count>1)
        {
            for (int i = 0; i < enemy.attackList.Count; i++)
            {
                if (Mathf.Abs(enemy.transform.position.x-enemy.attackList[i].position.x)<
                    Mathf.Abs(enemy.transform.position.x-enemy.targetPoint.position.x))//和当前目标坐标值进行比较判断哪个更近
                {
                    enemy.targetPoint = enemy.attackList[0];//如果列表内目标更近则将目标改为列表内第一个目标
                }
            }
           
        }
        if (enemy.attackList.Count==1)
        {
            enemy.targetPoint = enemy.attackList[0];
        }

        if (enemy.targetPoint.CompareTag("Player"))
        {
            enemy.AttackAction();
        }
        if (enemy.targetPoint.CompareTag("Boomb"))
        {
            enemy.SkillAction();
        }

        enemy.MoveToTarget();
    }
}
