using Digger.Modules.Core.Sources;
using Digger.Modules.Runtime.Sources;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

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
    [SerializeField] private UpgradeTools tools;

    [Header("Other")]
    [SerializeField] private Battery battery;
    [SerializeField] private float energyShovelCost = 1f;
    [SerializeField] private JetPuck jetPuck;
    [SerializeField] private Inventory backPuck;
    [SerializeField] private Text height;
    [SerializeField] private GameObject restartButton;
    [SerializeField] private Terrain currentTerrain;
    [SerializeField] private Camera fpsCamera;

    private DiggerMasterRuntime diggerMasterRuntime;
    private Transform playerTransform;
    private bool isCanDigging = true;
    private FirstPersonMovement movement;

    private void Start()
    {
        //Cursor.lockState = CursorLockMode.Locked;

        diggerMasterRuntime = FindObjectOfType<DiggerMasterRuntime>();
        movement = GetComponent<FirstPersonMovement>();

        if (diggerMasterRuntime != null)
        {
            ResetTerrainHoles();
        }
        else
        {
            Debug.LogWarning("DiggerMasterRuntime не найден после инициализации.");
        }

        playerTransform = transform;
        defaultSize = size;

        if (!diggerMasterRuntime)
        {
            enabled = false;
            Debug.LogWarning("DiggerRuntime требует DiggerMasterRuntime. Скрипт отключён.");
        }
    }

    private void Update()
    {
        UpdateUI();
        CheckPlayerState();
        HandleDiggingInput();
    }

    private void UpdateUI()
    {
        height.text = $"Height: {transform.position.y:F1} m";
    }

    private void CheckPlayerState()
    {
        bool isDead = transform.position.y <= -75f || battery.energy <= 0.5f;

        movement.canRun = !isDead;
        restartButton.SetActive(isDead);
        isCanDigging = !isDead;
    }

    private void HandleDiggingInput()
    {
        if (!IsDigInput())
            return;

        backPuck.UpdateInventoryRaycast();

        if (battery.energy > 0)
            Digging();
    }


    private bool IsDigInput()
    {
        if (Input.touchCount > 0)
        {
            Vector2 touchPos = Input.GetTouch(0).position;
            if (IsInDigArea(touchPos) && Input.GetTouch(0).phase == TouchPhase.Began)
                return true;
        }

        if (Input.GetMouseButtonDown(0))
        {
            Vector2 mousePos = Input.mousePosition;
            if (IsInDigArea(mousePos))
                return true;
        }

        return false;
    }

    private bool IsInDigArea(Vector2 position)
    {
        float screenWidth = Screen.width;
        float screenHeight = Screen.height;

        // Пример: только правая треть экрана
        Rect digArea = new Rect(screenWidth * 0.23f, 0, screenWidth * 0.6f, screenHeight);

        return digArea.Contains(position);
    }

    private Vector2 GetInputPosition()
    {
        // Если есть тач — используем его
        if (Input.touchCount > 0)
            return Input.GetTouch(0).position;

        // Если нажата кнопка мыши — используем мышь
        if (Input.GetMouseButton(0) || Input.GetMouseButtonDown(0))
            return Input.mousePosition;

        // Если ничего нет — возвращаем Vector2.zero
        return Vector2.zero;
    }

    public void Digging()
    {
        if (!isCanDigging || diggerMasterRuntime == null)
            return;

        Vector2 inputPos = GetInputPosition();

        Ray ray = fpsCamera.ScreenPointToRay(inputPos);
        if (Physics.Raycast(ray, out var hit, 2000f))
        {
            float distance = Vector3.Distance(playerTransform.position, hit.point);

            if (distance <= digDistance)
            {
                CheckYHeight();

#if UNITY_WEBGL
                diggerMasterRuntime.Modify(hit.point, brush, action, textureIndex, opacity, size);
#else
                if (editAsynchronously)
                    diggerMasterRuntime.ModifyAsyncBuffured(hit.point, brush, action, textureIndex, opacity, size);
                else
                    diggerMasterRuntime.Modify(hit.point, brush, action, textureIndex, opacity, size);
#endif

                battery.MinusBatteryEnergy(energyShovelCost);
            }
        }
    }

    private void CheckYHeight()
    {
        float y = transform.position.y;
        size = defaultSize;

        if (y <= -70f)
            transform.position = transform.position + new Vector3(0f, -10f, 0f);
        else if (y <= -30f)
        {
            textureIndex = 3;
            size -= 1f;
        }
        else if (y <= -15f)
        {
            textureIndex = 2;
            size -= 0.5f;
        }
        else if (y <= -1f)
        {
            textureIndex = 1;
        }
        else if (y >= 1f)
        {
            textureIndex = 0;
        }
    }

    public void RestartScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private void ResetTerrainHoles()
    {
        var terrainData = currentTerrain.terrainData;
        var res = terrainData.holesResolution;
        var holes = new bool[res, res];

        for (int x = 0; x < res; x++)
        {
            for (int y = 0; y < res; y++)
            {
                holes[x, y] = true;
            }
        }

        terrainData.SetHoles(0, 0, holes);
    }
}
