using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SequencerToggle : MonoBehaviour
{
    private Toggle tog;

    [SerializeField]
    private List<SequencerGridTraverser> traversers = new List<SequencerGridTraverser>();
    private int currentTraverser = 0;

    // Use this for initialization
    void Start()
    {
        tog = GetComponent<Toggle>();
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
        currentTraverser = Mathf.Clamp(current, 0, traversers.Count);
        tog.isOn = traversers[currentTraverser].Running;
    }

    public void UpdateToggleValue(bool current)
    {
        traversers[currentTraverser].Running = current;
    }
}
