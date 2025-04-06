using Digger.Modules.Core.Sources;
using Digger.Modules.Runtime.Sources;
using UnityEngine;
using DG.Tweening;

namespace Digger
{
    public class DiggerRuntimeUsageExample : MonoBehaviour
    {
        [Header("Async parameters")]
        public bool editAsynchronously = true;

        [Header("Modification parameters")]
        public BrushType brush = BrushType.Sphere;
        public ActionType action = ActionType.Dig;
        [Range(0, 7)] public int textureIndex;
        [Range(0.5f, 10f)] public float size = 4f;
        [Range(0f, 1f)] public float opacity = 0.5f;

        [Header("Restriction parameters")]
        public float digDistance = 5f;

        [Header("Persistence parameters")]
        public KeyCode keyToPersistData = KeyCode.P;
        public KeyCode keyToDeleteData = KeyCode.K;

        [Header("Shovel animation")]
        public Transform shovel;
        public float swingDuration = 0.2f;
        public float swingAngle = 60f;

        private DiggerMasterRuntime diggerMasterRuntime;
        private Transform playerTransform;

        private void Start()
        {
            diggerMasterRuntime = FindObjectOfType<DiggerMasterRuntime>();
            playerTransform = transform;

            if (!diggerMasterRuntime)
            {
                enabled = false;
                Debug.LogWarning("DiggerRuntimeUsageExample требует DiggerMasterRuntime в сцене. Скрипт будет отключен.");
            }
        }

        private void Update()
        {
            if (Input.GetKeyDown(keyToPersistData))
            {
                diggerMasterRuntime.PersistAll();
#if !UNITY_EDITOR
                Debug.Log("Persisted all modified chunks");
#endif
            }
            else if (Input.GetKeyDown(keyToDeleteData))
            {
                diggerMasterRuntime.DeleteAllPersistedData();
#if !UNITY_EDITOR
                Debug.Log("Deleted all persisted data");
#endif
            }

            if (Input.GetMouseButtonDown(0))
            {
                Digging();
            }
        }

        public void Digging()
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out var hit, 2000f))
            {
                float distance = Vector3.Distance(playerTransform.position, hit.point);

                if (distance <= digDistance)
                {
                    if (editAsynchronously)
                    {
                        diggerMasterRuntime.ModifyAsyncBuffured(hit.point, brush, action, textureIndex, opacity, size);
                    }
                    else
                    {
                        diggerMasterRuntime.Modify(hit.point, brush, action, textureIndex, opacity, size);
                    }

                    AnimateShovel(hit.point);
                }
            }
        }

        private void AnimateShovel(Vector3 targetPoint)
        {
            shovel.DOKill();

            Vector3 originalRotation = shovel.localEulerAngles;
            Vector3 swingForward = originalRotation + new Vector3(-swingAngle, 0f, 0f);

            shovel
                .DOLocalRotate(swingForward, swingDuration * 0.5f, RotateMode.Fast)
                .SetEase(Ease.OutQuad)
                .OnComplete(() =>
                {
                    shovel
                        .DOLocalRotate(originalRotation, swingDuration * 0.5f, RotateMode.Fast)
                        .SetEase(Ease.InQuad);
                });
        }

    }
}
