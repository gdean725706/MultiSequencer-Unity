using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SineGenerator : MonoBehaviour {

    private AudioSource audioSource;

    [Range(0.0f, 1.0f)]
    public float Amp = 0.8f;
    public float Frequency = 440f;

    private float phase;
    private const float DOUBLE_PI = 6.28318530718f;
    private float st;

	// Use this for initialization
	void Start () {
        audioSource = GetComponent<AudioSource>();

        if (audioSource == null)
            audioSource = gameObject.AddComponent<AudioSource>();
        
        st = 1.0f / AudioSettings.outputSampleRate;

	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnAudioFilterRead(float[] data, int channels)
    {

        if (st == 0.0f)
            return;

        float f = Frequency * st;
        for (int j = 0; j < data.Length; j += channels)
        {
            float t = Mathf.Sin(DOUBLE_PI * phase) * Amp;
            phase += f;
            phase -= Mathf.Floor(phase);

            for (int i = 0; i < channels; i++)
                data[j + i] = t;
        }
    }
}
