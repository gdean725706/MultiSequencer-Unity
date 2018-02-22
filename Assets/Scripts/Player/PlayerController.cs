using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public BlockSpawnManager blockSpawnManager;

    [SerializeField]
    private float spawnDistance = 1;
    

    public bool write = false;
    public bool noWrite = true;

    public Transform player;

    public Camera PlayerCamera;
    public Camera SceneCamera;

    private enum ActiveCamera
    {
        Player = 0,
        Scene = 1
    }

    private ActiveCamera activeCam = 0;

    private bool sceneCamActive = false;
    private int spawned = 0;

	// Use this for initialization
	void Start ()
    {
        if (blockSpawnManager == null)
        {
            blockSpawnManager = GameObject.Find("BlockSpawnManager").GetComponent<BlockSpawnManager>();
        }
        
        player = transform;

        // Find Cameras
        if (PlayerCamera == null)
            PlayerCamera = GetComponentInChildren<Camera>();

        if (SceneCamera == null)
            SceneCamera = GameObject.Find("SceneCamera").GetComponent<Camera>();


	}
	
	// Update is called once per frame
	void Update ()
    {
        // Spawn cube
		if (Input.GetKeyDown(KeyCode.C))
        {
            Vector3 spawnPos = player.position + player.forward * spawnDistance;

            blockSpawnManager.AddNewBlock(spawnPos, player.rotation);
        }

        // Undo spawn cube
        if (Input.GetKeyDown(KeyCode.Z))
        {
            blockSpawnManager.RemoveLastBlock();
        }

        if (Input.GetKeyDown(KeyCode.G))
        {
            write = !write;
        }

        if (Input.GetKeyDown(KeyCode.H))
        {
            noWrite = !noWrite;
        }

        if (Input.GetKeyDown(KeyCode.K))
        {
            sceneCamActive = !sceneCamActive;
            SceneCamera.gameObject.SetActive(sceneCamActive);
            PlayerCamera.gameObject.SetActive(!sceneCamActive);
            activeCam = 1 - activeCam;
        }

        //if (activeCam == ActiveCamera.Scene)
        //{
        //    if (Input.GetMouseButtonDown(0))
        //    {
        //        RaycastHit hit;
        //        Ray ray = SceneCamera.ScreenPointToRay(Input.mousePosition);
        //    }
        //}
        
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


}
