using System;
using UnityEngine;

public class Tile : MonoBehaviour {

    public event EventHandler OnDeactivate;
    public bool IsImmortal { get; set; }

    public void OnMouseDown()
    {
        if (this.IsImmortal)
        {
            return;
        }

        this.gameObject.SetActive(false);
        if (OnDeactivate != null)
        {
            OnDeactivate(this, EventArgs.Empty);
        }        
    }
}
