using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using DG.Tweening;

public class DuckBlock : Block
{
    public override BlockTypes blockType => BlockTypes.Duck;
    public override Vector3 spriteSize => new Vector3(0.45f, 0.45f, 0.45f);
    private SpriteRenderer duckSR;
    public static event Action<int, int> duckLeavedGridEvent;
    public override void DoTappedActions()
    {

    }
    public override void SetupBlock()
    {
        duckSR = gameObject.GetComponentInChildren<SpriteRenderer>();
        duckSR.transform.localScale = spriteSize;
        duckSR.sortingOrder = -(int)gridIndex.y + 1;
        duckSR.sprite = ImageLibrary.duckBlockSprite;
        duckSR.transform.localPosition = new Vector3(0, -0.04f, 0);
    }
    public override void MoveToTarget(float arriveTime)
    {
        DOTween.Kill(transform);
        transform.DOKill();
        DuckSquezeEffect();
        transform.DOMove(target.position, arriveTime).SetEase(Ease.OutBounce).OnComplete(() =>
        {
            //call anim
            if ((int)gridIndex.y == LevelManager.Instance.CurrentLevelData.GameGrid.GridSizeY - 1)
            {
                ExplodeDuck();
            }
          
        });
    }
    private void DuckSquezeEffect()
    {
        if (!duckSR)
        {
            duckSR = gameObject.GetComponentInChildren<SpriteRenderer>();
        }
        duckSR.transform.DOScaleY(0.4f, 0.2f).SetEase(Ease.InOutBack).OnComplete(() =>
        {
            duckSR.transform.DOScaleY(0.5f, 0.2f).SetEase(Ease.InOutBack);
        });
        duckSR.transform.DOScaleX(0.55f, 0.2f).SetEase(Ease.InOutBack).OnComplete(() =>
        {
            duckSR.transform.DOScaleX(0.5f, 0.2f).SetEase(Ease.InOutBack);
        });
    }
    private void ExplodeDuck()
    {

        AudioManager.Instance.PlayDuckExplodeAudio();

        NeighbourManager.Instance.DoSingleObjAction(gridIndex);
        MovesPanel.Instance.Moves = MovesPanel.Instance.Moves - 1;
        target = null;
        DOTween.Kill(gameObject);
        transform.DOKill();
        duckLeavedGridEvent?.Invoke((int)gridIndex.x, (int)gridIndex.y);
        if (GoalPanel.Instance.CheckIsInGoals(blockType))
        {
            SetSortingLayerName("UI");
            SetSortingOrder(10);
            
            float arriveTime = 0.9f;
            Vector3 targetPos = GoalPanel.Instance.GetGoalPos(blockType);
            transform.DOMove(targetPos, arriveTime).SetEase(Ease.InOutBack).OnComplete(() =>
            {
                GoalPanel.Instance.DecereaseGoal(blockType);
                Destroy(gameObject);
            });
        }
        else
        {
            Destroy(gameObject);
        }
        FillManager.Instance.Fill();
    }
    public override void UpdateSortingOrder()
    {
        if (!duckSR)
            duckSR = gameObject.GetComponentInChildren<SpriteRenderer>();
        duckSR.sortingOrder = -(int)gridIndex.y + 1;
    }
    public override void SetSortingLayerName(string layerName)
    {
        if (!duckSR)
            duckSR = gameObject.GetComponentInChildren<SpriteRenderer>();

        duckSR.sortingLayerName = layerName;
    }
    public override void SetSortingOrder(int index)
    {
        if (!duckSR)
            duckSR = gameObject.GetComponentInChildren<SpriteRenderer>();

        duckSR.sortingOrder = index;
    }

}
