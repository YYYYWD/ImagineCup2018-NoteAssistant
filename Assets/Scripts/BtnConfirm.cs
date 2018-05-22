using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using OpenCVForUnity.RectangleTrack;
using OpenCVForUnity;
using Newtonsoft.Json;
//using System.Drawing;



/// <summary>
/// 点击对勾时进行框内截图,CropCam始终跟踪CropBox,所以使用CropCam截图
/// </summary>
public class BtnConfirm : MonoBehaviour {


    void OnSelect()
    {
        Debug.Log("点击确认按钮");
        //获取cropbox的各项属性
        GameObject EditPanel = GameObject.Find("ObjectManager").GetComponent<ObjectManager>().CallBackEditPanel();
        RectTransform CropBoardRect = EditPanel.GetComponent<RectTransform>();


        int resWidth = 500;
        int resHeight = 280;
        //*****************************************************************************//
        Camera camera;
        RenderTexture rt = new RenderTexture(resWidth, resHeight, 24);
        camera = GameObject.Find("CropCam").GetComponent<Camera>();
        camera.targetTexture = rt;
        Texture2D screenShot = new Texture2D(resWidth, resHeight, TextureFormat.RGB24, false);
        camera.Render();
        RenderTexture.active = rt;
        screenShot.ReadPixels(new UnityEngine.Rect(0, 0, resWidth, resHeight), 0, 0);
        camera.targetTexture = null;
        RenderTexture.active = null; // JC: added to avoid errors
        Destroy(rt);


        byte[] bytes = screenShot.EncodeToPNG();
        string filename = "C:\\Data\\Users\\DefaultAccount\\Pictures\\Camera Roll\\screenshot_"
            + DateTime.Now.ToString("yyyy_MM_dd-HH_mm_ss") + ".jpg";
       System.IO.File.WriteAllBytes(filename, bytes);
        Debug.Log(string.Format("Took screenshot to: {0}", filename));



        //传送照的照片,然后截过后的精灵就存在于ObjectManager里的UploadImage
        GameObject.Find("ObjectManager").GetComponent<ObjectManager>().ConvertUploadImage(filename);
    GameObject.Find("ObjectManager").GetComponent<ObjectManager>().CallBackPageList().SetActive(true);
    GameObject.Find("ObjectManager").GetComponent<ObjectManager>().CallBackEditPanel().SetActive(false);
    //Debug.Log("点击确认按钮");

    //Root parsedResponseBody = JsonConvert.DeserializeObject<Root>("{\"textAngle\":0,\"orientation\":\"NotDetected\",\"language\":\"en\",\"regions\":[{\"boundingBox\":\"40,155,1827,867\",\"lines\":[{\"boundingBox\":\"558,155,365,197\",\"words\":[{\"boundingBox\":\"558,155,365,197\",\"text\":\"position\"}]},{\"boundingBox\":\"40,305,475,223\",\"words\":[{\"boundingBox\":\"40,305,475,223\",\"text\":\"Changing\"}]},{\"boundingBox\":\"349,507,179,121\",\"words\":[{\"boundingBox\":\"349,507,179,121\",\"text\":\"Out.\"}]},{\"boundingBox\":\"88,603,229,131\",\"words\":[{\"boundingBox\":\"88,625,134,109\",\"text\":\"SAO\"},{\"boundingBox\":\"220,603,97,74\",\"text\":\"V\"}]},{\"boundingBox\":\"685,917,1182,105\",\"words\":[{\"boundingBox\":\"685,917,494,105\",\"text\":\"Checking\"},{\"boundingBox\":\"1249,920,618,85\",\"text\":\"Connection\"}]}]}]}");
    ////再把JSON格式转成html格式
    //string HTMLcontent = "";

    //foreach (RegionsItem region in parsedResponseBody.regions)
    //{
    //    ArrayList box = new ArrayList(region.boundingBox.Split(','));
    //    int x = Convert.ToInt32(box[0]);
    //    int y = Convert.ToInt32(box[1]);
    //    int sizex = Convert.ToInt32(box[2]);
    //    int sizey = Convert.ToInt32(box[3]);

    //    // Rectangle rect = new Rectangle(new Point(x, y), new Size(sizex, sizey));
    //    Debug.Log("region" + region.boundingBox);
    //    //  CvInvoke.Rectangle(image, rect, new MCvScalar(0, 255, 0), 5);
    //}


    //foreach (LinesItem line in parsedResponseBody.regions[0].lines)
    //{
    //    ArrayList box = new ArrayList(line.boundingBox.Split(','));
    //    int x = Convert.ToInt32(box[0]);
    //    int y = Convert.ToInt32(box[1]);
    //    int sizex = Convert.ToInt32(box[2]);
    //    int sizey = Convert.ToInt32(box[3]);

    //    //Rectangle rect = new Rectangle(new Point(x, y), new Size(sizex, sizey));
    //    Debug.Log("line" + line.boundingBox);
    //    // CvInvoke.Rectangle(image, rect, new MCvScalar(0, 0, 255), 30);


    //    HTMLcontent += " <p> ";
    //    foreach (WordsItem word in line.words)
    //    {
    //        ArrayList boxx = new ArrayList(word.boundingBox.Split(','));
    //        int xx = Convert.ToInt32(boxx[0]);
    //        int yy = Convert.ToInt32(boxx[1]);
    //        int sizexx = Convert.ToInt32(boxx[2]);
    //        int sizeyy = Convert.ToInt32(boxx[3]);

    //        //Rectangle rectt = new Rectangle(new Point(xx, yy), new Size(sizexx, sizeyy));
    //        Debug.Log("word" + word.boundingBox);



    //        HTMLcontent += word.text + " ";
    //    }
    //    HTMLcontent += " </p>";


    //}
    //Debug.Log("html格式的结果是: " + HTMLcontent);


    ////********************************************************************//
    ////获取cropbox的各项属性
    //    GameObject EditPanel = GameObject.Find("ObjectManager").GetComponent<ObjectManager>().CallBackEditPanel();
    //RectTransform CropBoardRect = EditPanel.GetComponent<RectTransform>();


    //int resWidth = 500;
    //int resHeight = 280;
    ////*****************************************************************************//
    //Camera camera;
    //RenderTexture rt = new RenderTexture(resWidth, resHeight, 24);
    //camera = GameObject.Find("CropCam").GetComponent<Camera>();
    //camera.targetTexture = rt;
    //Texture2D screenShot = new Texture2D(resWidth, resHeight, TextureFormat.RGB24, false);
    //camera.Render();
    //RenderTexture.active = rt;
    //screenShot.ReadPixels(new UnityEngine.Rect(0, 0, resWidth, resHeight), 0, 0);
    //camera.targetTexture = null;
    //RenderTexture.active = null; // JC: added to avoid errors
    //Destroy(rt);


    //byte[] bytes = screenShot.EncodeToPNG();
    //string filename = "C:\\Data\\Users\\DefaultAccount\\Pictures\\Camera Roll\\screenshot_"
    //    + DateTime.Now.ToString("yyyy_MM_dd-HH_mm_ss") + ".jpg";
    //System.IO.File.WriteAllBytes(filename, bytes);
    //Debug.Log(string.Format("Took screenshot to: {0}", filename));



    ////传送照的照片,然后截过后的精灵就存在于ObjectManager里的UploadImage
    //GameObject.Find("ObjectManager").GetComponent<ObjectManager>().ConvertUploadImage(filename);
    //GameObject.Find("ObjectManager").GetComponent<ObjectManager>().CallBackPageList().SetActive(true);
    //GameObject.Find("ObjectManager").GetComponent<ObjectManager>().CallBackEditPanel().SetActive(false);

}

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    //如果好用，请收藏地址，帮忙分享。
    public class WordsItem
    {
        /// <summary>
        /// 
        /// </summary>
        public string boundingBox { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string text { get; set; }
    }

    public class LinesItem
    {
        /// <summary>
        /// 
        /// </summary>
        public string boundingBox { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public List<WordsItem> words { get; set; }
    }

    public class RegionsItem
    {
        /// <summary>
        /// 
        /// </summary>
        public string boundingBox { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public List<LinesItem> lines { get; set; }
    }

    public class Root
    {
        /// <summary>
        /// 
        /// </summary>
        public int textAngle { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string orientation { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string language { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public List<RegionsItem> regions { get; set; }
    }
}
