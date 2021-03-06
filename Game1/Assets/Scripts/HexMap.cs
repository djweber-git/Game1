﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HexMap : MonoBehaviour
{
    public const int CURRENT_RADIUS = 2;
    //This array of vectors denotes the relation of all 6 neighboring chunk centers at chunk radius of 2
    //could be eventually mathmatically determined by CURRENT_RADIUS to ensure proper generation with other radius sizes
    public static Vector3[] NeighborChunkCenter = new []  {new Vector3(-2,5,-3),
                                                    new Vector3(3,2,-5), 
                                                    new Vector3(5,-3,-2), 
                                                    new Vector3(2,-5,3), 
                                                    new Vector3(-3,-2,5), 
                                                    new Vector3(-5,3,2)};
    public GameObject HexPrefab;
    public GameObject ChunkPreviewPrefab;
    public Material[] HexMaterials;
    public Material[] PreviewMaterials;
    
    private Dictionary<(int,int), Hex> hexDict;
    private Dictionary<(int,int), GameObject> hexGODict; 
    private List<Hex> storehouseList;
    private Dictionary<(int,int), GameObject> chunkPreviewDict;
    
    // Start is called before the first frame update
    void Start()
    {
        hexDict = new Dictionary<(int, int), Hex>();
        hexGODict = new Dictionary<(int, int), GameObject>();
        storehouseList = new List<Hex>();
        chunkPreviewDict = new Dictionary<(int, int), GameObject>();
        GenerateChunk(0,0,CURRENT_RADIUS);

        //GenerateChunk(-2,5,CURRENT_RADIUS);
        //GenerateChunk(3,2,CURRENT_RADIUS);
        //GenerateChunk(5,-3,CURRENT_RADIUS);
        //GenerateChunk(2,-5,CURRENT_RADIUS);
        //GenerateChunk(-3,-2,CURRENT_RADIUS);
        //GenerateChunk(-5,3,CURRENT_RADIUS);

        GenerateChunkPreviews();
    }

    // Update is called once per frame
    void Update()
    {
         
    }
        
    //creates a chunk at a given x and y value and a radius, will break with anything more than 2 in current implementation
    public void GenerateChunk(int xInit, int yInit, int radius)
    {
        int zInit = -(xInit + yInit);
        int numWater = 0;
        int numTree = 0;
        for (int x = (-radius) + xInit; (-radius) + xInit <= x && x <= radius + xInit ; x++)
        {
            for (int y = (-radius) + yInit; (-radius) + yInit <= y && y<= radius + yInit ; y++)
            {
                for (int z = (-radius) + zInit; (-radius) + zInit <= z && z<= radius + zInit ; z++)
                {
                    if (x + y + z == 0)
                    {
                        Hex h = new Hex(x,y);
                        GameObject hGO = Instantiate(HexPrefab, h.Position(), Quaternion.identity, this.transform);
                        hexDict.Add((x,y),h);
                        hexGODict.Add((x,y),hGO);
                        if (x == xInit && y == yInit) 
                        {
                            h.SetHexType(Hex.Types.storehouse);
                            storehouseList.Add(h);
                        }
                        else
                        {
                            int i = Random.Range(0, 8);
                            if (i == 0 || (numWater <= 2 && i==2))
                            {                   
                                h.SetHexType(Hex.Types.water);
                                numWater++;
                            }
                            else if (i == 1 || (numWater > 2 && numTree <= 2 && i==2) || (numTree < 1 && i==3))
                            {                   
                                h.SetHexType(Hex.Types.tree);
                                numTree++;
                            }
                        }
                        UpdateHex(x,y);
                        
                    }
                }
            }
        }
       
    }

    public void UpdateHex(int x, int y)
    {
        if (hexGODict.ContainsKey((x,y)))
        {
            Hex h = hexDict[(x,y)];
            GameObject hGO = hexGODict[(x,y)];
            MeshRenderer mr = hGO.GetComponentInChildren<MeshRenderer>();
            mr.material = HexMaterials[(int)h.GetHexType()];
            hGO.name = h.ToString();
            hGO.GetComponentInChildren<TextMesh>().text = h.ToString();
        }
    }

    //Returns an array of vectors length 6 to locate the storehouse of the neighboring chunk from the current chunk
    public Vector3[] FindNeighborChunks(Hex hexInit)
    {
        Vector3[] results = new Vector3[6];
        Vector3 vectorInit = new Vector3(hexInit.GetX() ,hexInit.GetY() ,hexInit.GetZ());
        for (int i = 0; i < results.Length; i++)
        {
            results[i] = vectorInit + NeighborChunkCenter[i];
        }
        return results;
    }
    
    public void GenerateChunkPreviews()
    {
        foreach(Hex storehouse in storehouseList)
        {
            Vector3[] neighbors = FindNeighborChunks(storehouse);
            for(int i = 0; i < neighbors.Length; i++)
            {
                Hex hexTemp = new Hex((int)neighbors[i].x, (int)neighbors[i].y);
                if(!storehouseList.Contains(hexTemp) && !chunkPreviewDict.ContainsKey((hexTemp.GetX(),hexTemp.GetY())))
                {
                    GameObject hexPreviewGO = Instantiate(ChunkPreviewPrefab, hexTemp.Position(), Quaternion.identity, this.transform);
                    hexPreviewGO.transform.Translate(0f,-0.1f,0f);
                    hexPreviewGO.name = hexTemp.ToString() + ",Chunk Preview";
                    hexPreviewGO.GetComponentInChildren<TextMesh>().text = (Mathf.Abs(hexTemp.GetX()) + Mathf.Abs(hexTemp.GetY()) + Mathf.Abs(hexTemp.GetZ())).ToString();
                    
                    chunkPreviewDict.Add((hexTemp.GetX(), hexTemp.GetY()),hexPreviewGO);

                }
            }
        }
    }

    public void DeleteChunkPreview(int x, int y)
    {
        if (chunkPreviewDict.ContainsKey((x,y)))
        {
            Destroy(chunkPreviewDict[(x,y)]);
            chunkPreviewDict.Remove((x,y));
        }
        
    }

    public void GenerateChunkFromPreview(GameObject obj)
    {
        string name = obj.name;
        string[] parts = name.Split(',');
        Hex hexTemp = new Hex(int.Parse(parts[0]), int.Parse(parts[1]));
        DeleteChunkPreview(hexTemp.GetX(),hexTemp.GetY());
        GenerateChunk(hexTemp.GetX(),hexTemp.GetY(),CURRENT_RADIUS);
        GenerateChunkPreviews();
    }
}
