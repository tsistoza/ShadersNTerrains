using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GenerateSmoothHeightMap : MonoBehaviour
{
    // Start is called before the first frame update
    public Terrain baseTerrain;
    public TerrainData terrainData;
    public float scaleHeight = 0.1f;
    public int kernelSize = 3;

    void Start()
    {
        if (baseTerrain == null)
        {
            return;
        }
        terrainData = baseTerrain.terrainData;
        int res = terrainData.heightmapResolution;
        float normalizedScaleHeight = scaleHeight / res;

        float[,] heightMap = terrainData.GetHeights(0, 0, res, res);

        // Randomize HeightMap
        /* for (int y=0; y<heightMap.GetLength(0); y++)
            for (int x=0; x<heightMap.GetLength(1); x++)
                heightMap[y, x] = Random.Range(-normalizedScaleHeight, normalizedScaleHeight); */

        // Smooth HeightMap using BoxBlur Algorithm
        int halfKernel = kernelSize / 2;
        for (int y=0; y<heightMap.GetLength(0); y++)
        {
            for (int x=0; x<heightMap.GetLength(1); x++)
            {
                int count = 0;
                float sum = BoxBlurSum(halfKernel, y, x, ref heightMap, ref count);
                heightMap[y, x] = sum / count;
            }
        }
        terrainData.SetHeights(0, 0, heightMap);
    }

    private float BoxBlurSum(int halfKernel, int y, int x, ref float[,] heightMap, ref int count)
    {
        float sum = 0f;
        for (int ky=-halfKernel; ky<halfKernel; ky++)
        {
            for (int kx=-halfKernel; kx<halfKernel; kx++)
            {
                int newY = y + ky;
                int newX = x + kx;

                if (newY < 0 || newX < 0 || newY >= heightMap.GetLength(0) || newX >= heightMap.GetLength(1)) continue;
                sum += heightMap[newY, newX];
                count++;
            }
        }

        return sum;
    }

}
