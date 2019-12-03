﻿using System.Collections;
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
    private List<List<int>> floorList = new List<List<int>>();

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
        SpawnMap();
    }

    void SpawnMap()
    {
        for(int i = 0; i < mapSize; i++)
        {
            for (int j = 0; j < mapSize; j++)
            {
                if (Floor != myPrefab)
                {
                    Floor = myPrefab;
                }
                else
                {
                Floor = myPrefab2;
                }
                Instantiate(Floor, new Vector3((i * x) - (mapSize), y, (j * z) - (mapSize)), Quaternion.Euler(90.0f, 0.0f, 0.0f));
            }

        }


    }


//    void SpawnMap()
//    {
//            int i = 0;
//            path = Application.dataPath + "/Map" + i + ".txt";
//        string loadedLine = "";
//        while (File.Exists(path))
//        {
//            StreamReader sReader = new StreamReader(path);

//            // Create File if path isnt found 
//            if (!File.Exists(path))
//            {
//                //break;
//            }
//            z = i;
//            loadedLine = sReader.ReadLine();
//            x = int.Parse(loadedLine);
//            loadedLine = sReader.ReadLine();
//            y = int.Parse(loadedLine);

//            for (int j = 0; j < x; j++)
//            {
//                floorList.Add(new List<int>());
//                loadedLine = sReader.ReadLine();
//                for (int k = 0; k < y; k++)
//                {
//                    //char breakLine = loadedLine[i];
//                    //floorList[k].Add(loadedLine[k] - '0');

//                    // Instantiate at position (0, 0, 0) and zero rotation.
//                    Instantiate(myPrefab, new Vector3(0, 0, 0), Quaternion.identity);

//                }
//            }

//sReader.Close();
//            i++;

            
//        }
        


//    }


}
