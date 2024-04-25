
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Collections.Generic;
using UnityEngine.Rendering;
using System;

public static class SaveSystem
{
   public static void SavePlayer (Player player)
   {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/player.save";
        FileStream stream = new FileStream(path, FileMode.Create);

        PlayerData data = new PlayerData(player);

        formatter.Serialize(stream, data);
        stream.Close();
   }

   public static void SaveWorld (List<ChunkData> World)
   {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/world.save";
        FileStream stream = new FileStream(path, FileMode.Create);

        formatter.Serialize(stream, World);
        stream.Close();
    }

    public static PlayerData LoadPlayer()
    {
        string path = Application.persistentDataPath + "/player.save";
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter ();
            FileStream stream = new FileStream (path, FileMode.Open);

            formatter.Deserialize(stream);

            PlayerData data = formatter.Deserialize(stream) as PlayerData;
            stream.Close();

            return data;

        } 
        else
        {
            Debug.LogError("save file not found in path " + path);
            return null;
        }
    }
    public static List<ChunkData> LoadWorld()
    {
        string path = Application.persistentDataPath + "/world.save";
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            formatter.Deserialize(stream);

            List<ChunkData> data = formatter.Deserialize(stream) as List<ChunkData>;
            stream.Close();

            return data;

        }
        else
        {
            Debug.LogError("save file not found in path " + path);
            return null;
        }
    }
}
