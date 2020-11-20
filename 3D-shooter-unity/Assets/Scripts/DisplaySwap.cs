using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisplaySwap : MonoBehaviour
{
    [SerializeField] private GameObject[] CloseGameObject = null;   //list of objects that you want to set not active
    [SerializeField] private GameObject[] OpenGameObject = null;    //list of objects that you want to set active

    public void Open()
    {
        foreach (GameObject g in CloseGameObject)                   //loop every object inside the list to close
            g.SetActive(false);
        foreach (GameObject g in OpenGameObject)                    //loop every object inside the list to open
            g.SetActive(true);
    }

    public void GoMainMenu()
    {
        SoundManager.instance.PlayBGM(SoundManager.instance.BgmSounds[0].soundName);    //turn back on the background music song
        foreach (GameObject g in CloseGameObject)                                       //loop every object inside the list to close
            g.SetActive(false);
        foreach (GameObject g in OpenGameObject)                                        //loop every object inside the list to open
            g.SetActive(true);
    }

    public void ResumeGame()
    {
        foreach (GameObject g in CloseGameObject)                   //loop every object inside the list to close
            g.SetActive(false);
        foreach (GameObject g in OpenGameObject)                    //loop every object inside the list to open
            g.SetActive(true);
        Time.timeScale = 1;
    }
}
