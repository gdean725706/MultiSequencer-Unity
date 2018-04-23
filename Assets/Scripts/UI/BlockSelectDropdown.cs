using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public class BlockSelectDropdown : MonoBehaviour
{
    private List<GameObject> activeCubes = new List<GameObject>();
    private List<string> activeCubesNames = new List<string>();

    private Dropdown drop;

    [SerializeField]
    private Slider startFreq;
    [SerializeField]
    private Slider endFreq;
    [SerializeField]
    private Slider pitchDecay;
    [SerializeField]
    private Slider ampDecay;
    [SerializeField]
    private Slider sinSqMix;

    // Use this for initialization
    void Start ()
    {
        drop = GetComponent<Dropdown>();
	}
	
	// Update is called once per frame
	void Update ()
    {
	}

    // Called when a step is spawned
    // Adds to dropdown
    public void AddStep(GameObject obj)
    {
        activeCubes.Add(obj);
        activeCubesNames.Add(obj.name);
        drop.ClearOptions();
        drop.AddOptions(activeCubesNames);
    }

    // called when step is deleted
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

    public void ValueChanged(int value)
    {
        startFreq.value = activeCubes[value].GetComponent<StepBlock>()
    }
}
