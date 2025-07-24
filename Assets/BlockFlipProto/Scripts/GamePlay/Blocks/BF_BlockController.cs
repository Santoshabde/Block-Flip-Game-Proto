using System;
using UnityEngine;

namespace BlockFlipProto.Gameplay
{
    public class BF_BlockController : MonoBehaviour
    {

        [Header("Parent Components")]
        [SerializeField] private BF_BlockMovementController blockMovementController;
        [SerializeField] private BF_BlockTileChecker blockTileChecker;
        [SerializeField] private BF_BlockVFXController blockVFXController;

        public BF_BlockMovementController BlockMovementController => blockMovementController;

        public BF_BlockTileChecker BlockTileChecker => blockTileChecker;
        public BF_BlockVFXController BlockVFXController => blockVFXController;
    }
}