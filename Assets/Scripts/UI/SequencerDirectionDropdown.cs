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

    // Use this for initialization
    void Start()
    {
        drop = GetComponent<Dropdown>();

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
    }

    public void UpdateDirection(int direction)
    {
        traversers[currentTraverser].SequencerDirection = (SequencerGridTraverser.Mode)direction;
    }
}
