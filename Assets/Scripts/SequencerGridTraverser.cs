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

    public int currentStep;

    public SequencerGrid grid;
    private BPMTimer clock;

    // Use this for initialization
    void Start ()
    {
        grid = GameObject.Find("SequencerGrid").GetComponent<SequencerGrid>();
        clock = GameObject.Find("Timer").GetComponent<BPMTimer>();
	}
	
	// Update is called once per frame
	void Update ()
    {
		if (grid != null)
        {
            if (SequencerDirection == Mode.Forward)
            {
                for (int i = 0; i < grid.mainGrid.Length; i++)
                {
                    grid.GetPad(i).CurrentStep = (clock.GetBeat() % (grid.xSize * grid.ySize) == i);
                }
            }
        }
	}
}
