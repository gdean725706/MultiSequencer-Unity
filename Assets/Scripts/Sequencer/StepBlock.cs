using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

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

    public enum Mode
    {
        Sample,
        Voice,
        Control
    }

    public Mode PlaybackMode = Mode.Sample;

    public Sound SoundType = Sound.Hat;

    [Range(0f, 1f)]
    public float HatDecay = 0.8f;
    private float prevHatDecay;

    public GameObject DrumVoiceSource;

    private NoiseGenerator noise;

    [SerializeField]
    private DrumVoice[] voiceChannels;

    [SerializeField]
    private int channel = 0;

    private DrumVoice voice;

    public float assocPadAliveSum = 0f;
    public bool GOLLifetimeModulation = false;

    private double nextStepSeconds = 0;

    private BPMTimer timer;
    private bool playActive = false;

    private bool isSelected = false;
    private Color colorOff = new Color(1f, 1f, 1f, 0.0f);
    private Color colorSelected = new Color(1f, 0f, 0f, 1.0f);

    [SerializeField]
    private BlockSpawnManager spawnManager;

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

        voiceChannels = DrumVoiceSource.GetComponentsInChildren<DrumVoice>();

        if (voiceChannels != null)
            voice = voiceChannels[channel];

        UpdateSample(currentSample = (int)Random.Range(0, Samples.Count));

        if (PlaybackMode == Mode.Sample)
        {
            audioSource.loop = false;
            audioSource.playOnAwake = false;
        }

        if (spawnManager == null)
        {
            spawnManager = GameObject.Find("BlockSpawnManager").GetComponent<BlockSpawnManager>();
        }
    }

    public DrumVoice GetVoice()
    {
        return voice;
    }

    // Update is called once per frame
    void Update()
    {
        // Has the play switch been toggled from the audio thread?
        if (playActive)
        {
            // Yes, schedule to play on next step
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

        // Color
        if (isSelected)
        {
            rend.material.SetColor("_Color", colorSelected);
        }
        else
        {
            rend.material.SetColor("_Color", colorOff);
        }
        
        isSelected = (spawnManager.GetSelectedBlock() == gameObject);
        
    }

    public void setVoiceChannel(int v)
    {
        channel = v;
        if (voiceChannels != null)
            voice = voiceChannels[channel];
    }

    public int getVoiceChannel()
    {
        return channel;
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

    public void OnMouseOver()
    {
        // Check mouse isn't over UI
        if (!EventSystem.current.IsPointerOverGameObject())
        {
            if (BlockSpawnManager.BrushMode)
            {
                if (Input.GetMouseButton(0))
                {
                    spawnManager.UpdateSelectedBlock(gameObject);
                }
                if (Input.GetMouseButton(1))
                {
                    spawnManager.RemoveBlock(gameObject);
                    DestroyStep();
                }
            }
            else
            {
                if (Input.GetMouseButtonDown(0))
                {
                    spawnManager.UpdateSelectedBlock(gameObject);
                    // Select block...
                }
                if (Input.GetMouseButtonDown(1))
                {
                    spawnManager.RemoveBlock(gameObject);
                    DestroyStep();
                }
            }
           
        }
    }

    // -- Called from Audio Thread -- 
    void playStepSound(int stepNumber)
    {
        switch (PlaybackMode)
        {
            case Mode.Sample:
                playActive = true;
                break;
            case Mode.Voice:
                voice.Ping();
                break;
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

    public void UpdateMode(StepBlock.Mode mode)
    {
        PlaybackMode = mode;
    }

    public Mode GetMode()
    {
        return PlaybackMode;
    }


}
