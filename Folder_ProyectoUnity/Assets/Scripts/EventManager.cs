using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : MonoBehaviour
{
    //SCRIPTABLE OBJECT
    public EventManagerData eventManagerData;

    //LOCAL VARIABLES
    [SerializeField] private PlayerController Player;

    void Awake()
    {
        eventManagerData.Player = Player;
    }
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
