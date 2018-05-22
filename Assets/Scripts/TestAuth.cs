using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

//using System.Net.Http.Headers;
//using System.Text;
//using System.Net.Http;
//using System.Collections.Generic;
//using System.Threading.Tasks;
//using System.IO;
//using Newtonsoft.Json;
//using Microsoft.Graph;
//using Microsoft.Identity.Client;


public class TestAuth : MonoBehaviour {
    //认证与笔记修改类
    //public async Task<string> Auth()
    //{
    //    var Auth = new GraphAuthenticator();//创建认证实例
    //    var clientAU = new GraphServiceClient(Auth);//创建客户端代理
    //    await clientAU.Me.Request().GetAsync();//认证请求   
    //    string temp = await Auth.returnToken();
    //    //打印token
    //    Debug.Log("Token: " + temp);
    //    return temp;
    //}


    //class GraphAuthenticator : IAuthenticationProvider
    //{
    //    public string token;
    //    static DateTimeOffset Expiration;
    //    public async Task<string> returnToken()
    //    {
    //        return token;
    //    }

    //    public async Task AuthenticateRequestAsync(HttpRequestMessage request)
    //    {
    //        //申请的AppID    用的是国际版OAuth 2.0
    //        string clientID = "a08cee85-9280-4d5b-a256-2ea6ac78edf1";
    //        //权限
    //        string[] scopes = { "user.read", "mail.read", "mail.send", "notes.read", "Notes.Read.All" };
    //        var app = new PublicClientApplication(clientID);

    //        AuthenticationResult result = null;
    //        try
    //        {
    //            result = await app.AcquireTokenSilentAsync(scopes);
    //            token = result.Token;
    //        }
    //        catch (Exception)
    //        {
    //            if (string.IsNullOrEmpty(token) || Expiration <= DateTimeOffset.UtcNow.AddMinutes(5))
    //            {
    //                result = await app.AcquireTokenAsync(scopes);
    //                Expiration = result.ExpiresOn;
    //                token = result.Token;
    //            }
    //        }

    //        request.Headers.Add("Authorization", $"Bearer {token}");


    //    }
    //}



    // Use this for initialization
    void Start () {
        
        //Auth();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
