using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class UserJSONData 
{
    public string _id = null;
    public string email = null;
    public string name = null;
    public string jwt = null;
    public Int32 permissions = -1;
    public List<string> modelIds = null;
    public List<string> modelNames = null;
    public List<bool> filesExist = null;
}
