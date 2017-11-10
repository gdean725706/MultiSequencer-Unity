using UnityEngine;

// The code example shows how to implement a metronome that procedurally
// generates the click sounds via the OnAudioFilterRead callback.
// While the game is paused or suspended, this time will not be updated and sounds
// playing will be paused. Therefore developers of music scheduling routines do not have
// to do any rescheduling after the app is unpaused

[RequireComponent(typeof(AudioSource))]
public class BPMTimer : MonoBehaviour
{
    public double bpm = 140.0F;
    public float gain = 0.5F;
    public int signatureHi = 4;
    public int signatureLo = 4;

    [SerializeField]
    private double nextTick = 0.0f;
    [SerializeField]
    private float amp = 0.0f;
    [SerializeField]
    private float phase = 0.0f;
    [SerializeField]
    private double sampleRate = 0.0f;
    [SerializeField]
    private int accent;
    [SerializeField]
    private bool running = false;
    private double nextHalfTick = 0.0f;
    [SerializeField]
    private int halfAccent;

    [SerializeField]
    private int halfAccentSum = 0;
    [SerializeField]
    private int accentSum = 0;

    void Start()
    {
        accent = signatureHi;
        halfAccent = signatureHi;
        double startTick = AudioSettings.dspTime;
        sampleRate = AudioSettings.outputSampleRate;
        nextTick = startTick * sampleRate;
        nextHalfTick = startTick * (sampleRate * 0.5f);
        running = true;
    }

    public double GetNextBeatTime()
    {
        return nextTick / sampleRate;
    }

    public int GetBar()
    {
        return accentSum;
    }

    public int GetBeat()
    {
        return halfAccentSum;
    }

    void OnAudioFilterRead(float[] data, int channels)
    {
        if (!running)
            return;
         
        double samplesPerTick = sampleRate * 60.0f / bpm * 4.0f / signatureLo;
        double samplesPerHalfTick = sampleRate * 30.0f / bpm * 4.0f / signatureLo;
        double sample = AudioSettings.dspTime * sampleRate;
        int dataLen = data.Length / channels;

        int n = 0;
        while (n < dataLen)
        {
            float x = gain * amp * Mathf.Sin(phase);
            int i = 0;
            while (i < channels)
            {
                data[n * channels + i] += x;
                i++;
            }
            while (sample + n >= nextHalfTick)
            {
                nextHalfTick += samplesPerHalfTick;
                if (++halfAccent > signatureHi)
                {
                    halfAccent = 1;
                }

                ++halfAccentSum;
            }
            while (sample + n >= nextTick)
            {
                nextTick += samplesPerTick;
                amp = 1.0F;
                if (++accent > signatureHi)
                {
                    accent = 1;
                    amp *= 2.0F;
                }

                ++accentSum;

            }
            phase += amp * 0.3F;
            amp *= 0.993F;
            n++;
        }
    }
}