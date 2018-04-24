using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InputFieldUpdateText : MonoBehaviour
{
    private InputField inp;

    [SerializeField]
    private string suffix = "";

	// Use this for initialization
	void Start () {
        inp = GetComponent<InputField>();
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void UpdateText(float value)
    {
        inp.text = value.ToString() + " " + suffix;
    }
}
