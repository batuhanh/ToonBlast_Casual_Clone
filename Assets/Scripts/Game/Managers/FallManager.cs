using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using DG.Tweening;
public class FallManager : MonoBehaviour
{
    [SerializeField] private GridManager gridManager;
    [SerializeField] private Transform spawnedBlocksParent;
    [SerializeField] private GameObject blockPrefab;
    public static FallManager Instance { get; private set; }
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
    public void Fall()
    {
        for (int i = 0; i < gridManager.changingColumns.Count; i++)
        {
            var item = gridManager.changingColumns.ElementAt(i);
            var itemKey = item.Key;
            var itemValue = item.Value;

            Vector3 startPos = gridManager.allPosObjs[itemKey].rows[0].transform.position +
                new Vector3(0, 2, 0);
            for (int j = 0; j < itemValue; j++)
            {
                Vector3 spawnPos = startPos + new Vector3(0, j * 1f, 0);
                int yIndex = (itemValue - 1) - j;
                Transform targetTransform = gridManager.allPosObjs[itemKey].rows[yIndex].transform;

                GameObject spawnedBlockObj = AddRandomBlockToGrid(itemKey, yIndex, spawnPos, targetTransform);
                float arriveTime = Mathf.Clamp(Vector3.Distance(targetTransform.position, spawnPos)*0.2f,0.5f,0.8f);
                spawnedBlockObj.GetComponent<Block>().MoveToTarget(arriveTime);
            }
        }
        gridManager.changingColumns = new Dictionary<int, int>();

    }
    private GameObject AddRandomBlockToGrid(int x, int y, Vector3 spawnPos, Transform targetTransform)
    {
        GameObject spawnedBlockObj = Instantiate(blockPrefab, spawnPos, Quaternion.identity, spawnedBlocksParent);
        gridManager.allBlocks[x].rows[y] = spawnedBlockObj;

        BlockTypes curBlockType = BlockTypes.Cube;
        CubeTypes curCubeType = (CubeTypes)UnityEngine.Random.Range(0, System.Enum.GetValues(typeof(CubeTypes)).Length);

        var myBlockType = BlockFactory.GetBlock(curBlockType);
        Block currentBlock = spawnedBlockObj.AddComponent(myBlockType.GetType()) as Block;
        spawnedBlockObj.GetComponent<CubeBlock>().cubeType = curCubeType;

        currentBlock.gridIndex = new Vector2(x, y);
        currentBlock.target = targetTransform;
        currentBlock.SetupBlock();

        return spawnedBlockObj;
    }
}
