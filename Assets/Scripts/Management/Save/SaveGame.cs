using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveGame
{

    //serialized
    public string PlayerName = "Player";
    public int XP = 0;
    public int whichPart;
    private static string _gameDataFileName = "data.json";

    private static SaveGame _instance;
    public static SaveGame Instance
    {
        get
        {
            if (_instance == null)
                Load();
            return _instance;
        }

    }

    public static void Save()
    {
        FileManager.Save(_gameDataFileName, _instance);
    }

    public static void Load()
    {
        _instance = FileManager.Load<SaveGame>(_gameDataFileName);
    }

}
