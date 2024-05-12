using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicHenchmanController : EnemyClass
{
    void Start()
    {
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
    }
}