using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SineGenerator : MonoBehaviour {

    private AudioSource audioSource;

    [Range(0.0f, 1.0f)]
    public float Amp = 0f;
    public float KickStartFrequency = 167f;
    public float KickEndFrequency = 20f;
    public float KickDecayRate = 0.93f;
    public float AmpDecayRate = 0.95f;

    private float frequency = 440f;
    private float decay = 1.0f;
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

    public void Ping()
    {
        Amp = 0.99f;
        decay = 0.99f;
    }

    public float scaleRange(float input, float inputStart, float inputEnd, float outputStart, float outputEnd)
    {
        return outputStart + ((outputEnd - outputStart) / (inputEnd - inputStart)) * (input - inputStart);
    }

    private void OnAudioFilterRead(float[] data, int channels)
    {

        if (st == 0f)
            return;

        if (decay > 0f)
            decay *= KickDecayRate;

        if (Amp > 0f)
            Amp *= AmpDecayRate;

        if (decay < 0.00001f)
        {
            decay = 0f;
            return;
        }

        if (Amp < 0.00001f)
        {
            Amp = 0f;
            return;
        }

        frequency = scaleRange(decay, 1, 0, KickStartFrequency, KickEndFrequency);

        float f = frequency * st;
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
