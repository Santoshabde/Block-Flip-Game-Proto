using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using SNGames.CommonModule;
using UnityEngine;
using UnityEngine.UIElements;

namespace BlockFlipProto.Gameplay
{
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
        // Serialized variables
        [Header("Required Components")]
        [SerializeField] private BF_BlockController blockController;

        [Header("DEBUG-ONLY values")]
        [SerializeField] private int forwardBlockCount;
        [SerializeField] private int rightBlockCount;
        [SerializeField] private int depthBlockCount;
        [SerializeField] private BlockRotationDirection possibleRotationDirections;

        //Private Region
        private Vector3 forwardRotationPoint;
        private Vector3 rightRotationPoint;
        private Vector3 backwardRotationPoint;
        private Vector3 leftRotationPoint;
        private Vector2 swipeStart;
        private bool isSwiping = false;
        private bool isRotating = false;
        private float blockRotationSpeed;
        private bool blockMovement = false;
        private bool blockDestroyerPowerupActivated = false;

        // Public Region
        public Action<GameObject> onBlockDimentionCalculationBegin;
        public Action<GameObject> onBlockDimentionCalculationEnd;
        public Action OnBlockSettledDown;

        public void Init(float blockRotationSpeed)
        {
            this.blockRotationSpeed = blockRotationSpeed;

            blockController.BlockTileChecker.CalculateTilesWhichBlockOccupied();
            CalculateBlockDimentionsAndRotationPoints();

            SNEventsController<InGameEvents>.RegisterEvent(InGameEvents.PowerupActivated, PowerupActivated);
            SNEventsController<InGameEvents>.RegisterEvent(InGameEvents.PowerUpDeactivated, PowerUpDeactivated);
        }

        private void PowerUpDeactivated(object obj)
        {
            PowerupTypeInGame powerupActivated = (PowerupTypeInGame)obj;
            if (powerupActivated == PowerupTypeInGame.BlockDestroyer)
            {
                blockDestroyerPowerupActivated = false;
            }
        }

        private void PowerupActivated(object obj)
        {
            PowerupTypeInGame powerupActivated = (PowerupTypeInGame)obj;
            if (powerupActivated == PowerupTypeInGame.BlockDestroyer)
            {
                blockDestroyerPowerupActivated = true;
            }
        }

        void Update()
        {
            if (blockMovement)
                return;

            if (!blockDestroyerPowerupActivated)
                TryRotateTheBlock();
            else
                TryDestroyTheBlock();
        }

        #region Public Methods  

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

        #endregion

        #region Private Method

        private enum MovementDirections
        {
            Left,
            Right,
            Forward,
            Backward
        }

        private void TryDestroyTheBlock()
        {
            if (Input.GetMouseButtonDown(0))
            {
                RaycastHit hit;
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out hit))
                {
                    if (hit.collider != null && hit.collider.gameObject == gameObject)
                    {
                        //Opening all tiles it activated
                        List<BF_TileData> occupiedTiles = blockController.BlockTileChecker.CalculateTilesWhichBlockOccupied();
                        foreach (BF_TileData tile in occupiedTiles)
                        {
                            tile.RestoreTileStatusToEmptyOrHome();
                        }

                        SNEventsController<InGameEvents>.TriggerEvent(InGameEvents.BlockSettledInHome, this.gameObject);
                        SNEventsController<InGameEvents>.TriggerEvent(InGameEvents.PowerUpDeactivated, PowerupTypeInGame.BlockDestroyer);

                        BF_BlocksController.Instance.ClearBlock(this);
                    }
                }
            }
        }

        private MovementDirections? continuousDirection = null;
        private float rotationTimer = 0f;
        private void TryRotateTheBlock()
        {
            if (!isRotating)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    RaycastHit hit;
                    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                    if (Physics.Raycast(ray, out hit))
                    {
                        if (hit.collider != null && hit.collider.gameObject == gameObject)
                        {
                            Debug.Log("[BlockFlip_Gameplay] Block clicked: " + gameObject.name);
                            blockController.BlockVFXController.SetOutlineActive(true);

                            isSwiping = true;
                            swipeStart = Input.mousePosition;

                            CalculateBlockDimentionsAndRotationPoints();
                            possibleRotationDirections = blockController.BlockTileChecker.CalculatePosibleMovements(blockController.BlockTileChecker.CalculateTilesWhichBlockOccupied(), forwardBlockCount, rightBlockCount, depthBlockCount);
                        }
                    }
                }

                if (Input.GetMouseButton(0) && isSwiping)
                {
                    Vector2 swipeDelta = (Vector2)Input.mousePosition - swipeStart;

                    Debug.Log("[BlockFlip_Gameplay] swipeStart: " + swipeStart + ", swipeEnd: " + Input.mousePosition + ", swipeDelta: " + swipeDelta);
                    if (swipeDelta.magnitude > 10f) // set initial direction
                    {
                        if (Mathf.Abs(swipeDelta.x) > Mathf.Abs(swipeDelta.y))
                        {
                            continuousDirection = swipeDelta.x > 0 ? MovementDirections.Right : MovementDirections.Left;
                        }
                        else
                        {
                            continuousDirection = swipeDelta.y > 0 ? MovementDirections.Forward : MovementDirections.Backward;
                        }

                        // rotationTimer = 0.4f; // trigger first move instantly
                        Debug.Log("[BlockFlip_Gameplay] continuousDirection: " + continuousDirection);
                    }

                    // Perform continuous rotation
                    if (continuousDirection != null)
                    {
                        rotationTimer += Time.deltaTime;
                        if (rotationTimer >= 0.06f)
                        {
                            RotateInDirection(continuousDirection.Value);
                            rotationTimer = 0f;
                            swipeStart = Input.mousePosition;
                        }
                    }
                }
            }

            if (Input.GetMouseButtonUp(0) && isSwiping)
            {
                Debug.Log("[BlockFlip_Gameplay] Mouse button released, stopping rotation.");
                isSwiping = false;
                continuousDirection = null;

                blockController.BlockVFXController.SetOutlineActive(false);
            }
        }

        private void RotateInDirection(MovementDirections direction)
        {
            switch (direction)
            {
                case MovementDirections.Left: RotateLeft(); break;
                case MovementDirections.Right: RotateRight(); break;
                case MovementDirections.Forward: RotateForward(); break;
                case MovementDirections.Backward: RotateBackward(); break;
            }
        }

        private void RotateForward()
        {
            if (possibleRotationDirections.HasFlag(BlockRotationDirection.Forward))
                RotateBlock(forwardRotationPoint, Vector3.right);
            else
            {
                Debug.Log("[BlockFlip_Gameplay] Forward rotation not possible.");
                FakeRotation(forwardRotationPoint, Vector3.right);
            }
        }

        private void RotateRight()
        {
            if (possibleRotationDirections.HasFlag(BlockRotationDirection.Right))
                RotateBlock(rightRotationPoint, -Vector3.forward);
            else
            {
                Debug.Log("[BlockFlip_Gameplay] Right rotation not possible.");
                FakeRotation(rightRotationPoint, -Vector3.forward);
            }
        }

        private void RotateBackward()
        {
            if (possibleRotationDirections.HasFlag(BlockRotationDirection.Backward))
                RotateBlock(backwardRotationPoint, -Vector3.right);
            else
            {
                Debug.Log("[BlockFlip_Gameplay] Backward rotation not possible.");
                FakeRotation(backwardRotationPoint, -Vector3.right);
            }
        }

        private void RotateLeft()
        {
            if (possibleRotationDirections.HasFlag(BlockRotationDirection.Left))
                RotateBlock(leftRotationPoint, Vector3.forward);
            else
            {
                Debug.Log("[BlockFlip_Gameplay] Left rotation not possible.");
                FakeRotation(leftRotationPoint, Vector3.forward);
            }
        }

        private void FakeRotation(Vector3 rotationPoint, Vector3 rotationAxis)
        {
            SNEventsController<InGameEvents>.TriggerEvent(InGameEvents.BlockRotationNotPossible, gameObject);
            StartCoroutine(FakeRotationIEnum(rotationPoint, rotationAxis));
        }

        private IEnumerator FakeRotationIEnum(Vector3 rotationPoint, Vector3 rotationAxis)
        {
            isRotating = true;

            float rotated = 0f;
            float rotationSpeed = blockRotationSpeed;

            // Rotate 30 degrees
            while (rotated < 15f)
            {
                float step = rotationSpeed * Time.deltaTime;
                if (rotated + step > 15f)
                    step = 15f - rotated;

                transform.RotateAround(rotationPoint, rotationAxis, step);
                rotated += step;

                yield return null;
            }

            // Optional pause at the top
            yield return new WaitForSeconds(0.1f);

            // Rotate back to original position
            rotated = 0f;
            while (rotated < 15f)
            {
                float step = rotationSpeed * Time.deltaTime;
                if (rotated + step > 15f)
                    step = 15f - rotated;

                // Rotate back by negative angle
                transform.RotateAround(rotationPoint, rotationAxis, -step);
                rotated += step;

                yield return null;
            }

            isRotating = false;
        }

        private void RotateBlock(Vector3 rotationPoint, Vector3 rotationAxis)
        {
            StartCoroutine(RotateBlockIEnum(rotationPoint, rotationAxis));
        }

        private IEnumerator RotateBlockIEnum(Vector3 rotationPoint, Vector3 rotationAxis)
        {
            isRotating = true;
            float rotated = 0f;
            float rotationSpeed = blockRotationSpeed;

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
            var occupiedTiles = blockController.BlockTileChecker.CalculateTilesWhichBlockOccupied();
            possibleRotationDirections = blockController.BlockTileChecker.CalculatePosibleMovements(occupiedTiles, forwardBlockCount, rightBlockCount, depthBlockCount);
            blockMovement = blockController.BlockTileChecker.CheckForBlockInHomeTile(occupiedTiles);
            isRotating = false;
        }

        private int ForwardLengthDetector()
        {
            Vector3 detectOriginPoint = transform.position + Vector3.up * 4f;

            int blockCount = 0;
            while (true)
            {
                Vector3 nextForwardPoint = detectOriginPoint + Vector3.forward * 0.47f * (blockCount + 1);
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

            int blockCount = 0;
            while (true)
            {
                Vector3 nextRightPoint = detectOriginPoint + Vector3.right * 0.47f * (blockCount + 1);
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

            int blockCount = 0;
            while (true)
            {
                Vector3 nextDepthPoint = detectOriginPoint + Vector3.down * 0.47f * (blockCount + 1);
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

        #endregion

        #region  Gizmos
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
        #endregion
    }
}