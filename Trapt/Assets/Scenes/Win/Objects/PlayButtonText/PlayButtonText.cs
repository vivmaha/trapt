using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PlayButtonText : MonoBehaviour {

	// Use this for initialization
	void Start () {
        var text = this.GetComponent<Text>();
        text.text = string.Format(text.text, Game.level);
	}	
}
