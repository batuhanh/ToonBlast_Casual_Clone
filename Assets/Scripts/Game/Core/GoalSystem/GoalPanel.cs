using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GoalPanel : MonoBehaviour
{
    [SerializeField] private LevelManager levelManager;

    [SerializeField] private GoalObject balloonGoal;
    [SerializeField] private GoalObject duckGoal;
    [SerializeField] private GoalObject greenCubeGoal;
    [SerializeField] private GoalObject redCubeGoal;
    [SerializeField] private GoalObject blueCubeGoal;
    [SerializeField] private GoalObject yellowCubeGoal;
    [SerializeField] private GoalObject purpleCubeGoal;

    public static event Action allGoalsEndedEvent;
    public static event Action goalsFailedEvent;

    private Goal myGoal;
    public Goal MyGoal
    {
        get { return myGoal; }
        set { myGoal = value; }
    }
    public static GoalPanel Instance { get; private set; }
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
    public void DecereaseGoal(CubeTypes cubeType)
    {
        GoalObject goalObject = DetectGoalType(cubeType);
        if (goalObject)
        {
            goalObject.Count = goalObject.Count - 1;
        }
        if (CheckGoalsAchieved())
        {
            allGoalsEndedEvent?.Invoke();
        }
    }
    public void DecereaseGoal(BlockTypes blockType)
    {
        GoalObject goalObject = DetectGoalType(blockType);
        if (goalObject)
        {
            goalObject.Count = goalObject.Count - 1;
        }
        if (CheckGoalsAchieved())
        {
            allGoalsEndedEvent?.Invoke();
        }
    }
    public bool CheckGoalsAchieved()
    {
        bool allGoalsEnded = true;
        if (balloonGoal.gameObject.activeSelf && balloonGoal.Count > 0)
            allGoalsEnded = false;
        if (duckGoal.gameObject.activeSelf && duckGoal.Count > 0)
            allGoalsEnded = false;
        if (greenCubeGoal.gameObject.activeSelf && greenCubeGoal.Count > 0)
            allGoalsEnded = false;
        if (redCubeGoal.gameObject.activeSelf && redCubeGoal.Count > 0)
            allGoalsEnded = false;
        if (blueCubeGoal.gameObject.activeSelf && blueCubeGoal.Count > 0)
            allGoalsEnded = false;
        if (purpleCubeGoal.gameObject.activeSelf && purpleCubeGoal.Count > 0)
            allGoalsEnded = false;
        if (yellowCubeGoal.gameObject.activeSelf && yellowCubeGoal.Count > 0)
            allGoalsEnded = false;

        return allGoalsEnded;
        
    }
    private void CheckForFail()
    {
        if (!CheckGoalsAchieved())
        {
            goalsFailedEvent?.Invoke();
        }
    }
    private GoalObject DetectGoalType(CubeTypes cubeType)
    {
        switch (cubeType)
        {
            case CubeTypes.Red:
                {
                    return redCubeGoal;
                }
            case CubeTypes.Blue:
                {
                    return blueCubeGoal;
                }
            case CubeTypes.Yellow:
                {
                    return yellowCubeGoal;
                }
            case CubeTypes.Green:
                {
                    return greenCubeGoal;
                }
            case CubeTypes.Purple:
                {
                    return purpleCubeGoal;
                }
            default: return redCubeGoal;

        }
    }
    private GoalObject DetectGoalType(BlockTypes blockType)
    {
        switch (blockType)
        {
            case BlockTypes.Balloon:
                {
                    return balloonGoal;
                }
            case BlockTypes.Duck:
                {
                    return duckGoal;
                }
            default: return duckGoal;

        }
    }
    public Vector3 GetGoalPos(CubeTypes cubeType)
    {
        switch (cubeType)
        {
            case CubeTypes.Red:
                {
                    return redCubeGoal.gameObject.transform.position;
                }
            case CubeTypes.Blue:
                {
                    return blueCubeGoal.gameObject.transform.position;
                }
            case CubeTypes.Yellow:
                {
                    return yellowCubeGoal.gameObject.transform.position;
                }
            case CubeTypes.Green:
                {
                    return greenCubeGoal.gameObject.transform.position;
                }
            case CubeTypes.Purple:
                {
                    return purpleCubeGoal.gameObject.transform.position;
                }
            default: return redCubeGoal.gameObject.transform.position;
        }
    }
    public Vector3 GetGoalPos(BlockTypes blockType)
    {
        switch (blockType)
        {
            case BlockTypes.Balloon:
                {
                    return balloonGoal.gameObject.transform.position;
                }
            case BlockTypes.Duck:
                {
                    return duckGoal.gameObject.transform.position;
                }
            default: return balloonGoal.gameObject.transform.position;
        }
    }
    public bool CheckIsInGoals(CubeTypes cubeType)
    {
        switch (cubeType)
        {
            case CubeTypes.Red:
                {
                    if (redCubeGoal.Count > 0)
                        return true;
                    else
                        return false;
                }
            case CubeTypes.Blue:
                {
                    if (blueCubeGoal.Count > 0)
                        return true;
                    else
                        return false;
                }
            case CubeTypes.Yellow:
                {
                    if (yellowCubeGoal.Count > 0)
                        return true;
                    else
                        return false;
                }
            case CubeTypes.Green:
                {
                    if (greenCubeGoal.Count > 0)
                        return true;
                    else
                        return false;
                }
            case CubeTypes.Purple:
                {
                    if (purpleCubeGoal.Count > 0)
                        return true;
                    else
                        return false;
                }
            default: return false;
        }
    }
    public bool CheckIsInGoals(BlockTypes blockType)
    {
        switch (blockType)
        {
            case BlockTypes.Duck:
                {
                    if (duckGoal.Count > 0)
                        return true;
                    else
                        return false;
                }
            case BlockTypes.Balloon:
                {
                    if (balloonGoal.Count > 0)
                        return true;
                    else
                        return false;
                }
            default: return false;
        }
    }
    public void SetupGoalObjects()
    {

        myGoal = levelManager.CurrentLevelData.goal;

        balloonGoal.Count = myGoal.balloonCount;
        duckGoal.Count = myGoal.duckCount;
        greenCubeGoal.Count = myGoal.greenCubeCount;
        redCubeGoal.Count = myGoal.redCubeCount;
        blueCubeGoal.Count = myGoal.blueCubeCount;
        yellowCubeGoal.Count = myGoal.yellowCubeCount;
        purpleCubeGoal.Count = myGoal.purpleCubeCount;

        if (balloonGoal.Count > 0)
            balloonGoal.gameObject.SetActive(true);
        if (duckGoal.Count > 0)
            duckGoal.gameObject.SetActive(true);
        if (greenCubeGoal.Count > 0)
            greenCubeGoal.gameObject.SetActive(true);
        if (redCubeGoal.Count > 0)
            redCubeGoal.gameObject.SetActive(true);
        if (blueCubeGoal.Count > 0)
            blueCubeGoal.gameObject.SetActive(true);
        if (yellowCubeGoal.Count > 0)
            yellowCubeGoal.gameObject.SetActive(true);
        if (purpleCubeGoal.Count > 0)
            purpleCubeGoal.gameObject.SetActive(true);

    }
    private void OnEnable()
    {
        LevelManager.levelLoadedEvent += SetupGoalObjects;
        MovesPanel.movesFinishedEvent += CheckForFail;
    }
    private void OnDisable()
    {
        LevelManager.levelLoadedEvent -= SetupGoalObjects;
        MovesPanel.movesFinishedEvent -= CheckForFail;
    }

}
