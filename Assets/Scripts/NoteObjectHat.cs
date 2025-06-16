using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;

public class NoteObjectHat : MonoBehaviour
{
    public bool canBePressed;

    public bool playNote = false;

    public bool noteDetected;

    public AudioSource metronomeAudio;


    void Update()
    {
        if (canBePressed == true)
        {
            if (playNote == true)
            {
                gameObject.SetActive(false);

                GameManager.instance.NoteHit();
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Activator")
        {
            canBePressed = true;

            if (noteDetected == false)
            {
                metronomeAudio.Play();
                noteDetected = true;
            }
        }

        if (other.tag == "Floor")
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Activator")
        {
            canBePressed = false;

            GameManager.instance.NoteMissed();
        }
    }

    public void PlayNoteActivator()
    {
        playNote = true;
    }

    public void PlayNoteDeActivator()
    {
        playNote = false;
    }
}