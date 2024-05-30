using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBulletController : BulletClass
{
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        MovementDirection = eventManagerData.Player._bulletDirection;
        rb.velocity =  MovementDirection * Speed;
        transform.rotation = Quaternion.LookRotation(MovementDirection);
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Enemy")
        {
            other.GetComponent<EnemyClass>().GetBulletDamage(Damage);
        }
        if(other.tag == "Boss")
        {
            other.GetComponentInParent<EnemyClass>().GetBulletDamage(Damage);
        }
        if (other.tag != "Player")
        {
            Destroy(gameObject);
        }
    }
}