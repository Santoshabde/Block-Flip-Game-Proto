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
            PlayClearEffect();
            //  Sequence sequence = DOTween.Sequence();
            //  sequence.Append(transform.DOScale(Vector3.one * 1.2f, 0.2f))
            //          .Append(transform.DOScale(Vector3.one, 0.2f));

            //  sequence.OnComplete(() => MoveBlockInDesiredDirection(homeTileType));
        }

        private void MoveBlockInDesiredDirection(TileType homeTileType)
        {
            MovementDirections movementDirection = BF_BlocksController.Instance.TileMovementDirectionsInHome.Find(t => t.tileIndex == homeTileType).movementDirection;

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

        public void PlayClearEffect()
        {
            Transform blockTransform = transform;

            blockTransform.localScale = Vector3.one;
            Material mat = blockMeshRenderer.material;
            Color originalColor = mat.color;

            Sequence seq = DOTween.Sequence();

            // 0. Rise up by 2 units
            seq.Append(blockTransform.DOMoveY(blockTransform.position.y + 2f, 0.2f).SetEase(Ease.OutCubic));

            // 1. Flash Color (white flash effect)
            seq.Append(mat.DOColor(Color.white, 0.05f));
            seq.Append(mat.DOColor(originalColor, 0.1f));

            // 2. Pop scale up with squash
            seq.Join(blockTransform.DOScale(new Vector3(1.4f, 0.7f, 1.4f), 0.1f).SetEase(Ease.OutQuad));

            // 3. Stretch back before vanish
            seq.Append(blockTransform.DOScale(new Vector3(0.6f, 1.3f, 0.6f), 0.1f).SetEase(Ease.InOutQuad));

            // 4. Quick spin + scale vanish
            seq.Join(blockTransform.DORotate(new Vector3(0, 0, 180), 0.2f, RotateMode.FastBeyond360).SetEase(Ease.InQuad));
            seq.Append(blockTransform.DOScale(Vector3.zero, 0.2f).SetEase(Ease.InBack));

            // 5. (Optional) disable or destroy
            seq.OnComplete(() =>
            {
                gameObject.SetActive(false);
            });
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