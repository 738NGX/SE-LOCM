using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

using Fungus;

public class StorySceneLoader : MonoBehaviour
{
    private LocalSaveData localSaveData;
    public SceneFader sf;
    private void Start()
    {
        localSaveData=LocalSaveDataManager.LoadLocalData();
    }
    public void Prologue()
    {
        if(localSaveData.status==LocalSaveStatus.Gaming)
        {
            sf.FadeOut("Scenes/story/s1-01");
        }
        else
        {
            sf.FadeOut("Scenes/wiki");
        }
    }
    public void Transmission()
    {
        if(localSaveData.status==LocalSaveStatus.Gaming)
        {
            sf.FadeOut("Scenes/reward");
        }
        else
        {
            sf.FadeOut("Scenes/wiki");
        }
    }
}