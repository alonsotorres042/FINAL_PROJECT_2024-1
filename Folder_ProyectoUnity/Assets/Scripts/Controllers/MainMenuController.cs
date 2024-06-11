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

        //TESTING
        if (Input.GetKeyDown(KeyCode.P))
        {

<<<<<<< HEAD
=======
        audioSource.GetSpectrumData(Spectrum, 0, FFTWindow.Rectangular);
        for(int i = 0; i < Spectrum.Length; i++)
        {
            if (Spectrum[i] * 3 >= LimitFrequency)
            {
                iter = 0;
                rythmVisualsController.DecreaseBassScale();
            }
            else
            {
                iter++;
                //rythmVisualsController.DecreaseOtherScale();
            }
        }
    }
    //SCENE C-A
    public void SceneToGo(string scene)
    {
        SceneManager.LoadScene(scene);
    }
    //AUDIO C-A
    public void MusicPlayer()
    {
        if (!audioSource.isPlaying)
        {
            int rnd = Random.Range(0, musicData.MusicArray.Length - 1);
            audioSource.clip = musicData.MusicArray[rnd];
            audioSource.Play();
>>>>>>> b304648aafcc9abcae60f1e408e83da850b64941
        }
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
    //AUDIO C-A
}