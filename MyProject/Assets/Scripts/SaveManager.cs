using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

public class SaveManager : MonoBehaviour
{
    public static SaveManager instance;
    public SaveData activeSave;
    public bool hasLoad;

    public void Awake()
    {
        instance = this;

        Load();
    }

    public void Save()
    {
        string dataPath = Application.persistentDataPath;
        var serializer  = new XmlSerializer(typeof(SaveData));
        var stream = new FileStream(dataPath + "/" + activeSave.saveName + ".save", FileMode.Create);
        serializer.Serialize(stream, activeSave);
        stream.Close();

        Debug.Log("Saved");
    }

    public void Load()
    {
        string dataPath = Application.persistentDataPath;
        //Debug.Log(dataPath.ToString());

        if (System.IO.File.Exists(dataPath + "/" + activeSave.saveName + ".save"))
        {
            var serializer = new XmlSerializer(typeof(SaveData));
            var stream = new FileStream(dataPath + "/" + activeSave.saveName + ".save", FileMode.Open);
            activeSave = serializer.Deserialize(stream) as SaveData;
            stream.Close();

            Debug.Log("Load");

            hasLoad = true;
        }
    }

    public void DeleteSaveData()
    {
        string dataPath = Application.persistentDataPath;

        if (System.IO.File.Exists(dataPath + "/" + activeSave.saveName + ".save"))
        {
            File.Delete(dataPath + "/" + activeSave.saveName + ".save");

            Debug.Log("Delete");
        }
    }

    public bool DetectSaveFile()
    {
        string dataPath = Application.persistentDataPath;

        if (System.IO.File.Exists(dataPath + "/" + activeSave.saveName + ".save"))
        {
            return true;
        }
        else
            return false;
    }

    [System.Serializable]
    public class SaveData
    {
        public string saveName;
        public Vector3 respawnPosition;
        public bool doorOpen;
        public int goldkey;
        public int diamondkey;
        public int greenkey;
        public int cherry;
        public int carrot;
        public int gem;
    }
}
