using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class DataManager : MonoBehaviour
{
    /* ��� ����� - ���������� �����-���������� */
    [System.Serializable]
    public class DataContainer
    {
        //�������� ���� ����������, ������� ������� ���������
        //��������� ����� ������ ���������� ���� ������
        //(string, int, float, bool, byte � ��., � ����� �� ������� int[], float[] � ��.)
        //���� �� ������ ��������� �������� ���� - ���������� ��� ��� byte[3] ��� float[3]

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
