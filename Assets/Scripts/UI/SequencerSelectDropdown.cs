using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SequencerSelectDropdown : MonoBehaviour
{
    [SerializeField]
    private List<SequencerGridTraverser> traversers = new List<SequencerGridTraverser>();
    private List<string> traverserNames = new List<string>();
    private Dropdown drop;

    // Use this for initialization
    void Start()
    {
        drop = GetComponent<Dropdown>();
        if (traversers.Count == 0)
        {
            var objs = GameObject.FindGameObjectsWithTag("Traverser");
            int i = 1;
            foreach (var obj in objs)
            {
                traversers.Add(obj.GetComponent<SequencerGridTraverser>());
                traverserNames.Add("Sequencer " + i);
                i++;
            }
        }
        drop.ClearOptions();
        drop.AddOptions(traverserNames);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
