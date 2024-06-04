using UnityEngine;

[CreateAssetMenu(fileName = "EventManagerData", menuName = "ScriptableObjects/EventManagerData", order = 0)]
public class EventManagerData : ScriptableObject
{
    public VolumeData _VolumeData;
    public EventManager _EventManager;
    public UIManager _UIManager;
    public AudioManager _AudioManager;
    public ScenesManager _ScenesManager;
    public PlayerController Player;
    public BossController Triceratops;
}
