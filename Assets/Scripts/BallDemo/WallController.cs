using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallController : MonoBehaviour {

	public Material wallMaterial;
	private Renderer rend;
	private Color currentColour;

	public AudioSource audioSfx;
	public AudioClip[] impactSfxs;
	public bool pickRandomSound = true;

    private BPMTimer Quantiser;

	// Use this for initialization
	void Start () {
        Quantiser = GameObject.Find("Timer").GetComponent<BPMTimer>();
		rend = GetComponent<Renderer> ();
		audioSfx = GetComponent<AudioSource> ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnCollisionEnter(Collision coll)
	{
		if (coll.gameObject.CompareTag("Ball"))
		{
			
			ballController ball = coll.gameObject.GetComponent<ballController>();
			if (ball != null) {
				currentColour = ball.BallColour;
			}


			rend.material.SetColor ("_Color", currentColour);

			if (impactSfxs != null) {
				audioSfx.clip = impactSfxs [Random.Range (0, impactSfxs.Length)];
                //Debug.Log(Quantiser.GetNextBeatTime() + " , dsptime = " + AudioSettings.dspTime);
                if (!audioSfx.isPlaying)
                    audioSfx.PlayScheduled(Quantiser.GetNextBeatTime());

				//audioSfx.Play ();
			}
		}
        if (coll.gameObject.CompareTag("Bullet"))
        {
            Destroy(coll.gameObject);
        }
	}
}
