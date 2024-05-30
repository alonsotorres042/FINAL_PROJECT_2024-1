using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EventManager : MonoBehaviour
{
    //DATA
    public EventManagerData eventManagerData;

    //EVENTS
    public event Action Victory;

    //LOCAL VARIABLES
    [SerializeField] private PlayerController Player;
    [SerializeField] private BossController Triceratops;

    void Awake()
    {
        eventManagerData._EventManager = this;
        eventManagerData.Player = Player;
        eventManagerData.Triceratops = Triceratops;
    }

    // Update is called once per frame
    void Update()
    {
        if(Triceratops._currentLife <= 0)
        {
            Victory?.Invoke();
        }
    }
}