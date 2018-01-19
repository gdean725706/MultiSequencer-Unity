using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public class SampleSelectDropdown : MonoBehaviour
{
    private List<GameObject> activeCubes = new List<GameObject>();
    private List<string> activeCubesNames = new List<string>();

    private Dropdown drop;

	// Use this for initialization
	void Start ()
    {
        drop = GetComponent<Dropdown>();
	}
	
	// Update is called once per frame
	void Update ()
    {
	}

    public void AddStep(GameObject obj)
    {
        activeCubes.Add(obj);
        activeCubesNames.Add(obj.name);
        drop.ClearOptions();
        drop.AddOptions(activeCubesNames);
    }

    public void RemoveLastStep()
    {
        // Safely empty list if about to delete final remaining cube
        if (activeCubes.Count <= 1)
        {
            activeCubes.Clear();
            activeCubesNames.Clear();
            // Add "None" to list
            drop.ClearOptions();
            List<string> str = new List<string> { "None" };
            drop.AddOptions(str);
            return;
        }

        activeCubes.RemoveAt(activeCubes.Count - 1);
        activeCubesNames.RemoveAt(activeCubes.Count - 1);
        drop.ClearOptions();
        drop.AddOptions(activeCubesNames);
    }
}
