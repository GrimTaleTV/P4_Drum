using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using Unity.Mathematics;
using System.Threading;
using System;

public class GameManager : MonoBehaviour
{
    public AudioSource KickDrumAudio;

    public bool startPlaying;

    public BeatScroller theBS;

    public static GameManager instance;

    public InputActionReference StartKey;

    public InputActionReference KickDrumKey;

    private float endGameTimer = 0f;

    // This part is for keeping score

    private int currentScore;
    private int scorePerNote = 100;
    private int scorePerPerfectNote = 200;
    private float notesPlayed;
    private float notesHit;
    private float accuracy;
    public int perfectNotesHit;

    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI accText;

    // This is to control game speed.

    public bool gameSpeedMedium = false;
    public bool gameSpeedHard = false;

    //for spawning gameObjects.

    public Transform BeatScroller; // The parent object

    public GameObject DrumNoteObject; // The prefabs to instantiate
    public GameObject HighHatNoteObject; 
    public GameObject KickDrumNoteObject; 

    public Vector3 drumSpawnPosition; // The specific XYZ position for the new object
    public Vector3 highHatSpawnPosition;
    public Vector3 kickDrumSpawnPosition;

    public float drumInterval = 2f; // Time (in seconds) between spawns
    public float highHatInterval = 2f; 

    public float timer = 0f; // Tracks time for spawning

    private bool spawnFirstObject = true; // Keeps track of which object to spawn next

    // Spageti part, this is gonna be a bunch of has run functions for each note.
    public bool hasRunOne = false;
    public bool hasRunTwo = false;
    public bool hasRunThree = false;
    public bool hasRunFour = false;
    public bool hasRunFive = false;

    // Start is called before the first frame update
    void Start()
    {
        instance = this; 
    }

    // Update is called once per frame
    void Update()
    {
        if (!startPlaying)
        {
            if (StartKey.action != null && StartKey.action.WasPressedThisFrame())
            {
                startPlaying = true;
                theBS.hasStarted = true;

                notesPlayed = 0f;
                notesHit = 0f;
                currentScore = 0;
            }
        }

        if (KickDrumKey.action != null && KickDrumKey.action.WasPressedThisFrame())
        {
            KickDrumAudio.Play();
        }

        if (startPlaying == true)
        {
            // Increment the timer by the time passed since the last frame
            timer += Time.deltaTime;
            endGameTimer += Time.deltaTime;

            if (gameSpeedMedium == true)
            {
                timer += Time.deltaTime;
            }

            if (gameSpeedHard == true)
            {
                timer += Time.deltaTime + Time.deltaTime;
                theBS.bpm = 150;
            }

            if (endGameTimer >= 30)
            {
                startPlaying = false;
                endGameTimer = 0;
            }
        }

        if (timer >= 1 && timer <= 2)
        {
            if (hasRunOne == false)
            {
                HighHatNoteGenerator();
                KickDrumNoteGenerator();
                hasRunOne = true;
            }
        }

        if (timer >= 2 && timer <= 3)
        {
            if (hasRunTwo == false)
            {
                HighHatNoteGenerator();
                DrumNoteGenerator();
                hasRunTwo = true;
            }
        }

        if (timer >= 3 && timer <= 4)
        {
            if (hasRunThree == false)
            {
                HighHatNoteGenerator();
                hasRunThree = true;
            }
        }

        if (timer >= 4)
        {
            if (hasRunFour == false)
            {
                HighHatNoteGenerator();
                DrumNoteGenerator();
                timer = 0f;
                hasRunOne = false;
                hasRunTwo = false;
                hasRunThree = false;
            }
        }
    }

    public void NoteHit()
    {
        Debug.Log("Hit On Time");

        notesPlayed++;
        notesHit++;

        currentScore += scorePerNote;
        string currString = currentScore.ToString();
        scoreText.text = "score:" + currString;

        accuracy = (notesHit / notesPlayed) * 100f;
        string formAccuracy = accuracy.ToString("F2");
        accText.text = "Acc%:" + formAccuracy;
    }

    public void NoteMissed()
    {
        Debug.Log("Missed Note");

        notesPlayed++;

        accuracy = (notesHit / notesPlayed) * 100f;

        string formAccuracy = accuracy.ToString("F2");
        accText.text = "Acc%:" + formAccuracy;
    }

    // this is all to spawn new nodes.
    public void DrumNoteGenerator()
    {
        // Create a rotation of 45 degrees on the X-axis
        Quaternion rotation = Quaternion.Euler(45f, 0f, 0f);

        // Instantiate the prefab at the specified spawn position
        GameObject newObject = Instantiate(DrumNoteObject, drumSpawnPosition, rotation);

        // Set the new object as a child to the parentTransform
        newObject.transform.SetParent(BeatScroller);
    }

    
    public void HighHatNoteGenerator()
    {
        // Create a rotation of 45 degrees on the X-axis
        Quaternion rotation = Quaternion.Euler(45f, 0f, 0f);

        // Instantiate the prefab at the specified spawn position
        GameObject newObject = Instantiate(HighHatNoteObject, highHatSpawnPosition, rotation);

        // Set the new object as a child to the parentTransform
        newObject.transform.SetParent(BeatScroller);
    }
    
    public void KickDrumNoteGenerator()
    {
        // Create a rotation of 90 degrees on the z-axis
        Quaternion rotation = Quaternion.Euler(-45f, 0f, 90f);

        // Instantiate the prefab at the specified spawn position
        GameObject newObject = Instantiate(KickDrumNoteObject, kickDrumSpawnPosition, rotation);

        // Set the new object as a child to the parentTransform
        newObject.transform.SetParent(BeatScroller);
    }
}
