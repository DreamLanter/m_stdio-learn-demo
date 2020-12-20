using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    EnemyBaseSate currentState;//定义当前状态类型（抽象类）
    public Animator anim;
    public int animState;

    [Header("Movement")]
    public Transform pointA;
    public Transform pointB;
    public Transform targetPoint;
    public float speed;

    [Header("AttackaSetting")]
    public float attackRate;//攻击时间间隔
    private float nextAttack = 0;
    public float attackRange/*普通攻击距离*/, skillRange/*特殊攻击距离*/;

    public List<Transform> attackList = new List<Transform>();//列表具体含义在unity中显示

    public PratelState pratelState = new PratelState();//继承的子集 第一种状态巡逻状态
    public AttackState attackState = new AttackState();

    public virtual void Init()//定义虚函数
    {
        anim = GetComponent<Animator>();
    }

    public void Awake()//最优先执行此函数的内容以免出错
    {
        Init();
    }

    void Start()
    {
        TransitionToState(pratelState);
    }

    void Update()
    {
        currentState.OnUpdate(this);
        anim.SetInteger("state", animState);//对运行中的每一帧都检测参数是否变化
    }

    public void TransitionToState(EnemyBaseSate sate)//用于切换不同的状态
    {
        currentState = sate;//有攻击目标切换到目标
        currentState.EnterState(this);//没攻击目标切换到巡逻
    }

    public void MoveToTarget()//向玩家移动
    {
        transform.position = Vector2.MoveTowards(transform.position, targetPoint.position, speed * Time.deltaTime);
        FilpDirection();
    }

    public void AttackAction()//攻击玩家
    {
        
        if (Vector2.Distance(transform.position,targetPoint.position)/*判断两个vector2之间的距离*/<attackRange)//之间距离小于普通攻击间隔距离
        {
            if (Time.time>nextAttack)//现在时间大于下一次攻击间隔时间
            {
                anim.SetTrigger("attack");//播放攻击动画
                Debug.Log("攻击玩家");
                nextAttack = Time.time + attackRate;//重置下一次攻击时间（当前时间+设定间隔时间）
            }
        }
    }

    public virtual void SkillAction()//攻击炸弹 每个敌人不同方式
    {
        
        if (Vector2.Distance(transform.position, targetPoint.position)/*判断两个vector2之间的距离*/< skillRange)//之间距离小于技能攻击间隔距离
        {
            if (Time.time > nextAttack)//现在时间大于下一次攻击间隔时间
            {
                anim.SetTrigger("skill");//播放技能动画
                Debug.Log("攻击炸弹");
                nextAttack = Time.time + attackRange;//重置下一次攻击时间（当前时间+设定间隔时间）
            }
        }
    }

    public void FilpDirection()//反转方向
    {
        if (transform.position.x<targetPoint.position.x)
        {
            transform.rotation = Quaternion.Euler(0f, 0f, 0f);
        }
        else
        {
            transform.rotation = Quaternion.Euler(0f, 180f, 0f);
        }
    }

    public void SwitchPoint()
    {
        if (Mathf.Abs(pointA.position.x - transform.position.x) > Mathf.Abs(pointB.position.x - transform.position.x))
        {
            targetPoint = pointA;
            //Debug.Log("目标点变为A");
        }
        else
        {
            targetPoint = pointB;
            //Debug.Log("目标点变为B");
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (!attackList.Contains(collision.transform))
        {
            attackList.Add(collision.transform);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        attackList.Remove(collision.transform);
    }
}
