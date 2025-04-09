using Digger.Modules.Core.Sources;
using Digger.Modules.Runtime.Sources;
using UnityEngine;

public class DiggerRuntime : MonoBehaviour
{
    [Header("Async parameters")]
    public bool editAsynchronously = true;

    [Header("Modification parameters")]
    public BrushType brush = BrushType.Sphere;
    public ActionType action = ActionType.Dig;
    [Range(0, 7)] public int textureIndex;
    [Range(0.5f, 10f)] public float size = 4f;
    [Range(0f, 1f)] public float opacity = 0.5f;
    public float defaultSize;

    [Header("Restriction parameters")]
    public float digDistance = 5f;

    [Header("Persistence parameters")]
    public KeyCode keyToPersistData = KeyCode.P;
    public KeyCode keyToDeleteData = KeyCode.K;

    [Header("Shovel")]
    [SerializeField] private ShovelSettings Shovel;

    [Header("Other")]
    [SerializeField] private Battery battery;
    [SerializeField] private float energyShovelCost = 1f;
    [SerializeField] private JetPuck jetPuck;
    [SerializeField] private Inventory backPuck;

    private DiggerMasterRuntime diggerMasterRuntime;
    private Transform playerTransform;

    private void Start()
    {
        diggerMasterRuntime = FindObjectOfType<DiggerMasterRuntime>();
        playerTransform = transform;
        defaultSize = size;

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
            backPuck.UpdateInventoryRaycast();

            if (battery.energy > 0)
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
                CheckYHeight();

                if (editAsynchronously)
                    diggerMasterRuntime.ModifyAsyncBuffured(hit.point, brush, action, textureIndex, opacity, size);
                else
                    diggerMasterRuntime.Modify(hit.point, brush, action, textureIndex, opacity, size);

                Shovel?.Swing();
                battery.MinusBatteryEnergy(energyShovelCost);
            }
        }
    }


    private void CheckYHeight()
    {
        float y = transform.position.y;
        size = defaultSize;

        if (y <= -30f)
        {
            textureIndex = 3;
            size -= 1f;
        }
        else if (y <= -15f)
        {
            textureIndex = 4;
            size -= 0.5f;
        }
        else if (y <= -1f)
        {
            textureIndex = 2;
            size = defaultSize;
        }
        else if (y >= 1f)
        {
            textureIndex = 0;
            size = defaultSize;
        }
    }

}
