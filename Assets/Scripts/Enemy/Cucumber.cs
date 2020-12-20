using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cucumber : Enemy
{
    public void SetOff()//Animator Event
    {
        targetPoint.GetComponent<Boomb>().TurnOff();
    }
}
