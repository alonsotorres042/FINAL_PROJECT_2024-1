using UnityEngine;

[CreateAssetMenu(fileName = "PlayerSoundData", menuName = "ScriptableObjects/PlayerSoundData", order = 2)]
public class PlayerSoundData : ScriptableObject
{
    public AudioClip Jump;
    public AudioClip Shot;
    public AudioClip Hurt;
    public AudioClip Death;
}
