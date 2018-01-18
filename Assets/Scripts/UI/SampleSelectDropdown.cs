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
        activeCubes.RemoveAt(activeCubes.Count - 1);
    }
}
