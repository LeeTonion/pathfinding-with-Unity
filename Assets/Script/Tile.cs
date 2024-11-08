using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.EventSystems;
[System.Serializable]
public class  Tile 
{
    private float hcost;
    private float gcost;
    private Tile parent;
    private bool walkable;
    private string name => GridX.ToString() + GridY.ToString();
    public int GridX { get; private set; }
    public int GridY{ get; private set; }
    public Vector2 PosGrid{ get; private set; }

    public float Fcost => hcost + gcost;

    public Tile(Vector2 pos,int gridx,int gridy,bool walk)
    {
       this.GridX = gridx;
        this.GridY = gridy;
        this.PosGrid = pos;
        this.walkable = walk;
    }
    public float Hcost { get => hcost; set => hcost = value; }
    public float Gcost { get => gcost; set => gcost = value; }
    public Tile Parent { get => parent; set => parent = value; }

    public bool Walkable { get => walkable ; set => walkable =value ; }



}