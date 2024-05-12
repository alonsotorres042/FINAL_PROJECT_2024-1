using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBulletController : BulletClass
{
    void Start()
    {
        MovementDirection = eventManagerData.Player._bulletDirection;
        rb = GetComponent<Rigidbody>();
        rb.velocity =  MovementDirection * Speed;
        transform.rotation = Quaternion.LookRotation(MovementDirection);
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Enemy")
        {
            other.GetComponent<EnemyClass>().GetBulletDamage(Damage);
        }
        if (other.tag != "Player")
        {
            Destroy(gameObject);
        }
    }
}