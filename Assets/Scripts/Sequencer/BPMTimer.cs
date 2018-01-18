using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Clock generator class.
/// Passes static event callback for beats and bars.
/// Metronome click track based on Unity example https://docs.unity3d.com/ScriptReference/AudioSettings-dspTime.html
/// Added functionality for beat divisions, callbacks and direction changing
/// </summary>
[RequireComponent(typeof(AudioSource))]
public class BPMTimer : MonoBehaviour
{
    [Tooltip("Set tempo in Beats per minute")]
    [Range(1f, 300f)]
    public double bpm = 140.0F;
    [Tooltip("Click track gain")]
    public float gain = 0.5F;
    // Time sigs
    public int signatureHi = 4;
    public int signatureLo = 4;
    
    // To store next ticks
    private double nextTick = 0.0f;
    private double nextStepTick = 0.0f;
    // metro amp
    private float amp = 0.0f;
    // metro phase
    private float phase = 0.0f;
    private double sampleRate = 0.0f;
    private bool running = false;

    // Current division counts
    private int beatCount;
    private int stepCount;
    // Sum counts
    private int stepCountSum = 0;
    private int beatCountSum = 0;

    private int direction = 0;

    // Delegate callback event when beat occurs
    // Add methods from other classes to this delegate to be called when step occurrs
    // Will be called from audio thread
    public delegate void IncrementStep(int step);
    public static event IncrementStep stepOccurred;

    // Same for bars
    public delegate void IncrementBeat(int beat);
    public static event IncrementBeat beatOccurred;

    void Start()
    {
        // Start at time sig high
        beatCount = signatureHi;
        stepCount = signatureHi;

        // Fetch samplerate from audio engine
        sampleRate = AudioSettings.outputSampleRate;
        // Get Start tick from audio settings (seconds)
        double startTick = AudioSettings.dspTime;

        // Next tick in samples will be dsp time (seconds) * samplerate
        nextTick = startTick * sampleRate;
        nextStepTick = startTick * (sampleRate * 0.5f);
        running = true;
    }

    #region Getters
    // Getters for beat and bar info
    // Not sample accurate as requires polling from update thread
    // For timing critical purproses use event delegates instead as these will be called from the audio thread

    /// <summary>
    /// Returns next beat in samples
    /// </summary>
    /// <returns></returns>
    public double GetNextBeatTime()
    {
        return nextTick / sampleRate;
    }
    
    /// <summary>
    /// Returns current bar count
    /// </summary>
    /// <returns></returns>
    public int GetBar()
    {
        return beatCountSum;
    }

    /// <summary>
    /// Returns current beat count
    /// </summary>
    /// <returns></returns>
    public int GetBeat()
    {
        return stepCountSum;
    }
#endregion

    public double GetNextStepTick()
    {
        return nextStepTick;
    }
    /// <summary>
    /// Sets the direction of the clock (0 - Forwards (default), 1 - Backwards)
    /// </summary>
    /// <param name="dir"></param>
    public void setDirection(int dir)
    {
        direction = Mathf.Clamp(dir, 0, 1);
        
    }

    public void UpdateBPM(float in_bpm)
    {
        bpm = Mathf.Clamp(in_bpm, 0, 300);
    }

    // Audio thread callback
    void OnAudioFilterRead(float[] data, int channels)
    {
        if (!running)
            return;

        // Beat divisions
        // Number of samples in a minute / beats in a minute * time sig
        double samplesPerTick = sampleRate * 60.0f / bpm * 4.0f / signatureLo;
        // Quarter divisions
        double samplesPerStep = sampleRate * 15.0f / bpm * 4.0f / signatureLo;

        // Get current audio engine sample number
        double sample = AudioSettings.dspTime * sampleRate;

        // Iterate through all the samples in the buffer
        for (int j = 0; j < data.Length; j += channels)
        {
            // Iterate through each channel
            for (int k = 0; k < channels; k++)
            {
                // Fill buffer with sine click
                data[j + k] += gain * amp * Mathf.Sin(phase);

                // Count Quarter beat (Step) divisions
                // have we reached the step tick?
                while (sample + j >= nextStepTick)
                {
                    // Yes, so update the next step tick
                    nextStepTick += samplesPerStep;
                    // Divison wrapping
                    if (++stepCount > signatureHi)
                    {
                        stepCount = 1;
                    }
                    // Direction and step count handling
                    switch (direction)
                    {
                        // Forwards
                        case 0:
                            // Increment step count
                            ++stepCountSum;
                            break;
                        // Backwards
                        case 1:
                            --stepCountSum;
                            break;
                        default:
                            ++stepCountSum;
                            break;
                    }

                    // Trigger beat event callback
                    // This how we can trigger methods in other classes from the audio thread
                    // Sample accurate :o)
                    if (stepOccurred != null)
                        stepOccurred(stepCountSum);
                }
                // Couting beat divisions, same logic as steps
                while (sample + j >= nextTick)
                {
                    nextTick += samplesPerTick;
                    // Reset amp envelope to produce click
                    amp = 1.0F;
                    // Wrapping around
                    if (++beatCount > signatureHi)
                    {
                        beatCount = 1;
                        // Bar is complete so produce a louder click
                        amp *= 2.0F;
                    }

                    // Direction handling
                    switch (direction)
                    {
                        case 0:
                            ++beatCountSum;
                            break;
                        case 1:
                            --beatCountSum;
                            break;
                        default:
                            ++beatCountSum;
                            break;
                    }

                    if (beatOccurred != null)
                        beatOccurred(beatCountSum);

                }
                // Increment sine phase
                phase++;
                // Quick and dirty amplitude envelope
                amp *= 0.95F;
            }
        }
    }
}