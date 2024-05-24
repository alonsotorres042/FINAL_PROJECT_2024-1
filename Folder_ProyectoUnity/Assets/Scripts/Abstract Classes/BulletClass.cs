using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BulletClass : MonoBehaviour
{
    //GENERAL
    [SerializeField] protected EventManagerData eventManagerData;

    //COMPONENTS
    protected Rigidbody rb;

    //BULLET ESCENTIALS
    [SerializeField] protected float Speed;
    [SerializeField] protected float Damage;
    protected Vector3 MovementDirection;

    //PARABOLIC SETTINGS
    [SerializeField] protected float ParabolicForce;
    protected float _x;
    protected float _y;
    protected float _z;
}