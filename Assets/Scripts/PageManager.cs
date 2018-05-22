
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.Graph;
using Microsoft.Identity.Client;
using Newtonsoft.Json;

public class PageManager : MonoBehaviour
{
    public string PageTitle;

    public string PageUrl;

    private string ImageUrl { get; set; }
    void OnSelect()
    {
        Debug.Log(PageTitle);
        UploadImage();
    }
    private async void UploadImage()
    {
        ImageUrl = GameObject.Find("ObjectManager").GetComponent<ObjectManager>().UploadImageUrl;
        await MakeRequest(PageUrl, ImageUrl);

        GameObject.Find("ObjectManager").GetComponent<ObjectManager>().CallBackPageList().SetActive(false);
        GameObject.Find("ObjectManager").GetComponent<ObjectManager>().CallBackTipText().SetActive(true);
        //在这里可以根据MakeRequest的结果修改TipText的内容
    }

    static byte[] GetImageAsByteArray(string imageFilePath)
    {
        FileStream fileStream = new FileStream(imageFilePath, FileMode.Open, FileAccess.Read);
        BinaryReader binaryReader = new BinaryReader(fileStream);
        return binaryReader.ReadBytes((int)fileStream.Length);
    }

    private static async Task<string> MakeRequest(string PageUrl, string ImageUrl)
    {
        Debug.Log("PageUrl:" + PageUrl);
        var client = new HttpClient();
        HttpResponseMessage response;


        //client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", "b4dc0737c5b442e9a3b04ae367edd9fc");//旧
        client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", "08e838b57d1341fda059065e4a2da504");//新

        //目前来说去掉也没什么问题的参数
        // Request parameters
        //queryString["language"] = "unk";
        //queryString["detectOrientation "] = "true";
        // string requestParameters = "language=en";
        var uri = "https://westcentralus.api.cognitive.microsoft.com/vision/v2.0/ocr?";// + requestParameters;


        byte[] byteData = GetImageAsByteArray(ImageUrl);
        using (var content = new ByteArrayContent(byteData))
        {
            //网络图片请MediaTypeHeaderValue("application/octet-stream")
            content.Headers.ContentType =
                new MediaTypeHeaderValue("application/octet-stream");
            Debug.Log("OCR开始");
            response = await client.PostAsync(uri, content);
            Debug.Log(response);
            string strres = (response.ToString());
            int index = strres.IndexOf("StatusCode: 401");
            if (index>-1)
            {
                Debug.Log("*************等待response出错!!!!!!!!");
                return "<p>*************等待response出错!!!!!!!!</p>";
            }

            Debug.Log("OCR完成");
            
            //接受OCR后的响应
            var responseBody = await response.Content.ReadAsStringAsync();
         
            if (responseBody == "{\"language\":\"unk\",\"orientation\":\"NotDetected\",\"textAngle\":0.0,\"regions\":[]}")
            {
                Debug.Log("html没有结果!!!");
                await NoteUpdate(PageUrl, " <p> empty </p>");
                return "<p>you did't catch nothing</p>";
            }


            else
            {
                //按照JSON的格式处理接受的响应字符串
                JsonParser parsedResponseBody = JsonConvert.DeserializeObject<JsonParser>(responseBody);
                //再把JSON格式转成html格式
                string HTMLcontent = "";
                int count = 0;
                foreach (Lines line in parsedResponseBody.Regions[0].Lines)
                {

                    if (count < 2)
                    {
                        HTMLcontent += " <p> ";
                        foreach (Words word in line.Words)
                        {
                            HTMLcontent += word.Text + " ";
                        }
                        HTMLcontent += " </p>";
                        HTMLcontent = HTMLcontent.Replace("'", " ");
                        HTMLcontent = HTMLcontent.Replace("•", " ");
                        HTMLcontent = HTMLcontent.Replace("\"", " ");

                        count++;
                    }
                    else
                    {
                        Debug.Log("两行: " + HTMLcontent);
                        NoteUpdate(PageUrl, HTMLcontent);
                        count = 0;
                        HTMLcontent = "";
                        HTMLcontent += " <p> ";
                        foreach (Words word in line.Words)
                        {
                            HTMLcontent += word.Text + " ";
                        }
                        HTMLcontent += " </p>";
                        HTMLcontent = HTMLcontent.Replace("'", " ");
                        HTMLcontent = HTMLcontent.Replace("•", " ");
                        HTMLcontent = HTMLcontent.Replace("\"", " ");
                        count++;
                    }

                }
                Debug.Log("html格式的结果是: " + HTMLcontent);

                //新建笔记请求实例

                //Update笔记
                NoteUpdate(PageUrl, HTMLcontent);
                return HTMLcontent;
            }
        }

    }


    public static async Task<string> Auth()//登陆
    {
        var Auth = new GraphAuthenticator();//创建认证实例
        var clientAU = new GraphServiceClient(Auth);//创建客户端代理
        await clientAU.Me.Request().GetAsync();//认证请求   
        string temp = await Auth.returnToken();
        //打印token
        Debug.Log("Token: " + temp);
        return temp;
    }

    /*请求更新某一笔记页*/
    private static async
    /*请求更新某一笔记页*/
    Task NoteUpdate(string PageUrl, string HTMLcontent)

    {
        //发起认证请求并获取token
        var token = await Auth();
        HttpRequestMessage request;
        //这个Uri我写死成自己笔记里的一页了,根据需要更改
        //var NoteUri =
        //    new Uri("https://graph.microsoft.com/v1.0/me/onenote/pages/0-3909680fe60b00800f0490d0b8442621!1-24EAE01D57C46DAD!2682/content");
        var NoteUpdateMethod = new HttpMethod("PATCH");

        request = new HttpRequestMessage(NoteUpdateMethod, PageUrl);
        request.Content = new StringContent("[{'target':'body','action':'append','content':'" + HTMLcontent + "'}]");
        request.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
        request.Headers.Add("Authorization", $"Bearer {token}");


        var client = new HttpClient();
        //发送请求
        var response = await client.SendAsync(request);
        //下面就是接受响应字符串和响应状态码了
        //正常状态应该是打印Null
        string responseDescription = "";

        string responseBody = await response.Content.ReadAsStringAsync();

        var parsedResponseBody = JsonConvert.DeserializeObject(responseBody);
        responseBody = JsonConvert.SerializeObject(parsedResponseBody, Formatting.Indented);

        responseDescription += Environment.NewLine + Environment.NewLine + responseBody;
        Debug.Log(response.StatusCode.ToString());
        Debug.Log(responseDescription);

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





    //**********************************************************************************
    public class JsonParser
    {
        public string Language { get; set; }
        public float TextAngle { get; set; }
        public string Orientation { get; set; }
        public List<Regions> Regions { get; set; }
    }
    public class Regions
    {
        public string Boundingbox { get; set; }
        public List<Lines> Lines { get; set; }
    }
    public class Lines
    {
        public string Boundingbox { get; set; }
        public List<Words> Words { get; set; }
    }
    public class Words
    {
        public string Boundingbox { get; set; }
        public string Text { get; set; }
    }


    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
