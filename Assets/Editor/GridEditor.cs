using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.EditorTools;
using System;

[CustomEditor(typeof(GridManager))]
public class GridEditor : Editor
{
    private GridManager myTarget;
    private Vector2 scrollPosition = Vector2.zero;

    private string gridsizeXStr;
    private string gridsizeYStr;
    private string movesCountStr;
    private int gridXSize;
    private int gridYSize;
    private int movesCount;

    private string redCubeGoalStr;
    private string blueCubeGoalStr;
    private string yellowCubeGoalStr;
    private string greenCubeGoalStr;
    private string purpleCubeGoalStr;
    private string balloonGoalStr;
    private string duckGoalStr;

    private int redCubeGoal;
    private int blueCubeGoal;
    private int yellowCubeGoal;
    private int greenCubeGoal;
    private int purpleCubeGoal;
    private int balloonGoal;
    private int duckGoal;

    private BlockTypes[,] blockTypes = new BlockTypes[1, 1];
    private CubeTypes[,] cubeTypes = new CubeTypes[1, 1];

    private bool isExecuted = false;

    void OnSceneGUI()
    {
        myTarget = (GridManager)target;
        if (!isExecuted)
        {
            myTarget.LoadGridData();
            gridsizeXStr = myTarget.myGrid.GridSizeX.ToString();
            gridsizeYStr = myTarget.myGrid.GridSizeY.ToString();
            movesCountStr = myTarget.moves.ToString();

            redCubeGoalStr = myTarget.myGoal.redCubeCount.ToString();
            blueCubeGoalStr = myTarget.myGoal.blueCubeCount.ToString();
            yellowCubeGoalStr = myTarget.myGoal.yellowCubeCount.ToString();
            greenCubeGoalStr = myTarget.myGoal.greenCubeCount.ToString();
            purpleCubeGoalStr = myTarget.myGoal.purpleCubeCount.ToString();
            balloonGoalStr = myTarget.myGoal.balloonCount.ToString();
            duckGoalStr = myTarget.myGoal.duckCount.ToString();

            isExecuted = true;
        }

        Handles.BeginGUI();
        {

            GUILayout.BeginArea(new Rect(10, Screen.height - 480, 600, 400), new GUIStyle("window"));
            {
                GUIStyle guiStyle = new GUIStyle("BoldLabel");
                guiStyle.fontSize = 15;
                GUILayout.Label(myTarget.gameObject.name, guiStyle, GUILayout.Height(20));

                DisplayGridSizePanel();
                GUILayout.Space(15f);
                DisplayGrid();
                GUILayout.Space(15f);
                DisplayButtons();

            }
            GUILayout.EndArea();

            GUILayout.BeginArea(new Rect(50, 10, 560, 150), new GUIStyle("window"));
            {
                GUIStyle guiStyle = new GUIStyle("BoldLabel");
                guiStyle.fontSize = 15;
                GUILayout.Label("Goal Editor", guiStyle, GUILayout.Height(20));
                DisplayGoals();
                DisplayMovesMenu();
                if (GUILayout.Button("Update Moves Count and Goals"))
                {
                    int tmpMoves;
                    int.TryParse(movesCountStr, out tmpMoves);
                    myTarget.moves = tmpMoves;

                    int.TryParse(redCubeGoalStr, out redCubeGoal);
                    myTarget.myGoal.redCubeCount = redCubeGoal;

                    int.TryParse(blueCubeGoalStr, out blueCubeGoal);
                    myTarget.myGoal.blueCubeCount = blueCubeGoal;

                    int.TryParse(yellowCubeGoalStr, out yellowCubeGoal);
                    myTarget.myGoal.yellowCubeCount = yellowCubeGoal;

                    int.TryParse(greenCubeGoalStr, out greenCubeGoal);
                    myTarget.myGoal.greenCubeCount = greenCubeGoal;

                    int.TryParse(purpleCubeGoalStr, out purpleCubeGoal);
                    myTarget.myGoal.purpleCubeCount = purpleCubeGoal;

                    int.TryParse(duckGoalStr, out duckGoal);
                    myTarget.myGoal.duckCount = duckGoal;

                    int.TryParse(balloonGoalStr, out balloonGoal);
                    myTarget.myGoal.balloonCount = balloonGoal;

                    UpdateLevelData();
                }
            }
            GUILayout.EndArea();
        }
        Handles.EndGUI();


    }
    private void DisplayGridSizePanel()
    {
        EditorGUILayout.BeginHorizontal();
        GUILayout.Label("GameGrid Size (X/Y): ", GUILayout.Width(120), GUILayout.Height(15));

        gridsizeXStr = EditorGUILayout.TextArea(gridsizeXStr, EditorStyles.textArea, GUILayout.Width(40));
        gridsizeYStr = EditorGUILayout.TextArea(gridsizeYStr, EditorStyles.textArea, GUILayout.Width(40));

        if (GUILayout.Button("Setup GameGrid", GUILayout.Width(150)))
        {
            int tmpX;
            int.TryParse(gridsizeXStr, out tmpX);
            myTarget.myGrid.GridSizeX = tmpX;

            int tmpY;
            int.TryParse(gridsizeYStr, out tmpY);
            myTarget.myGrid.GridSizeY = tmpY;

            myTarget.SetupGrid();
            FillBlocksArrays();
        }
        EditorGUILayout.EndHorizontal();
    }
    private void DisplayMovesMenu()
    {
        GUILayout.FlexibleSpace();
        EditorGUILayout.BeginHorizontal();
        GUILayout.Label("Total Moves: ", GUILayout.Width(80), GUILayout.Height(15));
        movesCountStr = EditorGUILayout.TextArea(movesCountStr, EditorStyles.textArea, GUILayout.Width(40));
       
        EditorGUILayout.EndHorizontal();
    }
    private void FillBlocksArrays()
    {
        gridXSize = myTarget.myGrid.GridSizeX;
        gridYSize = myTarget.myGrid.GridSizeY;

        gridsizeXStr = gridXSize.ToString();
        gridsizeYStr = gridYSize.ToString();

        blockTypes = new BlockTypes[gridXSize, gridYSize];
        cubeTypes = new CubeTypes[gridXSize, gridYSize];

        myTarget.UpdateArrays();

        for (int i = 0; i < gridYSize; i++)
        {
            for (int j = 0; j < gridXSize; j++)
            {
                blockTypes[j, i] = myTarget.blockTypes[j, i];
                cubeTypes[j, i] = myTarget.cubeTypes[j, i];
            }
        }

    }
    private void DisplayGoals()
    {
      
        EditorGUILayout.BeginHorizontal();

        GUILayout.Label("Red Cube: ", GUILayout.Width(80), GUILayout.Height(15));
        redCubeGoalStr = EditorGUILayout.TextArea(redCubeGoalStr, EditorStyles.textArea, GUILayout.Width(40));

        GUILayout.Label("Yellow Cube: ", GUILayout.Width(80), GUILayout.Height(15));
        yellowCubeGoalStr = EditorGUILayout.TextArea(yellowCubeGoalStr, EditorStyles.textArea, GUILayout.Width(40));

        GUILayout.Label("Blue Cube: ", GUILayout.Width(80), GUILayout.Height(15));
        blueCubeGoalStr = EditorGUILayout.TextArea(blueCubeGoalStr, EditorStyles.textArea, GUILayout.Width(40));

        GUILayout.Label("Purple Cube: ", GUILayout.Width(80), GUILayout.Height(15));
        purpleCubeGoalStr = EditorGUILayout.TextArea(purpleCubeGoalStr, EditorStyles.textArea, GUILayout.Width(40));

        EditorGUILayout.EndHorizontal();
        EditorGUILayout.BeginHorizontal();
        GUILayout.Label("Green Cube: ", GUILayout.Width(80), GUILayout.Height(15));
        greenCubeGoalStr = EditorGUILayout.TextArea(greenCubeGoalStr, EditorStyles.textArea, GUILayout.Width(40));

        GUILayout.Label("Balloon: ", GUILayout.Width(80), GUILayout.Height(15));
        balloonGoalStr = EditorGUILayout.TextArea(balloonGoalStr, EditorStyles.textArea, GUILayout.Width(40));

        GUILayout.Label("Duck: ", GUILayout.Width(80), GUILayout.Height(15));
        duckGoalStr = EditorGUILayout.TextArea(duckGoalStr, EditorStyles.textArea, GUILayout.Width(40));

        EditorGUILayout.EndHorizontal();
    }
    private void DisplayGrid()
    {
        scrollPosition = GUILayout.BeginScrollView(scrollPosition);
        if (blockTypes.GetLength(0) != myTarget.myGrid.GridSizeX)
        {
            FillBlocksArrays();
        }

        for (int i = 0; i < gridYSize; i++)
        {
            EditorGUILayout.BeginHorizontal();
            for (int j = 0; j < gridXSize; j++)
            {
                EditorGUILayout.BeginVertical();
                GUILayout.Label("Cell " + j + "-" + i, GUILayout.Width(70));
                blockTypes[j, i] = (BlockTypes)EditorGUILayout.EnumPopup(blockTypes[j, i], GUILayout.Width(70));
                if (blockTypes[j, i] == BlockTypes.Cube)
                {
                    cubeTypes[j, i] = (CubeTypes)EditorGUILayout.EnumPopup(cubeTypes[j, i], GUILayout.Width(40));
                }
                EditorGUILayout.EndVertical();

            }
            EditorGUILayout.EndHorizontal();
        }

        GUILayout.EndScrollView();
    }
    private void DisplayButtons()
    {
        GUILayout.FlexibleSpace();
        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("Randomize With Cubes", GUILayout.Height(30)))
        {
            for (int i = 0; i < gridYSize; i++)
            {
                for (int j = 0; j < gridXSize; j++)
                {
                    blockTypes[j, i] = BlockTypes.Cube;
                    CubeTypes randomCube = (CubeTypes) UnityEngine.Random.Range(0, System.Enum.GetValues(typeof(CubeTypes)).Length);
                    cubeTypes[j, i] = randomCube;
                }
            }
            UpdateLevelData();
        }
        if (GUILayout.Button("Update Blocks Data", GUILayout.Height(30)))
        {
            UpdateLevelData();
        }
        EditorGUILayout.EndHorizontal();
    }
    private void UpdateLevelData()
    {
        myTarget.PopulateBlocksData(blockTypes, cubeTypes);
        myTarget.SaveGridData();
        EditorUtility.SetDirty(myTarget.dataLevel);
        EditorUtility.SetDirty(myTarget);
    }
}