using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class SequencerGrid : MonoBehaviour {

    public int xSize = 16;
    public int ySize = 16;

    public float scaleFactor = 1;

    public GameObject Pad;
    public AudioMixerGroup Mixer;
    public AudioClip Silence;

    private Vector3[] vertices = new Vector3[0];
    private GameObject[] grid;
    private SequencerStep[] steps;
    private Mesh mesh;
    private BPMTimer clock;
    
    public SequencerStep[] mainGrid { get { return steps; } }

	// Use this for initialization
	void Awake ()
    {
        GenerateGrid();
	}
    private void Start()
    {
        clock = GameObject.Find("Timer").GetComponent<BPMTimer>();

    }

    // Update is called once per frame 
    void Update ()
    {
		
	}

    void GenerateGrid()
    {

        vertices = new Vector3[(xSize) * (ySize)];
        grid = new GameObject[xSize * ySize];
        steps = new SequencerStep[xSize * ySize];

        for (int x = 0, i = 0; x < xSize; ++x)
        {
            for (int y = 0; y < ySize; ++y, ++i)
            {
                vertices[i] = new Vector3(x, 0, y);
                vertices[i] *= scaleFactor;

                grid[i] = Instantiate(Pad, vertices[i], Quaternion.identity, transform);

                steps[i] = grid[i].AddComponent<SequencerStep>();
                steps[i].stepNumber = i;
            }
        }

    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.black;
        for (int i = 0; i < vertices.Length; i++)
        {
            Gizmos.DrawSphere(vertices[i], 0.1f);
        }
    }

    public SequencerStep GetPad(int padNumber)
    {
        if (padNumber >= steps.Length || padNumber <= 0)
        {
            padNumber = Mathf.Abs(padNumber % steps.Length);
        }
        return steps[padNumber];
    }
}
