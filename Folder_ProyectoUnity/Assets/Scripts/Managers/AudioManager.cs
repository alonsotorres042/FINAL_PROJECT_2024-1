using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Timeline;
using static UnityEngine.Rendering.DebugUI;

public class AudioManager : MonoBehaviour
{
    //DATA
    [SerializeField] private EventManagerData eventManagerData;

    //GENERAL
    [SerializeField] private AudioMixer audioMixer;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        SetVolumeValues();
    }
    public void SetVolumeValues()
    {
        audioMixer.SetFloat("_Master", Mathf.Log10(eventManagerData._VolumeData.Master) * 20);
        audioMixer.SetFloat("_Music", Mathf.Log10(eventManagerData._VolumeData.Music) * 20);
        audioMixer.SetFloat("_SFX", Mathf.Log10(eventManagerData._VolumeData.SFX) * 20);
    }
}