using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Rendering;
using UnityEngine;

public class BossController : EnemyClass
{

    [SerializeField] private Transform HenchmanSpawner;
    [SerializeField] private GameObject HenchmanBasic;
    [SerializeField] private GameObject HenchmanKamikaze;
    [SerializeField] private GameObject Triceratops;

    void Awake()
    {
        CurrentLife = MaxLife;
    }
    void Start()
    {
        AttackTarget = eventManagerData.Player._transform;
        rb = Triceratops.GetComponent<Rigidbody>();
        StartCoroutine(SpawnBasicHenchmen());
        StartCoroutine(SpawnKamikazeHenchmen());
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

    public IEnumerator SpawnBasicHenchmen()
    {
        while (true)
        {
            Instantiate(HenchmanBasic, HenchmanSpawner.position, Quaternion.identity);
            yield return new WaitForSeconds(2f);
        }
    }
    public IEnumerator SpawnKamikazeHenchmen()
    {
        while (true)
        {
            yield return new WaitForSeconds(2.5f);
            Instantiate(HenchmanKamikaze, HenchmanSpawner.position, Quaternion.identity);
        }
    }
}