using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public static class Authentication
{
    private static List<AuthPair> loginList = new();
    private static bool authenticated = false;
    private static string _username = "DEBUG";
    private static readonly bool debug = true;




    public static bool Login(string username, string password)
    {
        LoadData();
        if (loginList.Contains(new AuthPair(username, password)))
        {
            authenticated = true;
            _username = username;
            return true;
        }
        return false;
    }

    public static void Register(string username, string password)
    {
        LoadData();
        loginList.Add(new AuthPair(username, password));
        authenticated = true;
        _username = username;
        SaveData();
    }


    private static void SaveData()
    {

        File.WriteAllText(SaveFileName(), JsonUtility.ToJson(GetAuthSaveData(), true));
    }

    private static void LoadData()
    {
        string saveContent;
        try
        {
            saveContent = File.ReadAllText(SaveFileName());
        }
        catch (FileNotFoundException)
        {
            saveContent = null;
        }

        if (saveContent is null)
        {
            loginList = new List<AuthPair>();
            SaveData();
            return;
        }

        AuthenticationSaveData savedData = JsonUtility.FromJson<AuthenticationSaveData>(saveContent);
        loginList = savedData.authPairs;
    }


    private static string SaveFileName()
    {
        return Application.persistentDataPath + "/important.hash";
    }

    public static bool CheckAuthenticated()
    {
        return authenticated;
    }

    public static bool CheckDebug()
    {
        return debug;
    }
    public static string GetCurrentUsername() { return _username; }

    public static AuthenticationSaveData GetAuthSaveData()
    {
        return new AuthenticationSaveData(loginList);
    }

}


[Serializable]
public struct AuthenticationSaveData
{
    public List<AuthPair> authPairs;

    public AuthenticationSaveData(List<AuthPair> authPairs)
    {
        this.authPairs = authPairs;
    }

}

[Serializable]
public struct AuthPair
{
    public AuthPair(string username, string password)
    {
        this.username = username;
        this.password = password;
    }

    public string username;
    public string password;
}