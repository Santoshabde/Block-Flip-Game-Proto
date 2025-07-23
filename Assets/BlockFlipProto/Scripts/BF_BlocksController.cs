using System;
using System.Collections.Generic;
using UnityEngine;

public class BF_BlocksController : MonoBehaviour
{
    [SerializeField] private List<BF_BlockMovementController> blocksController;

    void Start()
    {
        foreach (BF_BlockMovementController block in blocksController)
        {
            block.onBlockDimentionCalculationBegin += OnBlockSelected;
            block.onBlockDimentionCalculationEnd += OnBlockDeselected;
        }

        foreach (BF_BlockMovementController block in blocksController)
        {
            block.CalculateBlockDimentionsAndRotationPoints();
        }
    }

    private void OnBlockDeselected(GameObject block)
    {
        foreach (BF_BlockMovementController controller in blocksController)
        {
            controller.GetComponent<BoxCollider>().enabled = true;
        }
    }

    private void OnBlockSelected(GameObject block)
    {
        foreach (BF_BlockMovementController controller in blocksController)
        {
            if (controller.gameObject != block)
                controller.GetComponent<BoxCollider>().enabled = false;
        }
    }

    void OnDestroy()
    {
        foreach (BF_BlockMovementController block in blocksController)
        {
            block.onBlockDimentionCalculationBegin -= OnBlockSelected;
            block.onBlockDimentionCalculationEnd -= OnBlockDeselected;
        }
    }
}
