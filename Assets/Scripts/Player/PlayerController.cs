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

    public Camera PlayerCamera;
    public Camera SceneCamera;

    public SampleSelectDropdown SelectDropdown;
    public SampleSelectSlider SelectSlider;

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
            if (StepBlockPrefab != null)
            {
                Vector3 spawnPos = player.position + player.forward * spawnDistance;
                var obj = Instantiate(StepBlockPrefab, spawnPos, player.rotation, StepBlockSpawnParent);
                spawned++;
                obj.name = obj.name.Substring(0,4) + " " + spawned;
                spawnedSteps.Push(obj);

                // Update UI
                SelectDropdown.AddStep(obj);
                SelectSlider.AddStep(obj);
            }
        }

        // Undo spawn cube
        if (Input.GetKeyDown(KeyCode.Z))
        {
            if (spawnedSteps.Count != 0)
            {
                var remove = spawnedSteps.Pop();
                remove.GetComponent<StepBlock>().DestroyStep();
                spawned--;
                // Update UI
                SelectDropdown.RemoveLastStep();
                SelectSlider.RemoveLastStep();
            }
        }

        if (Input.GetKeyDown(KeyCode.K))
        {
            sceneCamActive = !sceneCamActive;
            SceneCamera.gameObject.SetActive(sceneCamActive);
            PlayerCamera.gameObject.SetActive(!sceneCamActive);
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
