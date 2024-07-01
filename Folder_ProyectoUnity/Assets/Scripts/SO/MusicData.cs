using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "MusicData", menuName = "ScriptableObjects/MusicData", order = 3)]
public class MusicData : ScriptableObject
{
    private bool OnDrums = false;
    public void FrequencyInteratcion(Song song, AudioSource[] band, Transform[] rythmVisuals, float[][] stemSpectrums)
    {
        for (int i = 0; i < band.Length; i++)
        {
            band[i].GetSpectrumData(stemSpectrums[i], 0, FFTWindow.Rectangular);
            for (int j = 0; j < stemSpectrums[i].Length; j++)
            {
                if (stemSpectrums[i][j] * 3 >= song.MinFrequencies[i])
                {
                    ModifyScale(rythmVisuals[i], stemSpectrums[i][j] * 3);
                    if(i == 0)
                    {
                        OnDrums = true;
                    }
                }
                OnDrums = false;
            }
            if (i % 2 == 0)
            {
                VisualBehaviour(rythmVisuals[i], 0.05f, "Right", 10f);
            }
            else
            {
                VisualBehaviour(rythmVisuals[i], 0.05f, "Left", 10f);
            }
        }
    }
    public void VisualBehaviour(Transform victim, float RotationSpeed, string direction, float ReScaleSpeed)
    {
        ReachRotationValue(victim, RotationSpeed, direction);
        ReachScaleValue(victim, ReScaleSpeed);
    }
    public void ReachRotationValue(Transform victim, float Speed, string direction)
    {
        if (direction == "Left")
        {
            victim.rotation *= Quaternion.Euler(0, 0, Speed);
        }
        else if (direction == "Right")
        {
            victim.rotation *= Quaternion.Euler(0, 0, -Speed);
        }
    }
    public void ReachScaleValue(Transform victim, float speed)
    {
        victim.localScale = Vector3.Lerp(victim.transform.localScale, new Vector3(1f, 1f, 1f), speed * Time.deltaTime);
    }
    public void ModifyScale(Transform victim, float scale)
    {
        scale = scale / 75f;
        victim.localScale = victim.localScale + new Vector3(scale, scale, 0);
    }
    public void MusicPlayer(Song song, AudioSource[] band, AudioSource bandSource)
    {
        for (int i = 0; i < band.Length; i++)
        {
            band[i].clip = song.Stems[i];
            band[i].Play();
        }
        try
        {
            bandSource.clip = song.Stems[1];
            bandSource.Play();
        }
        catch
        {

        }
    }
    public void StopMusicPlayer(AudioSource[] band)
    {
        for (int i = 0; i < band.Length; i++)
        {
            band[i].Stop();
        }
    }
}