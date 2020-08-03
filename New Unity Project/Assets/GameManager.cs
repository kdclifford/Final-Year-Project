using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class GameManager : MonoBehaviour
{
    //Creating one instance of Game Manager...Singleton
    public static GameManager instance;
    public string path;
    public bool isMapSpawned;
    public int mapSize;


    public int x, y, z;
    //private List<List<int>> floorList = new List<List<int>>();
    //private List<List<GameObject>> floorList = new List<List<GameObject>>();
    public GameObject[,] floorList;

    // Reference to the Prefab. Drag a Prefab into this field in the Inspector.
    public GameObject myPrefab;
    public GameObject myPrefab2;
    private GameObject Floor;

    //private int[, ,] floorList = new int[z, x, y];
    void Awake()
    {
        MakeSingleton();
    }
    private void MakeSingleton()
    {
        if (instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    void Start()
    {
        floorList = new GameObject[mapSize, mapSize];
        SpawnMap();
    }

    void SpawnMap()
    {
        for (int i = 0; i < mapSize; i++)
        {
            for (int j = 0; j < mapSize; j++)
            {
                if ((j + i) % 2 == 0)
                {
                    Floor = myPrefab;
                }
                else
                {
                    Floor = myPrefab2;
                }
                float tileSize = ((((x + z) / 2) * mapSize) - 3) / 2;

                floorList[i, j] = (GameObject)Instantiate(Floor, new Vector3((i * x) - (tileSize), y, (j * z) - (tileSize)), Quaternion.Euler(90.0f, 0.0f, 0.0f));
            }
        }
    }
    private void Update()
    {
   
        
    }

}
