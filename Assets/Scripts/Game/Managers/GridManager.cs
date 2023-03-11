using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

//[ExecuteInEditMode]
[System.Serializable]
public class GridManager : MonoBehaviour
{
    [SerializeField] public Level dataLevel;
    [SerializeField] private Transform spawnedBlocksParent;
    [SerializeField] private Transform spawnedPosObjsParent;
    [SerializeField] private Transform gridCornerTransform;
    [SerializeField] private GameObject posObjPrefab;
    [SerializeField] private GameObject blockObjPrefab;
    [HideInInspector] public GameGrid myGrid;
    [HideInInspector] public Goal myGoal;
    [HideInInspector] public int moves;
    [HideInInspector] public BlockTypes[,] blockTypes = new BlockTypes[1, 1];
    [HideInInspector] public CubeTypes[,] cubeTypes = new CubeTypes[1, 1];
    [HideInInspector] public GameObject2DArray[] allBlocks;
    [HideInInspector] public GameObject2DArray[] allPosObjs;
    [HideInInspector] public Dictionary<int, int> changingColumns = new Dictionary<int, int>();//key is column index value is changing block count
    public Transform SpawnedBlocksParent
    {
        get { return spawnedBlocksParent; }
        set { spawnedBlocksParent = value; }
    }
    public void UpdateArrays()
    {
        blockTypes = new BlockTypes[myGrid.GridSizeX, myGrid.GridSizeY];
        cubeTypes = new CubeTypes[myGrid.GridSizeX, myGrid.GridSizeY];
        for (int i = 0; i < myGrid.GridSizeX; i++)
        {
            for (int j = 0; j < myGrid.GridSizeY; j++)
            {
                blockTypes[i, j] = dataLevel.GameGrid.blockTypes[i].rows[j];
                cubeTypes[i, j] = dataLevel.GameGrid.cubeTypes[i].rows[j];
            }
        }
    }
    public void PopulateBlocksData(BlockTypes[,] newBlocks, CubeTypes[,] newCubes)
    {
        blockTypes = new BlockTypes[newBlocks.GetLength(0), newBlocks.GetLength(1)];
        cubeTypes = new CubeTypes[newCubes.GetLength(0), newCubes.GetLength(1)];
        for (int i = 0; i < cubeTypes.GetLength(0); i++)
        {
            for (int j = 0; j < cubeTypes.GetLength(1); j++)
            {
                blockTypes[i, j] = newBlocks[i, j];
                cubeTypes[i, j] = newCubes[i, j];
            }
        }
    }
    public void LoadGridData()
    {
        myGrid = dataLevel.GameGrid;
        myGoal = dataLevel.goal;
        moves = dataLevel.moves;

        UpdateArrays();
    }
    public void SaveGridData()
    {
        myGrid.UpdateGridData(blockTypes, cubeTypes);
        dataLevel.moves = moves;
        SpawnStartBlocks();
        SetGridCornerSize();
    }
    public void SetGridCornerSize()
    {
        float blockSize = 0.755f;
        float gapSize = 0.1f;
        gridCornerTransform.localScale = new Vector3((blockSize * myGrid.GridSizeX) + (gapSize * (myGrid.GridSizeX)),
            (blockSize * myGrid.GridSizeY) + (gapSize * (myGrid.GridSizeY)) + 0.2f, 1);
    }
    public void SpawnStartBlocks()
    {
        int tmpChildCount = spawnedPosObjsParent.childCount;
        for (int i = 0; i < tmpChildCount; i++)
        {
            DestroyImmediate(spawnedPosObjsParent.GetChild(0).gameObject);
        }
        tmpChildCount = spawnedBlocksParent.childCount;
        for (int i = 0; i < tmpChildCount; i++)
        {
            DestroyImmediate(spawnedBlocksParent.GetChild(0).gameObject);
        }
        float blockSize = 0.4f;
        float gapSize = 0.1f;
        Vector2 startPos = new Vector2(-(((myGrid.GridSizeX * blockSize) + ((myGrid.GridSizeX - 1) * gapSize)) / 2f) + blockSize / 2f,
            (((myGrid.GridSizeY * blockSize) + ((myGrid.GridSizeY - 1) * gapSize)) / 2f) - blockSize / 2f);

        allBlocks = new GameObject2DArray[myGrid.GridSizeX];
        allPosObjs = new GameObject2DArray[myGrid.GridSizeX];
        for (int i = 0; i < myGrid.GridSizeX; i++)
        {
            allBlocks[i] = new GameObject2DArray();
            allBlocks[i].rows = new GameObject[myGrid.GridSizeY];

            allPosObjs[i] = new GameObject2DArray();
            allPosObjs[i].rows = new GameObject[myGrid.GridSizeY];

            for (int j = 0; j < myGrid.GridSizeY; j++)
            {
                Vector3 spawnPos = transform.position + new Vector3(startPos.x + (i * blockSize) + (i * gapSize),
                    startPos.y - ((j * blockSize) + (j * gapSize)), 0);
                GameObject spawnedPosObj = Instantiate(posObjPrefab, spawnPos, Quaternion.identity,
                    spawnedPosObjsParent);
                GameObject spawnedBlockObj = Instantiate(blockObjPrefab, spawnPos, Quaternion.identity,
                    spawnedBlocksParent);
                AddBlockTypeToBlockObj(spawnedBlockObj, i, j, spawnedPosObj.transform);
                allBlocks[i].rows[j] = spawnedBlockObj;
                allPosObjs[i].rows[j] = spawnedPosObj;
            }
        }

    }
    public void SetupGrid()
    {
        myGrid.UpdateGridSize();
    }
    public void AddBlockTypeToBlockObj(GameObject blockObj, int xIndex, int yIndex, Transform spawnedPosTransform)
    {
        BlockTypes curBlockType = blockTypes[xIndex, yIndex];
        CubeTypes curCubeType = cubeTypes[xIndex, yIndex];

        //BlockFactory.Reset();
        var myBlockType = BlockFactory.GetBlock(curBlockType);
        Block currentBlock = blockObj.AddComponent(myBlockType.GetType()) as Block;
        if (myBlockType is CubeBlock)
        {
            blockObj.GetComponent<CubeBlock>().cubeType = curCubeType;

        }
        currentBlock.gridIndex = new Vector2(xIndex, yIndex);
        currentBlock.target = spawnedPosTransform;
        currentBlock.SetupBlock();

    }
    public void AddNewChangingColumn(int columnIndex)
    {
        if (changingColumns.ContainsKey(columnIndex))
        {
            int tmp = changingColumns[columnIndex];
            changingColumns[columnIndex] = tmp + 1;
        }
        else
        {
            changingColumns.Add(columnIndex, 1);
        }
    }
    public void DecreaseChangingColumn(int columnIndex)
    {
        if (changingColumns.ContainsKey(columnIndex))
        {
            int tmp = changingColumns[columnIndex];
            changingColumns[columnIndex] = tmp - 1;
            if (changingColumns[columnIndex] == 0)
            {
                changingColumns.Remove(columnIndex);
            }
        }
      
    }
    private void ClearCell(int x, int y)
    {
        allBlocks[x].rows[y] = null;
    }
    private void OnEnable()
    {
        CubeBlock.cubeLeavedGridEvent += ClearCell;
        DuckBlock.duckLeavedGridEvent += ClearCell;
    }
    private void OnDisable()
    {
        CubeBlock.cubeLeavedGridEvent -= ClearCell;
        DuckBlock.duckLeavedGridEvent -= ClearCell;
    }
}
