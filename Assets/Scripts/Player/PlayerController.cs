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
    private Transform player;

	// Use this for initialization
	void Start ()
    {
        player = transform.GetChild(0);
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
	}

    private void OnGUI()
    {
        
    }
}
