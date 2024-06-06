using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MusicData", menuName = "ScriptableObjects/MusicData", order = 3)]

public class MusicData : ScriptableObject
{
    public AudioClip[] MusicArray;
}
