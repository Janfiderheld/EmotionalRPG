using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicFadeInOut : MonoBehaviour {
    private AudioSource[] clipSources;
    public AudioSource mainSource;
    public AudioSource dialogSource;


	// Use this for initialization
	void Start () {
		clipSources = GetComponents<AudioSource>();
        mainSource = clipSources[0];
        dialogSource = clipSources[1];

        mainSource.volume = (float)1.0;
        dialogSource.volume = (float)0.0;

    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void FadeOUT()
    {
        if (mainSource.volume > 0)
        {
            mainSource.volume -= (float)0.1;
        }
        if(mainSource.volume < 0.5)
        {
            FadeIN();
        }
    }

    public void FadeIN()
    {
        if(dialogSource.volume < 1)
        {
            dialogSource.volume += (float)0.1;
        }
    }
}
