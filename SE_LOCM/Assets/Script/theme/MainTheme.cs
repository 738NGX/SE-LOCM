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
    
    public void PopWinodw(int index)
    {
        popWindow=true;
        themePops[index].gameObject.SetActive(true);
        themePops[index].gameObject.transform.DOScale(1,0.25f).From(0);
    }
    public void DisPopWindow(int index)
    {
        popWindow=false;
        themePops[index].gameObject.transform.DOScale(0,0.25f).From(1);
    }
    public void StartGameCheck()
    {
        string path=Application.persistentDataPath+"/users/localsave.json";
        if(File.Exists(path)) PopWinodw(0);
    }
    private void StartGame()
    {

    }
    public void ExitGame()
    {
        #if UNITY_EDITOR
            EditorApplication.isPlaying=false;
        #else
            Application.Quit();
        #endif
    }
}
