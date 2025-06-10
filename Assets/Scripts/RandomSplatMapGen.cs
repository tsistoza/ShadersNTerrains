using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class RandomSplatMapGen : MonoBehaviour
{
    // Start is called before the first frame update
    public Terrain baseTerrain;
    public int splatMapResolution = 512;

    void Start()
    {
        if (baseTerrain == null)
            return;

        TerrainData terrainData = baseTerrain.terrainData;
        int textures = terrainData.alphamapTextureCount;

        float[,,] currAlphaMap = terrainData.GetAlphamaps(0, 0, terrainData.alphamapWidth, terrainData.alphamapHeight);

        for (int y=0; y<terrainData.alphamapHeight; y++)
        {
            for (int x=0; x<terrainData.alphamapWidth; x++)
            {
                int maxLayer = 0;

                for (int i=0; i<terrainData.alphamapLayers; i++)
                {
                    currAlphaMap[y, x, i] = Random.value;
                    maxLayer = (currAlphaMap[y, x, i] > currAlphaMap[y, x, maxLayer]) ? i : maxLayer;
                }

                // Gives a clear distinction between the layers, instead of blending them
                for (int i = 0; i < terrainData.alphamapLayers; i++) // Normalize
                {
                    if (i == maxLayer) currAlphaMap[y, x, i] = 1.0f;
                    else currAlphaMap[y, x, i] = 0.0f;
                }
            }
        }

        terrainData.SetAlphamaps(0, 0, currAlphaMap);
    }
}
