using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private Level[] allLevels;
    [SerializeField] private Level currentLevelData;
    [SerializeField] private GridManager gridManager;
    private int currentLevelIndex;
    public static event Action levelLoadedEvent;
    public static event Action levelSuccesedEvent;
    public static event Action levelFailedEvent;
    public bool isLevelActive = false;
    public static LevelManager Instance { get; private set; }
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
    public Level CurrentLevelData
    {
        get { return currentLevelData; }
        set { currentLevelData = value; }
    }
    private void Start()
    {
        LoadLevel();
    }
    private void LoadLevel()
    {
        currentLevelIndex = PlayerPrefs.GetInt("Level", 0) % allLevels.Length;
        currentLevelData = allLevels[currentLevelIndex];
        gridManager.dataLevel = currentLevelData;
        gridManager.LoadGridData();
        gridManager.SpawnStartBlocks();
        gridManager.SetGridCornerSize();
        isLevelActive = true;
        levelLoadedEvent?.Invoke();
    }
    public void LevelFailed()
    {
        if (isLevelActive)
        {
            isLevelActive = false;
            levelFailedEvent?.Invoke();
        }
        
    }
    public void LevelSuccesed()
    {
        if (isLevelActive)
        {
            isLevelActive = false;
            currentLevelIndex += 1;
            PlayerPrefs.SetInt("Level", currentLevelIndex);
            levelSuccesedEvent?.Invoke();
        }
    }
    public void RestartScene()
    {
        SceneManager.LoadScene(0);
    }
    private void OnEnable()
    {
        GoalPanel.allGoalsEndedEvent += LevelSuccesed;
        GoalPanel.goalsFailedEvent += LevelFailed;
    }
    private void OnDisable()
    {
        GoalPanel.allGoalsEndedEvent -= LevelSuccesed;
        GoalPanel.goalsFailedEvent -= LevelFailed;
    }
}
