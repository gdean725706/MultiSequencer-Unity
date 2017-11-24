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
        Noise,
        Kick
    }

    public Sound SoundType = Sound.Noise;

    private NoiseGenerator noise;
    private SineGenerator kick;

    private 

    // Use this for initialization
    void Start()
    {
        rend = GetComponent<Renderer>();
        noise = gameObject.AddComponent<NoiseGenerator>();
        kick = gameObject.AddComponent<SineGenerator>();
    }

    // Update is called once per frame
    void Update()
    {
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
            case Sound.Noise:
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
