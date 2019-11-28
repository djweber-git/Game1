using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hex 
{
    static readonly float WIDTH_MULTIPLIER = Mathf.Sqrt(3) / 2;
    private int x;
    private int y;
    private int z;
    private int type;

    public Hex(int xInit, int yInit, int typeInit)
    {
        this.x = xInit;
        this.y = yInit;
        this.z = -(xInit + yInit);
        this.type = typeInit;
    }

    public Vector3 Position()
    {
        float radius = 1f;
        float height = radius * 2;
        float width = WIDTH_MULTIPLIER * height;
        
        float horiz = width;
        float vert = height * 0.75f;

        return new Vector3(
            horiz * (this.x + this.y/2f),
            0,
            vert * this.y
        );
    }
    
    public override string ToString(){return this.x + "," + this.y + "," + this.z;}
    public int GetX () {return x;}
    public void SetX(int nX) {this.x = nX;}
    public int GetY () {return y;}
    public void SetY(int nY) {this.y = nY;}
    public int GetZ () {return z;}
    public void SetZ(int nZ) {this.z = nZ;}
    public int GetHexType () {return type;}
    public void SetHexType(int nType) {this.type = nType;}

}
