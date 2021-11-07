using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayableSceneManager : MonoBehaviour
{

    [SerializeField] private int levelIndexDynamic = -1;
    public static int levelIndex = 0;                             //Это число выставляется перед запуском сцены из меню
    public static int itemsSpawnedOnStart = 3;                    //Количество вещей на старте уровня
    public static int maxItemLevelToSpawn = 0;                    //Максимальный уровень вещи, который можно спавнить (начинается с 0)
    static public int blockedCellsOnLevel = 1 * (levelIndex + 1); //Зависимость количества блоков от номера уровня

    [Header("System Information:"), Space(10)]
    public Transform playfieldParent;
    public int fieldWidth, fieldHeight;
    public GameObject itemPrefab;
    public OnItemController[,] playField;
    public Vector2[,] cellPositions;
    public static PlayableSceneManager instance;
    public Transform itemsParent;
    public GameObject lockedCellPrefab;
    bool canPlay = true;
    public Image protoFace;
    public Text chapterNameText;

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

        protoFace.sprite = DataManager.instance.levels[levelIndex].mainArt;
        chapterNameText.text = "#" + (levelIndex + 1) + ": " + DataManager.instance.levels[levelIndex].chapterName;

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

        //var pos = cellPositions[cellPositions.GetLength(0) / 2, cellPositions.GetLength(1) / 2];
        //Camera.main.transform.position = new Vector3(pos.x, pos.y, -10);

        for (int i = 0; i < itemsSpawnedOnStart; i++)
        {
            var position = getRandomPositionOnField();
            int x = position[0], y = position[1];
            var item = Instantiate(itemPrefab, itemsParent);
            item.transform.position = cellPositions[x, y];
            item.GetComponent<OnItemController>().id = Random.Range(0, maxItemLevelToSpawn + 1);
            playField[x, y] = item.GetComponent<OnItemController>();
        }

        for (int i = 0; i < blockedCellsOnLevel; i++)
        {
            var position = getRandomPositionOnField();
            int x = position[0], y = position[1];
            var item = Instantiate(lockedCellPrefab, itemsParent);
            item.transform.position = cellPositions[x, y];
            playField[x, y] = item.GetComponent<OnItemController>();
        }
    }

    int[] getRandomPositionOnField()
    {
        bool haveEmptyCell = false;
        foreach (OnItemController controller in playField)
        {
            if (controller == null)
            {
                haveEmptyCell = true;
                break;
            }
        }
        if (haveEmptyCell)
        {
            while (true)
            {
                var x = Random.Range(0, fieldWidth);
                var y = Random.Range(0, fieldHeight);
                if (playField[x, y] == null) return new int[] { x, y };
            }
        }
        else
        {
            return null;
        }
    }

    void SpawnItem()
    {
        for (int i = 0; i < 3; i++)
        {
            var position = getRandomPositionOnField();
            if (position != null)
            {
                int x = position[0], y = position[1];
                var item = Instantiate(itemPrefab, itemsParent);
                item.transform.position = cellPositions[x, y];
                item.GetComponent<OnItemController>().id = Random.Range(0, maxItemLevelToSpawn + 1);
                playField[x, y] = item.GetComponent<OnItemController>();
            }
            else
            {
                canPlay = false;
                LostScreenController.level = levelIndex;
                SceneManagerPlayable.LoadScene(5); //Вставить сюда индекс сцены с проигрышем
                break;
            }
        }
    }

    int CheckEnd(int prevX, int prevY)
    {
        if (playField[prevX, prevY].id == 4)
        {
            DataManager.LoadData();
            DataManager.data.level += 1;
            if (DataManager.data.level > 3) DataManager.data.level = 3;
            DataManager.SaveData();
            TextScreenController.level = levelIndex;
            SceneManagerPlayable.LoadScene(4);
            return 1;
        }
        else return 0;
    }

    void Update()
    {
        if (canPlay)
        {
            if (Input.GetKeyUp(KeyCode.LeftArrow) || Input.GetKeyUp(KeyCode.A))
            {
                for (int y = 0; y < playField.GetLength(1); y++)
                {
                    int prevX = 0, prevY = 0;
                    for (int x = 0; x < playField.GetLength(0); x++)
                    {
                        if (playField[x, y] != null)
                        {
                            if (playField[prevX, prevY] == null || playField[prevX, prevY].id != playField[x, y].id
                                || playField[prevX, prevY].id == -1 || x == prevX)
                            {
                                prevY = y;
                                prevX = x;
                            }
                            else
                            {
                                playField[x, y].FadeAndDestroy();
                                //Destroy(playField[x, y].gameObject); //Потом сделать функцию, чтобы обьект исчезал красиво
                                playField[x, y] = null;
                                playField[prevX, prevY].id += 1;
                                if (CheckEnd(prevX, prevY) == 1) break;
                                prevX = x;
                                prevY = y;
                            }
                        }
                    }
                }

                //Подвинем все обьекты до упора влево
                for (int y = 0; y < playField.GetLength(1); y++)
                {
                    for (int x = 1; x < playField.GetLength(0); x++)
                    {
                        if (playField[x, y] != null && playField[x, y].id != -1)
                        {
                            while (x > 0)
                            {
                                if (playField[x - 1, y] == null)
                                {
                                    playField[x - 1, y] = playField[x, y];
                                    playField[x, y] = null;
                                }
                                else break;
                                x--;
                            }
                        }
                    }
                }

                SpawnItem();

            }

            if (Input.GetKeyUp(KeyCode.RightArrow) || Input.GetKeyUp(KeyCode.D))
            {
                for (int y = 0; y < playField.GetLength(1); y++)
                {
                    int prevX = 0, prevY = 0;
                    for (int x = playField.GetLength(0) - 1; x >= 0; x--)
                    {
                        if (playField[x, y] != null)
                        {
                            if (playField[prevX, prevY] == null || playField[prevX, prevY].id != playField[x, y].id
                                || playField[prevX, prevY].id == -1 || x == prevX)
                            {
                                prevY = y;
                                prevX = x;
                            }
                            else
                            {
                                playField[x, y].FadeAndDestroy();
                                //Destroy(playField[x, y].gameObject); //Потом сделать функцию, чтобы обьект исчезал красиво
                                playField[x, y] = null;
                                playField[prevX, prevY].id += 1;
                                if (CheckEnd(prevX, prevY) == 1) break;
                                prevX = x;
                                prevY = y;
                            }
                        }
                    }
                }

                //Подвинем все обьекты до упора вправо (?)
                for (int y = 0; y < playField.GetLength(1); y++)
                {
                    for (int x = playField.GetLength(0) - 2; x >= 0; x--)
                    {
                        if (playField[x, y] != null && playField[x, y].id != -1)
                        {
                            while (x < playField.GetLength(0) - 1)
                            {
                                if (playField[x + 1, y] == null)
                                {
                                    playField[x + 1, y] = playField[x, y];
                                    playField[x, y] = null;
                                }
                                else break;
                                x++;
                            }
                        }
                    }
                }

                SpawnItem();

            }

            if (Input.GetKeyUp(KeyCode.DownArrow) || Input.GetKeyUp(KeyCode.S))
            {
                for (int x = 0; x < playField.GetLength(0); x++)
                {
                    int prevX = 0, prevY = 0;
                    for (int y = playField.GetLength(1) - 1; y >= 0; y--)
                    {
                        if (playField[x, y] != null)
                        {
                            if (playField[prevX, prevY] == null || playField[prevX, prevY].id != playField[x, y].id
                                || playField[prevX, prevY].id == -1 || y == prevY)
                            {
                                prevY = y;
                                prevX = x;
                            }
                            else
                            {
                                playField[x, y].FadeAndDestroy();
                                //Destroy(playField[x, y].gameObject); //Потом сделать функцию, чтобы обьект исчезал красиво
                                playField[x, y] = null;
                                playField[prevX, prevY].id += 1;
                                if (CheckEnd(prevX, prevY) == 1) break;
                                prevX = x;
                                prevY = y;
                            }
                        }
                    }
                }

                //Подвинем все обьекты до упора вниз (?)
                for (int x = 0; x < playField.GetLength(1); x++)
                {
                    for (int y = playField.GetLength(0) - 2; y >= 0; y--)
                    {
                        if (playField[x, y] != null && playField[x, y].id != -1)
                        {
                            while (y < playField.GetLength(0) - 1)
                            {
                                if (playField[x, y + 1] == null)
                                {
                                    playField[x, y + 1] = playField[x, y];
                                    playField[x, y] = null;
                                }
                                else break;
                                y++;
                            }
                        }
                    }
                }

                SpawnItem();

            }


            if (Input.GetKeyUp(KeyCode.UpArrow) || Input.GetKeyUp(KeyCode.W))
            {
                for (int x = 0; x < playField.GetLength(0); x++)
                {
                    int prevX = 0, prevY = 0;
                    for (int y = 0; y < playField.GetLength(1); y++)
                    {
                        if (playField[x, y] != null)
                        {
                            if (playField[prevX, prevY] == null || playField[prevX, prevY].id != playField[x, y].id
                                || playField[prevX, prevY].id == -1 || y == prevY)
                            {
                                prevY = y;
                                prevX = x;
                            }
                            else
                            {
                                playField[x, y].FadeAndDestroy();
                                //Destroy(playField[x, y].gameObject); //Потом сделать функцию, чтобы обьект исчезал красиво
                                playField[x, y] = null;
                                playField[prevX, prevY].id += 1;
                                if (CheckEnd(prevX, prevY) == 1) break;
                                prevX = x;
                                prevY = y;
                            }
                        }
                    }
                }

                //Подвинем все обьекты до упора вниз (?)
                for (int x = 0; x < playField.GetLength(1); x++)
                {
                    for (int y = 1; y < playField.GetLength(1); y++)
                    {
                        if (playField[x, y] != null && playField[x, y].id != -1)
                        {
                            while (y > 0)
                            {
                                if (playField[x, y - 1] == null)
                                {
                                    playField[x, y - 1] = playField[x, y];
                                    playField[x, y] = null;
                                }
                                else break;
                                y--;
                            }
                        }
                    }
                }

                SpawnItem();

            }
        }
    }
}
