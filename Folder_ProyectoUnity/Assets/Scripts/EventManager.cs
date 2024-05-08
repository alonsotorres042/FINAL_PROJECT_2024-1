using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class EventManager : MonoBehaviour
{
    //VARIABLES
    [SerializeField] private PlayerController Player;

    //EVENTS
    public event Action ShotEvent;

    public void OnShoot(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            ShotEvent?.Invoke();
        }
    }
}