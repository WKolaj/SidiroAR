using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;

public class AssetModelService
{
    static string fileApiURL = "http://192.168.0.57:5000/api/models";

    /// <summary>
    /// Method for downloading file asynchronously
    /// </summary>
    /// <param name="userId">
    /// Id of user of model
    /// </param>
    /// <param name="assemblyId">
    /// Id of assembly
    /// </param>
    /// <param name="filePath">
    /// path to save file
    /// </param>
    /// <returns>
    /// Handler to downloading action
    /// </returns>
    public static WebClient DownloadAssembly(string userId, string assemblyId, string filePath)
    {
        var webClient = new WebClient();

        webClient.DownloadFileAsync(new Uri(String.Format("{0}/{1}/{2}", fileApiURL,userId, assemblyId)), filePath);
        
        return webClient;
    }
}
