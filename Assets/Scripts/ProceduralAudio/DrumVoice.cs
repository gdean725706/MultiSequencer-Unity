﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.ProceduralAudio;

/// <summary>
/// Wavetable Drum voice
/// </summary>
/// 

    public struct DrumVoiceParams
{
    float StartFrequency;
    float EndFrequency;
    float PitchDecayRate;
    float AmpDecayRate;
    float FMModIndex;
    float FMFrequency;
}

public class DrumVoice : MonoBehaviour {

    private AudioSource audioSource;

    DrumVoiceParams voiceParams;



    // Voice parameters
    [Range(0.0f, 1.0f)]
    public float Amp = 0f;
    public float KickStartFrequency = 167f;
    public float KickEndFrequency = 20f;
    [Range(0f, 1f)]
    public float KickDecayRate = 0.53f;
    [Range(0f, 1f)]
    public float AmpDecayRate = 0.95f;
    
    public float modIndex = 0f;
    public float FMFrequency = 800f;

    private float carrierFrequency = 440f;
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
        m_fmPhasor.SetFrequency(FMFrequency);
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

    public void Ping()
    {
        Amp = 0.99f;
        decay = 0.99f;
    }

    public float scaleRange(float input, float inputStart, float inputEnd, float outputStart, float outputEnd)
    {
        return outputStart + ((outputEnd - outputStart) / (inputEnd - inputStart)) * (input - inputStart);
    }

    public void UpdateStartFrequency(float frequency)
    {
        frequency = Mathf.Clamp(frequency, 0.0f, 20000.0f);
        KickStartFrequency = frequency;
    }

    public void UpdateEndFrequency(float frequency)
    {
        frequency = Mathf.Clamp(frequency, 0.0f, 20000.0f);
        KickEndFrequency = frequency;
    }

    public void UpdatePitchDecayRate(float rate)
    {
        rate = Mathf.Clamp(rate, 0.0f, 1.0f);
        KickDecayRate = rate;
    }

    public void UpdateAmpDecayRate(float rate)
    {
        rate = Mathf.Clamp(rate, 0.0f, 1.0f);
        AmpDecayRate = rate;
    }

    public void UpdateModIndex(float index)
    {
        index = Mathf.Clamp(index, 0f, 1f);
        modIndex = index;
    }

    public void UpdateFMFrequency(float frequency)
    {
        frequency = Mathf.Clamp(frequency, 0f, 8000f);
        FMFrequency = frequency;
    }

    public void UpdateSineSquareMix(float mix)
    {
        mix = Mathf.Clamp(mix, 0f, 1f);
        SineSquareMix = mix;
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

        carrierFrequency = scaleRange(decay, 1, 0, KickStartFrequency, KickEndFrequency);

        m_phasor.SetFrequency(carrierFrequency);
        float sq = 0f;
        float sin = 0f;
        float fmModFreq = 0f;
        float mix = 0f;

        float noise_fm = 0f;
        float fm_mix = 0f;

        float mod = 0f;

        float f = carrierFrequency * st;
        for (int j = 0; j < data.Length; j += channels)
        {   
            for (int i = 0; i < channels; i++)
            {
                if (modIndex != 0)
                {
                    m_fmPhasor.SetFrequency(FMFrequency);
                    fmModFreq = (float)m_sineFM.LinearLookup(m_fmPhasor.GetPhase()) * m_sineFM.GetSize() * Amp;

                    mod = carrierFrequency + (fmModFreq * modIndex);

                    m_phasor.SetFrequency(mod);
                    m_fmPhasor.Tick();
                }

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
