using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayScene : MonoBehaviour
{
    public void MainmenuSceneTransition(string SceneName)
    {
        SceneManager.LoadScene(SceneName);
        SoundManager.instance.PlayBGM(SoundManager.instance.BgmSounds[1].soundName);
        Time.timeScale = 1;
    }

    public void GoMainMenu(string SceneName)
    {
        SceneManager.LoadScene(SceneName);
        SoundManager.instance.PlayBGM(SoundManager.instance.BgmSounds[0].soundName);
    }
}
