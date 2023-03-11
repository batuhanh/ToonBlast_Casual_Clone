using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class FillManager : MonoBehaviour
{
    [SerializeField] private GridManager gridManager;
    [SerializeField] private GameObject blockPrefab;

    public static FillManager Instance { get; private set; }
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
    public void Fill()
    {

        for (int i = 0; i < gridManager.changingColumns.Count; i++)
        {
            var item = gridManager.changingColumns.ElementAt(i);
            var itemKey = item.Key;
            var itemValue = item.Value;

            int rowLength = gridManager.allBlocks[itemKey].rows.Length;
            for (int j = rowLength - 1; j >= 0; j--)
            {
                if (gridManager.allBlocks[itemKey].rows[j] == null || (gridManager.allBlocks[itemKey].rows[j]
                    && gridManager.allBlocks[itemKey].rows[j].GetComponent<Block>().target == null))
                {
                    for (int k = j; k >= 0; k--)
                    {
                        if (gridManager.allBlocks[itemKey].rows[k]
                            && gridManager.allBlocks[itemKey].rows[k].GetComponent<Block>().target != null)
                        {
                            GameObject newTargetObj = gridManager.allPosObjs[itemKey].rows[j].gameObject;
                            Block curBlock = gridManager.allBlocks[itemKey].rows[k].gameObject.GetComponent<Block>();
                            curBlock.target = newTargetObj.transform;
                            curBlock.gridIndex = new Vector2(itemKey, j);

                            gridManager.allBlocks[itemKey].rows[j] = gridManager.allBlocks[itemKey].rows[k];
                            gridManager.allBlocks[itemKey].rows[k] = null;
                            curBlock.UpdateSortingOrder();
                            curBlock.MoveToTarget(0.5f);
                            break;
                        }
                    }

                }
            }
        }
        
        FallManager.Instance.Fall();
    }
    public void FillOnlyOneBlock(BlockTypes blockType, Vector2 gridIndex)
    {
        int x = (int)gridIndex.x;
        int y = (int)gridIndex.y;
        Vector3 spawnPos = gridManager.allPosObjs[(int)gridIndex.x].rows[(int)gridIndex.y].transform.position;
        GameObject spawnedBlock = Instantiate(blockPrefab, spawnPos, Quaternion.identity, gridManager.SpawnedBlocksParent);
        gridManager.allBlocks[x].rows[y] = spawnedBlock;

        BlockTypes curBlockType = blockType;

        var myBlockType = BlockFactory.GetBlock(curBlockType);
        Block currentBlock = spawnedBlock.AddComponent(myBlockType.GetType()) as Block;

        currentBlock.gridIndex = gridIndex;
        currentBlock.target = gridManager.allPosObjs[x].rows[y].transform;
        currentBlock.SetupBlock();

        gridManager.DecreaseChangingColumn((int)gridIndex.x);
    }

}
