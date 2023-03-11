using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class NeighbourManager : MonoBehaviour
{
    [SerializeField] private GridManager gridManager;
    public static NeighbourManager Instance { get; private set; }
    List<GameObject> neighbours = new List<GameObject>();
    List<GameObject> foundedBaloons = new List<GameObject>();
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }
    public void DoSingleObjAction(Vector2 gridIndex)
    {
        //gridManager.changingColumns = new Dictionary<int, int>();
        gridManager.AddNewChangingColumn((int)gridIndex.x);
    }
    public List<GameObject> FindSameNeighbours(CubeTypes cubeType, Vector2 gridIndex)
    {
        neighbours = new List<GameObject>();
        foundedBaloons = new List<GameObject>();

        gridManager.changingColumns = new Dictionary<int, int>();
        gridManager.AddNewChangingColumn((int)gridIndex.x);
        neighbours.Add(gridManager.allBlocks[(int)gridIndex.x].rows[(int)gridIndex.y]);

        RecursiveNeighbourSearch(cubeType, gridIndex);

        return neighbours;
    }
    public List<GameObject> FindUpBlocks(Vector2 gridIndex)
    {
        List<GameObject> upBlocks = new List<GameObject>();
        for (int i = 0; i < (int)gridIndex.y; i++)
        {
            gridManager.AddNewChangingColumn((int)gridIndex.x);
            upBlocks.Add(gridManager.allBlocks[(int)gridIndex.x].rows[i]);
        }
        return upBlocks;
    }
    public List<GameObject> FindDownBlocks(Vector2 gridIndex)
    {
        List<GameObject> downBlocks = new List<GameObject>();
        for (int i = (int)gridIndex.y + 1; i < gridManager.myGrid.GridSizeY; i++)
        {
            gridManager.AddNewChangingColumn((int)gridIndex.x);
            downBlocks.Add(gridManager.allBlocks[(int)gridIndex.x].rows[i]);
        }
        return downBlocks;
    }
    public List<GameObject> FindRightBlocks(Vector2 gridIndex)
    {
        List<GameObject> rightBlocks = new List<GameObject>();
        for (int i = (int)gridIndex.x + 1; i < gridManager.myGrid.GridSizeX; i++)
        {
            gridManager.AddNewChangingColumn(i);
            rightBlocks.Add(gridManager.allBlocks[i].rows[(int)gridIndex.y]);
        }
        return rightBlocks;
    }
    public List<GameObject> FindLeftBlocks(Vector2 gridIndex)
    {
        List<GameObject> leftBlocks = new List<GameObject>();
        for (int i = 0; i < (int)gridIndex.x; i++)
        {
            gridManager.AddNewChangingColumn(i);
            leftBlocks.Add(gridManager.allBlocks[i].rows[(int)gridIndex.y]);
        }
        return leftBlocks;
    }
    public void RecursiveNeighbourSearch(CubeTypes cubeType, Vector2 gridIndex)
    {
        int x = (int)gridIndex.x;
        int y = (int)gridIndex.y;
        if (x + 1 < gridManager.allBlocks.Length)
        {
            Block curBlock = gridManager.allBlocks[x + 1].rows[y].GetComponent<Block>();
            if (curBlock is CubeBlock &&
                gridManager.allBlocks[x + 1].rows[y].GetComponent<CubeBlock>().cubeType == cubeType)
            {
                if (!neighbours.Contains(gridManager.allBlocks[x + 1].rows[y]))
                {
                    gridManager.AddNewChangingColumn(x + 1);
                    neighbours.Add(gridManager.allBlocks[x + 1].rows[y]);
                    RecursiveNeighbourSearch(cubeType, new Vector2(x + 1, y));
                }
            }
            else if (curBlock is BalloonBlock)
            {

                if (!foundedBaloons.Contains(gridManager.allBlocks[x + 1].rows[y]))
                {
                    gridManager.AddNewChangingColumn(x + 1);
                    foundedBaloons.Add(gridManager.allBlocks[x + 1].rows[y]);
                }
            }
        }
        if (y + 1 < gridManager.allBlocks[0].rows.Length)
        {
            Block curBlock = gridManager.allBlocks[x].rows[y + 1].GetComponent<Block>();
            if (curBlock is CubeBlock &&
                gridManager.allBlocks[x].rows[y + 1].GetComponent<CubeBlock>().cubeType == cubeType)
            {
                if (!neighbours.Contains(gridManager.allBlocks[x].rows[y + 1]))
                {
                    gridManager.AddNewChangingColumn(x);
                    neighbours.Add(gridManager.allBlocks[x].rows[y + 1]);
                    RecursiveNeighbourSearch(cubeType, new Vector2(x, y + 1));
                }
            }
            else if (curBlock is BalloonBlock)
            {

                if (!foundedBaloons.Contains(gridManager.allBlocks[x].rows[y + 1]))
                {
                    gridManager.AddNewChangingColumn(x);
                    foundedBaloons.Add(gridManager.allBlocks[x].rows[y + 1]);
                }
            }
        }
        if (x - 1 >= 0)
        {
            Block curBlock = gridManager.allBlocks[x - 1].rows[y].GetComponent<Block>();
            if (curBlock is CubeBlock &&
                gridManager.allBlocks[x - 1].rows[y].GetComponent<CubeBlock>().cubeType == cubeType)
            {
                if (!neighbours.Contains(gridManager.allBlocks[x - 1].rows[y]))
                {
                    gridManager.AddNewChangingColumn(x - 1);
                    neighbours.Add(gridManager.allBlocks[x - 1].rows[y]);
                    RecursiveNeighbourSearch(cubeType, new Vector2(x - 1, y));
                }
            }
            else if (curBlock is BalloonBlock)
            {

                if (!foundedBaloons.Contains(gridManager.allBlocks[x - 1].rows[y]))
                {
                    gridManager.AddNewChangingColumn(x - 1);
                    foundedBaloons.Add(gridManager.allBlocks[x - 1].rows[y]);
                }
            }
        }
        if (y - 1 >= 0)
        {
            Block curBlock = gridManager.allBlocks[x].rows[y - 1].GetComponent<Block>();
            if (curBlock is CubeBlock &&
                 gridManager.allBlocks[x].rows[y - 1].GetComponent<CubeBlock>().cubeType == cubeType)
            {
                if (!neighbours.Contains(gridManager.allBlocks[x].rows[y - 1]))
                {
                    gridManager.AddNewChangingColumn(x);
                    neighbours.Add(gridManager.allBlocks[x].rows[y - 1]);
                    RecursiveNeighbourSearch(cubeType, new Vector2(x, y - 1));
                }
            }
            else if (curBlock is BalloonBlock)
            {

                if (!foundedBaloons.Contains(gridManager.allBlocks[x].rows[y - 1]))
                {
                    gridManager.AddNewChangingColumn(x);
                    foundedBaloons.Add(gridManager.allBlocks[x].rows[y - 1]);
                }
            }
        }
    }
    public void BlowUpBallons()
    {
        foreach (GameObject balloon in foundedBaloons)
        {
            balloon.GetComponent<BalloonBlock>().Explode();
        }
        foundedBaloons = new List<GameObject>();
    }

}
