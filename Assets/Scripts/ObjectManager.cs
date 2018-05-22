
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using Academy.HoloToolkit.Unity;

using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Graph;
using Newtonsoft.Json.Linq;
using Microsoft.Identity.Client;


/// <summary>
/// Start初始化用于存储编辑框和提示字符
/// </summary>
public class ObjectManager : MonoBehaviour
{
    public GameObject EditPanel;
    public GameObject TipText;
    public GameObject PagesList;
    private Sprite myFirstImage;
    public Sprite UploadImage;
    public string UploadImageUrl { get; set; }
    // Use this for initialization
    void Start()
    {
        

        EditPanel = GameObject.Find("EditPanel");
        TipText = GameObject.Find("TipText");
        PagesList = GameObject.Find("PageManager"); //PageManager

        EditPanel.SetActive(false);
        TipText.SetActive(true);


       FirstAuth();


    }

    public async void FirstAuth()
    {
        await NoteAskforPages();
        //在deactive前要先填充pages
        PagesList.SetActive(false);
    }

    public async Task<string> Auth()//登陆
    {
        var Auth = new GraphAuthenticator();//创建认证实例
        var clientAU = new GraphServiceClient(Auth);//创建客户端代理
        await clientAU.Me.Request().GetAsync();//认证请求   
        string temp = await Auth.returnToken();
        //打印token
        Debug.Log("Token: " + temp);
        return temp;
    }



    /*请求获取所有笔记页*/
    private async Task NoteAskforPages()
    {
        //发起认证请求并获取token
        var token = await Auth();
        HttpRequestMessage request;
        var NoteUri = new Uri("https://graph.microsoft.com/v1.0/me/onenote/pages");
        var NoteAskforPagesMethod = new HttpMethod("GET");
        request = new HttpRequestMessage(NoteAskforPagesMethod, NoteUri);
        request.Headers.Add("Authorization", $"Bearer {token}");

        var client = new HttpClient();
        var response = await client.SendAsync(request);
        string responseDescription = "";
        //读取响应字符串
        string responseBody = await response.Content.ReadAsStringAsync();
        //转成JSON格式

        JObject myobject = JObject.Parse(responseBody);
        JArray jlist = JArray.Parse(myobject["value"].ToString());

        List<GameObject> notelist = new List<GameObject>();

        float t = 0.05f;
        //建立PageList------------------->PageManager
        GameObject PagesList = GameObject.Find("PageManager");
        //for (int i = 0; i < 6; i++)//jlist.Count
        int i = 0, displaynum = 0;
        while (displaynum < 5)
        {
            i++;
            if (jlist[i]["title"].ToString() != "")
            {
                displaynum++;
                GameObject Page = new GameObject();


                Page.transform.localScale = new Vector3(0.05f, 0.05f, 0.05f);//控制放缩
                Page.name = jlist[i]["title"].ToString();//名字

                Page.AddComponent<PageManager>();
                Page.GetComponent<PageManager>().PageUrl = jlist[i]["contentUrl"].ToString();//赋值
                Page.GetComponent<PageManager>().PageTitle = jlist[i]["title"].ToString();


                Page.AddComponent<TextMesh>();
                Page.GetComponent<TextMesh>().text = jlist[i]["title"].ToString();//写标题
                Page.GetComponent<TextMesh>().fontSize = 20;//字体大小
                Page.GetComponent<TextMesh>().anchor = (TextAnchor)6;
                Page.GetComponent<TextMesh>().color = new Color(255f, 0f, 0f);

                Page.AddComponent<BoxCollider>();//碰撞机制
                var xxx = Page.GetComponent<BoxCollider>().size.x;
                var xxy = Page.GetComponent<BoxCollider>().size.y;
                Page.GetComponent<BoxCollider>().size = new Vector3(xxx, xxy, 10);
                //Page.AddComponent<Billboard>();
                //xx.AddComponent<SimpleTagalong>();

                Page.transform.parent = GameObject.Find("PageBoard").transform;
                Page.transform.localPosition = new Vector3(0f, 0f, 0f);
                Page.transform.localPosition = new Vector3(0f, t, 0f);
                //Page.GetComponent<RectTransform>().anchorMin = new Vector2(0f, 1f);
                //Page.GetComponent<RectTransform>().anchorMax = new Vector2(0f, 1f);
                //Page.GetComponent<RectTransform>().pivot = new Vector2(0f, t);
                //new Vector3(0.1f, t, 0.1f);//控制间隔
                t -= 25f;
                notelist.Add(Page);
            }
        }







    }








    class GraphAuthenticator : IAuthenticationProvider
    {
        public string token;
        static DateTimeOffset Expiration;
        public async Task<string> returnToken()
        {
            return token;
        }

        public async Task AuthenticateRequestAsync(HttpRequestMessage request)
        {
            //申请的AppID    用的是国际版OAuth 2.0
            string clientID = "a08cee85-9280-4d5b-a256-2ea6ac78edf1";
            //权限
            string[] scopes = { "user.read", "mail.read", "mail.send", "Notes.Read", "Notes.Read.All", "Notes.Create", "Notes.ReadWrite", "Notes.ReadWrite.All" };
            var app = new PublicClientApplication(clientID);

            AuthenticationResult result = null;
            try
            {
                result = await app.AcquireTokenSilentAsync(scopes);
                token = result.Token;
            }
            catch (Exception)
            {
                if (string.IsNullOrEmpty(token) || Expiration <= DateTimeOffset.UtcNow.AddMinutes(5))
                {
                    result = await app.AcquireTokenAsync(scopes);
                    Expiration = result.ExpiresOn;
                    token = result.Token;
                }
            }

            request.Headers.Add("Authorization", $"Bearer {token}");


        }
    }


    ///*****************************************************************
    public GameObject CallBackEditPanel()
    {
        return EditPanel;
    }
    public GameObject CallBackTipText()
    {
        return TipText;
    }
    public GameObject CallBackPageList()
    {
        return PagesList;
    }
    public Sprite ShotOnMainCam()
    {
        int resWidth = 1920;
        int resHeight = 1080;

        Camera camera;
        RenderTexture rt = new RenderTexture(resWidth, resHeight, 24);
        camera = GameObject.Find("Main Camera").GetComponent<Camera>();
        camera.targetTexture = rt;
        Texture2D screenShot = new Texture2D(resWidth, resHeight, TextureFormat.RGB24, false);
        camera.Render();
        RenderTexture.active = rt;
        screenShot.ReadPixels(new Rect(0, 0, resWidth, resHeight), 0, 0);
        camera.targetTexture = null;
        RenderTexture.active = null;
        Destroy(rt);


        //保存截图,第一次打开虚拟机时好像他自己还没有这个文件夹,得手动启动照相机拍一张照片才行

        byte[] bytes = screenShot.EncodeToPNG();
        string filename = "C:\\Data\\Users\\DefaultAccount\\Pictures\\Camera Roll\\screenshot_"
            + DateTime.Now.ToString("yyyy_MM_dd-HH_mm_ss") + ".jpg";

        System.IO.Directory.CreateDirectory("C:\\Data\\Users\\DefaultAccount\\Pictures\\Camera Roll");


        System.IO.File.WriteAllBytes(filename, bytes);
        Debug.Log(string.Format("Took screenshot to: {0}", filename));

        //更新myFirstImage为新截图
        myFirstImage = LoadByIO(filename);


        return myFirstImage;
    }

    public void ConvertUploadImage(string url)
    {
        UploadImage = LoadByIO(url);
        UploadImageUrl = url;
    }
    private Sprite LoadByIO(string filename)
    {
        double startTime = (double)Time.time;
        //创建文件读取流
        FileStream fileStream = new FileStream(filename, FileMode.Open, FileAccess.Read);
        fileStream.Seek(0, SeekOrigin.Begin);
        //创建文件长度缓冲区
        byte[] bytes = new byte[fileStream.Length];
        //读取文件
        fileStream.Read(bytes, 0, (int)fileStream.Length);
        //释放文件读取流

        fileStream.Dispose();
        fileStream = null;

        //创建Texture
        int width = 300;
        int height = 372;
        Texture2D texture = new Texture2D(width, height);
        texture.LoadImage(bytes);

        //创建Sprite
        Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
        // image.sprite = sprite;

        startTime = (double)Time.time - startTime;
        Debug.Log("IO加载用时:" + startTime);

        return sprite;
    }
    // Update is called once per frame
    void Update()
    {

    }
}
