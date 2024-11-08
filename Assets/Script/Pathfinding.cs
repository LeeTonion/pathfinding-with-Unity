using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class Pathfinding : MonoBehaviour
{
    private CreateMap grid;
    private List<Tile> path;
    private bool IsMoving = false;
    private static Pathfinding instance;
    [SerializeField] private GameObject Player;
    public static Pathfinding Instance {get => instance;}
    private void Awake()
    {   
        if (instance == null)
        {
            instance = this;
        }
        else 
        {
            Destroy(gameObject); 
        }
        GameObject taomap = GameObject.Find("CreateMap");
        grid = taomap.GetComponent<CreateMap>();
    }
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mousepos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousepos.z = 0;
            LayerMask tileLayer = LayerMask.GetMask("Tile");
            Collider2D TileEnd = Physics2D.OverlapCircle(mousepos, 0.00001f, tileLayer);
            if (TileEnd != null)
            {
                Pathfinding.Instance.FindPath(Player, TileEnd.gameObject);
            }
            else
            {
                Debug.Log("Mày đang click vào đâu vậy");
            }
        }
        if (Input.GetMouseButtonDown(1)) 
        {

            Vector3 mousepos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousepos.z = 0;
            LayerMask tileLayer = LayerMask.GetMask("Tile");
            Collider2D TileEnd = Physics2D.OverlapCircle(mousepos, 0.00001f, tileLayer);
            if (TileEnd != null)
            {
                SpriteRenderer spriteRenderer = TileEnd.gameObject.GetComponent<SpriteRenderer>();
                spriteRenderer.color = (spriteRenderer.color == Color.white) ? Color.red : Color.white;
                Tile clicktile = grid.TileFormWorldPoint(TileEnd.transform.position);
                clicktile.Walkable = !clicktile.Walkable;
            }
            else
            {
                Debug.Log("Mày đang click vào đâu vậy");
            }
        }
    }
    public void FindPath(GameObject StartPos, GameObject TargetPos)
    {
        Tile StartTile = grid.TileFormWorldPoint(StartPos.transform.position);
        Tile TargetTile = grid.TileFormWorldPoint(TargetPos.transform.position);

        List<Tile> Openlist = new List<Tile>();
        HashSet<Tile> Closedlist = new HashSet<Tile>();
        Openlist.Add(StartTile);

        while (Openlist.Count > 0)
        {
            Tile currentTile = Openlist[0];

            for (int i = 0; i < Openlist.Count; i++)
            {
                if (Openlist[i].Fcost < currentTile.Fcost ||
                    (Openlist[i].Fcost == currentTile.Fcost && Openlist[i].Hcost < currentTile.Hcost))
                {
                    currentTile = Openlist[i];
                }
            }

            Openlist.Remove(currentTile);
            Closedlist.Add(currentTile);

            if (currentTile == TargetTile)
            {
                RetracePath(StartTile, TargetTile);
                return;
            }

            foreach (Tile neighbor in grid.Get_Tile_Neighbous(currentTile))
            {
                if (Closedlist.Contains(neighbor) || !neighbor.Walkable)
                    continue;

                float newMovementCostToNeighbor = currentTile.Gcost + Vector2.Distance(currentTile.PosGrid, neighbor.PosGrid);
                if (newMovementCostToNeighbor < neighbor.Gcost || !Openlist.Contains(neighbor) )
                {
                    neighbor.Gcost = newMovementCostToNeighbor;
                    neighbor.Hcost = Vector2.Distance(neighbor.PosGrid, TargetTile.PosGrid);
                    neighbor.Parent = currentTile;

                    if (!Openlist.Contains(neighbor))
                    {
                        Openlist.Add(neighbor);
                    }
                }
            }
        }
    }

    void RetracePath(Tile StartTile, Tile EndTile)
    {
        path = new List<Tile>();
        Tile currentTile = EndTile;

        while (currentTile != StartTile)
        {
            path.Add(currentTile);
            currentTile = currentTile.Parent;
        }
        path.Reverse();
        StartCoroutine(MoveAlongPath());

    }
    void OnDrawGizmos()
    {
        if (path != null)
        {
            foreach (Tile tile in path)
            {
                Gizmos.color = Color.red;
                Gizmos.DrawCube(tile.PosGrid, Vector3.one * 0.5f);
            }
        }
    }
    IEnumerator MoveAlongPath()
    {
        if (IsMoving == false)
            {           
            foreach (Tile tile in path)
                {
                    Vector3 targetPosition = tile.PosGrid;
                    while (Player.transform.position != targetPosition)
                    {
                        float step = 5* Time.deltaTime;
                    Player.transform.position = Vector3.MoveTowards(Player.transform.position, targetPosition, step);

                        IsMoving = true;
                        yield return null;
                    }
                    IsMoving = false;
                }
            
            }
    }
}



