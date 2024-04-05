using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEditor;
using System.IO;

public class MainTheme : MonoBehaviour
{
    public SceneFader sf;
    public List<ThemePop> themePops;
    public bool popWindow=false;
    public Wiki wiki;
    public AudioClip sfxPop;
    
    public void PopWinodw(int index)
    {
        PlayAudio(sfxPop,gameObject);
        popWindow=true;
        themePops[index].gameObject.SetActive(true);
        themePops[index].gameObject.transform.DOScale(1,0.25f).From(0);
    }
    public void DisPopWindow(int index)
    {
        popWindow=false;
        themePops[index].gameObject.transform.DOScale(0,0.25f).From(1);
    }
    public void StartGame()
    {
        string path=Application.persistentDataPath+"/users/localsave.json";
        if(File.Exists(path)) PopWinodw(0);
        else StartNewGame();
    }
    public void StartNewGame()
    {
        LocalSaveDataManager.SaveInitLocalData();
        LocalSaveData localSaveData=LocalSaveDataManager.LoadLocalData();
        localSaveData.status=LocalSaveStatus.Gaming;
        LocalSaveDataManager.SaveLocalData(localSaveData);
        sf.FadeOut("Scenes/story/s0-00");
    }
    public void ContinueGame()
    {
        LocalSaveData localSaveData=LocalSaveDataManager.LoadLocalData();
        localSaveData.status=LocalSaveStatus.Gaming;
        LocalSaveDataManager.SaveLocalData(localSaveData);
        sf.FadeOut(NextScene(localSaveData));
    }
    public void ExitGame()
    {
        #if UNITY_EDITOR
            EditorApplication.isPlaying=false;
        #else
            Application.Quit();
        #endif
    }
    public static string NextScene(LocalSaveData localSaveData)
    {
        string nextScene=localSaveData.route[^1] switch 
        {
            100=>"Scenes/story/s1-02",
            _=>"Scenes/map/m"+localSaveData.Level,
        };
        bool needResaving=true;
        switch(localSaveData.route[^1])
        {
            case 100: localSaveData.route.Add(101); break;
            default: needResaving=false; break;
        }
        if(needResaving) LocalSaveDataManager.SaveLocalData(localSaveData);
        return nextScene;
    }
    public static void PlayAudio(AudioClip audioClip,GameObject gameObject)
    {
        AudioSource audioSource=gameObject.AddComponent<AudioSource>();
        audioSource.clip=audioClip;
        audioSource.Play();
    }
}
