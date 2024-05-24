using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyClass : MonoBehaviour
{
    //GENERAL
    [SerializeField] protected EventManagerData eventManagerData;

    //COMPONENTS
    protected Rigidbody rb;

    //ESCENTIALS
    [SerializeField] protected float Life;
    protected Transform AttackTarget;

    //MOVEMENT
    [SerializeField] protected float ChaseSpeed;

    //DAMAGE
    protected float MeleeDamage;
    protected float BulletDamage;

    protected IEnumerator Shot()
    {
        yield return null;
    }
    protected void StartChasingPlayer()
    {
        rb.velocity = Vector3.Slerp(rb.velocity, (AttackTarget.position - transform.position).normalized * ChaseSpeed, ChaseSpeed * Time.deltaTime);
        transform.LookAt(eventManagerData.Player.transform);
    }
    protected void XYChasingPlayer()
    {
        rb.velocity = Vector3.Slerp(rb.velocity, new Vector3((AttackTarget.position - transform.position).normalized.x * ChaseSpeed, rb.velocity.y, (AttackTarget.position - transform.position).normalized.z * ChaseSpeed), ChaseSpeed * Time.deltaTime);
        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(new Vector3((AttackTarget.position - transform.position).x, transform.rotation.y, (AttackTarget.position - transform.position).z)), 5f * Time.deltaTime);
    }
    protected void StopChasingPlayer()
    {
        rb.velocity = Vector3.zero;
        transform.rotation = Quaternion.identity;
    }
    protected void Death()
    {
        Destroy(gameObject);
    }
    public void GetBulletDamage(float BulletDamage)
    {
        Life = Life - BulletDamage;
    }
}