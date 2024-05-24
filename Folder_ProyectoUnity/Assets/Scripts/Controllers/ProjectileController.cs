using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class ProjectileController : BulletClass
{
    //COMPONENTS

    //MOVEMENT 
    [SerializeField] private float ExplosionRadius;
    [SerializeField] private float ExplosionPower;
    [SerializeField] private float ExplosionUpPower;
    private bool _isColled;
    private float time;

    void Start()
    {
        time = 0;
        _isColled = false;
        MovementDirection = eventManagerData.Player._bulletDirection;
    }
    void FixedUpdate()
    {
        if (_isColled == false)
        {
            CompoundMovement();
        }
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.tag != "PlayerBullet" && other.tag != "Player")
        {
            SetNormalizeMovement();
            StartCoroutine(Explotion());
        }
    }
    public IEnumerator Explotion()
    {
        yield return new WaitForSeconds(1.5f);
        Collider[] colliders = Physics.OverlapSphere(transform.position, ExplosionRadius);
        for(int i = 0; i < colliders.Length; i++)
        {
            if (colliders[i].tag != "PlayerBullet" && colliders[i].GetComponent<Rigidbody>() == true)
            {
                Rigidbody rb = colliders[i].GetComponent<Rigidbody>();
                rb.AddExplosionForce(ExplosionPower, transform.position, ExplosionRadius, ExplosionUpPower, ForceMode.Impulse);
                if (colliders[i].GetComponent<EnemyClass>())
                {
                    colliders[i].GetComponent<EnemyClass>().GetBulletDamage(Damage);
                }
            }
        }
        Destroy(gameObject);
    }
    public void SetNormalizeMovement()
    {
        _isColled = true;
        BoxCollider _myBC = GetComponent<BoxCollider>();
        Rigidbody _myRB = GetComponent<Rigidbody>();
        _myBC.isTrigger = false;
        _myRB.useGravity = true;
    }
    protected void CompoundMovement()
    {
        _x = transform.position.x + ((MovementDirection.x * ParabolicForce) * time);
        _z = transform.position.z + ((MovementDirection.z * ParabolicForce) * time);
        _y = transform.position.y + ((ParabolicForce * 1.5f) * time) - (0.5f * (9.81f * time * time));

        transform.position = new Vector3(_x, _y, _z);
        time += Time.deltaTime;
    }
}