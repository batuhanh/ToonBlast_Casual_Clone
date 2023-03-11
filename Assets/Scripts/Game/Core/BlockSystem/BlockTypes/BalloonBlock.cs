using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class BalloonBlock : Block
{
    public override BlockTypes blockType => BlockTypes.Balloon;
    public override Vector3 spriteSize => new Vector3(0.45f, 0.45f, 0.45f);
    private SpriteRenderer balloonSR;

    public override void DoTappedActions()
    {

    }
    public void Explode()
    {
        AudioManager.Instance.PlayBalloonPopAudio();
        
        EffectsController.Instance.SpawnBalloonCrackEffect(transform.position);
        target = null;
        DOTween.Kill(gameObject);
        transform.DOKill();
        GoalPanel.Instance.DecereaseGoal(blockType);
        Destroy(gameObject);
    }
    public override void SetupBlock()
    {
        balloonSR = gameObject.GetComponentInChildren<SpriteRenderer>();
        balloonSR.transform.localScale = spriteSize;
        balloonSR.sortingOrder = -(int)gridIndex.y + 1;
        balloonSR.sprite = ImageLibrary.balloonBlockSprite;
        balloonSR.transform.localPosition = new Vector3(0, -0.04f, 0);
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
        if (!balloonSR)
            balloonSR = gameObject.GetComponentInChildren<SpriteRenderer>();
        balloonSR.sortingOrder = -(int)gridIndex.y + 1;
    }
    public override void SetSortingLayerName(string layerName)
    {
        if (!balloonSR)
            balloonSR = gameObject.GetComponentInChildren<SpriteRenderer>();

        balloonSR.sortingLayerName = layerName;
    }
    public override void SetSortingOrder(int index)
    {
        if (!balloonSR)
            balloonSR = gameObject.GetComponentInChildren<SpriteRenderer>();

        balloonSR.sortingOrder = index;
    }
}
