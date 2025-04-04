using Digger.Modules.Core.Sources;
using Digger.Modules.Runtime.Sources;
using UnityEngine;

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
        public float digRadius = 5f;

        [Header("Persistence parameters")]
        public KeyCode keyToPersistData = KeyCode.P;
        public KeyCode keyToDeleteData = KeyCode.K;

        private DiggerMasterRuntime diggerMasterRuntime;
        private Transform playerTransform;

        private void Start()
        {
            diggerMasterRuntime = FindObjectOfType<DiggerMasterRuntime>();
            playerTransform = transform;

            if (!diggerMasterRuntime)
            {
                enabled = false;
                Debug.LogWarning(
                    "DiggerRuntimeUsageExample ������� DiggerMasterRuntime � �����. ������ ����� ��������.");
            }
        }

        private void Update()
        {
            if (Input.GetMouseButton(0))
            {
                if (Physics.Raycast(transform.position, transform.forward, out var hit, 2000f))
                {
                    float distance = Vector3.Distance(playerTransform.position, hit.point);

                    if (distance <= digRadius)
                    {
                        if (editAsynchronously)
                        {
                            diggerMasterRuntime.ModifyAsyncBuffured(hit.point, brush, action, textureIndex, opacity, size);
                        }
                        else
                        {
                            diggerMasterRuntime.Modify(hit.point, brush, action, textureIndex, opacity, size);
                        }
                    }
                }
            }

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
        }
    }
}
