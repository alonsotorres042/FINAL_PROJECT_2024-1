using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    [SerializeField] private BulletDirection_SO bulletDirection_SO;
    private Rigidbody rb;
    [SerializeField] private float Speed;
    private Vector3 MovementDirection;
    void Start()
    {
        MovementDirection = bulletDirection_SO.BulletDirection;
        transform.rotation = Quaternion.LookRotation(MovementDirection);
        rb = GetComponent<Rigidbody>();
        rb.velocity = MovementDirection * Speed;
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag != "Player")
        {
            Destroy(gameObject);
        }
    }
}