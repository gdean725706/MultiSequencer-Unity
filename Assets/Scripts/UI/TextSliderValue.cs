using UnityEngine;
using UnityEngine.UI;

public class TextSliderValue : MonoBehaviour
{

    Text textComponent;
	// Use this for initialization
	void Start ()
    {
        textComponent = GetComponent<Text>();
	}
	
	// Update is called once per frame
	void Update ()
    {
		
	}

    public void UpdateTextValue(float sliderValue)
    {
        textComponent.text = Mathf.Round(sliderValue).ToString();
    }
}
