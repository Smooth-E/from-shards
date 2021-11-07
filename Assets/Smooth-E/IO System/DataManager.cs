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
    //Текст-филлер
    const string loremIpsumShort = "Lorem ipsum dolor sit amet...";
    const string loremIpsumLong = "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum.";

    [System.Serializable]
    public class Item
    {
        public string name = loremIpsumShort;
        public Sprite icon;
        public string description = loremIpsumLong;
        public string conversationSnippet = loremIpsumShort;
    }

    [System.Serializable]
    public class LevelBundle
    {
        public string chapterName = loremIpsumShort;
        public Sprite mainArt;
        public Item[] items = new Item[5];
        public string chapterStory = loremIpsumLong;
    }

    public static DataManager instance;
    public static DataContainer data;
    static string filePath = "\\savefile";

    public LevelBundle[] levels = new LevelBundle[4];

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
