using System;
using System.Collections;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.UIElements;

[System.Flags]
public enum BlockRotationDirection
{
    None = 0,
    Left = 1 << 0,  // 1
    Right = 1 << 1,  // 2
    Forward = 1 << 2,  // 4
    Backward = 1 << 3   // 8
}


public class BF_BlockMovementController : MonoBehaviour
{
    [Header("Required Components")]
    [SerializeField] private BF_BlockTileChecker blockTileChecker;

    [Header("DEBUG-ONLY values")]
    [SerializeField] private int forwardBlockCount;
    [SerializeField] private int rightBlockCount;
    [SerializeField] private int depthBlockCount;
    [SerializeField] private BlockRotationDirection possibleRotationDirections;

    private Vector3 forwardRotationPoint;
    private Vector3 rightRotationPoint;
    private Vector3 backwardRotationPoint;
    private Vector3 leftRotationPoint;

    private Vector2 swipeStart;
    private bool isSwiping = false;
    private bool isRotating = false;

    public Action<GameObject> onBlockDimentionCalculationBegin;
    public Action<GameObject> onBlockDimentionCalculationEnd;
    public Action OnBlockSettledDown;

    void Start()
    {
        var occupiedTiles = blockTileChecker.CalculateTilesWhichBlockOccupied();
    }

    void Update()
    {
        if (!isRotating)
        {
            if (Input.GetMouseButtonDown(0) && !isSwiping)
            {
                RaycastHit hit;
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out hit))
                {
                    if (hit.collider != null && hit.collider.gameObject == gameObject)
                    {
                        Debug.Log("[BlockFlip_Gameplay] Block clicked: " + gameObject.name);

                        isSwiping = true;
                        swipeStart = Input.mousePosition;

                        CalculateBlockDimentionsAndRotationPoints();
                        possibleRotationDirections = blockTileChecker.CalculatePosibleMovements(blockTileChecker.CalculateTilesWhichBlockOccupied(), forwardBlockCount, rightBlockCount, depthBlockCount);
                    }
                }
            }

            if (Input.GetMouseButtonUp(0) && isSwiping)
            {
                Vector2 swipeEnd = Input.mousePosition;
                Vector2 swipeDelta = swipeEnd - swipeStart;

                Debug.Log("[BlockFlip_Gameplay] swipeStart: " + swipeStart + ", swipeEnd: " + Input.mousePosition + ", swipeDelta: " + swipeDelta);

                if (swipeDelta.magnitude > 30f) // Threshold for swipe detection
                {
                    if (Mathf.Abs(swipeDelta.x) > Mathf.Abs(swipeDelta.y))
                    {
                        if (swipeDelta.x > 0)
                            RotateRight();
                        else
                            RotateLeft();
                    }
                    else
                    {
                        if (swipeDelta.y > 0)
                            RotateForward();
                        else
                            RotateBackward();
                    }
                }

                isSwiping = false;
            }
        }
    }

    public void CalculateBlockDimentionsAndRotationPoints()
    {
        onBlockDimentionCalculationBegin?.Invoke(gameObject);

        forwardBlockCount = ForwardLengthDetector();
        rightBlockCount = RightLengthDetector();
        depthBlockCount = DepthDetector();

        Vector3 initialPosition = transform.position + Vector3.down * 0.5f * depthBlockCount;

        forwardRotationPoint = initialPosition + (Vector3.forward * 0.5f * forwardBlockCount);
        rightRotationPoint = initialPosition + (Vector3.right * 0.5f * rightBlockCount);
        backwardRotationPoint = initialPosition - (Vector3.forward * 0.5f * forwardBlockCount);
        leftRotationPoint = initialPosition - (Vector3.right * 0.5f * rightBlockCount);

        onBlockDimentionCalculationEnd?.Invoke(gameObject);
    }

    private void RotateForward()
    {
        if (possibleRotationDirections.HasFlag(BlockRotationDirection.Forward))
            RotateBlock(forwardRotationPoint, Vector3.right);
        else
            Debug.Log("[BlockFlip_Gameplay] Forward rotation not possible.");
    }

    private void RotateRight()
    {
        if (possibleRotationDirections.HasFlag(BlockRotationDirection.Right))
            RotateBlock(rightRotationPoint, -Vector3.forward);
        else
            Debug.Log("[BlockFlip_Gameplay] Right rotation not possible.");
    }

    private void RotateBackward()
    {
        if (possibleRotationDirections.HasFlag(BlockRotationDirection.Backward))
            RotateBlock(backwardRotationPoint, -Vector3.right);
        else
            Debug.Log("[BlockFlip_Gameplay] Backward rotation not possible.");
    }

    private void RotateLeft()
    {
        if (possibleRotationDirections.HasFlag(BlockRotationDirection.Left))
            RotateBlock(leftRotationPoint, Vector3.forward);
        else
            Debug.Log("[BlockFlip_Gameplay] Left rotation not possible.");
    }

    private void RotateBlock(Vector3 rotationPoint, Vector3 rotationAxis)
    {
        StartCoroutine(RotateBlockIEnum(rotationPoint, rotationAxis));
    }

    private IEnumerator RotateBlockIEnum(Vector3 rotationPoint, Vector3 rotationAxis)
    {
        isRotating = true;
        float rotated = 0f;
        float rotationSpeed = 360f; // degrees per second

        while (rotated < 90f)
        {
            float step = rotationSpeed * Time.deltaTime;

            if (rotated + step > 90f)
                step = 90f - rotated;

            transform.RotateAround(rotationPoint, rotationAxis, step);
            rotated += step;

            yield return null;
        }

        yield return new WaitForSeconds(0.1f);
        OnBlockSettledDown?.Invoke();
        CalculateBlockDimentionsAndRotationPoints();
        var occupiedTiles = blockTileChecker.CalculateTilesWhichBlockOccupied();
        possibleRotationDirections = blockTileChecker.CalculatePosibleMovements(occupiedTiles, forwardBlockCount, rightBlockCount, depthBlockCount);
        isRotating = false;
    }

    private int ForwardLengthDetector()
    {
        Vector3 detectOriginPoint = transform.position + Vector3.up * 4f;

        int blockCount = 1;
        while (true)
        {
            Vector3 nextForwardPoint = detectOriginPoint + Vector3.forward * 0.95f * blockCount;
            RaycastHit hit;
            if (Physics.Raycast(nextForwardPoint, -Vector3.up, out hit, 100f))
            {
                if (hit.collider.gameObject == gameObject)
                {
                    blockCount += 1;
                }
                else
                {
                    Debug.Log("[BlockFlip_Gameplay] Total unitys detected in forward direction: " + blockCount);
                    return blockCount;
                }
            }
            else
            {
                Debug.Log("[BlockFlip_Gameplay] Total unitys detected in forward direction: " + blockCount);
                return blockCount; ;
            }
        }
    }

    private int RightLengthDetector()
    {
        Vector3 detectOriginPoint = transform.position + Vector3.up * 4f;

        int blockCount = 1;
        while (true)
        {
            Vector3 nextRightPoint = detectOriginPoint + Vector3.right * 0.95f * blockCount;
            RaycastHit hit;
            if (Physics.Raycast(nextRightPoint, -Vector3.up, out hit, 100f))
            {
                if (hit.collider.gameObject == gameObject)
                {
                    blockCount += 1;
                }
                else
                {
                    Debug.Log("[BlockFlip_Gameplay] Total units detected in right direction: " + (blockCount));
                    return blockCount;
                }
            }
            else
            {
                Debug.Log("[BlockFlip_Gameplay] Total units detected in right direction: " + (blockCount));
                return blockCount;
            }
        }
    }


    private int DepthDetector()
    {
        Vector3 detectOriginPoint = transform.position + Vector3.forward * 4f;

        int blockCount = 1;
        while (true)
        {
            Vector3 nextDepthPoint = detectOriginPoint + Vector3.down * 0.95f * blockCount;
            RaycastHit hit;
            if (Physics.Raycast(nextDepthPoint, -Vector3.forward, out hit, 100f))
            {
                if (hit.collider.gameObject == gameObject)
                {
                    blockCount += 1;
                }
                else
                {
                    Debug.Log("[BlockFlip_Gameplay] Total units detected in depth direction: " + blockCount);
                    return blockCount;
                }
            }
            else
            {
                Debug.Log("[BlockFlip_Gameplay] Total unitss detected in depth direction: " + blockCount);
                return blockCount; ;
            }
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(forwardRotationPoint, 0.1f);
        Gizmos.color = Color.green;
        Gizmos.DrawSphere(rightRotationPoint, 0.1f);
        Gizmos.color = Color.blue;
        Gizmos.DrawSphere(backwardRotationPoint, 0.1f);
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(leftRotationPoint, 0.1f);
    }
}
