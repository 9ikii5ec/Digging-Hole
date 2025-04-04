using UnityEngine;

public class PrefabSettings : MonoBehaviour
{
    private void Start()
    {
        Destroy(gameObject);

        transform.Rotate(0, Random.Range(0, 360), 0);
        float scaleVariation = Random.Range(0.9f, 1.1f);
        transform.localScale *= scaleVariation;
    }
}
