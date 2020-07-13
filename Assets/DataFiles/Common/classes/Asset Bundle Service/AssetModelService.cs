using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;

public class AssetModelService
{
    static string fileApiURL = "https://sidiro.pl/sidiroar/api/file/me";
    static string iosFileApiURL = "https://sidiro.pl/sidiroar/api/file/ios/me";

    /// <summary>
    /// Method for downloading file asynchronously
    /// </summary>
    /// <param name="jwt">
    /// jwt of user of model
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
    public static WebClient DownloadAssembly(string jwt, string assemblyId, string filePath)
    {
        if (UserLoader.LoggedUser == null) throw new InvalidOperationException("User is not logged in!");

        using (var webClient = new WebClient())
        {
            webClient.Headers[HttpRequestHeader.ContentType] = "application/json";
            webClient.Headers["x-auth-token"] = jwt;

            #region PLATFORM_DEPENDED_CODE

            Common.RunplatformDependendCode(
                () => {
                    //Android Code

                    webClient.DownloadFileAsync(new Uri(String.Format("{0}/{1}", fileApiURL, assemblyId)), filePath);

                    return null;
                }, () =>
                {
                    //IOS Code

                    webClient.DownloadFileAsync(new Uri(String.Format("{0}/{1}", iosFileApiURL, assemblyId)), filePath);

                    return null;
                });

            #endregion PLATFORM_DEPENDED_CODE

            
            return webClient;
        }

    }
}
