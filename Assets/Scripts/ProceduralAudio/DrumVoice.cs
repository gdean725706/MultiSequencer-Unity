using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.ProceduralAudio;

/// <summary>
/// Wavetable Drum voice
/// </summary>
public class DrumVoice : MonoBehaviour {

    private AudioSource audioSource;

    // Voice parameters
    [Range(0.0f, 1.0f)]
    public float Amp = 0f;
    public float KickStartFrequency = 167f;
    public float KickEndFrequency = 20f;
    [Range(0f, 1f)]
    public float KickDecayRate = 0.53f;
    [Range(0f, 1f)]
    public float AmpDecayRate = 0.95f;

    [Range(0f, 0.5f)]
    public float FMAmount = 0f;

    private float frequency = 440f;
    private float decay = 1.0f;
    private float phase;
    private const float DOUBLE_PI = 6.28318530718f;
    private float st;

    private static ulong randomSeed;

    // Create wavetables & phasors for the components of the voice
    private Wavetable m_square = new Wavetable(1024);
    private Wavetable m_sine = new Wavetable(1024);
    private Wavetable m_sineFM = new Wavetable(1024);

    private Phasor m_phasor;
    private Phasor m_fmPhasor;

    [Range(0f, 1f)]
    public float ModulationAmountMultiplier = 0f;

    [Range(0f, 1f)]
    public float SineSquareMix = 0.5f;

    private void Awake()
    {
        m_phasor = new Phasor(AudioSettings.outputSampleRate, 440.0f, 0f);
        m_fmPhasor = new Phasor(AudioSettings.outputSampleRate, 2000.0f, 0f);
        m_square.CreateSquare();
        m_sine.CreateSine();
        m_sineFM.CreateSine();
        m_fmPhasor.SetFrequency(4000f);
    }

    // Use this for initialization
    void Start () {
        audioSource = GetComponent<AudioSource>();

        if (audioSource == null)
            audioSource = gameObject.AddComponent<AudioSource>();
        
        st = 1.0f / AudioSettings.outputSampleRate;

	}

    public void ApplyModulation(float amount)
    {

        //KickDecayRate += scaleRange(amount, 0, 1000, amount * ModulationAmountMultiplier;
        AmpDecayRate += amount * ModulationAmountMultiplier;
    }

    // Linear-feedback shift register pseudorandom number generator
    float GetRandom()
    {
        const float scale = 1.0f / (float)0x7FFFFFFF;
        randomSeed = randomSeed * 69069 + 1;
        return (((randomSeed >> 16) ^ randomSeed) & 0x7FFFFFFF) * scale;
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

        if (decay < 0.0001f && Amp < 0.0001f)
        {
            decay = 0f;
            Amp = 0f;
            return;
        }

        frequency = scaleRange(decay, 1, 0, KickStartFrequency, KickEndFrequency);

        m_phasor.SetFrequency(frequency);
        float sq = 0f;
        float sin = 0f;
        float fm = 0f;
        float mix = 0f;

        float noise_fm = 0f;
        float fm_mix = 0f;

        float f = frequency * st;
        for (int j = 0; j < data.Length; j += channels)
        {   
            for (int i = 0; i < channels; i++)
            {
                if (FMAmount != 0)
                {
                    fm = (float)m_sineFM.LinearLookup(m_fmPhasor.GetPhase()) * m_sineFM.GetSize() * Amp;

                    frequency += (fm * FMAmount);

                    m_fmPhasor.Tick();
                }

                m_phasor.SetFrequency(frequency);
                sq = (float)m_square.LinearLookup(m_phasor.GetPhase() * m_square.GetSize()) * Amp;
                sin = (float)m_sine.LinearLookup(m_phasor.GetPhase() * m_sine.GetSize()) * Amp;

                // Mix sine square
                mix = (sin * (1 - SineSquareMix)) + (sq * SineSquareMix);


                data[j + i] = mix;
                m_phasor.Tick();
                

            }
        }
    }
}
