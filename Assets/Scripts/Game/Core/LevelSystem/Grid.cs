using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class GameGrid
{
    [SerializeField] private Vector2 gridSize;

    public CubeTypes2DArray[] cubeTypes;
    public BlockTypes2DArray[] blockTypes;

    public int GridSizeX
    {
        get { return (int)gridSize.x; }
        set { gridSize.x = value; }
    }
    public int GridSizeY
    {
        get { return (int)gridSize.y; }
        set { gridSize.y = value; }
    }
    public void UpdateGridSize()
    {
        cubeTypes = new CubeTypes2DArray[(int)gridSize.x];
        blockTypes = new BlockTypes2DArray[(int)gridSize.x];
        for (int i = 0; i < cubeTypes.Length; i++)
        {
            cubeTypes[i] = new CubeTypes2DArray();
            cubeTypes[i].rows = new CubeTypes[(int)gridSize.y];

            blockTypes[i] = new BlockTypes2DArray();
            blockTypes[i].rows = new BlockTypes[(int)gridSize.y];
        } 
    }
    public void UpdateGridData(BlockTypes[,] _blockTypes, CubeTypes[,] _cubeTypes)
    {
        for (int i = 0; i < cubeTypes.Length; i++)
        {
            for (int j = 0; j < cubeTypes[i].rows.Length; j++)
            {
                cubeTypes[i].rows[j] = _cubeTypes[i, j];
                blockTypes[i].rows[j] = _blockTypes[i, j];
            }
        }
    }

}





