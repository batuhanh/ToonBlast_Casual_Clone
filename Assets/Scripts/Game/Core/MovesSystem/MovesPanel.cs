using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;

public class MovesPanel : MonoBehaviour
{
    public static event Action movesFinishedEvent;

    [SerializeField] private LevelManager levelManager;
    [SerializeField] private TMP_Text movesText;
    private int moves;
    public int Moves
    {
        get { return moves; }
        set
        {
            moves = value;
            if (moves <=0)
            {
                moves = 0;
                StartCoroutine(MovesFinishedDelay());
            }
            movesText.text = moves.ToString();
        }
    }
    public static MovesPanel Instance { get; private set; }
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
    public void GetMovesData()
    {
        Moves = levelManager.CurrentLevelData.moves;
    }
    private void OnEnable()
    {
        LevelManager.levelLoadedEvent += GetMovesData;
    }
    private void OnDisable()
    {
        LevelManager.levelLoadedEvent -= GetMovesData;
    }

    private IEnumerator MovesFinishedDelay()
    {
        yield return new WaitForSeconds(1f);
        movesFinishedEvent?.Invoke();
    }
}
