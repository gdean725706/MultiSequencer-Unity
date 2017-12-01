using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    public GameObject StepBlockPrefab;
    public Transform StepBlockSpawnParent;

    [SerializeField]
    private float spawnDistance = 1;

    private Stack<GameObject> spawnedSteps = new Stack<GameObject>();

    public bool write = false;
    public bool noWrite = true;

    public Transform player;

	// Use this for initialization
	void Start ()
    {
        player = transform;
	}
	
	// Update is called once per frame
	void Update ()
    {
        // Spawn cube
		if (Input.GetKeyDown(KeyCode.K))
        {
            if (StepBlockPrefab != null)
            {
                Vector3 spawnPos = player.position + player.forward * spawnDistance;
                var obj = Instantiate(StepBlockPrefab, spawnPos, player.rotation, StepBlockSpawnParent);
                spawnedSteps.Push(obj);
            }
        }

        // Undo spawn cube
        if (Input.GetKeyDown(KeyCode.Z))
        {
            if (spawnedSteps.Count != 0)
            {
                var remove = spawnedSteps.Pop();
                remove.GetComponent<StepBlock>().DestroyStep();
            }
        }

        if (Input.GetKeyDown(KeyCode.G))
        {
            write = !write;
        }

        if (Input.GetKeyDown(KeyCode.H))
        {
            noWrite = !noWrite;
        }
	}

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Pads"))
        {
            var pad = other.gameObject.GetComponent<SequencerPad>();
            if (!noWrite)
                pad.State = write ? 1 : 0;
        }
    }

    private void OnGUI()
    {
        
    }


}
