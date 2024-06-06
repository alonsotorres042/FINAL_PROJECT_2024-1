using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

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
    public void SetVolumeValues(AudioMixer myAm)
    {
        myAm.SetFloat("_Master", Mathf.Log10(Master) * 20);
        myAm.SetFloat("_Music", Mathf.Log10(Music) * 20);
        myAm.SetFloat("_SFX", Mathf.Log10(SFX) * 20);
    }
    public void SetInitalSlidersValues(Slider master, Slider music, Slider sfx)
    {
        master.value = Master;
        music.value = Music;
        sfx.value = SFX;
        master.onValueChanged.AddListener(delegate { UpdateValue("Master", master.value); });
        music.onValueChanged.AddListener(delegate { UpdateValue("Music", music.value); });
        sfx.onValueChanged.AddListener(delegate { UpdateValue("SFX", sfx.value); });
    }
}