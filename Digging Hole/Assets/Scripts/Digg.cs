using UnityEngine;

public class Digg : MonoBehaviour
{
    [Header("Dig Settings")]
    public float digRadius = 2f;         // Радиус копания в метрах
    public float digDepth = 0.3f;       // Глубина ямы в метрах
    public Terrain terrain;             // Ссылка на Terrain

    [Header("Visual Effects")]
    public ParticleSystem digEffect;    // Эффект копания
    public GameObject holeDecal;        // Декаль ямы (опционально)
    public AudioClip digSound;         // Звук копания

    private TerrainData terrainData;
    private int heightmapWidth;
    private int heightmapHeight;

    void Start()
    {
        if (terrain == null)
            terrain = Terrain.activeTerrain;

        terrainData = terrain.terrainData;
        heightmapWidth = terrainData.heightmapResolution;
        heightmapHeight = terrainData.heightmapResolution;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            TryDigHole();
        }
    }

    void TryDigHole()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit) && hit.collider is TerrainCollider)
        {
            DigAtPoint(hit.point);
        }
    }

    void DigAtPoint(Vector3 worldPosition)
    {
        // Конвертируем мировые координаты в координаты террейна
        Vector3 terrainPos = worldPosition - terrain.transform.position;
        Vector3 normalizedPos = new Vector3(
            terrainPos.x / terrainData.size.x,
            0,
            terrainPos.z / terrainData.size.z
        );

        // Получаем текущие высоты
        int radiusInPixels = (int)(digRadius * heightmapWidth / terrainData.size.x);
        int pixelX = (int)(normalizedPos.x * heightmapWidth);
        int pixelY = (int)(normalizedPos.z * heightmapHeight);

        // Рассчитываем область для изменения
        int startX = Mathf.Max(0, pixelX - radiusInPixels);
        int startY = Mathf.Max(0, pixelY - radiusInPixels);
        int width = Mathf.Min(radiusInPixels * 2, heightmapWidth - startX);
        int height = Mathf.Min(radiusInPixels * 2, heightmapHeight - startY);

        // Получаем данные высот
        float[,] heights = terrainData.GetHeights(startX, startY, width, height);

        // Модифицируем высоты
        float depthInHeightmap = digDepth / terrainData.size.y;

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                float distance = Vector2.Distance(
                    new Vector2(x, y),
                    new Vector2(width / 2, height / 2)
                );

                if (distance <= radiusInPixels)
                {
                    float falloff = 1 - (distance / radiusInPixels);
                    heights[y, x] = Mathf.Max(0, heights[y, x] - depthInHeightmap * falloff);
                }
            }
        }

        // Применяем изменения
        terrainData.SetHeights(startX, startY, heights);

        // Визуальные эффекты
        PlayDigEffects(worldPosition);
    }

    void PlayDigEffects(Vector3 position)
    {
        // Эффект частиц
        if (digEffect != null)
        {
            digEffect.transform.position = position;
            digEffect.Play();
        }

        // Декаль ямы
        if (holeDecal != null)
        {
            Instantiate(holeDecal, position, Quaternion.identity);
        }

        // Звук
        if (digSound != null)
        {
            AudioSource.PlayClipAtPoint(digSound, position);
        }
    }
}
