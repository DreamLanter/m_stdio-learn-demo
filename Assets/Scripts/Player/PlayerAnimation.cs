using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    private Animator anim;
    private Rigidbody2D rb;
    private PlayerControler controler;
    
    void Start()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        controler = GetComponent<PlayerControler>();
    }

    
    void Update()
    {
        anim.SetFloat("speed", Mathf.Abs(rb.velocity.x));//会出现速度小于零设置绝对值
        anim.SetFloat("velocityY", rb.velocity.y);
        anim.SetBool("jump", controler.isJump);
        anim.SetBool("ground", controler.isGround);
    }
}
