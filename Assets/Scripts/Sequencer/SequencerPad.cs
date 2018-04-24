using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class for the pad that sequencer blocks sit on
/// Handles colours and passes on active step triggering to the relevant voice
/// </summary>
public class SequencerPad : MonoBehaviour {

    [SerializeField]
    private BlockSpawnManager blockSpawnManager;

    [SerializeField]
    private Transform spawnMarker;

    [SerializeField]
    private float spawnDistance = 1.1f;

    [SerializeField]
    public bool Active = false;

    [SerializeField]
    public bool CurrentStep = false;

    [SerializeField]
    public int State = 0;
    private int prevState = 0;

    public float GOLTimeAlive = 0f;

    [SerializeField]
    private float DefaultAmp = 0.99f;

    public int padNumber = 0;

    private Renderer rend;
    private float amp = 0f;

    private Color colorOff = new Color(1f, 1f, 1f, 0.05f);
    private Color colorCurrentStep = new Color(0f,1f,0f, 0.2f);
    private Color colorAlive = new Color(0f, 0f, 1f, 0.2f);
    private Color colorActive = new Color(1f, 0f, 0f, 0.2f);

    public delegate void PlayStep(int stepNumber);
    public event PlayStep stepOccurred;

    public void setSpawnManager(BlockSpawnManager manager)
    {
        blockSpawnManager = manager;
    }

    // Use this for initialization
    void Start ()
    {

        rend = transform.GetChild(0).GetComponent<Renderer>();

        // Add stepHanlder to BPM Timer delegate
        BPMTimer.stepOccurred += stepHandler;
	}

    // Update is called once per frame
    void Update()
    {
        if (CurrentStep)
        {
            rend.material.SetColor("_Color", colorCurrentStep);
            amp = DefaultAmp;
        }
        else if (State == 1)
        {
            rend.material.SetColor("_Color", colorAlive);
        }
        else
        {
            Color col = Active ? colorActive : colorOff;
            amp *= amp;
            rend.material.SetColor("_Color", col);
        }
        
        if (State == 0 && prevState == 1)
        {
            GOLTimeAlive = 0;
        }
        else if (State == 1)
        {
            ++GOLTimeAlive;
        }

        prevState = State;
    }

    private void OnTriggerStay(Collider other)
    {
        //if (other.CompareTag("Block"))
         //   Debug.Log(gameObject.name + " " + other.gameObject.name);
    }

    // Called from clock source each step
    void stepHandler(int step)
    {
        if (Active && CurrentStep)
        {
            if (stepOccurred != null)
                stepOccurred(padNumber);
        }
        else if (Active && State == 1 && GameOfLife.running)
        {
            if (stepOccurred != null)
                stepOccurred(padNumber);
        }
    }

    public void SpawnBlock(float offset = 0f)
    {
        // Prevent blocks spawning inside each other
        if (Active && !BlockSpawnManager.ChaosMode)
            return;

        if (spawnMarker == null)
        {
            spawnMarker = transform.GetChild(1);
        }

        Vector3 spawnPos = spawnMarker.position + ((transform.up * spawnDistance) + (transform.up * offset));
        blockSpawnManager.AddNewBlock(spawnPos, transform.rotation);
    }

   


}
