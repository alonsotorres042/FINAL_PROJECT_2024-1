using UnityEngine;
using System.Collections;

public class Song
{
    public Song(AudioClip drums, float drumsFrequency, AudioClip bass, float bassFrequency, AudioClip synths, float synthsFrequency, AudioClip vocals, float vocalFrequency)
    {
        Stems = new AudioClip[] {drums, bass, synths, vocals};
        MinFrequencies = new float[] {drumsFrequency, bassFrequency, synthsFrequency, vocalFrequency};
    }
    public AudioClip[] Stems;
    public float[] MinFrequencies;
}
public class RythmVisualsController : MonoBehaviour
{
    //DATA
    [SerializeField] private MusicData musicData;
    [SerializeField] private ScenesManager scenesManager;

    //ARRAYS
    [SerializeField] private Transform[] RythmVisuals;
    [SerializeField] private AudioSource[] Band;

    //SPECTRUMS
    private float[] DrumsSpectrum = new float[256];
    private float[] BassSpectrum = new float[256];
    private float[] SynthsSpectrum = new float[256];
    private float[] VocalsSpectrum = new float[256];
    public float[][] StemSpectrums;


    //SONG CENTER
    private Song CurrentSong;
    private Song[] MenuSongArray;

    [Header("Menu Song Arrays")]
    [SerializeField] private AudioClip[] PrettyCvnt_Array;
    private Song PrettyCvnt_Song;
    [SerializeField] private AudioClip[] ItsGraduationRight_Array;
    private Song ItsGraduationRight_Song;
    [SerializeField] private AudioClip[] TheAwakening_Array;
    private Song TheAwakening_Song;
    [SerializeField] private AudioClip[] BehindTheFallen_Array;
    private Song BehindTheFallen_Song;

    [Header("Gameplaye Song Array")]
    [SerializeField] private AudioClip[] newSonog_Array;
    private Song newSonog_Song;

    void Awake()
    {
        StemSpectrums = new float[][] { DrumsSpectrum, BassSpectrum, SynthsSpectrum, VocalsSpectrum };
        //Song Declarations
        //---Menu---//
        PrettyCvnt_Song = new Song(PrettyCvnt_Array[0], 0.5f, PrettyCvnt_Array[1], 1f, PrettyCvnt_Array[2], 0.75f, PrettyCvnt_Array[3], 0.2f);
        ItsGraduationRight_Song = new Song(ItsGraduationRight_Array[0], 0.75f, ItsGraduationRight_Array[1], 1.5f, ItsGraduationRight_Array[2], 1.25f, ItsGraduationRight_Array[3], 0.1f);
        TheAwakening_Song = new Song(TheAwakening_Array[0], 0.05f, TheAwakening_Array[1], 0.005f, TheAwakening_Array[2], 0.075f, TheAwakening_Array[3], 0.05f);
        BehindTheFallen_Song = new Song(BehindTheFallen_Array[0], 0.05f, BehindTheFallen_Array[1], 0.0025f, BehindTheFallen_Array[2], 0.075f, BehindTheFallen_Array[3], 0.025f);

        MenuSongArray = new Song[] { PrettyCvnt_Song, ItsGraduationRight_Song, TheAwakening_Song, BehindTheFallen_Song };
    }

    // Update is called once per frame
    void Update()
    {
        if (!Band[0].isPlaying)
        {
            //MusicPlayer(SongArray[3]);
            if (scenesManager.GetCurrentScene() == "MainMenu")
            {
                CurrentSong = MenuSongArray[Random.Range(0, MenuSongArray.Length)];
            }
            else if (scenesManager.GetCurrentScene() == "Game")
            {
                CurrentSong = MenuSongArray[Random.Range(0, MenuSongArray.Length)];
            }
            try
            {
                musicData.MusicPlayer(CurrentSong, Band);
            }
            catch
            {

            }
        }

        if (Input.GetKeyDown(KeyCode.O))
        {
            try
            {
                musicData.StopMusicPlayer(Band);
            }
            catch
            {

            }
        }
        try
        {
            musicData.FrequencyInteratcion(CurrentSong, Band, RythmVisuals, StemSpectrums);
        }
        catch
        {

        }
    }
}