using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BossController : EnemyClass
{
    private int RandomSpawner1 = 0;
    private int RandomSpawner2 = 0;
    [SerializeField] private GameObject basicHenchan;
    [SerializeField] private Transform[] Spawners;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(RandomHenchmanSpawn());
    }
    public IEnumerator RandomHenchmanSpawn()
    {
        while (true)
        {
            GetTwoRandoms();
            Instantiate(basicHenchan, Spawners[RandomSpawner1].position, Quaternion.identity);
            Instantiate(basicHenchan, Spawners[RandomSpawner2].position, Quaternion.identity);
            yield return new WaitForSeconds(3.5f);
        }
    }
    public void GetTwoRandoms()
    {
        RandomSpawner1 = Random.Range(0, 10);
        RandomSpawner2 = Random.Range(0, 10);
        if (RandomSpawner1 == RandomSpawner2)
        {
            GetTwoRandoms();
        }
        else
        {
            return;
        }
    }
}