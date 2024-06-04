using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using TMPro;
using System;
using UnityEngine.Assertions.Must;

public class UIManager : MonoBehaviour
{
    //DATA
    [SerializeField] private EventManagerData eventManagerData;

    //GENERAL
    [SerializeField] private Slider BossLife;

    //PAUSE
    private bool IsPaused;
    [SerializeField] private GameObject PauseHolder;
    [SerializeField] private GameObject ScreenBackground;
    [SerializeField] private GameObject PausePanel;

    //VOLUME|
    [SerializeField] private Slider Master;
    [SerializeField] private Slider Music;
    [SerializeField] private Slider SFX;

    //PUBLIC GETTERS
    public bool _isPaused { get { return IsPaused; } private set { } }
    public float _master { get { return Master.value; } private set { } }
    public float _music { get { return Music.value; } private set { } }
    public float _sfx { get { return SFX.value; } private set { } }

    // Start is called before the first frame update
    void Start()
    {
        if(eventManagerData._ScenesManager.GetCurrentScene() == "Game")
        {
            SetGameSettings();
        }
        SetInitalSlidersValues();
        Master.onValueChanged.AddListener(delegate { eventManagerData._VolumeData.UpdateValue("Master", _master); });
        Music.onValueChanged.AddListener(delegate { eventManagerData._VolumeData.UpdateValue("Music", _music); });
        SFX.onValueChanged.AddListener(delegate { eventManagerData._VolumeData.UpdateValue("SFX", _sfx); });
    }

    // Update is called once per frame
    void Update()
    {
        BossLife.value = Mathf.Lerp(BossLife.value, eventManagerData.Triceratops._currentLife, 10f * Time.deltaTime);
    }
    public void OnPause(InputAction.CallbackContext context)
    {
        if(context.performed)
        {
            if(IsPaused == false)
            {
                IsPaused = true;
                Pause(IsPaused);
            }
            else if(IsPaused == true)
            {
                IsPaused = false;
                Pause(IsPaused);
            }
        }
    }
    public void RectMove(RectTransform Victim, RectTransform target)
    {
        PausePanel.GetComponent<RectTransform>().position = Vector3.Lerp(PausePanel.GetComponent<RectTransform>().position, target.position, 20f * Time.deltaTime);
    }
    public void SetInitalSlidersValues()
    {
        Master.value = eventManagerData._VolumeData.Master;
        Music.value = eventManagerData._VolumeData.Music;
        SFX.value = eventManagerData._VolumeData.SFX;
    }
    public void Pause(bool isPaused)
    {
        if (isPaused)
        {
            PauseHolder.SetActive(true);
            Time.timeScale = 0;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            PauseHolder.SetActive(false);
            Time.timeScale = 1;
        }
    }
    public void SetGameSettings()
    {
        IsPaused = false;
        Pause(IsPaused);
        BossLife.maxValue = eventManagerData.Triceratops._maxLife;
        BossLife.value = eventManagerData.Triceratops._currentLife;
    }
}