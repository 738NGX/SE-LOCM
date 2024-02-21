using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

using Fungus;

public class SceneLoader : MonoBehaviour
{
    LocalSaveData save=LocalSaveDataManager.LoadLocalData();
    void Transmission()
    {
        if(save.status==LocalSaveStatus.Gaming)
        {
            SceneManager.LoadScene("xxx");//跳转剧情
        }
        else
        {
            SceneManager.LoadScene("xxx");//跳转图鉴
        }
    }




}
