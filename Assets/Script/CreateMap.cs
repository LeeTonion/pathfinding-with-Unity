using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Http.Headers;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

public class CreateMap: MonoBehaviour
{
    [SerializeField] private int width;
    [SerializeField] private int height;
    
    [SerializeField] private GameObject Tile1,Tile2;
    private Tile[,] gridArray;

    private void Awake()
    {
        CreateGrid(width, height,Tile1,Tile2);

    }
    public void CreateGrid(int width, int height, GameObject Tile1, GameObject Tile2)
        {
            gridArray = new Tile[width,height];
            for (int i = 0; i < width; i++) {
                for (int j = 0; j < height; j++)
                {
                    GameObject PrefabTile ;
                    if (i % 2 == 1 && j % 2 == 0 || i % 2 == 0 && j % 2 == 1)
                    {
                        PrefabTile = GameObject.Instantiate(Tile1, new Vector2(i, j), Quaternion.identity,this.transform);
                    }
                    else
                    {
                        PrefabTile = GameObject.Instantiate(Tile2, new Vector2(i, j), Quaternion.identity,this.transform);
                    }
                Vector2 worldPoint = new Vector2(i,j); 
                gridArray[i, j] = new Tile( worldPoint,i, j,true);
            }
            }
            Camera mainCamera = Camera.main;
            if (mainCamera != null)
            {
                mainCamera.transform.position = new Vector3((float)width / 2 - 0.5f, (float)height / 2 - 0.5f, -10);

            }
        }
    public List<Tile>  Get_Tile_Neighbous( Tile Tile)
    { 
        List<Tile> List_Tile_Neighous = new List<Tile>();
        for (int x = -1; x <= 1; x++)
        {
            for (int y = -1; y <= 1; y++)
            {
                if (x == 0 && y == 0)
                {
                    continue;
                }
                int checkX = Tile.GridX + x;
                int checkY = Tile.GridY + y;
                if (checkX >= 0 && checkY >= 0 && checkY < height && checkX < width)
                {
                        List_Tile_Neighous.Add(gridArray[checkX,checkY]);
                } 
            }
        }
        return List_Tile_Neighous;
    }


    public Tile TileFormWorldPoint(Vector3 worldpos)
    {
        int X = Mathf.RoundToInt(worldpos.x);
        int Y = Mathf.RoundToInt(worldpos.y);
        return gridArray[X, Y];

    }
} 

