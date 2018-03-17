using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogTest : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void PrintEvent(string s)
    {
        Debug.Log("PrintEvent: " + s + "called at: " + Time.time);
    }
}
