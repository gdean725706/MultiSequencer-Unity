using UnityEngine;
using UnityEngine.UI;

public class TextSliderValue : MonoBehaviour
{

    Text textComponent;

    [SerializeField]
    bool round = true;

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
        if (round)
        {
            textComponent.text = Mathf.Round(sliderValue).ToString();
        }
        else
            textComponent.text = sliderValue.ToString("f2");
    }
}
