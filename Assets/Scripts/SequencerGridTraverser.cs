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
        Random,
        Diagonal,
        Brownian
    }

    public Mode SequencerDirection;

    public int currentStep = 0;

    private int currentTick = 0;
    private int lastTick = 0;

    private int totalSteps = 256;
    private bool awake = false;

    public SequencerGrid grid;
    private BPMTimer clock;

    private bool pingpongForward = true;

    private static ulong randomSeed;

    // Use this for initialization
    void Start ()
    {
        // Cannot access Unity random from Audio Thread so we have to use 
        var r = new System.Random();
        randomSeed = (ulong)r.Next();

        grid = GameObject.Find("SequencerGrid").GetComponent<SequencerGrid>();
        clock = GameObject.Find("Timer").GetComponent<BPMTimer>();

        BPMTimer.beatOccurred += incrementBeat;

        totalSteps = (grid.xSize * grid.ySize);

        awake = true;
    }
	
	// Update is called once per frame
	void Update ()
    {

        totalSteps = (grid.xSize * grid.ySize);

        if (grid != null)
        {
            grid.GetPad(currentTick).CurrentStep = true;
            grid.GetPad(lastTick).CurrentStep = false;
        }
	}

    void incrementBeat(int beat)
    {
        if (!awake) return;

        lastTick = currentTick;

        switch (SequencerDirection)
        {
            case Mode.Forward:
                if (currentTick >= totalSteps) currentTick = 0;
                currentTick += beat;
                break;
            case Mode.Reverse:
                if (currentTick <= 0) currentTick = totalSteps;
                currentTick -= beat;
                break;
            case Mode.Diagonal:
                if (currentTick >= totalSteps) currentTick = 0;
                currentTick += beat * grid.xSize;
                break;
            case Mode.PingPong:
                if (currentTick >= totalSteps) pingpongForward = false;
                if (currentTick <= 0) pingpongForward = true;
                if (pingpongForward)
                    currentTick += beat;
                else
                    currentTick -= beat;
                break;
            case Mode.Brownian:
                if (currentTick >= totalSteps) currentTick = 0;
                if (currentTick <= 0) currentTick = totalSteps;
                if (GetRandom() > 0.5f)
                    currentTick += beat;
                else
                    currentTick -= beat;
                break;
            default:
                currentTick += beat;
                break;
        }
        
    }

    // Linear-feedback shift register pseudorandom number generator
    float GetRandom()
    {
        const float scale = 1.0f / (float)0x7FFFFFFF;
        randomSeed = randomSeed * 69069 + 1;
        return (((randomSeed >> 16) ^ randomSeed) & 0x7FFFFFFF) * scale;
    }
}
