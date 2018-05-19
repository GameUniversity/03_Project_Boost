using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rocket : MonoBehaviour {

    Rigidbody rigidBody;
    AudioSource audioSource;
    bool m_PlayThrust = false;

	// Use this for initialization
	void Start () {
        rigidBody = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
	}

	// Update is called once per frame
	void Update () {
        ProcessInput();
        PlaySounds();
	}


    private void PlaySounds()
    {
        // We don't want to re-play if the clip is always playing.
        if ( m_PlayThrust == true && ! audioSource.isPlaying )
        {
            audioSource.Play();
        }

        if ( m_PlayThrust == false && audioSource.isPlaying )
        {
            audioSource.Stop();
        }

    }

    private void ProcessInput()
    {
        if( Input.GetKey(KeyCode.Space))
        {
            m_PlayThrust = true;
            rigidBody.AddRelativeForce(Vector3.up);
        }
        else
        {
            m_PlayThrust = false;
        }

        if ( Input.GetKey(KeyCode.A))
        {
            transform.Rotate(Vector3.forward);
        } else if( Input.GetKey(KeyCode.D))
        {
            transform.Rotate(-Vector3.forward);
        }
    }
}
