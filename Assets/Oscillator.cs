using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class Oscillator : MonoBehaviour {

    [SerializeField] Vector3 movementVector = new Vector3(10f,10f,10f);
    float movementFactor;
    [SerializeField] float period = 2f;


    Vector3 startingPos;

	// Use this for initialization
	void Start () {
        startingPos = transform.position;
	}
	
	// Update is called once per frame
	void Update () {

        // todo protect against period == 0
        float cycles = Time.time / period;

        const float tau = Mathf.PI * 2f; // about 6.28

        float rawSinWave = Mathf.Sin(cycles * tau); // -1 to +1

        // shift sine up 0.5 so result is between 0 - 1
        movementFactor = rawSinWave / 2f + 0.5f;

        Vector3 offset = movementVector * movementFactor;
        transform.position = startingPos + offset;
	}
}
