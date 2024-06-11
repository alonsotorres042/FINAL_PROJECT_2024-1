using JetBrains.Annotations;
using UnityEngine;

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
<<<<<<< HEAD
    //DATA
    [SerializeField] private MusicData musicData;
    [SerializeField] private ScenesManager scenesManager;

    //ARRAYS
    [SerializeField] private Transform[] RythmVisuals;
    [SerializeField] private AudioSource[] Band;

    //TESTING
    private float[] DrumsSpectrum = new float[256];
    private float[] BassSpectrum = new float[256];
    private float[] SynthsSpectrum = new float[256];
    private float[] VocalsSpectrum = new float[256];
    public float[][] StemSpectrums;

    private Song[] MenuSongArray;
    private Song CurrentSong;

    //SONGS
    [Header("Menu Song Arrays")]
    [SerializeField] private AudioClip[] PrettyCvnt_Array;
    private Song PrettyCvnt_Song;
    [SerializeField] private AudioClip[] ItsGraduationRight_Array;
    private Song ItsGraduationRight_Song;
    [SerializeField] private AudioClip[] TheAwakening_Array;
    private Song TheAwakening_Song;
    [SerializeField] private AudioClip[] BehindTheFallen_Array;
    private Song BehindTheFallen_Song;
=======
    [SerializeField] private Transform Bass;
    [SerializeField] private Transform Other;
    private bool IsOnBeat;
<<<<<<< Updated upstream
=======
>>>>>>> b304648aafcc9abcae60f1e408e83da850b64941
>>>>>>> Stashed changes
 
    void Awake()
    {
<<<<<<< Updated upstream
        VisualBeatRef = VisualBeat(Other, 87f);
        StartCoroutine(VisualBeatRef);
=======
<<<<<<< HEAD
        StemSpectrums = new float[][] { DrumsSpectrum, BassSpectrum, SynthsSpectrum, VocalsSpectrum };
        //Song Declarations
        //---Menu---//
        PrettyCvnt_Song = new Song(PrettyCvnt_Array[0], 0.5f, PrettyCvnt_Array[1], 1f, PrettyCvnt_Array[2], 0.75f, PrettyCvnt_Array[3], 0.2f);
        ItsGraduationRight_Song = new Song(ItsGraduationRight_Array[0], 0.75f, ItsGraduationRight_Array[1], 1.5f, ItsGraduationRight_Array[2], 1.25f, ItsGraduationRight_Array[3], 0.1f);
        TheAwakening_Song = new Song(TheAwakening_Array[0], 0.05f, TheAwakening_Array[1], 0.005f, TheAwakening_Array[2], 0.075f, TheAwakening_Array[3], 0.05f);
        BehindTheFallen_Song = new Song(BehindTheFallen_Array[0], 0.05f, BehindTheFallen_Array[1], 0.0025f, BehindTheFallen_Array[2], 0.075f, BehindTheFallen_Array[3], 0.025f);

        MenuSongArray = new Song[] { PrettyCvnt_Song, ItsGraduationRight_Song, TheAwakening_Song, BehindTheFallen_Song };
=======
        VisualBeatRef = VisualBeat(Other, 87f);
        StartCoroutine(VisualBeatRef);
>>>>>>> b304648aafcc9abcae60f1e408e83da850b64941
>>>>>>> Stashed changes
    }

    // Update is called once per frame
    void Update()
    {
<<<<<<< Updated upstream
=======
<<<<<<< HEAD
        if (!Band[0].isPlaying)
        {
            //MusicPlayer(SongArray[3]);
            if(scenesManager.GetCurrentScene() == "MainMenu")
            {
                CurrentSong = MenuSongArray[Random.Range(0, MenuSongArray.Length)];
            }
            musicData.MusicPlayer(CurrentSong, Band);
=======
>>>>>>> Stashed changes
        VisualBehaviour(Bass, 0.025f, "Left");
        VisualBehaviour(Other, 0.025f, "Right");
    }
    public IEnumerator VisualBeat(Transform victim, float bpm)
    {
        while (true)
        {
            victim.localScale = new Vector3(0.8f, 0.8f, 0.8f);
            yield return new WaitForSeconds(60f / bpm);
<<<<<<< Updated upstream
=======
>>>>>>> b304648aafcc9abcae60f1e408e83da850b64941
>>>>>>> Stashed changes
        }
        //FrequencyInteratcion(SongArray[3]);

        //TESTING
        if (Input.GetKeyDown(KeyCode.Space))
        {
            musicData.StopMusicPlayer(Band);
        }

        musicData.FrequencyInteratcion(CurrentSong, Band, RythmVisuals, StemSpectrums);
    }
}