using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HexMap : MonoBehaviour
{
    public const int CURRENT_RADIUS = 2;
    //This array of vectors denotes the relation of all 6 neighboring chunk centers at chunk radius of 2
    //could be eventually mathmatically determined by CURRENT_RADIUS to ensure proper generation with other radius sizes
    public Vector3[] NeighborChunkCenter = new []  {new Vector3(-2,5,-3),
                                                    new Vector3(3,2,-5), 
                                                    new Vector3(5,-3,-2), 
                                                    new Vector3(2,-5,3), 
                                                    new Vector3(-3,-2,5), 
                                                    new Vector3(-5,3,2)};
    public GameObject HexPrefab;
    public Material[] HexMaterials;
    
    private Dictionary<(int,int), Hex> hexDict; 
    
    
    // Start is called before the first frame update
    void Start()
    {
        hexDict = new Dictionary<(int, int), Hex>();
        GenerateChunk(0,0,CURRENT_RADIUS);

        // GenerateChunk(-2,5,CURRENT_RADIUS);
        // GenerateChunk(3,2,CURRENT_RADIUS);
        // GenerateChunk(5,-3,CURRENT_RADIUS);
        // GenerateChunk(2,-5,CURRENT_RADIUS);
        // GenerateChunk(-3,-2,CURRENT_RADIUS);
        // GenerateChunk(-5,3,CURRENT_RADIUS);
    }

    // Update is called once per frame
    void Update()
    {
         
    }
        
    //creates a chunk at a given x and y value and a radius, will break with anything more than 2 in current implementation
    public void GenerateChunk(int xInit, int yInit, int radius)
    {
        int zInit = -(xInit + yInit);
        bool isStorehouse = true;
        for (int x = (-radius) + xInit; (-radius) + xInit <= x && x <= radius + xInit ; x++)
        {
            for (int y = (-radius) + yInit; (-radius) + yInit <= y && y<= radius + yInit ; y++)
            {
                for (int z = (-radius) + zInit; (-radius) + zInit <= z && z<= radius + zInit ; z++)
                {
                    if (x + y + z == 0)
                    {
                        Hex h = new Hex(x,y,1);
                        GameObject hGO = Instantiate(HexPrefab, h.Position(), Quaternion.identity, this.transform);
                        hexDict.Add((x,y),h);
                        if (isStorehouse) 
                        {
                            h.SetHexType(2);
                            isStorehouse = false;
                        }
                        else
                        {
                            Random.Range(0, HexMaterials.Length);
                        }
                        hGO.name = h.ToString();
                        MeshRenderer mr = hGO.GetComponentInChildren<MeshRenderer>();
                        mr.material = HexMaterials[h.GetHexType()];
                        hGO.GetComponentInChildren<TextMesh>().text = h.ToString();
                    }
                }
            }
        }
       
    }

    public Hex[] FindNeighborChunk(Hex hexInit)
    {
        Hex[] results = new Hex[6];
        
        return results;
    }
}
