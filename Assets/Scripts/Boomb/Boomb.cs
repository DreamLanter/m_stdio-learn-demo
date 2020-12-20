using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boomb : MonoBehaviour
{
    private Animator anim;
    private Collider2D coll;
    private Rigidbody2D rb;

    public float startTime;
    public float waitTime;
    public float boombForce;//设定爆炸产生的力

    [Header("Check")]
    public float radius;//检测范围
    public LayerMask targetLayer;//检测爆炸范围图层及哪些物体会受影响

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        coll = GetComponent<Collider2D>();
        rb = GetComponent<Rigidbody2D>();
        startTime = Time.time;//获得开始时间，让开始时间等于游戏的时钟
    }

    // Update is called once per frame
    void Update()
    {
        if (!anim.GetCurrentAnimatorStateInfo(0).IsName("boomb_off"))//如果播放的不是熄灭动画执行以下语句
        {
            if (Time.time > (startTime + waitTime))//当等待时间过完后播放爆炸动画
            {
                anim.Play("boomb_explotion");
            }
        }
    }

    public void Explotion()//爆炸时检测碰撞体并施加相应的力 Animation Event
    {
        coll.enabled = false;//取消勾选collided使炸弹对自身不产生碰撞
        Collider2D[] aroundObjects = Physics2D.OverlapCircleAll(transform.position, radius, targetLayer);//定义碰撞体数组，获取周围所有物体
        rb.gravityScale = 0;//将重力更改为0 防止炸弹因collided取消勾选而下坠
        foreach (var item in aroundObjects)//获得碰撞体刚体并给刚体施加力
        {
            Vector3 pos = transform.position - item.transform.position;//与当前炸弹坐标值进行对比并对被炸物体施加相反的力
            item.GetComponent<Rigidbody2D>().AddForce(-pos+Vector3.up * boombForce, ForceMode2D.Impulse);//
            if (item.CompareTag("Boomb")&&item.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("boomb_off"))//如果当前数组中含有炸弹并且炸弹播放熄灭动画
            {
                item.GetComponent<Boomb>().TurnOn();
            }
        }
    }

    public void DestoryThis()//销毁炸弹
    {
        Destroy(gameObject);
    }

    public void TurnOff()
    {
        anim.Play("boomb_off");
        gameObject.layer = LayerMask.NameToLayer("NPC");
    }

    public void TurnOn()
    {
        startTime = Time.time;//重新计算时间
        anim.Play("boomb_on");
        gameObject.layer = LayerMask.NameToLayer("Boomb");
    }

    public void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}
