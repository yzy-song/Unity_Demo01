using UnityEngine;

public class SpineDissolveEffect : MonoBehaviour
{
    private Material originalMaterial;
    public Material dissolveMaterial;
    private float dissolveAmount = 0f;
    private MeshRenderer meshRenderer;

    void Start()
    {
        meshRenderer = GetComponent<MeshRenderer>();

        // 保存原始材质
        originalMaterial = meshRenderer.material;

    }

    public void StartDissolve()
    {
        if (dissolveMaterial == null || meshRenderer == null)
        {
            Debug.LogError("溶解材质未找到！");
            return;
        }

        // 替换材质为溶解材质
        meshRenderer.material = dissolveMaterial;

        // 开始溶解动画
        StartCoroutine(DissolveCoroutine());
    }

    private System.Collections.IEnumerator DissolveCoroutine()
    {
        dissolveAmount = 0f;
        while (dissolveAmount < 1f)
        {
            dissolveAmount += Time.deltaTime * 0.5f; // 控制溶解速度
            meshRenderer.material.SetFloat("_DissolveAmount", dissolveAmount);
            yield return null;
        }

        // 溶解完成后销毁对象
        // Destroy(gameObject);
    }

    public void ResetMaterial()
    {
        // 恢复原始材质
        if (originalMaterial != null && meshRenderer != null)
        {
            meshRenderer.material = originalMaterial;
        }
    }
}
