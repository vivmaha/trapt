using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour {
        
    public void MoveTo(Vector3 position)
    {
        this.transform.position = position;
    }

	void Update () {
	    
	}
}
