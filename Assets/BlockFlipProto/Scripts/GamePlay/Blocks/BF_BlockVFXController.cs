using UnityEngine;
using DG.Tweening;
using BlockFlipProto.Level;
using UnityEngine.Rendering;
using UnityEngine.Tilemaps;

namespace BlockFlipProto.Gameplay
{
    public class BF_BlockVFXController : MonoBehaviour
    {
        [SerializeField] private MeshRenderer blockMeshRenderer;
        [SerializeField] private Outline outlineFX;

        public void AnimateBlockReachingHome(TileType homeTileType)
        {
            Sequence sequence = DOTween.Sequence();
            sequence.Append(transform.DOScale(Vector3.one * 1.2f, 0.2f))
                    .Append(transform.DOScale(Vector3.one, 0.2f));

            sequence.OnComplete(() => MoveBlockInDesiredDirection(homeTileType));
        }

        private void MoveBlockInDesiredDirection(TileType homeTileType)
        {
            MovementDirections movementDirection = BF_BlocksController.Instance.TileMovementDirectionsInHome.Find(t => t.tileIndex == homeTileType).movementDirection;

            Debug.Log($"#san Moving block in direction: {movementDirection} for tile type: {homeTileType}");
            switch (movementDirection)
            {
                case MovementDirections.Left:
                    transform.DOMoveX(-10f, 1f);
                    break;
                case MovementDirections.Right:
                    transform.DOMove(transform.position + Vector3.right * 10f, 1f);
                    break;
                case MovementDirections.Up:
                    transform.DOMoveZ(10f, 1f);
                    break;
                case MovementDirections.Down:
                    transform.DOMoveZ(-10f, 1f);
                    break;
            }
        }

        public void AssignBlockMaterial(TileType tileType)
        {
            foreach (BlockMaterialType blockMaterialType in BF_BlocksController.Instance.BlockMaterialType)
            {
                Debug.Log($"Assigning material for tile type: {tileType} with block material type: {blockMaterialType.tileType}");
                if (blockMaterialType.tileType == tileType)
                {
                    blockMeshRenderer.material = blockMaterialType.material;
                    return;
                }
            }

        }

        public void PlayDissolveEffect()
        {
            Vector3 currentOffset = new Vector3(0, -1f, 0);
            Vector3 endOffset = new Vector3(0, 1f, 0);

            DOTween.To(
                () => currentOffset,
                value =>
                {
                    currentOffset = value;
                    blockMeshRenderer.material.SetVector("DissolveOffest", value);
                },
                endOffset,
                2f
            ).SetEase(Ease.InOutSine);
        }


        public void SetOutlineActive(bool isActive)
        {
            if (outlineFX != null)
            {
                outlineFX.enabled = isActive;
            }
        }
    }
}