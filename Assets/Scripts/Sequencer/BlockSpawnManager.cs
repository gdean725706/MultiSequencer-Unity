using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockSpawnManager : MonoBehaviour
{
    public GameObject StepBlockPrefab;
    public Transform StepBlockSpawnParent;

    public BlockSelectDropdown SelectDropdown;
    public SampleSelectSlider SelectSlider;

    private Stack<GameObject> spawnedSteps = new Stack<GameObject>();
    private int spawned = 0;

    /// <summary>
    /// Creates a new step block at the specified position / rotation
    /// </summary>
    /// <param name="position"></param>
    /// <param name="rotation"></param>
    public void AddNewBlock(Vector3 position, Quaternion rotation)
    {
        if (StepBlockPrefab != null)
        {
            var obj = Instantiate(StepBlockPrefab, position, rotation, StepBlockSpawnParent);
            // Keep a tally of how many are spawned
            spawned++;
            // Format the name as this will be shown in the UI
            obj.name = obj.name.Substring(0, 4) + " " + spawned;
            // Add to stack
            spawnedSteps.Push(obj);

            // Update UI
            SelectDropdown.AddStep(obj);
            SelectSlider.AddStep(obj);
        }
    }

    /// <summary>
    /// Removes the last spawned step block like an undo button
    /// </summary>
    public void RemoveLastBlock()
    {
        if (spawnedSteps.Count != 0)
        {
            // Remove from stack
            var remove = spawnedSteps.Pop();
            remove.GetComponent<StepBlock>().DestroyStep();
            spawned--;
            // Update UI
            SelectDropdown.RemoveLastStep();
            SelectSlider.RemoveLastStep();
        }
    }
}
