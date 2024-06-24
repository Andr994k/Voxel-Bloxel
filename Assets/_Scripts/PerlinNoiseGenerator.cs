using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PerlinNoiseGenerator : MonoBehaviour
{
    public int width = 256;
    public int height = 256;
    public float scale = 20f;

    public NoiseSettings NoiseSettings;

    private void Start()
    {
        Renderer renderer = GetComponent<Renderer>();
        renderer.material.mainTexture = GenerateTexture();
    }

    private Texture2D GenerateTexture()
    {
        Texture2D texture = new Texture2D(width, height);

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                float xCoord = (float)x / width * scale;
                float yCoord = (float)y / height * scale;
                float sample = MyNoise.OctavePerlin(xCoord, yCoord, NoiseSettings);
                Color color = new Color(sample, sample, sample);
                texture.SetPixel(x, y, color);
            }
        }

        texture.Apply();
        return texture;
    }
}
