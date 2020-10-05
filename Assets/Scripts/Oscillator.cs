using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class Oscillator : MonoBehaviour
{

    [SerializeField] Vector3 movementVector = new Vector3(0, 10, 0);
    [SerializeField] float period = 2f *Mathf.PI;
    // Start is called before the first frame update

    [Range(0,1)]
    [SerializeField]
    float movementFactor; // 0 for not moved, 1 for fully moved

    Vector3 startingPos;

    void Start()
    {
        startingPos = transform.position;


       
    }

    // Update is called once per frame
    void Update()
    {
        if (period <= Mathf.Epsilon) { return; }

        float cycles = Time.time / period; // grows continually from 0

        const float tau = Mathf.PI * 2f;

        float rawSineWave = Mathf.Sin(cycles * tau);

        movementFactor = (rawSineWave / 2f) + 0.5f;

        transform.position = startingPos + movementVector * movementFactor;
    }
}
