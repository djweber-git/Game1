using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hex : MonoBehaviour
{
    private int x;
    private int y;
    private int z;

    public Hex(int xInit, int yInit)
    {
        this.x = xInit;
        this.y = yInit;
        this.z = -(xInit + yInit);
    }
    

    public int GetX () {return x;}
    public void SetX(int nX) {this.x = nX;}
    public int GetY () {return y;}
    public void SetY(int nY) {this.y = nY;}
    public int GetZ () {return z;}
     public void SetZ(int nZ) {this.z = nZ;}

}
