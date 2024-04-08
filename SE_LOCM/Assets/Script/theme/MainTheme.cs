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
    public bool popWindow = false;
    public Wiki wiki;
    public AudioClip sfxPop;

    public void PopWinodw(int index)
    {
        PlayAudio(sfxPop, gameObject);
        popWindow = true;
        themePops[index].gameObject.SetActive(true);
        themePops[index].gameObject.transform.DOScale(1, 0.25f).From(0);
    }
    public void DisPopWindow(int index)
    {
        popWindow = false;
        themePops[index].gameObject.transform.DOScale(0, 0.25f).From(1);
    }
    public void StartGame()
    {
        string path = Application.persistentDataPath + "/users/localsave.json";
        if (File.Exists(path))
        {
            var localSaveData = LocalSaveDataManager.LoadLocalData();
            if (localSaveData.status == LocalSaveStatus.Gaming || localSaveData.status == LocalSaveStatus.Break)
            {
                PopWinodw(0);
            }
            else StartNewGame();
        }
        else StartNewGame();
    }
    public void StartNewGame()
    {
        LocalSaveDataManager.SaveInitLocalData();
        var localSaveData = LocalSaveDataManager.LoadLocalData();
        localSaveData.status = LocalSaveStatus.Gaming;
        LocalSaveDataManager.SaveLocalData(localSaveData);
        sf.FadeOut("Scenes/story/s0-00");
    }
    public void ContinueGame()
    {
        LocalSaveData localSaveData = LocalSaveDataManager.LoadLocalData();
        localSaveData.status = LocalSaveStatus.Gaming;
        LocalSaveDataManager.SaveLocalData(localSaveData);
        sf.FadeOut(NextScene(localSaveData));
    }
    public void ExitGame()
    {
#if UNITY_EDITOR
        EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
    }
    public static string NextScene(LocalSaveData localSaveData)
    {
        string nextScene = localSaveData.route[^1] switch
        {
            100 => "Scenes/story/s1/s1-02",
            101 => "Scenes/story/s1/s1-03",
            133 => "Scenes/story/s2/s2-01",
            231 => "Scenes/story/s3/s3-01",
            335 => "Scenes/story/s4/s4-01",
            431 => "Scenes/story/s5/s5-01",
            532 => "Scenes/story/s6/s6-01",
            633 => "Scenes/story/s7/s7-01",
            732 => "Scenes/story/s8/s8-01",
            833 => "Scenes/story/s9/s9-01",
            933 => "Scenes/theme",
            _ => "Scenes/map/m" + localSaveData.Level,
        };
        bool needResaving = true;
        switch (localSaveData.route[^1])
        {
            case 100: localSaveData.route.Add(101); break;
            case 101: localSaveData.route.Add(102); break;
            case 133: localSaveData.route.Add(200); break;
            case 231: localSaveData.route.Add(300); break;
            case 335: localSaveData.route.Add(400); break;
            case 431: localSaveData.route.Add(500); break;
            case 532: localSaveData.route.Add(600); break;
            case 633: localSaveData.route.Add(700); break;
            case 732: localSaveData.route.Add(800); break;
            case 833: localSaveData.route.Add(900); break;
            case 933: localSaveData.status=LocalSaveStatus.Victory; break;
            default: needResaving = false; break;
        }
        if (needResaving) LocalSaveDataManager.SaveLocalData(localSaveData);
        return nextScene;
    }
    public static void PlayAudio(AudioClip audioClip, GameObject gameObject)
    {
        AudioSource audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.clip = audioClip;
        audioSource.Play();
    }
}
