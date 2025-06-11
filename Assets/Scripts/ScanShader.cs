using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScanShader : MonoBehaviour
{
    public float scanSpeed;
    public Terrain baseTerrain;

    private float baseScanSpeed;
    private Material terrainMaterial;
    private float scanDistance;

    // Start is called before the first frame update
    void Start()
    {
        terrainMaterial = baseTerrain.materialTemplate;
        scanDistance = 1.0f;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mousePos = Input.mousePosition;
            RaycastHit hit;
            if (Physics.Raycast(Camera.main.ScreenPointToRay(mousePos), out hit))
            {
                terrainMaterial.SetVector("_ScanOrigin", hit.point);
                scanDistance = 0.0f;
                baseScanSpeed = scanSpeed;
            }
        }
        scanDistance += Time.deltaTime * baseScanSpeed;
        if (scanDistance > 50f)
        {
            scanDistance = 100.0f;
            baseScanSpeed = 0.0f;
        }
        terrainMaterial.SetFloat("_ScanDistance", scanDistance);
    }
}
