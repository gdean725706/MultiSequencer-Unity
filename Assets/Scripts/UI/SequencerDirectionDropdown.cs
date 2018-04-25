using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SequencerDirectionDropdown : MonoBehaviour
{
    [SerializeField]
    private List<SequencerGridTraverser> traversers = new List<SequencerGridTraverser>();
    private int currentTraverser = 0;
    private Dropdown drop;
    [SerializeField]
    private InputField lastStepInput;
    [SerializeField]
    private InputField firstStepInput;

    // Use this for initialization
    void Start()
    {
        drop = GetComponent<Dropdown>();

        var inp = GetComponentsInChildren<InputField>();

        lastStepInput = inp[0];
        firstStepInput = inp[1];

        if (traversers.Count == 0)
        {
            var objs = GameObject.FindGameObjectsWithTag("Traverser");
            foreach (var obj in objs)
            {
                traversers.Add(obj.GetComponent<SequencerGridTraverser>());
            }
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void UpdateCurrentTraverser(int current)
    {
        currentTraverser = current;
        current = Mathf.Clamp(current, 0, traversers.Count);
        drop.value = current;
        lastStepInput.text = traversers[current].GetLastStep().ToString();
        firstStepInput.text = (traversers[current].GetFirstStep() + 1).ToString();
    }

    public void UpdateDirection(int direction)
    {
        traversers[currentTraverser].SequencerDirection = (SequencerGridTraverser.Mode)direction;
    }

    public void UpdateLastStep(string step)
    {
        int v = 0;
        int.TryParse(step, out v);
        boundsCheck(v);

        traversers[currentTraverser].SetLastStep(v);

        // Update input box
        lastStepInput.text = v.ToString();
        
    }

    public void UpdateFirstStep(string step)
    {
        int v = 0;
        int.TryParse(step, out v);
        boundsCheck(v);

        traversers[currentTraverser].SetFirstStep(v);

        firstStepInput.text = v.ToString();
    }

    // Clamps step value to be within range of seq grid
    private int boundsCheck(int v)
    {
        return v < 0 ? 0 : v > SequencerGrid.MaxSize ? SequencerGrid.MaxSize : v;
    }
}
