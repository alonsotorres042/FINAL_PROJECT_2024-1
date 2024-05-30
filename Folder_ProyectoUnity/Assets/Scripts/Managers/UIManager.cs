using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using TMPro;
using System.Collections;
using System;
using UnityEditor.Experimental.GraphView;
using Unity.VisualScripting;

public class UIManager : MonoBehaviour
{
    [SerializeField] private EventManagerData eventManagerData;

    [SerializeField] private Slider BossLife;

    private bool IsPaused;
    [SerializeField] private GameObject PauseHolder;
    [SerializeField] private GameObject ScreenBackground;
    [SerializeField] private GameObject PausePanel;

    // Start is called before the first frame update
    void Start()
    {
        IsPaused = false;
        BossLife.maxValue = eventManagerData.Triceratops._maxLife;
        BossLife.value = eventManagerData.Triceratops._currentLife;
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
    public void Pause(bool isPaused)
    {
        if (isPaused)
        {
            PauseHolder.SetActive(true);
            Time.timeScale = 0;
        }
        else
        {
            PauseHolder.SetActive(false);
            Time.timeScale = 1;
        }
    }
}