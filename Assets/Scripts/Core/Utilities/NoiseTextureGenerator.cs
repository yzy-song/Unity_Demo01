using UnityEngine;

public class NoiseTextureGenerator : MonoBehaviour
{
    public int width = 256;
    public int height = 256;

    void Start()
    {
        Texture2D noiseTexture = GenerateNoiseTexture();
        // 保存为纹理资源
        byte[] bytes = noiseTexture.EncodeToPNG();
        System.IO.File.WriteAllBytes(Application.dataPath + "/NoiseTexture.png", bytes);
        Debug.Log("Noise Texture saved to: " + Application.dataPath + "/NoiseTexture.png");
    }

    private Texture2D GenerateNoiseTexture()
    {
        Texture2D texture = new Texture2D(width, height);

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                float value = Mathf.PerlinNoise((float)x / width * 10f, (float)y / height * 10f);
                texture.SetPixel(x, y, new Color(value, value, value));
            }
        }

        texture.Apply();
        return texture;
    }
}
