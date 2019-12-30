using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class UserLoader 
{
    private static string loggedUserLabel = "appData_currentlyLoggedUser";

    /// <summary>
    /// Currently logged user - null if there is no user logged
    /// </summary>
    private static User _loggedUser = null;
    public static User LoggedUser
    {
        get
        {
            return _loggedUser;
        }
    }

    /// <summary>
    /// Class for loading users from server
    /// </summary>
    public UserLoader()
    {
        Init();
    }

    /// <summary>
    /// Method for initializing loader
    /// </summary>
    private void Init()
    {
        //TO DO LATER
    }

    /// <summary>
    /// Method for generating asset model list based on string lists
    /// </summary>
    /// <param name="user">
    /// User of asset model loaders
    /// </param>
    /// <param name="ids">
    /// list of ids of all asset model loaders
    /// </param>
    /// <param name="names">
    /// list of names of all asset model loaders
    /// </param>
    /// <returns>
    /// Collection of generated AssetModelLoaders
    /// </returns>
    public static List<AssetModelLoader> GenerateAssetModelLoaders(User user, List<string> ids, List<string> names)
    {
        //Checking length of both - ids and names
        if (ids.Count != names.Count)
            throw new InvalidOperationException(String.Format("length of id and name collection of models has to be the same! id length: {0} name length: {1}", ids.Count, names.Count));

        //Creating and fetching list to return
        List<AssetModelLoader> listToReturn = new List<AssetModelLoader>();

        for(var i=0; i<ids.Count; i++)
        {
            var id = ids[i];
            var name = names[i];

            var assetModelLoader = new AssetModelLoader(id, name, user);
            listToReturn.Add(assetModelLoader);
        }

        return listToReturn;
    }

    /// <summary>
    /// Method for getting model name list based on given models collection
    /// </summary>
    /// <param name="models">
    /// Models collection
    /// </param>
    /// <returns>
    /// list of all names
    /// </returns>
    public static List<String> GenerateAssetModelNames(List<AssetModelLoader> models)
    {
        //Creating and fetching list to return
        List<string> listToReturn = new List<string>();

        //Has to be for - in order to ensure order together with id generation in different method
        for (var i = 0; i < models.Count; i++)
        {
            listToReturn.Add(models[i].ModelName);
        }

        return listToReturn;
    }

    /// <summary>
    /// Method for getting model id list based on given models collection
    /// </summary>
    /// <param name="models">
    /// Models collection
    /// </param>
    /// <returns>
    /// list of all ids
    /// </returns>
    public static List<String> GenerateAssetModelIds(List<AssetModelLoader> models)
    {
        //Creating and fetching list to return
        List<string> listToReturn = new List<string>();

        //Has to be for - in order to ensure order together with name generation in different method
        for (var i = 0; i < models.Count; i++)
        {
            listToReturn.Add(models[i].ID);
        }

        return listToReturn;
    }

    /// <summary>
    /// Method for generating json user data based on given user object
    /// </summary>
    /// <param name="user">
    /// user Object
    /// </param>
    /// <returns>
    /// User data in the form of JSON
    /// </returns>
    public static UserJSONData GenerateJSONDataFromUser(User user)
    {
        var jsonToReturn = new UserJSONData();
        jsonToReturn.id = user.ID;
        jsonToReturn.name = user.Name;
        jsonToReturn.modelIds = UserLoader.GenerateAssetModelIds(user.ModelList);
        jsonToReturn.modelNames = UserLoader.GenerateAssetModelNames(user.ModelList);

        return jsonToReturn;
    }

    /// <summary>
    /// Method for generating user based on json data
    /// </summary>
    /// <param name="jsonData">
    /// Json data of user
    /// </param>
    /// <param name="loader">
    /// User loader used to generate user object
    /// </param>
    /// <returns>
    /// User object
    /// </returns>
    public static User GenerateUserFromJSONData(UserJSONData jsonData, UserLoader loader)
    {
        return new User(jsonData, loader);
    }

    /// <summary>
    /// Method for converting string yo user json data
    /// </summary>
    /// <param name="jsonDataString">
    /// String containing json data
    /// </param>
    /// <returns>
    /// json data of user
    /// </returns>
    public static UserJSONData GetUserJSONDataFromString(string jsonDataString)
    {
        return JsonUtility.FromJson<UserJSONData>(jsonDataString);
    }

    /// <summary>
    /// Method for converting string yo user json data
    /// </summary>
    /// <param name="jsonDataString">
    /// object containing json data
    /// </param>
    /// <returns>
    /// json data string of user
    /// </returns>
    public static string GetStringFromUserJSONData(UserJSONData jsonData)
    {
        return JsonUtility.ToJson(jsonData);
    }

    /// <summary>
    /// Method for loggining in a user - set as currently logged
    /// </summary>
    /// <param name="user">
    /// user to log
    /// </param>
    public void LoginUser(User user)
    {
        UserLoader._loggedUser = user;
        SaveLoggedUserToPlayerPrefs();
    }

    /// <summary>
    /// Method for loggining out a user - deleting it as currently logged
    /// </summary>
    /// <param name="user">
    /// user to log
    /// </param>
    public void LogoutUser()
    {
        UserLoader._loggedUser = null;
        SaveLoggedUserToPlayerPrefs();
    }

    /// <summary>
    /// Method for loading loggedUser based on player prefs content
    /// </summary>
    public void LoginUserFromPlayerPrefs()
    {
        //Generating string from player prefs
        var jsonString = PlayerPrefs.GetString(loggedUserLabel, null);
       
        //if json is not null - user exists in player prefs
        if (!String.IsNullOrEmpty(jsonString))
        {
            //Generating json data from string
            var jsonData = UserLoader.GetUserJSONDataFromString(jsonString);

            //Generating user from json data
            var user = UserLoader.GenerateUserFromJSONData(jsonData, this);

            UserLoader._loggedUser = user;
        }
        else
        {
            //if json is null - there is no user in player prefs - set currently logged user as null
            UserLoader._loggedUser = null;
        }
    }

    /// <summary>
    /// Method for saving logged user data into player prefs
    /// </summary>
    public void SaveLoggedUserToPlayerPrefs()
    {
        if (UserLoader.LoggedUser != null)
        {
            var jsonData = UserLoader.GenerateJSONDataFromUser(UserLoader.LoggedUser);
            var jsonString = UserLoader.GetStringFromUserJSONData(jsonData);

            //Setting json string to player prefs
            PlayerPrefs.SetString(loggedUserLabel, jsonString);
        }
        else
        {
            //Removing data from player prefs if there is no logged user
            PlayerPrefs.DeleteKey(loggedUserLabel);
        }
    }

    /// <summary>
    /// Method for getting data about user from server and adjusting it in user object
    /// </summary>
    /// <param name="user">
    /// User object
    /// </param>
    public async Task RefreshUsersDataFromServer(User user)
    {
        var newUserData = await UserService.GetUserDataFromServer(user.JWT);

        if (newUserData != null)
            user.SetData(newUserData);

        //Saving all new data to player prefs - if modified user is the one currently logged in
        if (user == LoggedUser)
            SaveLoggedUserToPlayerPrefs();
    }

    /// <summary>
    /// Method for logging the user data from server
    /// </summary>
    /// <param name="id">
    /// Users id
    /// </param>
    /// <param name="password">
    /// Users password
    /// </param>
    public async Task LoginUserFromServer(string id, string password)
    {
        var jsonData = await UserService.GetUserDataFromServer(id, password);

        //Loading user from retrieved data - if it exists
        if (jsonData != null)
        {
            var newUser = new User(jsonData, this);
            LoginUser(newUser);
        }
    }

    /// <summary>
    /// Method for logging the user data from server
    /// </summary>
    /// <param name="jwt">
    /// JWT of user
    /// </param>
    public async void LoginUserFromServer(string jwt)
    {
        var jsonData = await UserService.GetUserDataFromServer(jwt);

        //Loading user from retrieved data - if it exists
        if (jsonData != null)
        {
            var newUser = new User(jsonData, this);
            LoginUser(newUser);
        }
    }

}
