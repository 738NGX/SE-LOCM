using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

using Fungus;

public class SceneLoader : MonoBehaviour
{
    LocalSaveData pro = LocalSaveDataManager.LoadLocalData();
    void Transmission()
    { 
        if(pro.status == LocalSaveStatus.Gaming)
      {
            SceneManager.LoadScene("xxx");//��ת����
       }
        else
      {
            SceneManager.LoadScene("xxx");//��תͼ��
       }
    }
   
    
    
    
}
