using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Threading.Tasks;

public class ApplicationInitializer : MonoBehaviour
{

    private bool _appIninitalied = false;
    /// <summary>
    /// Property indicating whether application has already been initialized
    /// </summary>
    public bool AppInitialized
    {
        get
        {
            return _appIninitalied;
        }
    }


    // Start is called before the first frame update
    void Awake()
    {
        //try
        //{
            //Initializing app if not initialzied
            if (!AppInitialized) InitApp();
        //}
        //catch(Exception err)
        //{
        //    Debug.Log(err.Message);
        //}
    }

    /// <summary>
    /// Method for initializing application
    /// </summary>
    private void InitApp()
    {
        //Create directories if not exist
        if (!Directory.Exists(Common.AppDirPath)) Directory.CreateDirectory(Common.AppDirPath);

        //Create directories if not exist
        if (!Directory.Exists(Common.ModelsDirPath)) Directory.CreateDirectory(Common.ModelsDirPath);

        //Create directories if not exist
        if (!Directory.Exists(Common.DefaultUserDirPath)) Directory.CreateDirectory(Common.DefaultUserDirPath);

        //Purelty for test - remove later
        var userLoader = new UserLoader();
        userLoader.LoginUserFromPlayerPrefs();
        if(UserLoader.LoggedUser == null)
        {
            Debug.Log(null);
        }
        else
        {
            Debug.Log(UserLoader.LoggedUser.ID);
            Debug.Log(UserLoader.LoggedUser.Name);
            Debug.Log(UserLoader.LoggedUser.JWT);

            foreach(var model in UserLoader.LoggedUser.ModelList)
            {
                Debug.Log(model.ID + " - " + model.ModelName);
            }

        }

        var task = userLoader.LoginUserFromServer("witold.kolaj@siemens.com","1234");


        task.ContinueWith((a)=> {
            if (UserLoader.LoggedUser == null)
            {
                Debug.Log(null);
            }
            else
            {
                Debug.Log(UserLoader.LoggedUser.ID);
                Debug.Log(UserLoader.LoggedUser.Name);
                Debug.Log(UserLoader.LoggedUser.JWT);

                foreach (var model in UserLoader.LoggedUser.ModelList)
                {
                    Debug.Log(model.ID + " - " + model.ModelName);
                }

            }
        });

        _appIninitalied = true;
    }
}
