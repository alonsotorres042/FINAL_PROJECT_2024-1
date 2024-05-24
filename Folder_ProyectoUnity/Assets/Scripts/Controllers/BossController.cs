using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Rendering;
using UnityEngine;

public class BossController : EnemyClass
{
    [SerializeField] private GameObject Henchman;
    [SerializeField] private GameObject Triceratops;
    
    void Start()
    {
        AttackTarget = eventManagerData.Player._transform;
        rb = Triceratops.GetComponent<Rigidbody>();
        StartCoroutine(SpawnHenchmen());
    }

    void FixedUpdate()
    {
        XYChasingPlayer();
    }

    public IEnumerator SpawnHenchmen()
    {
        while (true)
        {
            Instantiate(Henchman, transform.position, Quaternion.identity);
            yield return new WaitForSeconds(1f);
        }
    }
}