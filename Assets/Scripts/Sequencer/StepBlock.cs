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
    private double nextStepSeconds = 0;

    private BPMTimer timer;
    private bool playActive = false;

    // Use this for initialization
    void Start()
    {
        audioSource = GetComponent<AudioSource>();

        timer = GameObject.Find("Timer").GetComponent<BPMTimer>();
        rend = GetComponent<Renderer>();

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

        // Get next step in seconds
        nextStepSeconds = timer.GetNextStepTick() / AudioSettings.outputSampleRate;
    }

    // Handle collision and corresponding pad activation
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
    // Deactivate pad on exit
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
    }

    // Destroy 
    public void DestroyStep()
    {
        foreach (var pad in associatedPads)
        {
            pad.Active = false;
        }
        associatedPads.Clear();
        Destroy(gameObject);
    }

    // Recives from UI slider, looks up sample in sample list
    public void UpdateSample(int sampleNumber)
    {
        sampleNumber = Mathf.Clamp(sampleNumber, 0, Samples.Count);
        audioSource.clip = Samples[sampleNumber];
        currentSample = sampleNumber;
    }
}
