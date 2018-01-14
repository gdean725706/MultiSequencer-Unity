using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

[RequireComponent(typeof(AudioSource))]
public class StepBlock : MonoBehaviour
{
    [SerializeField]
    bool play = false;

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
    

    // Use this for initialization
    void Start()
    {
        if (DrumVoiceSource == null)
        {
            DrumVoiceSource = GameObject.Find("DrumVoices");
        }
        rend = GetComponent<Renderer>();

        noise = DrumVoiceSource.GetComponentInChildren<NoiseGenerator>();
        kick = DrumVoiceSource.GetComponentInChildren<DrumVoice>();
    }

    // Update is called once per frame
    void Update()
    {
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
}
