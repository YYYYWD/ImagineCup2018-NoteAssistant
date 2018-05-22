using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Academy.HoloToolkit.Unity;
using System;

/// <summary>
/// 拖动红色框(CropBox)时运行OnNavigation系列函数
/// </summary>
public class MoveTool : MonoBehaviour {
    
    void OnNavigationStarted(Vector3 NavigationPosition)
    {
        //if (NavigationPosition.x>0)
        //{
        //    Vector3 moveVector = Vector3.zero;
        //    moveVector.x = 5;
        //    transform.position += moveVector;
        //}
        //if (NavigationPosition.x < 0)
        //{
        //    Vector3 moveVector = Vector3.zero;
        //    moveVector.x = -5;
        //    transform.position += moveVector;
        //}
        //if (NavigationPosition.y > 0)
        //{
        //    Vector3 moveVector = Vector3.zero;
        //    moveVector.y = 5;
        //    transform.position += moveVector;
        //}
        //if (NavigationPosition.y < 0)
        //{
        //    Vector3 moveVector = Vector3.zero;
        //    moveVector.y = -5;
        //    transform.position += moveVector;
        //}

    }

    void OnNavigationUpdated(Vector3 NavigationPosition)
    {
        //if (transform.localPosition.x >= -20.9 && transform.localPosition.x <= 78.5 &&
        //    transform.localPosition.y >= -12.9 && transform.localPosition.y <=12.7)
        if(System.Math.Abs(NavigationPosition.x)> System.Math.Abs(NavigationPosition.y)&&
            System.Math.Abs(NavigationPosition.y)<0.003)
        {
            if (NavigationPosition.x > 0)
            {
                Vector3 moveVector = Vector3.zero;
                if (transform.localPosition.x + 0.005 >= 20.9)
                    moveVector.x = 0;
                else
                {
                    moveVector.x = 0.005f;
                    transform.position += moveVector;
                }
            }
            if (NavigationPosition.x < 0)
            {
                Vector3 moveVector = Vector3.zero;
                if (transform.localPosition.x - 0.005 <= -20.9)
                    moveVector.x = 0;
                else
                {
                    moveVector.x = -0.005f;
                    transform.position += moveVector;
                }
            }

        }


        else { 
            if (NavigationPosition.y > 0)
            {
                Vector3 moveVector = Vector3.zero;
                if (transform.localPosition.y + 0.005 >= 14.9)
                    moveVector.y = 0;
                else
                {
                    moveVector.y = 0.005f;
                    transform.position += moveVector;
                }
            }
            if (NavigationPosition.y < 0)
            {
                Vector3 moveVector = Vector3.zero;
                if (transform.localPosition.y - 0.005 <= -14.9)
                    moveVector.y = 0;
                else
                {
                    moveVector.y = -0.005f;
                    transform.position += moveVector;
                }
            }
        }
        // else
        Debug.Log("Navi.x:" + NavigationPosition.x + ", Navi.y:" + NavigationPosition.y);
            //Debug.Log("不符合移动条件 x:"+ transform.localPosition.x+ ", y:"+transform.localPosition.y);
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
