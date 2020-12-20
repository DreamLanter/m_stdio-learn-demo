using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControler : MonoBehaviour,IDamageable
{
    private Rigidbody2D rb;//只能在函数中访问无法在unity窗口中查看

    public float speed;//定义speed变量
    public float jumpForce;

    [Header("Player State")]
    public float health;
    public bool isDead;

    [Header("Ground Check")]
    public Transform groundCheck;
    public float checkRadius;//检测范围
    public LayerMask groundLayer;//检测图层是否为ground图层

    [Header("States Check")]//区分不同参数
    public bool isGround;//检测地面
    public bool isJump;//检测是否播放动画jump 正在跳跃
    public bool canJump;//判断是否按下jump

    [Header("Jump FX")]//获得组件
    public GameObject jumpFX;
    public GameObject landFX;

    [Header("Attack Settings")]
    public GameObject boombPrefab;//创建炸弹预制体
    public float nextAttack = 0;//计时器
    public float attackRate;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();//获得Rigidbody2D组件
    }

    // Update is called once per frame
    void Update()
    {
        checkInput();
    }

    public void FixedUpdate()//锁帧
    {
        PhysicsCheck();
        Movement();
        jump();
    }

    void checkInput()//判断是否按下jump
    {
        if (Input.GetButtonDown("Jump") && isGround)
        {
            canJump = true;
        }
        if (Input.GetKeyDown(KeyCode.J))
        {
            Attack();
        }
    }

    void Movement()//新建movement函数
    {
        //float horizomtalMovement = Input.GetAxis("Horizontal");//定义横向控制变量（键盘控制变量） 输入大小为-1~1（包含小数和0）
        float horizomtalMovement = Input.GetAxisRaw("Horizontal");//输入变量不包含小数只有 -1，1
        rb.velocity = new Vector2(horizomtalMovement * speed, rb.velocity.y);//定义物理向量获得速度
        if ( horizomtalMovement!= 0)
        {
            transform.localScale = new Vector3(horizomtalMovement, 1, 1);
        }
    }

    void jump()
    {
        if (canJump)
        {
            isJump = true;
            jumpFX.SetActive(true);//按下跳跃时播放jumpFX
            jumpFX.transform.position = transform.position + new Vector3(0, -0.45f, 0);//获取位置
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            rb.gravityScale = 4;//跳跃时重力变为4
            canJump = false;//不按的时候落下
        }
    }

    public void Attack()
    {
        if (Time.time > nextAttack)//当前游戏时间大于下一次攻击可执行时间
        {
            Instantiate(boombPrefab, transform.position, boombPrefab.transform.rotation);//生成炸弹预制体
           
            nextAttack = Time.time + attackRate;//重置下一次攻击时间当前时间+设定间隔时间
        }
    }

    void PhysicsCheck()//物理检测
    {
        isGround = Physics2D.OverlapCircle(groundCheck.position, checkRadius, groundLayer);
        if (isGround)
        {
            rb.gravityScale = 1;//在地面重力为1
            isJump = false;
        }
    }

    public void LandFX()//Animation Event
    {
        landFX.SetActive(true);//落地时播放landFX
        landFX.transform.position = transform.position + new Vector3(0, -0.75f, 0);
    }

    public void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(groundCheck.position, checkRadius);
    }

    public void GetHit(float damage)
    {
        health -= damage;
        if (health<0)
        {
            health = 0;
            isDead = true;
        }
    }
}
