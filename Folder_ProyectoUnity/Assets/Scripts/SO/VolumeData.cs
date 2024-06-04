using UnityEngine;

[CreateAssetMenu(fileName = "VolumeData", menuName = "ScriptableObjects/VolumeData", order = 1)]
public class VolumeData : ScriptableObject
{
    public float Master;
    public float Music;
    public float SFX;
    public void UpdateValue(string target, float value)
    {
        if(target == "Master")
        {
            Master = value;
        }
        else if(target == "Music")
        {
            Music = value;
        }
        else if (target == "SFX")
        {
            SFX = value;
        }
    }
}