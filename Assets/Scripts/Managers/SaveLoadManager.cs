using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using UnityEngine;

[System.Serializable]
public class SaveLoadManager : MonoBehaviour
{
    public static void SavePlayerData(VisualNovelManager player, string NameFile)
    {
        string path = Application.persistentDataPath + "/SaveData";
        BinaryFormatter formatter = new BinaryFormatter();

        if(!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);
        }

        FileStream stream = new FileStream(path + "/" +NameFile, FileMode.Create);

        VNMData data = new VNMData(player);

        formatter.Serialize(stream, data);
        
        stream.Close();
    }

    public static VNMData LoadPlayerData(VisualNovelManager player, string NameFile)
    {
        string path = Application.persistentDataPath + "/SaveData" + "/" + NameFile;
        if(File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();

            FileStream stream = new FileStream(path, FileMode.Open);

            // Player sett
            VNMData data = (VNMData)formatter.Deserialize(stream);
            stream.Close();

            return data;
        }
        else
        {
            return null;
        }
    }
}
