using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rocket : MonoBehaviour
{

    Rigidbody rigidBody;
    AudioSource audioSource;
    [SerializeField] float rcsThrust = 200f;
    [SerializeField] float rocketThrust = 45f;


    // Start is called before the first frame update
    void Start()
    {

        rigidBody = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();

    }
    void Update()
    {
        Thrust();

        Rotate();

        resetPosition();

    }

    private void resetPosition()
    {
        if (Input.GetKey(KeyCode.R))
        {
            rigidBody.transform.position = new Vector3(-45, 5, 0);
            rigidBody.transform.rotation = Quaternion.Euler(0, 0, 0);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
       
        switch (collision.gameObject.tag)
        {
            case "Friendly":

                print("okay");
                break;

            case "Finish":

                print("Finished");
                    break;

            default:

                print("Dead");
                rigidBody.transform.position = new Vector3(-45, 5, 0);
                rigidBody.transform.rotation = Quaternion.Euler(0, 0, 0);

                break;
        }

    }

    private void Rotate()
    {

        rigidBody.freezeRotation = true;

        
        float rotationThisFrame;

        if (Input.GetKey(KeyCode.LeftArrow))
        {

            rotationThisFrame = rcsThrust * Time.deltaTime;

            transform.Rotate(Vector3.forward * rotationThisFrame); ;

        }
        else if (Input.GetKey(KeyCode.RightArrow))
        {
            rotationThisFrame = rcsThrust * Time.deltaTime;
            transform.Rotate(Vector3.back * rotationThisFrame);
        }

        rigidBody.freezeRotation = false;
    }

    private void Thrust()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            rigidBody.AddRelativeForce(Vector3.up * rocketThrust);
            if (!audioSource.isPlaying)
            {
                audioSource.Play();
            }


        }

        if (Input.GetKeyUp(KeyCode.Space))
        {
            rigidBody.AddRelativeForce(Vector3.up);
            if (audioSource.isPlaying)
            {
                audioSource.Stop();
            }


        }
    }
}