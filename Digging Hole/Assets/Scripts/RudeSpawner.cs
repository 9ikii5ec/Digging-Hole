using UnityEngine;

public class RudeSpawner : MonoBehaviour
{
    [Header("Ore Prefabs")]
    [SerializeField] private GameObject stonePrefab;
    [SerializeField] private GameObject ironPrefab;
    [SerializeField] private GameObject goldPrefab;

    [Header("Grid Settings")]
    [SerializeField] private int width = 10;
    [SerializeField] private int height = 30;
    [SerializeField] private float blockSize = 1f;

    [Header("Layer Heights")]
    [SerializeField] private int stoneHeight = 10;
    [SerializeField] private int ironHeight = 10;
    [SerializeField] private int goldHeight = 10;

    [Header("Randomization")]
    [Range(0f, 1f)][SerializeField] private float spawnChance = 0.9f;
    [SerializeField] private float zOffsetRange = 0.5f;

    private void Start()
    {
        SpawnOres();
    }

    private void SpawnOres()
    {
        Vector3 origin = transform.position;

        for (int y = 0; y < height; y++)
        {
            GameObject prefabToSpawn = null;

            if (y < goldHeight)
                prefabToSpawn = goldPrefab;
            else if (y < goldHeight + ironHeight)
                prefabToSpawn = ironPrefab;
            else if (y < goldHeight + ironHeight + stoneHeight)
                prefabToSpawn = stonePrefab;
            else
                continue;

            for (int x = 0; x < width; x++)
            {
                if (Random.value > spawnChance)
                    continue;

                float zOffset = Random.Range(-zOffsetRange, zOffsetRange);

                Vector3 spawnPos = origin + new Vector3(x * blockSize, y * blockSize, zOffset);
                Instantiate(prefabToSpawn, spawnPos, Quaternion.identity, transform);
            }
        }
    }

    private void OnDrawGizmos()
    {
        Vector3 origin = transform.position;
        int totalHeight = goldHeight + ironHeight + stoneHeight;

        for (int y = 0; y < totalHeight; y++)
        {
            Color layerColor;

            if (y < goldHeight)
                layerColor = new Color(1f, 0.84f, 0f, 0.2f);
            else if (y < goldHeight + ironHeight)
                layerColor = new Color(0.6f, 0.6f, 0.6f, 0.2f);
            else
                layerColor = new Color(0.4f, 0.3f, 0.2f, 0.2f);

            Gizmos.color = layerColor;
            Vector3 center = origin + new Vector3((width * blockSize) / 2f, y * blockSize + blockSize / 2f, 0f);
            Vector3 size = new Vector3(width * blockSize, blockSize, zOffsetRange * 2f);
            Gizmos.DrawCube(center, size);
        }

        Gizmos.color = Color.white;
        Vector3 totalCenter = origin + new Vector3((width * blockSize) / 2f, (totalHeight * blockSize) / 2f, 0f);
        Vector3 totalSize = new Vector3(width * blockSize, totalHeight * blockSize, zOffsetRange * 2f);
        Gizmos.DrawWireCube(totalCenter, totalSize);
    }
}
