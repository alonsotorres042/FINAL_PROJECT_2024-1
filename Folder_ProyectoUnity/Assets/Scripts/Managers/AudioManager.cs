using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    //DATA
    [SerializeField] private EventManagerData eventManagerData;

    //GENERAL
    [SerializeField] private AudioMixer audioMixer;

    void Update()
    {
        eventManagerData._VolumeData.SetVolumeValues(audioMixer);
    }
}