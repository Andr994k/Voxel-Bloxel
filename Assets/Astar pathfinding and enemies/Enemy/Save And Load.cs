using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public static class SaveAndLoad
{
   

    public static void SaveEnemy(EnemyMechanics enemyMechanics)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/Enemies.NotFun";
        FileStream stream = new FileStream(path, FileMode.Create);


        EnemyData data = new EnemyData(enemyMechanics);


        formatter.Serialize(stream, data);
        stream.Close();
    }

    public static EnemyData LoadEnemies()
    {
        string path = Application.persistentDataPath + "/Enemies.NotFun";
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            EnemyData data = formatter.Deserialize(stream) as EnemyData;
            stream.Close();

            return data;
        }
        else
        {
            Debug.Log("save file not found in " + path);
            return null;
        }

    }


}
