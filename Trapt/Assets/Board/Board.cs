using UnityEngine;
using System.Collections;
using System;

public class Board : MonoBehaviour {
    public GameObject originTile;
    public int size = 5;
	void Start () {
        
	    for (int i = -this.size; i <= this.size; i++)
        {
            for (int j = -this.size; j <= this.size; j++)
            {
                var newTile = Instantiate(originTile);
                
                newTile.transform.position = new Vector2(originTile.transform.position.x + i, originTile.transform.position.y + j * (float)(Math.Sqrt(3) / 2.0));
                
                if (Math.Abs(j) % 2 == 1)
                {
                    newTile.transform.position += new Vector3(0.5f, 0, 0);
                }

                newTile.transform.parent = this.transform;
            }
        }
        Destroy(originTile);
	}	
}
