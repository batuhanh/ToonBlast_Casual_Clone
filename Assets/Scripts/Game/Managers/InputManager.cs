using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    [SerializeField] private Camera mainCam;
    private bool canClick = false;
    void Update()
    {
        if (canClick)
        {

#if UNITY_EDITOR
        GetEditorInputs();
#else
		GetMobileTouches();
#endif

        }
    }
    private void GetEditorInputs()
    {
        if (Input.GetMouseButtonUp(0))
        {
            DetectHittedObject(Input.mousePosition);
        }
    }
    private void GetMobileTouches()
    {
        Touch touch = Input.GetTouch(0);
        if (touch.phase.Equals(TouchPhase.Ended))
        {
            DetectHittedObject(touch.position);
        }
    }
    private void DetectHittedObject(Vector3 touchedPos)
    {
        if (MovesPanel.Instance.Moves>0)
        {
            BoxCollider2D hittedCollider = Physics2D.OverlapPoint(mainCam.ScreenToWorldPoint(touchedPos)) as BoxCollider2D;
            if (hittedCollider)
            {
                GameObject hittedObj = hittedCollider.gameObject;
                if (hittedObj.CompareTag("Block"))
                {
                    hittedObj.gameObject.GetComponent<Block>().DoTappedActions();
                }
            }
        } 
    }
    private void EnableClicking()
    {
        if (LevelManager.Instance.isLevelActive)
        {
            canClick = true;
        }    
    }
    private void DisableClicking()
    {
        canClick = false;
    }
    private void OnEnable()
    {
        LevelManager.levelLoadedEvent += EnableClicking;
        LevelManager.levelSuccesedEvent += DisableClicking;
        LevelManager.levelFailedEvent += DisableClicking;
        RocketBlock.rocketStartedEvent += DisableClicking;
        RocketBlock.rocketEndedEvent += EnableClicking;
        CubeBlock.rocketSpawningEvent += DisableClicking;
        CubeBlock.rocketSpawnDoneEvent += EnableClicking;
    }
    private void OnDisable()
    {
        LevelManager.levelLoadedEvent -= EnableClicking;
        RocketBlock.rocketStartedEvent -= DisableClicking;
        RocketBlock.rocketEndedEvent -= EnableClicking;
        LevelManager.levelSuccesedEvent -= DisableClicking;
        LevelManager.levelFailedEvent -= DisableClicking;
        CubeBlock.rocketSpawningEvent -= DisableClicking;
        CubeBlock.rocketSpawnDoneEvent -= EnableClicking;
    }
}
