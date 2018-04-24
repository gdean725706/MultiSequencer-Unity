using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// List with stack push/pop functionality
public class ListStack<T>
{
    public List<T> items = new List<T>();

    public void Push(T item)
    {
        items.Add(item);
    }
    public T Pop()
    {
        if (items.Count > 0)
        {
            T temp = items[items.Count - 1];
            items.RemoveAt(items.Count - 1);
            return temp;
        }
        else
            return default(T);
    }
    public void Remove(int itemAtPosition)
    {
        items.RemoveAt(itemAtPosition);
    }
}

public class BlockSpawnManager : MonoBehaviour
{
    public GameObject StepBlockPrefab;
    public Transform StepBlockSpawnParent;

    public BlockSelectDropdown SelectDropdown;
    public SampleSelectSlider SelectSlider;

    private ListStack<GameObject> spawnedSteps = new ListStack<GameObject>();
    private int spawned = 0;

    private GameObject lastSelectedBlock;

    public static bool BrushMode = false;

    public void SetBrushMode(bool state)
    {
        BrushMode = state;
    }

    public void UpdateSelectedBlock(GameObject block)
    {
        lastSelectedBlock = block;
        SelectDropdown.SelectionChanged(block);
    }

    public GameObject GetSelectedBlock()
    {
        return lastSelectedBlock;
    }

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
            obj.name = obj.name.Substring(0, 4) + " " + (spawnedSteps.items.Count + 1);
            // Add to stack
            spawnedSteps.Push(obj);

            // Update UI
            SelectDropdown.AddStep(obj);
            SelectSlider.AddStep(obj);
        }
    }

    public void RemoveBlock(GameObject block)
    {
        if (spawnedSteps.items.Count != 0)
        {
            if (spawnedSteps.items.Contains(block))
            {
                spawnedSteps.items.Remove(block);
                spawned--;
                SelectDropdown.RemoveStep(block);
                SelectSlider.RemoveStep(block);
            }
        }
    }

    /// <summary>
    /// Removes the last spawned step block like an undo button
    /// </summary>
    public void RemoveLastBlock()
    {
        if (spawnedSteps.items.Count != 0)
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
