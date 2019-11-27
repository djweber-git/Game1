using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HexMap : MonoBehaviour
{
    public const int CURRENT_RADIUS = 2;
    public GameObject HexPrefab;
  
    
    
    // Start is called before the first frame update
    void Start()
    {

        GenerateChunk(0,0,CURRENT_RADIUS);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
        
    public void GenerateChunk(int x, int y, int radius)
    {
        GameObject hGO = Instantiate(HexPrefab, new Vector3(x,y), Quaternion.identity, this.transform);
        Hex h = new Hex(x,y);
        hGO.Hex = h;
    }
}
