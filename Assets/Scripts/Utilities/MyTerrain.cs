using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyTerrain : MonoBehaviour
{
    private static MyTerrain instance;
    public static MyTerrain Instance { get { return instance; }}
    private Terrain terrain;
    private float MinX;
    private float MaxX;
    private float MinZ;
    private float MaxZ;
    // Start is called before the first frame update
    void Awake()
    {
        instance = this;
        terrain = GetComponent<Terrain>();
        MinX = terrain.terrainData.size.x;
        MaxX = transform.position.x + terrain.terrainData.size.x;
        MinZ = terrain.terrainData.size.z;
        MaxZ = transform.position.z + terrain.terrainData.size.z;
    }

    // Update is called once per frame
    public Vector3 GetRandomPositionFromMyTerrain() {
        float RandX = UnityEngine.Random.Range(MinX, MinX + MaxX);
        float RandZ = UnityEngine.Random.Range(MinZ, MinZ + MaxZ);
        float Ypos = Terrain.activeTerrain.SampleHeight(new Vector3(RandX, 0, RandZ));
        Debug.Log("random coordinates x: " + RandX.ToString() + " y: " + Ypos.ToString() + "z: " + RandZ.ToString());
        return new Vector3(RandX, Ypos, RandZ);
    }
}
