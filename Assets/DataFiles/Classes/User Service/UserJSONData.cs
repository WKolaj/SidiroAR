using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class UserJSONData 
{
    public string id;
    public string name;
    public string jwt;
    public List<string> modelIds;
    public List<string> modelNames;
}
