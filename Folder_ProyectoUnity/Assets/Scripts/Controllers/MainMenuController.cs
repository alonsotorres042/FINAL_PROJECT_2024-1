using UnityEngine.SceneManagement;
using UnityEngine.Audio;
using UnityEngine.UI;
using UnityEngine;
using Unity.VisualScripting;
using System.Collections;

public class MainMenuController : MonoBehaviour
{
    //DATA
    [SerializeField] private VolumeData volumeData;
    [SerializeField] private MusicData musicData;

    //AUDIO
    [SerializeField] private AudioMixer audioMixer;
    [SerializeField] private RythmVisualsController rythmVisualsController;
    private AudioSource audioSource;


    private float[] Spectrum = new float[256];
    private float LimitFrequency;

    float iter = 0;

    //UI
    [SerializeField] private Slider Master;
    [SerializeField] private Slider Music;
    [SerializeField] private Slider SFX;
    [SerializeField] private GameObject AudioSettingsHolder;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        volumeData.SetInitalSlidersValues(Master, Music, SFX);

        //TESTING
        audioSource.clip = musicData.MusicArray[6];
        audioSource.Play();
    }

    void Update()
    {
        //MusicPlayer();  <- Commented for testing.
        volumeData.SetVolumeValues(audioMixer);

        if (audioSource.clip == musicData.MusicArray[6])
        {
            LimitFrequency = 2.5f;
        }

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
                if(iter % 2 == 0)
                {
                    rythmVisualsController.DecreaseOtherScale();

                }
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
}