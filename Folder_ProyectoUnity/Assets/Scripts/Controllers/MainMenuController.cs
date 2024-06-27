using UnityEngine.SceneManagement;
using UnityEngine.Audio;
using UnityEngine.UI;
using UnityEngine;
using System.Collections;

public class MainMenuController : MonoBehaviour
{
    //DATA
    [SerializeField] private VolumeData volumeData;

    //AUDIO
    [SerializeField] private AudioMixer audioMixer;
    private AudioSource audioSource;

    //UI
    [SerializeField] private Slider Master;
    [SerializeField] private Slider Music;
    [SerializeField] private Slider SFX;
    [SerializeField] private GameObject AudioSettingsHolder;

    //PARTICLES
    [SerializeField] private ParticleSystem Particle1;
    [SerializeField] private ParticleSystem Particle2;

    void Start()
    {
        Particle1.Play();
        Particle2.Play();
            
        audioSource = GetComponent<AudioSource>();
        volumeData.SetInitalSlidersValues(Master, Music, SFX);
    }

    void Update()
    {
        volumeData.SetVolumeValues(audioMixer);
    }
    //SCENE C-A
    public void SceneToGo(string scene)
    {
        SceneManager.LoadScene(scene);
    }
    //UI C-A
    public void EnableAudioSetting()
    {
        if(AudioSettingsHolder.activeSelf)
        {
            AudioSettingsHolder.SetActive(false);
        }
        else
        {
            AudioSettingsHolder.SetActive(true);
        }
    }
}