using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BulletClass : MonoBehaviour
{
    //GENERAL
    [SerializeField] protected EventManagerData eventManagerData;

    //COMPONENTS
    protected Rigidbody rb;

    //ESCENTIALS
    [SerializeField] protected float Speed;
    [SerializeField] protected float Damage;
    protected Vector3 MovementDirection;
}