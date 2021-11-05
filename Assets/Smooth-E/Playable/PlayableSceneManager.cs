using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayableSceneManager : MonoBehaviour
{

    [SerializeField] private int levelIndexDynamic = -1;
    public static int levelIndex = 0; //Это число выставляется перед запуском сцены из меню

    [Header("System Information:"), Space(10)]
    public Transform playfieldParent;
    public int fieldWidth, fieldHeight;
    public GameObject itemPrefab;

    enum Action : int
    {
        UP = 1,
        DOWN = 2,
        LEFT = 3,
        RIGHT = 4
    };

    void Start()
    {
        if (levelIndex < 0) levelIndex = levelIndexDynamic;
        fieldHeight = playfieldParent.childCount;
        fieldWidth = playfieldParent.GetChild(0).childCount;
    }

    void Update()
    {
        
    }
}
