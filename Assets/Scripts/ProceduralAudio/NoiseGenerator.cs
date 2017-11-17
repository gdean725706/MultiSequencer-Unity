using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

[RequireComponent(typeof(AudioSource))]
public class NoiseGenerator : MonoBehaviour
{

    private float lastImpactVelocity = 0f;
    private float impactVelocity = 0f;

    float impactDecayTime = 0.5f;
    float impactAmplitude = 0.9f;
    float amp = 0f;

    float randomFilterSeed = 0;

    private static ulong randomSeed;
    private int sampleRate;

    public AudioMixer NoiseMixer;

    private bool collided = false;

	// Use this for initialization
	void Start ()
    {
        // Cannot access Unity random from Audio Thread so we have to use 
        var r = new System.Random();
        randomSeed = (ulong)r.Next();
        sampleRate = AudioSettings.outputSampleRate;
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    // Linear-feedback shift register pseudorandom number generator
    float GetRandom()
    {
        const float scale = 1.0f / (float)0x7FFFFFFF;
        randomSeed = randomSeed * 69069 + 1;
        return (((randomSeed >> 16) ^ randomSeed) & 0x7FFFFFFF) * scale;
    }

    //private void OnCollisionEnter(Collision collision)
    //{
    //    var rb = GetComponent<Rigidbody>();
    //    impactVelocity = rb.mass * collision.relativeVelocity.magnitude * collision.relativeVelocity.magnitude;

    //    collided = true;

    //    if (amp < 0.1f)
    //        randomFilterSeed = Random.Range(20000f, 80000f);

    //    NoiseMixer.SetFloat("RandomSeed", randomFilterSeed);

    //    Debug.Log("Collision with velocity " + impactVelocity + ". Last velocity = " + lastImpactVelocity);
    //}

    private void OnCollisionStay(Collision collision)
    {
        
    }

    public void Ping()
    {
        amp = 0.98f;
    }

    private void OnAudioFilterRead(float[] data, int channels)
    {

        if (sampleRate == 0)
            return;

        //if (collided)
        //{
        //   amp = impactAmplitude;
        //   collided = false;
        //}

        if (amp > 0)
            Mathf.Clamp(amp *= 0.9f, 0f, 1f);

        if (amp < 0.0001f)
        {
            amp = 0f;
            return;
        }

        for (int j = 0; j < data.Length; j += channels)
        {
            float t = GetRandom() * 0.5f;

            for (int i = 0; i < channels; i++)
                data[j + i] += t * amp ;
        }

        //lastImpactVelocity = impactVelocity;
    }
}
