using UnityEngine;

public class RudeSpawner : MonoBehaviour
{
    [Header("Ore Prefabs")]
    [SerializeField] private GameObject stonePrefab;
    [SerializeField] private GameObject ironPrefab;
    [SerializeField] private GameObject goldPrefab;
    [SerializeField] private GameObject coalPrefab;
    [SerializeField] private GameObject copperPrefab;
    [SerializeField] private GameObject diamondPrefab;
    [SerializeField] private GameObject platinumPrefab;

    [Header("Grid Settings")]
    [SerializeField] private int width = 10;
    [SerializeField] private int height = 70;
    [SerializeField] private float blockSize = 1f;

    [Header("Layer Heights")]
    [SerializeField] private int platinumHeight = 5;
    [SerializeField] private int diamondHeight = 5;
    [SerializeField] private int goldHeight = 10;
    [SerializeField] private int copperHeight = 10;
    [SerializeField] private int ironHeight = 10;
    [SerializeField] private int coalHeight = 10;
    [SerializeField] private int stoneHeight = 20;

    [Header("Randomization")]
    [Range(0f, 100f)][SerializeField] private float spawnChance = 0.9f;
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

            int currentHeight = 0;

            if (y < (currentHeight += platinumHeight))
                prefabToSpawn = platinumPrefab;
            else if (y < (currentHeight += diamondHeight))
                prefabToSpawn = diamondPrefab;
            else if (y < (currentHeight += goldHeight))
                prefabToSpawn = goldPrefab;
            else if (y < (currentHeight += copperHeight))
                prefabToSpawn = copperPrefab;
            else if (y < (currentHeight += ironHeight))
                prefabToSpawn = ironPrefab;
            else if (y < (currentHeight += coalHeight))
                prefabToSpawn = coalPrefab;
            else if (y < (currentHeight += stoneHeight))
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
        int totalHeight = platinumHeight + diamondHeight + goldHeight + copperHeight + ironHeight + coalHeight + stoneHeight;

        for (int y = 0; y < totalHeight; y++)
        {
            Color layerColor;
            int currentHeight = 0;

            if (y < (currentHeight += platinumHeight))
                layerColor = new Color(0.9f, 0.9f, 1f, 0.2f);
            else if (y < (currentHeight += diamondHeight))
                layerColor = new Color(0.5f, 1f, 1f, 0.2f);
            else if (y < (currentHeight += goldHeight))
                layerColor = new Color(1f, 0.84f, 0f, 0.2f);
            else if (y < (currentHeight += copperHeight))
                layerColor = new Color(0.8f, 0.5f, 0.2f, 0.2f);
            else if (y < (currentHeight += ironHeight))
                layerColor = new Color(0.6f, 0.6f, 0.6f, 0.2f);
            else if (y < (currentHeight += coalHeight))
                layerColor = new Color(0.1f, 0.1f, 0.1f, 0.2f);
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
