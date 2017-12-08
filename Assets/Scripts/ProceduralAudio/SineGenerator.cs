using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.ProceduralAudio;

public class SineGenerator : MonoBehaviour {

    private AudioSource audioSource;

    [Range(0.0f, 1.0f)]
    public float Amp = 0f;
    public float KickStartFrequency = 167f;
    public float KickEndFrequency = 20f;
    [Range(0f, 1f)]
    public float KickDecayRate = 0.53f;
    [Range(0f, 1f)]
    public float AmpDecayRate = 0.95f;

    private float frequency = 440f;
    private float decay = 1.0f;
    private float phase;
    private const float DOUBLE_PI = 6.28318530718f;
    private float st;

    private Wavetable m_square = new Wavetable(512);
    private Wavetable m_sine = new Wavetable(512);
    private Phasor m_phasor;

    private void Awake()
    {
        m_phasor = new Phasor(AudioSettings.outputSampleRate, 440.0f, 0f);
        m_square.CreateSquare();
        m_sine.CreateSine();
    }

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

        if (decay < 0.0001f)
        {
            decay = 0f;
            return;
        }

        if (Amp < 0.0001f)
        {
            Amp = 0f;
            return;
        }

        frequency = scaleRange(decay, 1, 0, KickStartFrequency, KickEndFrequency);

        m_phasor.SetFrequency(frequency);
        float v = 0f;
        float s = 0f;
        float mix = 0f;

        float f = frequency * st;
        for (int j = 0; j < data.Length; j += channels)
        {
            //float t = Mathf.Sin(DOUBLE_PI * phase) * Amp;
            //phase += f;
            //phase -= Mathf.Floor(phase);
            
            for (int i = 0; i < channels; i++)
            {
                v = (float)m_square.LinearLookup(m_phasor.GetPhase() * m_square.GetSize()) * Amp;
                s = (float)m_sine.LinearLookup(m_phasor.GetPhase() * m_sine.GetSize()) * Amp;
                mix = v *= s;
                data[j + i] = mix;
                m_phasor.Tick();
            }
        }
    }
}
