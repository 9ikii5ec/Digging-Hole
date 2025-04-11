using Digger.Modules.Core.Sources;
using Digger.Modules.Runtime.Sources;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

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

    [Header("Settings")]
    [SerializeField] private ShovelSettings Shovel;
    [SerializeField] private UpgradeTools tools;

    [Header("Other")]
    [SerializeField] private Battery battery;
    [SerializeField] private float energyShovelCost = 1f;
    [SerializeField] private JetPuck jetPuck;
    [SerializeField] private Inventory backPuck;
    [SerializeField] private Text height;
    [SerializeField] private GameObject restartButton;

    private DiggerMasterRuntime diggerMasterRuntime;
    private Transform playerTransform;
    private bool isCanDigging = true;

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
        tools.PlaceLamp();
        height.text = "Height: " + transform.position.y.ToString("F1") + " m";
        if (transform.position.y <= -60f)
        {
            restartButton.SetActive(true);
            isCanDigging = false;
            Cursor.lockState = CursorLockMode.None;
        }

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
        if (!isCanDigging) return;

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

        if (y <= -50f)
        {
            action = ActionType.PaintHoles;
        }
        else if (y <= -30f)
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

    public void RestartScene()
    {
        int currentScene = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentScene);
    }
}
