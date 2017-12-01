using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOfLife : MonoBehaviour
{

    public SequencerGrid grid;
    private BPMTimer clock;

    private SequencerPad[,] _cells;
    private int[,] _nextCells;

    private int beatCount = 0;
    private int prevBeatCount = 0;

    public static bool running = false;

	// Use this for initialization
	void Start ()
    {
        grid = GameObject.Find("SequencerGrid").GetComponent<SequencerGrid>();
        clock = GameObject.Find("Timer").GetComponent<BPMTimer>();

        _cells = new SequencerPad[grid.xSize,grid.ySize];

        _nextCells = new int[grid.xSize, grid.ySize];

        for (int x = 0, i = 0; x < grid.xSize; ++x)
        {
            for (int y = 0; y < grid.ySize; ++y, ++i)
            {
                _cells[x,y] = grid.mainGrid[i];
            }
        }

        BPMTimer.beatOccurred += incrementBeat;

        clearCells();
	}
	
	// Update is called once per frame
	void Update ()
    {
		if (prevBeatCount != beatCount)
        {
            if (running)
                tick();
        }

        prevBeatCount = beatCount;

        if (Input.GetKeyDown(KeyCode.R))
        {
            randomiseCells();
        }

        if (Input.GetKeyDown(KeyCode.T))
        {
            running = !running;
        }

    }



    void tick()
    {
        int neighboursAlive = 0;
        int currentState = 0;

        for (int row = 0; row < grid.xSize; ++row)
        {
            for (int col = 0; col < grid.ySize; ++col)
            {
                neighboursAlive = 0;
                currentState = _cells[row,col].State;

                for (int i = -1; i < 2; ++i)
                {
                    for (int j = -1; j < 2; ++j)
                    {
                        neighboursAlive += _cells[(row + i + grid.xSize) % grid.xSize,(col + j + grid.ySize) % grid.ySize].State;
                    }
                }

                neighboursAlive -= currentState;

                if (currentState == 1 && neighboursAlive < 2) _nextCells[row,col] = 0;
                else if (currentState == 1 && neighboursAlive > 3) _nextCells[row,col] = 0;
                else if (currentState == 0 && neighboursAlive == 3) _nextCells[row,col] = 1;
                else _nextCells[row,col] = _cells[row,col].State;
            }
        }

        for (int row = 0; row < grid.xSize; ++row)
        {
            for (int col = 0; col < grid.ySize; ++col)
            {
                _cells[row,col].State = _nextCells[row,col];
            }
        }
    }

    void randomiseCells()
    {
        //foreach (var cell in grid.mainGrid)
        //{
        //    cell.State = (Random.value > 0.5f) ? 1 : 0;
        //}

        for (int row = 0; row < grid.xSize; ++row)
        {
            for (int col = 0; col < grid.ySize; ++col)
            {
                _cells[row,col].State = (Random.value > 0.5f) ? 1 : 0;
            }
        }
    }

    void clearCells()
    {
        for (int row = 0; row < grid.xSize; ++row)
        {
            for (int col = 0; col < grid.ySize; ++col)
            {
                _cells[row,col].State = 0;
            }
        }
    }

    void incrementBeat(int beat)
    {
        beatCount += beat;
    }
}
