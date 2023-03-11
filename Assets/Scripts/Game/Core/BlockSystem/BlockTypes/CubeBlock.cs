using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;
public class CubeBlock : Block
{
    public override BlockTypes blockType => BlockTypes.Cube;
    public override Vector3 spriteSize => new Vector3(0.5f, 0.5f, 0.5f);

    private SpriteRenderer mySpriteRenderer;
    public CubeTypes cubeType;
    public static event Action<int, int> cubeLeavedGridEvent;
    public static event Action rocketSpawningEvent;
    public static event Action rocketSpawnDoneEvent;
    public bool canTapped = true;
    public override void DoTappedActions()
    {
        if (canTapped)
        {
            List<GameObject> mySameNeighbours = NeighbourManager.Instance.FindSameNeighbours(cubeType, gridIndex);
            if (mySameNeighbours.Count >= 2)
            {
                AudioManager.Instance.PlayCubeExplosionAudio();

                NeighbourManager.Instance.BlowUpBallons();

                canTapped = false;
                MovesPanel.Instance.Moves = MovesPanel.Instance.Moves - 1;
                int index = 0;
                bool isRocketSpawned = false;
                bool canSpawnRocket = mySameNeighbours.Count >= 5;
                //bool canSpawnRocket = false;
                if (canSpawnRocket)
                {
                    rocketSpawningEvent?.Invoke();

                }
                foreach (GameObject neigh in mySameNeighbours)
                {

                    CubeBlock curBlock = neigh.gameObject.GetComponent<CubeBlock>();
                    curBlock.canTapped = false;
                    EffectsController.Instance.SpawnCubeCrackEffect(neigh.transform.position, curBlock.cubeType);
                    curBlock.target = null;
                    DOTween.Kill(neigh);
                    neigh.transform.DOKill();
                    if (canSpawnRocket)
                    {
                        curBlock.SetSortingLayerName("UI");
                        curBlock.SetSortingOrder(10);
                        cubeLeavedGridEvent?.Invoke((int)gridIndex.x, (int)gridIndex.y);
                        float arriveTime = 0.4f;
                        Vector3 targetPos = transform.position;

                        neigh.transform.DOMove(targetPos, arriveTime).SetEase(Ease.InOutBack).OnComplete(() =>
                        {
                            if (GoalPanel.Instance.CheckIsInGoals(cubeType))
                                GoalPanel.Instance.DecereaseGoal(cubeType);
                            Destroy(neigh);
                            if (!isRocketSpawned)
                            {
                                isRocketSpawned = true;

                                FillManager.Instance.FillOnlyOneBlock(BlockTypes.Rocket, gridIndex);
                                FillManager.Instance.Fill();
                                rocketSpawnDoneEvent?.Invoke();
                            }
                        });
                    }
                    else if (GoalPanel.Instance.CheckIsInGoals(cubeType))
                    {
                        curBlock.SetSortingLayerName("UI");
                        curBlock.SetSortingOrder(10);
                        cubeLeavedGridEvent?.Invoke((int)gridIndex.x, (int)gridIndex.y);
                        float arriveTime = 0.7f + (index * 0.15f);
                        Vector3 targetPos = GoalPanel.Instance.GetGoalPos(cubeType);
                        neigh.transform.DOMove(targetPos, arriveTime).SetEase(Ease.InOutBack).OnComplete(() =>
                        {
                            AudioManager.Instance.PlayCubeCollectAudio();

                            GoalPanel.Instance.DecereaseGoal(cubeType);
                            Destroy(neigh);
                        });
                    }
                    else
                    {
                        Destroy(neigh);
                    }
                    index++;
                }
                if (!canSpawnRocket)
                {
                    FillManager.Instance.Fill();
                }

            }
        }

    }

    public override void SetupBlock()
    {
        mySpriteRenderer = gameObject.GetComponentInChildren<SpriteRenderer>();
        mySpriteRenderer.transform.localScale = spriteSize;
        mySpriteRenderer.sortingOrder = -(int)gridIndex.y + 1;
        mySpriteRenderer.sprite = GetMySprite();
    }
    private Sprite GetMySprite()
    {

        switch (cubeType)
        {
            case CubeTypes.Red:
                {
                    return ImageLibrary.redCubeBlockSprite;
                }
            case CubeTypes.Blue:
                {
                    return ImageLibrary.blueCubeBlockSprite;
                }
            case CubeTypes.Purple:
                {
                    return ImageLibrary.purpleCubeBlockSprite;
                }
            case CubeTypes.Yellow:
                {
                    return ImageLibrary.yellowCubeBlockSprite;
                }
            case CubeTypes.Green:
                {
                    return ImageLibrary.greenCubeBlockSprite;
                }
            default: break;
        }
        return ImageLibrary.blueCubeBlockSprite;
    }

    public override void MoveToTarget(float arriveTime)
    {
        DOTween.Kill(transform);
        transform.DOKill();
        transform.DOMove(target.position, arriveTime).SetEase(Ease.OutBounce).OnComplete(() =>
        {

        });
    }

    public override void UpdateSortingOrder()
    {
        if (!mySpriteRenderer)
            mySpriteRenderer = gameObject.GetComponentInChildren<SpriteRenderer>();
        mySpriteRenderer.sortingOrder = -(int)gridIndex.y + 1;
    }
    public override void SetSortingLayerName(string layerName)
    {
        if (!mySpriteRenderer)
            mySpriteRenderer = gameObject.GetComponentInChildren<SpriteRenderer>();

        mySpriteRenderer.sortingLayerName = layerName;
    }
    public override void SetSortingOrder(int index)
    {
        if (!mySpriteRenderer)
            mySpriteRenderer = gameObject.GetComponentInChildren<SpriteRenderer>();

        mySpriteRenderer.sortingOrder = index;
    }
}

