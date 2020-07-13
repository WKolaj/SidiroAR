using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

public class UserService
{
    static string authApiURL = "https://sidiro.pl/sidiroar/api/auth";
    static string userApiURL = "https://sidiro.pl/sidiroar/api/user/me";

    /// <summary>
    /// Method for getting user data from server
    /// </summary>
    /// <param name="jwt">
    /// JWT of currently logged user
    /// </param>
    /// <returns>
    /// user data as JSON (string)
    /// </returns>
    public static async Task<UserJSONData> GetUserDataFromServer(string jwt)
    {
        using (var webClient = new WebClient())
        {
            webClient.Headers[HttpRequestHeader.ContentType] = "application/json";
            webClient.Headers["x-auth-token"] = jwt;

            var testJSONUserData = await webClient.DownloadStringTaskAsync(UserService.userApiURL);

            var userJsonObject = UserLoader.GetUserJSONDataFromString(testJSONUserData);
            return userJsonObject;

        }
    }

    /// <summary>
    /// Method for getting user data from server
    /// </summary>
    /// <param name="email">
    /// Email of user
    /// </param>
    /// <param name="password">
    /// User's password
    /// </param>
    /// <returns>
    /// user data as JSON (string)
    /// return null in case id or password is invalid
    /// </returns>
    public static async Task<UserJSONData> GetUserDataFromServer(string email, string password)
    {
        using (var webClient = new WebClient())
        {
            //Creating Object to parse to JSON content based on given credentials
            var newAuthObject = new AuthJSONData();
            newAuthObject.email = email;
            newAuthObject.password = password;

            //Generating JSON content
            var jsonAuthObject = JsonUtility.ToJson(newAuthObject);

            webClient.Headers[HttpRequestHeader.ContentType] = "application/json";

            var testJSONUserData = await webClient.UploadStringTaskAsync(UserService.authApiURL,jsonAuthObject);

            var userJsonObject = UserLoader.GetUserJSONDataFromString(testJSONUserData);
            return userJsonObject;

        }
    }
}
