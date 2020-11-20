using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundSetting : MonoBehaviour
{
    [SerializeField] private Slider soundslider = null;
    [SerializeField] private Text volumeText = null;

    public bool isSFXAdjust = false;

    //update the ui display of the current music volume
    private void Start()
    {
        if (SoundManager.instance != null)
        {
            if (isSFXAdjust)
            {
                soundslider.value = SoundManager.instance.baseSFXSoundVol;
                volumeText.text = (int)(soundslider.value * 100f) + "%";
            }
            else
            {
                Debug.Log(soundslider.value);
                soundslider.value = SoundManager.instance.baseBGMSoundVol;
                volumeText.text = (int)(soundslider.value * 100f) + "%";
            }
        }
    }

    //update the ui display of the current music volume
    private void Update()
    {
        if (SoundManager.instance != null)
        {
            if (isSFXAdjust)
            {
                soundslider.value = SoundManager.instance.baseSFXSoundVol;
                volumeText.text = (int)(soundslider.value * 100f) + "%";
            }
            else
            {
                Debug.Log(soundslider.value);
                soundslider.value = SoundManager.instance.baseBGMSoundVol;
                volumeText.text = (int)(soundslider.value * 100f) + "%";
            }
        }
    }
    //update the volume in bgm list
    public void changeBGMVolume()
    {
        SoundManager.instance.UpdateBGMVolume(soundslider.value);
    }
    //update the volume in song list
    public void changeSFXVolume()
    {
        SoundManager.instance.UpdateSFXVolume(soundslider.value);
    }
}