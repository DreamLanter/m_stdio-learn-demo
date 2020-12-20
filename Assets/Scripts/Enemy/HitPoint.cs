using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitPoint : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D otherCollision)
    {
        if (otherCollision.CompareTag("Player"))
        {
            Debug.Log("玩家受伤");
        }

        if (otherCollision.CompareTag("Boomb"))
        {
            
        }
    }
}
