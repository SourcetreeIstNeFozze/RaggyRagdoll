using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test : MonoBehaviour
{
    public float toConvert;
    public float converted;
    public float moduloed;
	
	// Start is called before the first frame update
	void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
		if (Input.GetKeyDown(KeyCode.Space)) 
        {
            converted = ExtensionMethods.FloatTo180Spectrum(toConvert);
            moduloed = toConvert % 360;
        }    
    }
}
