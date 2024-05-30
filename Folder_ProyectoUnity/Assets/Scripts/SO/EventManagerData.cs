using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EventManagerData", menuName = "ScriptableObjects/EventManagerData", order = 1)]
public class EventManagerData : ScriptableObject
{
    public EventManager _EventManager;
    public PlayerController Player;
    public BossController Triceratops;
}
