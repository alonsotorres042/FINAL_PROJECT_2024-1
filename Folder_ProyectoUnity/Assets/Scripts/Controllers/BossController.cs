using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Rendering;
using UnityEngine;

public class BossController : EnemyClass
{

    [SerializeField] private Transform HenchmanSpawner;
    [SerializeField] private GameObject Henchman;
    [SerializeField] private GameObject Triceratops;

    void Awake()
    {
        CurrentLife = MaxLife;
    }
    void Start()
    {
        AttackTarget = eventManagerData.Player._transform;
        rb = Triceratops.GetComponent<Rigidbody>();
        StartCoroutine(SpawnHenchmen());
    }
    private void OnEnable()
    {
        eventManagerData._EventManager.Victory += BossDeath;
    }
    private void OnDisable()
    {
        eventManagerData._EventManager.Victory -= BossDeath;
    }
    void FixedUpdate()
    {
        BossChasingPlayer();
    }

    public IEnumerator SpawnHenchmen()
    {
        while (true)
        {
            Instantiate(Henchman, HenchmanSpawner.position, Quaternion.identity);
            yield return new WaitForSeconds(2f);
        }
    }
}