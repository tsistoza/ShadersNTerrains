using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomHeightMap : MonoBehaviour
{
    public Terrain baseTerrain;
    public TerrainData terrainData;
    public float scaleHeight = 0.001f;

    // Start is called before the first frame update
    void Start()
    {
        if (baseTerrain == null)
            return;

        Vector3 terrainTransform = baseTerrain.gameObject.transform.position;
        terrainData = baseTerrain.terrainData;
        float[,] heightMap = terrainData.GetHeights(0, 0, terrainData.heightmapResolution, terrainData.heightmapResolution);

        float normalizedScaleHeight = scaleHeight / terrainData.heightmapResolution;
        for (int y=0; y<heightMap.GetLength(0); y++)
        {
            for (int x=0; x<heightMap.GetLength(1); x++)
            {
                if (x == 0 && y == 0)
                    heightMap[0, 0] = 0f;
                else if (x == 0)
                    heightMap[y, x] = heightMap[y - 1, heightMap.GetLength(1) - 1] + normalizedScaleHeight;
                else
                    heightMap[y, x] = heightMap[y, x - 1] + normalizedScaleHeight;
                if (heightMap[y, x] < 0.0f || heightMap[y, x] > 1.0f) heightMap[y, x] = Mathf.Clamp01(heightMap[y, x]);
            }
        }

        terrainData.SetHeights(0, 0, heightMap);
    }

    /*private void GetNeighbors(int y, int x, ref float[,] heightMap, ref List<(int,int)> neighbors)
    {
        int[] dirX = { -1, 0, 1, 0 };
        int[] dirY = { 0, -1, 0, 1 };
        for (int i=0; i<4; i++)
        {
            int newY = y + dirY[i];
            int newX = x + dirX[i];

            if (newY < 0 || newX < 0 || newY > heightMap.GetLength(0) || newX > heightMap.GetLength(1)) continue;

            neighbors.Add((newY, newX));
        }
        return;
    }*/
}
