using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ballController : MonoBehaviour {

	public AudioSource sfxAudioSource;
	public AudioClip[] bounceClips;
	public float pitchScaleAmt = 0.1f;

    private AudioSource explodeSource;
    public AudioClip ExplodeSFX;

	public Color BallColour;
	public Material mat;
	private Renderer rend;

    private BPMTimer Quantiser;

	// Use this for initialization
	void Awake () {
        explodeSource = GameObject.Find("ExplodeSource").GetComponent<AudioSource>();
        Quantiser = GameObject.Find("Timer").GetComponent<BPMTimer>();
		rend = GetComponent<Renderer> ();
		BallColour = new Color (Random.Range (0f, 1f), Random.Range (0f, 1f), Random.Range (0f, 1f));
		rend.material.SetColor("_Color", BallColour);
	}
	

	// Update is called once per frame
	void Update () {

	}

	void OnCollisionEnter(Collision collision)
	{
		//Debug.Log (Vector3.Magnitude(collision.relativeVelocity));

		sfxAudioSource.pitch = Vector3.Magnitude (collision.relativeVelocity) * pitchScaleAmt;

		if (bounceClips != null)
			sfxAudioSource.clip = bounceClips [Random.Range(0,bounceClips.Length)];

        if (!sfxAudioSource.isPlaying)
            sfxAudioSource.PlayScheduled(Quantiser.GetNextBeatTime());



        if (collision.gameObject.CompareTag("Bullet"))
        {
            if (explodeSource != null)
            {
                explodeSource.clip = ExplodeSFX;
                explodeSource.Play();
            }
            Destroy(gameObject);
        }
        //bounceSfx.Play ();
	}
}
