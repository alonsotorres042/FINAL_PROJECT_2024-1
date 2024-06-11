using UnityEngine;
using System;
using Unity.Collections;
using DG.Tweening;
using System.Collections;

public class KamikazeHenchmanController : EnemyClass
{
    // Start is called before the first frame update
    void Awake()
    {
        CurrentLife = MaxLife;
        AttackTarget = eventManagerData.Player.transform;
    }
    void Start()
    {
        DOTween.Init();
        StartCoroutine(InitialBehaviour());
    }

    // Update is called once per frame
    void Update()
    {
        if (CurrentLife <= 0)
        {
            Death();
        }
    }
    public IEnumerator InitialBehaviour()
    {
        float time = 0;
        while (time < 2.5f)
        {
            transform.position += new Vector3(0f, 0.025f, 0f);
            yield return null;
        }
        transform.DOMove(AttackTarget.position, 1.5f).SetEase(Ease.InOutElastic);
    }
}
