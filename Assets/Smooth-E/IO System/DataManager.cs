using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class DataManager : MonoBehaviour
{
    /* Это класс - реализация файла-сохранения */
    [System.Serializable]
    public class DataContainer
    {
        //Доюавьте сюла переменные, которые хотиете сохранять
        //Сохранять можно только ПРОСТЕЙШИЕ типы данный
        //(string, int, float, bool, byte и др., а также их массивы int[], float[] и тд.)
        //Если вы хотите сохранить например цвет - сохраняйте его как byte[3] или float[3]

        public int level = 0;
    }

    static DataManager instance;
    public static DataContainer data;
    static string filePath = "\\savefile";

    public void Awake()
    {
        if (instance != null) Destroy(this.gameObject);
        else
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
    }

    public static void SaveData()
    {
        try
        {
            var fileStram = new FileStream(Application.persistentDataPath + filePath, FileMode.OpenOrCreate);
            new BinaryFormatter().Serialize(fileStram, data);
        }
        catch (System.Exception e)
        {
            Debug.LogError(e);
        }
    }

    public static void LoadData()
    {
        try
        {
            var stream = new FileStream(Application.persistentDataPath + filePath, FileMode.Open);
            data = new BinaryFormatter().Deserialize(stream) as DataContainer;
        }
        catch (System.Exception e)
        {
            Debug.LogError(e);
            data = new DataContainer();
        }
    }
}
