using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnityEngine;

public class HighHatScript : NoteObjectHat
{
    // The square collider to detect intersections with
    public BoxCollider squareCollider;

    // Flag to track if the drum is already being struck
    private bool isBeingStruck = false;

    public int hitSec = 0;

    public NoteObject NoteObject;

    //public AudioSource HighHatAudio;

    public AudioSource topLeftTrack;   // Top Left Audio Source
    public AudioSource topRightTrack; // Top Right Audio Source
    public AudioSource bottomLeftTrack; // Bottom Left Audio Source
    public AudioSource bottomRightTrack; // Bottom Right Audio Source

    // save values for drum mixing
    public float xValue;
    public float yValue;

    private void PlayAllDrumNotes()
    {
        NoteObjectHat[] allNotes = FindObjectsOfType<NoteObjectHat>();
        foreach (NoteObjectHat b in allNotes)
        {
            b.PlayNoteActivator();
        }
    }

    private void DeactivateAllDrumNotes()
    {
        NoteObjectHat[] allNotes = FindObjectsOfType<NoteObjectHat>();
        foreach (NoteObjectHat b in allNotes)
        {
            b.PlayNoteDeActivator();
        }
    }

    private void Update()
    {
        if (isBeingStruck == true)
        {
            hitSec++;
        }

        if (isBeingStruck == false)
        {
            hitSec = 0;
        }

        if (hitSec >= 1 & hitSec <= 5)
        {
            PlayAllDrumNotes();
            DrumMix();
            //HighHatAudio.Play();
        }
        else
        {
            DeactivateAllDrumNotes();
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        // Ensure the square collider is not null
        if (squareCollider == null)
        {
            Debug.LogError("Square Collider is not assigned!");
            return;
        }

        // Only process the initial strike
        if (!isBeingStruck)
        {
            isBeingStruck = true; // Mark that the drum is being struck         

            // Loop through all contact points
            foreach (ContactPoint contact in collision.contacts)
            {
                // Check if the collider on this object is the square collider
                if (contact.thisCollider == squareCollider)
                {
                    // Get the world-space intersection point
                    Vector3 worldIntersectionPoint = contact.point;

                    // Convert the point to the local space of the drum to account for its rotation
                    Vector3 localIntersectionPoint = squareCollider.transform.InverseTransformPoint(worldIntersectionPoint);

                    // Log the x and y values in local space
                    // Debug.Log($"Initial Strike at X: {localIntersectionPoint.x}, Y: {localIntersectionPoint.y}");
                    xValue = localIntersectionPoint.x;
                    yValue = localIntersectionPoint.y;
                    break; // Exit the loop after the first valid contact point
                }
            }
        }
    }

    void OnCollisionExit(Collision collision)
    {
        // Reset the flag when the collision ends
        isBeingStruck = false;
    }

    public void DrumMix()
    {
        // Normalize xValue and yValue to range [0, 1]
        float normalizedX = Mathf.InverseLerp(-0.5f, 0.5f, xValue);
        float normalizedY = Mathf.InverseLerp(-0.5f, 0.5f, yValue);

        // Calculate weights for each corner
        float weightTL = (1 - normalizedX) * normalizedY; // Top Left
        float weightTR = normalizedX * normalizedY;       // Top Right
        float weightBL = (1 - normalizedX) * (1 - normalizedY); // Bottom Left
        float weightBR = normalizedX * (1 - normalizedY);       // Bottom Right

        // Set volumes for each audio track
        topLeftTrack.volume = weightTL;
        topRightTrack.volume = weightTR;
        bottomLeftTrack.volume = weightBL;
        bottomRightTrack.volume = weightBR;

        // Ensure all tracks are playing
        if (!topLeftTrack.isPlaying) topLeftTrack.Play();
        if (!topRightTrack.isPlaying) topRightTrack.Play();
        if (!bottomLeftTrack.isPlaying) bottomLeftTrack.Play();
        if (!bottomRightTrack.isPlaying) bottomRightTrack.Play();
    }
}