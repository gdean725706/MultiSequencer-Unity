using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SequencerGridTraverser : MonoBehaviour
{

    public enum Mode
    {
        Forward,
        Reverse,
        PingPong,
        Diagonal,
        Brownian,
        Random
    }

    public Mode SequencerDirection;

    public bool Running = false;

    public int JumpStepOffset = 0;
    public int InitialOffset = 0;

    private int currentTick = 0;
    private int lastTick = 0;

    private int totalSteps = 256;
    private bool awake = false;

    public SequencerGrid grid;
    private BPMTimer clock;

    private bool pingpongForward = true;

    private static ulong randomSeed;

    private bool disabledSelf = false;

    // Use this for initialization
    void Start ()
    {
        // Cannot access Unity random from Audio Thread so we have to use 
        var r = new System.Random();
        randomSeed = (ulong)r.Next();

        grid = GameObject.Find("SequencerGrid").GetComponent<SequencerGrid>();
        clock = GameObject.Find("Timer").GetComponent<BPMTimer>();

        BPMTimer.stepOccurred += incrementStep;

        totalSteps = (grid.xSize * grid.ySize);

        awake = true;

        currentTick += InitialOffset;

        currentTick = 0;
        lastTick = 0;
    }

    // Update is called once per frame
    void Update()
    {
        // Disable pads before stopping running
        if (!Running && !disabledSelf)
        {
            grid.GetPad(currentTick).CurrentStep = false;
            grid.GetPad(lastTick).CurrentStep = false;
            disabledSelf = true;
            return;
        }
        else if (Running && disabledSelf)
        {
            disabledSelf = false;
        }
        else if (!Running && disabledSelf) return;

        totalSteps = (grid.xSize * grid.ySize);

        // Update active pad and disable previous one
        if (grid != null)
        {
            grid.GetPad(currentTick).CurrentStep = true;
            grid.GetPad(lastTick).CurrentStep = false;
        }
        
	}

    void incrementStep(int tickIn)
    {
        if (!awake) return;

        if (!Running) return;
        
        lastTick = currentTick;

        switch (SequencerDirection)
        {
            case Mode.Forward:
                if (currentTick >= totalSteps) currentTick = 0;
                currentTick += 1;
                break;
            case Mode.Reverse:
                if (currentTick <= 0) currentTick = totalSteps;
                currentTick -= 1;
                break;
            case Mode.Diagonal:
                if (currentTick >= totalSteps) currentTick = 0;
                currentTick += 1 * grid.xSize + JumpStepOffset;
                break;
            case Mode.PingPong:
                if (currentTick >= totalSteps) pingpongForward = false;
                if (currentTick <= 0) pingpongForward = true;
                if (pingpongForward)
                    currentTick += 1;
                else
                    currentTick -= 1;
                break;
            case Mode.Brownian:
                if (currentTick >= totalSteps) currentTick = 0;
                if (currentTick <= 0) currentTick = totalSteps;
                // Fetch a random number - 50/50 chance it goes forward or backwards
                if (GetRandom() > 0.5f)
                    currentTick += 1;
                else
                    currentTick -= 1;
                break;
            case Mode.Random:
                if (currentTick >= totalSteps) currentTick = 0;
                if (currentTick <= 0) currentTick = totalSteps;
                if (GetRandom() > 0.5f)
                    currentTick += tickIn % totalSteps;
                else
                    currentTick -= tickIn % totalSteps;
                break;
            default:
                currentTick += 1;
                break;
        }

    }

    // Linear-feedback shift register pseudorandom number generator
    //https://docs.unity3d.com/Manual/AudioMixerNativeAudioPlugin.html
    float GetRandom()
    {
        const float scale = 1.0f / (float)0x7FFFFFFF;
        randomSeed = randomSeed * 69069 + 1;
        return (((randomSeed >> 16) ^ randomSeed) & 0x7FFFFFFF) * scale;
    }
}
