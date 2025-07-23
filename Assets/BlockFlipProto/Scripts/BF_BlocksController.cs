using System;
using System.Collections.Generic;
using UnityEngine;

public class BF_BlocksController : MonoBehaviour
{
    [Header("Gameplay Parameters")]
    [Tooltip("Speed at which blocks rotate when swiped - Degress Per Second")]
    [Range(0f, 720f)]
    [SerializeField] private float blockRotationSpeed = 360f;

    [SerializeField] private List<BF_BlockMovementController> blocksController;

    void Start()
    {
        AssignBlockDimentionsCalculationEvents();
        InitializeBlocks();
    }

    void OnDestroy()
    {
        UnAssignBlockDimentionsCalculationEvents();
    }

    private void AssignBlockDimentionsCalculationEvents()
    {
        foreach (BF_BlockMovementController block in blocksController)
        {
            block.onBlockDimentionCalculationBegin += onBlockDimentionCalculationBegin;
            block.onBlockDimentionCalculationEnd += onBlockDimentionCalculationEnd;
        }
    }

    private void UnAssignBlockDimentionsCalculationEvents()
    {
        foreach (BF_BlockMovementController block in blocksController)
        {
            block.onBlockDimentionCalculationBegin -= onBlockDimentionCalculationBegin;
            block.onBlockDimentionCalculationEnd -= onBlockDimentionCalculationEnd;
        }
    }

    private void InitializeBlocks()
    {
        blocksController.ForEach(block =>
        {
            block.Init(blockRotationSpeed);
        });
    }

    private void onBlockDimentionCalculationEnd(GameObject block)
    {
        foreach (BF_BlockMovementController controller in blocksController)
        {
            controller.GetComponent<BoxCollider>().enabled = true;
        }
    }

    private void onBlockDimentionCalculationBegin(GameObject block)
    {
        foreach (BF_BlockMovementController controller in blocksController)
        {
            if (controller.gameObject != block)
                controller.GetComponent<BoxCollider>().enabled = false;
        }
    }
}
