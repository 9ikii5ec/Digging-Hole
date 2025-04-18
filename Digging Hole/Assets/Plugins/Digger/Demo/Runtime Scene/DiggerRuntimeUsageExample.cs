﻿using Digger.Modules.Core.Sources;
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
        public float digDistance = 5f;

        [Header("Persistence parameters")]
        public KeyCode keyToPersistData = KeyCode.P;
        public KeyCode keyToDeleteData = KeyCode.K;

        [Header("References")]
        //[SerializeField] private ShovelSettings Shovel;

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
            }
            else if (Input.GetKeyDown(keyToDeleteData))
            {
                diggerMasterRuntime.DeleteAllPersistedData();
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
                        diggerMasterRuntime.ModifyAsyncBuffured(hit.point, brush, action, textureIndex, opacity, size);
                    else
                        diggerMasterRuntime.Modify(hit.point, brush, action, textureIndex, opacity, size);

                    //Shovel?.Swing();
                }
            }
        }
    }
}