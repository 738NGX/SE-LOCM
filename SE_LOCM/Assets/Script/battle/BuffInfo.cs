using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;

public class BuffInfo : MonoBehaviour
{
    public GameController gc;
    public GameObject page;
    public GameObject buffTemplate;
    public TextMeshProUGUI infoName;

    public void OpenPage(int index)
    {
        if(gc.dc.isOpeningPage) return;
        gc.dc.isOpeningPage=true;
        page.SetActive(true);
        infoName.text=index==-1 ? "侑" : gc.enemies[index].displayName.text;
        List<Buff> buffs=index==-1 ? gc.player.buffContainer.buffs : gc.enemies[index].buffContainer.buffs;
        foreach(var buff in buffs)
        {
            var obj=Instantiate(buffTemplate,transform);
            obj.SetActive(true);
            obj.transform.Find("状态名称").GetComponent<TextMeshProUGUI>().text=buff.name;
            obj.transform.Find("状态说明").GetComponent<TextMeshProUGUI>().text=buff.Effect;
            obj.transform.Find("Image").GetComponent<Image>().sprite=buff.Style switch
            {
                BuffStyle.Positive=>Resources.Load<Sprite>("UI/battle/intend_buff"),
                BuffStyle.Negative=>Resources.Load<Sprite>("UI/battle/intend_debuff"),
                _=>Resources.Load<Sprite>("UI/battle/intend_sleep"),
            };
        }
    }
    public void ClosePage()
    {
        gc.dc.isOpeningPage=false;
        foreach(Transform child in transform)
        {
            if(!child.gameObject.activeInHierarchy) continue;
            Destroy(child.gameObject);
        }
        page.SetActive(false);
    }
}
