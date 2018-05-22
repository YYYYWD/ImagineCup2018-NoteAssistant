using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BtnExit : MonoBehaviour {

     void OnSelect()
    {
        //延迟退出函数以免与全局点击重复造成冲突
        Invoke("HideEditPanel", 0.1f);
    }

    void HideEditPanel()
    {
        GameObject EditPanel = GameObject.Find("ObjectManager").GetComponent<ObjectManager>().CallBackEditPanel();
        GameObject TipText = GameObject.Find("ObjectManager").GetComponent<ObjectManager>().CallBackTipText();

        EditPanel.SetActive(false);
        TipText.SetActive(true);
        Debug.Log("*******************进入截图模式*******************");
    }
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
