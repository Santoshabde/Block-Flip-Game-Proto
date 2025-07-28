using System.Collections;
using UnityEngine;

namespace SNGames.CommonModule
{
    public class CameraShakeEffect : MonoBehaviour
    {
        private Vector3 originalPosition;
        private Coroutine shakeCoroutine;

        void Start()
        {
            SNEventsController<InGameEvents>.RegisterEvent(InGameEvents.FreshLevelStarted, FreshLevelStarted);
        }

        private void FreshLevelStarted(object obj)
        {
            DeInit();
            Init();
        }

        public void Init()
        {
            SNEventsController<InGameEvents>.RegisterEvent(InGameEvents.BlockRotationNotPossible, BlockRotationNotPossible);
            originalPosition = transform.localPosition;
        }

        public void DeInit()
        {
            SNEventsController<InGameEvents>.DeregisterEvent(InGameEvents.BlockRotationNotPossible, BlockRotationNotPossible);
        }

        void OnDestroy()
        {
            DeInit();
            SNEventsController<InGameEvents>.DeregisterEvent(InGameEvents.FreshLevelStarted, FreshLevelStarted);
        }

        private void BlockRotationNotPossible(object obj)
        {
            Debug.Log("[BlockFlip_Gameplay] #san Block rotation not possible, shaking camera.");
            Shake(0.15f, 0.07f);
        }

        public void Shake(float duration, float magnitude)
        {
            if (shakeCoroutine != null)
            {
                StopCoroutine(shakeCoroutine);
                transform.localPosition = originalPosition;
            }

            shakeCoroutine = StartCoroutine(ShakeCoroutine(duration, magnitude));
        }

        private IEnumerator ShakeCoroutine(float duration, float magnitude)
        {
            float elapsed = 0f;

            while (elapsed < duration)
            {
                transform.localPosition = originalPosition + Random.insideUnitSphere * magnitude;
                elapsed += Time.deltaTime;
                yield return null;
            }

            transform.localPosition = originalPosition;
            shakeCoroutine = null;
        }
    }
}
