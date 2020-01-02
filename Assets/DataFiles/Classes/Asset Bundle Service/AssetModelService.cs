using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;

public class AssetModelService
{
    static string fileApiURL = "http://localhost:5000/api/models";

    public static WebClient DownloadAssembly(string userId, string assemblyId, string filePath)
    {
        var webClient = new WebClient();

        webClient.DownloadFileAsync(new Uri(String.Format("{0}/{1}/{2}", fileApiURL,userId, assemblyId)), filePath);

        return webClient;
    }
}
