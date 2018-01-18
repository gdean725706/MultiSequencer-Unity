using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class StepBlock : MonoBehaviour
{

    public bool SampleMode = true;

    public List<AudioClip> Samples = new List<AudioClip>();
    public int currentSample = 0;
    private AudioSource audioSource;

    private List<SequencerPad> associatedPads = new List<SequencerPad>();

    private Renderer rend;

    private bool activeStep = false;

    public int counter = 0;

    private float amp = 0;

    public enum Sound
    {
        Hat,
        Kick
    }

    public Sound SoundType = Sound.Hat;

    [Range(0f, 1f)]
    public float HatDecay = 0.8f;
    private float prevHatDecay;

    public GameObject DrumVoiceSource;

    private NoiseGenerator noise;
    private DrumVoice kick;

    public float assocPadAliveSum = 0f;
    public bool GOLLifetimeModulation = false;

    private double nextStepSeconds = 0;

    private BPMTimer timer;
    private bool playActive = false;

    // Use this for initialization
    void Start()
    {
        audioSource = GetComponent<AudioSource>();

        timer = GameObject.Find("Timer").GetComponent<BPMTimer>();

        if (DrumVoiceSource == null)
        {
            DrumVoiceSource = GameObject.Find("DrumVoices");
        }
        rend = GetComponent<Renderer>();

        noise = DrumVoiceSource.GetComponentInChildren<NoiseGenerator>();
        kick = DrumVoiceSource.GetComponentInChildren<DrumVoice>();

        UpdateSample(currentSample = (int)Random.Range(0, Samples.Count));

        if (SampleMode)
        {
            audioSource.loop = false;
            audioSource.playOnAwake = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (playActive)
        {
            audioSource.PlayScheduled(nextStepSeconds);
            playActive = false;
        }

        if (HatDecay != prevHatDecay)
        {
            noise.AmpDecayTime = HatDecay;
        }
        prevHatDecay = HatDecay;

        if (GOLLifetimeModulation)
        {
            float padAliveTime = 0;

            foreach (var pad in associatedPads)
            {
                padAliveTime += pad.GOLTimeAlive;
            }

            assocPadAliveSum = padAliveTime;
        }

        nextStepSeconds = timer.GetNextStepTick() / AudioSettings.outputSampleRate;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Pads"))
        {
            var pad = other.gameObject.GetComponent<SequencerPad>();
            pad.stepOccurred += playStepSound;
            pad.Active = true;

            if (!associatedPads.Contains(pad))
                associatedPads.Add(pad);
            
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Pads"))
        {
            var pad = other.gameObject.GetComponent<SequencerPad>();
            pad.stepOccurred -= playStepSound;
            pad.Active = false;

            if (associatedPads.Contains(pad))
                associatedPads.Remove(pad);
            
        }
    }

    // -- Called from Audio Thread -- 
    void playStepSound(int stepNumber)
    {
        if (SampleMode)
        {
            playActive = true;
            return;
        }
        switch (SoundType)
        {
            case Sound.Kick:
                kick.Ping();
                break;
            case Sound.Hat:
                noise.Ping();
                break;
        }
    }

    public void DestroyStep()
    {
        foreach (var pad in associatedPads)
        {
            pad.Active = false;
        }
        associatedPads.Clear();
        Destroy(gameObject);
    }

    public void UpdateSample(int sampleNumber)
    {
        sampleNumber = Mathf.Clamp(sampleNumber, 0, Samples.Count);
        audioSource.clip = Samples[sampleNumber];
        currentSample = sampleNumber;
    }
}
