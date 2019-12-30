using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

public class UserService
{
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
        //CALCULATION MUST BE PREFORM ON BASIS OF JWT 
        //BY CALLING ENDPOINT EG. /me - every user can get info only about themselves

        //Code below purely for testing - remove all if else stament after

        //Simulating network delay
        await Task.Delay(2000);

        if (jwt == "simpleTestJWTString")
        {
            var testJSONUserData = "{\"id\":\"witold.kolaj@siemens.com\",\"name\":\"Witold Kolaj\",\"jwt\":\"simpleTestJWTString\",\"modelIds\":[\"RGnn\",\"OneCubicle\",\"8MF\"],\"modelNames\":[\"Switchboard RGnn\",\"One Cubicle\",\"Switchboard 8MF\"]}";
            var userJsonObject = UserLoader.GetUserJSONDataFromString(testJSONUserData);
            return userJsonObject;
        }
        else
        {
            return null;
        }

    }

    /// <summary>
    /// Method for getting user data from server
    /// </summary>
    /// <param name="id">
    /// Id of user
    /// </param>
    /// <param name="password">
    /// User's password
    /// </param>
    /// <returns>
    /// user data as JSON (string)
    /// return null in case id or password is invalid
    /// </returns>
    public static async Task<UserJSONData> GetUserDataFromServer(string id, string password)
    {
        //SECOND WAY TO GATHER ALL DATA OF USER IS BY SENDING THEIR ID AND PASSWORD ON AUTH ROUTE
        //eg. JSON with credentials -> /api/auth

        //Creating Object to parse to JSON content based on given credentials
        var newAuthObject = new AuthJSONData();
        newAuthObject.id = id;
        newAuthObject.password = password;

        //Generating JSON content
        var jsonAuthObject = JsonUtility.ToJson(newAuthObject);

        //Code below purely for testing - remove all if else stament after

        //Simulating network delay
        await Task.Delay(2000);

        if (String.Compare(jsonAuthObject,"{\"id\":\"witold.kolaj@siemens.com\",\"password\":\"1234\"}")==0)
        {
            var testJSONUserData = "{\"id\":\"witold.kolaj@siemens.com\",\"name\":\"Witold Kolaj\",\"jwt\":\"simpleTestJWTString\",\"modelIds\":[\"RGnn\",\"OneCubicle\",\"8MF\"],\"modelNames\":[\"Switchboard RGnn\",\"One Cubicle\",\"Switchboard 8MF\"]}";
            var userJsonObject = UserLoader.GetUserJSONDataFromString(testJSONUserData);
            return userJsonObject;
        }
        else
        {
            return null;
        }
    }
}
