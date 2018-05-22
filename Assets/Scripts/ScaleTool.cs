using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 拖动下方的双箭头时,对CropBox进行缩放
/// </summary>
public class ScaleTool : MonoBehaviour {
    void OnNavigationStarted(Vector3 NavigationPosition)
    {
     

    }

    void OnNavigationUpdated(Vector3 NavigationPosition)
    {
        GameObject cropbox;
        cropbox = GameObject.Find("CropBox");
        RectTransform rectTransform = cropbox.GetComponent<RectTransform>();
        if (NavigationPosition.x  > 0)
        {
            cropbox.GetComponent<RectTransform>().localScale = new Vector3(
                        rectTransform.localScale.x * 1.01f,
                        rectTransform.localScale.y * 1.01f,
                        rectTransform.localScale.z * 1.0f);
            GameObject.Find("CropCam").GetComponent<Camera>().enabled = true;
            float size = GameObject.Find("CropCam").GetComponent<Camera>().orthographicSize;
            float kp = (float)1.01;
            var x = GameObject.Find("CropCam").GetComponent<Transform>().localPosition.x;
            var y = GameObject.Find("CropCam").GetComponent<Transform>().localPosition.y;
            var z = GameObject.Find("CropCam").GetComponent<Transform>().localPosition.z;
            GameObject.Find("CropCam").GetComponent<Camera>().orthographicSize = size * kp;
            //GameObject.Find("CropCam").GetComponent<Transform>().localPosition = new Vector3(x,y,1.01f*z);
            GameObject.Find("CropCam").GetComponent<Camera>().enabled = false;
        }

        if(NavigationPosition.x<0)
        {
            cropbox.GetComponent<RectTransform>().localScale = new Vector3(
            rectTransform.localScale.x * 0.99f,
            rectTransform.localScale.y * 0.99f,
            rectTransform.localScale.z * 1.0f);
            GameObject.Find("CropCam").GetComponent<Camera>().enabled = true;
            float size = GameObject.Find("CropCam").GetComponent<Camera>().orthographicSize;
            float kp = (float)0.99;
            var x = GameObject.Find("CropCam").GetComponent<Transform>().localPosition.x;
            var y = GameObject.Find("CropCam").GetComponent<Transform>().localPosition.y;
            var z = GameObject.Find("CropCam").GetComponent<Transform>().localPosition.z;
            GameObject.Find("CropCam").GetComponent<Camera>().orthographicSize = size* kp;
            //GameObject.Find("CropCam").GetComponent<Transform>().localPosition = new Vector3(x, y, z * 0.99f);
            GameObject.Find("CropCam").GetComponent<Camera>().enabled = false;
        }

    }




    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
