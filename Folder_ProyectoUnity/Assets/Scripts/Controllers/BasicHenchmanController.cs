using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicHenchmanController : EnemyClass
{
    void Start()
    {
        AttackTarget = eventManagerData.Player._transform;
        rb = GetComponent<Rigidbody>();
    }
    void Update()
    {
        if(Life <= 0)
        {
            Death();
        }
    }
    void FixedUpdate()
    {
        StartChasingPlayer();
        transform.rotation *= Quaternion.Euler(0f, 0f, 10f);
    }
}