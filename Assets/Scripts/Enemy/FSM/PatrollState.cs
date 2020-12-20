using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PratelState : EnemyBaseSate
{
    public override void EnterState(Enemy enemy)
    {
        enemy.animState = 0;
        enemy.SwitchPoint();
    }

    public override void OnUpdate(Enemy enemy)
    {
        if (!enemy.anim.GetCurrentAnimatorStateInfo(0).IsName("idle"))//如果没有播放idle动画执行下列命令
        {
            enemy.animState = 1;
            enemy.MoveToTarget();
        }
        if (Mathf.Abs(enemy.transform.position.x - enemy.targetPoint.position.x) < 0.01f)
        {
            enemy.TransitionToState(enemy.pratelState);
        }

        if (enemy.attackList.Count>0)//如果检测到目标大于零进入攻击状态（函数）
        {
            enemy.TransitionToState(enemy.attackState);
        }
    }
}
