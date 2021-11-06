using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayableSceneManager : MonoBehaviour
{

    [SerializeField] private int levelIndexDynamic = -1;
    public static int levelIndex = 0; //Это число выставляется перед запуском сцены из меню
    public static int itemsSpawnedOnStart = 3; //Количество вещей на старте уровня
    public static int maxItemLevelToSpawn = 0; //Максимальный уровень вещи, который можно спавнить (начинается с 0)

    [Header("System Information:"), Space(10)]
    public Transform playfieldParent;
    public int fieldWidth, fieldHeight;
    public GameObject itemPrefab;
    public OnItemController[,] playField;
    public Vector2[,] cellPositions;
    public static PlayableSceneManager instance;
    public Transform itemsParent;

    enum Action : int
    {
        UP = 1,
        DOWN = 2,
        LEFT = 3,
        RIGHT = 4
    };

    void Start()
    {
        instance = this;
        if (levelIndex < 0) levelIndex = levelIndexDynamic;
        fieldHeight = playfieldParent.childCount;
        fieldWidth = playfieldParent.GetChild(0).childCount;
        playField = new OnItemController[fieldWidth, fieldHeight]; // Item (x ; y)
        cellPositions = new Vector2[fieldWidth, fieldHeight];
        for (int xIndex = 0; xIndex < fieldWidth; xIndex++)
        {
            for (int yIndex = 0; yIndex < fieldHeight; yIndex++)
            {
                Vector3 actualPosition = playfieldParent.GetChild(yIndex).GetChild(xIndex).position;
                cellPositions[xIndex, yIndex] = new Vector2(actualPosition.x, actualPosition.y);
            }
        }
        for (var x = 0; x < fieldWidth; x++) for (var y = 0; y < fieldHeight; y++) playField[x, y] = null;
        for (int i = 0; i < itemsSpawnedOnStart; i++)
        {
            var position = getRandomPositionOnField();
            int x = position[0], y = position[1];
            var item = Instantiate(itemPrefab, itemsParent);
            item.transform.position = cellPositions[x, y];
            item.GetComponent<OnItemController>().id = Random.Range(0, maxItemLevelToSpawn + 1);
        }
    }

    int[] getRandomPositionOnField()
    {
        while (true)
        {
            var x = Random.Range(0, fieldWidth);
            var y = Random.Range(0, fieldHeight);
            if (playField[x, y] == null) return new int[] { x, y };
        }
    }

    void Update()
    {
        
    }
}
