using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "EventManagerData", menuName = "ScriptableObjects/EventManagerData", order = 1)]
public class EventManagerData : ScriptableObject
{
    public PlayerController Player;
}
