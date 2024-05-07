using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestEnemy : MonoBehaviour
{
    public float smoothTime = 0.3F;
    private Vector3 Velocity;
    public Transform target;
    private Rigidbody rb;
    public PlayerController playerController;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        Velocity = (target.position - transform.position).normalized;
        rb.velocity = Velocity * 7f;
    }
    void FixedUpdate()
    {
        Debug.DrawRay(transform.position,  Velocity * 10, Color.white);
    }
}
