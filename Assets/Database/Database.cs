using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using UnityEditor.U2D.Animation;
using UnityEngine;



public static class Database 
{
    public static void SaveData(Character character)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/HS";

        FileStream stream = new FileStream(path, FileMode.Create);

        CharacterData charData = new CharacterData(character);

        formatter.Serialize(stream, charData);
        stream.Close();
    }

    public static CharacterData LoadData()
    {
        string path = Application.persistentDataPath + "/HS";

        if(File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            CharacterData data = formatter.Deserialize(stream) as CharacterData;

            stream.Close();

            return data;
        } else
        {
            Debug.LogError("Error: Save file not found in " + path);
            return null;
        }
    }
}

//vttps://weeklyhow.com/how-to-save-load-game-in-unity/