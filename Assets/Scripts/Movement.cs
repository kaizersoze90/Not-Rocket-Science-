using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    [SerializeField] float mainThrust = 10f;
    [SerializeField] float rotationThrust = 6f;
    [SerializeField] ParticleSystem thrustEffect, leftThrustEffect, rightThrustEffect;

    Rigidbody myRigidbody;
    AudioSource audioSource;

    void Start()
    {
        myRigidbody = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
        thrustEffect.Stop();
    }

    void Update()
    {
        ProcessThrust();
        ProcessRotation();
    }

    void ProcessThrust()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            StartBoost();
        }
        else
        {
            StopBoost();
        }
    }

    void StopBoost()
    {
        audioSource.Stop();
        thrustEffect.Stop();
    }

    void StartBoost()
    {
        myRigidbody.AddRelativeForce(Vector3.up * mainThrust * Time.deltaTime);
        if (!audioSource.isPlaying)
        {
            audioSource.Play();
        }
        if (!thrustEffect.isPlaying)
        {
            thrustEffect.Play();
        }
    }

    void ProcessRotation()
    {
        if (Input.GetKey(KeyCode.A))
        {
            StartLeftBoost();
        }
        else if (Input.GetKey(KeyCode.D))
        {
            StartRightBoost();
        }
        else
        {
            StopSideBoosts();
        }
    }

    void StopSideBoosts()
    {
        rightThrustEffect.Stop();
        leftThrustEffect.Stop();
    }

    void StartRightBoost()
    {
        ApplyRotation(-rotationThrust);
        if (!leftThrustEffect.isPlaying)
        {
            leftThrustEffect.Play();
        }
    }

    void StartLeftBoost()
    {
        ApplyRotation(rotationThrust);
        if (!rightThrustEffect.isPlaying)
        {
            rightThrustEffect.Play();
        }
    }

    void ApplyRotation(float rotationThisFrame)
    {
        myRigidbody.freezeRotation = true;
        transform.Rotate(Vector3.forward * rotationThisFrame * Time.deltaTime);
        myRigidbody.freezeRotation = false;
    }
}
