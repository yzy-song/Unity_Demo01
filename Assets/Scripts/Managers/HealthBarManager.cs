using UnityEngine;

public class HealthBarManager : Singleton<HealthBarManager>
{
    public GameObject healthBarPrefab;
    public Canvas canvas;

    public GameObject CreateHealthBar(Transform target)
    {
        GameObject healthBar = Instantiate(healthBarPrefab, canvas.transform);
        return healthBar;
    }
}
