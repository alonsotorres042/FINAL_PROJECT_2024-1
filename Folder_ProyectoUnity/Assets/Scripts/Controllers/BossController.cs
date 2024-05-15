using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BossController : EnemyClass
{

    void Start()
    {
        AttackTarget = eventManagerData.Player._transform;
        rb = GetComponent<Rigidbody>();
    }
    void FixedUpdate()
    {
        if (Life <= 0)
        {
            Death();
        }
        StartChasingPlayer();
    }
}