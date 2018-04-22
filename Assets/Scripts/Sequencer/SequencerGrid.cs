using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class SequencerGrid : MonoBehaviour {

    [SerializeField]
    private BlockSpawnManager spawnManager;

    public int xSize = 16;
    public int ySize = 16;

    private float scaleFactor = 2;

    public GameObject Pad;
    public AudioMixerGroup Mixer;
    public AudioClip Silence;

    private Vector3[] vertices = new Vector3[0];
    private GameObject[] grid;
    private SequencerPad[] steps;
    private Mesh mesh;
    private BPMTimer clock;
    
    public SequencerPad[] mainGrid { get { return steps; } }

    public static int MaxSize;

	// Use this for initialization
	void Awake ()
    {
        GenerateGrid();
	}
    private void Start()
    {
        if (spawnManager == null)
        {
            spawnManager = GameObject.Find("BlockSpawnManager").GetComponent<BlockSpawnManager>();
        }
        clock = GameObject.Find("Timer").GetComponent<BPMTimer>();
    }

    // Update is called once per frame 
    void Update ()
    {
		MaxSize = xSize * ySize;
	}

    void GenerateGrid()
    {

        vertices = new Vector3[(xSize) * (ySize)];
        grid = new GameObject[xSize * ySize];
        steps = new SequencerPad[xSize * ySize];

        for (int x = 0, i = 0; x < xSize; ++x)
        {
            for (int y = 0; y < ySize; ++y, ++i)
            {
                vertices[i] = new Vector3(x, 0, y);
                vertices[i] *= scaleFactor;

                grid[i] = Instantiate(Pad, vertices[i], Quaternion.identity, transform);
                grid[i].name = "Pad" + i;

                // Attach sequencer pad component and link to block spawn manager
                steps[i] = grid[i].AddComponent<SequencerPad>();
                steps[i].setSpawnManager(spawnManager);
                steps[i].padNumber = i;
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

    public SequencerPad GetPad(int padNumber)
    {
        if (padNumber >= steps.Length || padNumber <= 0)
        {
            padNumber = Mathf.Abs(padNumber % steps.Length);
        }
        return steps[padNumber];
    }
}
