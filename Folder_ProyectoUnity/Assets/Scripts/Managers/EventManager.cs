using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EventManager : MonoBehaviour
{
    //DATA
    public EventManagerData eventManagerData;

    //EVENTS
    public event Action Victory;
    public event Action Defeat;

    //LOCAL VARIABLES
    [SerializeField] private VolumeData _VolumeData;
    [SerializeField] private UIManager _UIManager;
    [SerializeField] private AudioManager _AudioManager;
    [SerializeField] private ScenesManager _ScenesManager;
    [SerializeField] private PlayerController Player;
    [SerializeField] private BossController Triceratops;

    void Awake()
    {
        eventManagerData._EventManager = this;
        eventManagerData._VolumeData = _VolumeData;
        eventManagerData._UIManager = _UIManager;
        eventManagerData._AudioManager = _AudioManager;
        eventManagerData._ScenesManager = _ScenesManager;
        eventManagerData.Player = Player;
        eventManagerData.Triceratops = Triceratops;
    }

    void Start()
    {
        StartCoroutine(EvaluateGameStatus());
    }
    IEnumerator EvaluateGameStatus()
    {
        while(Triceratops._currentLife > 0 && Player._currentLife > 0)
        {
            yield return null;
        }
        if(Player._currentLife <= 0)
        {
            Defeat?.Invoke();
        }
        else if(Triceratops._currentLife <= 0)
        {
            Victory?.Invoke();
        }
    }
}