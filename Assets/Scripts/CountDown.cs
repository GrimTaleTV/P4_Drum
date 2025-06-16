using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CountDown : MonoBehaviour
{
    public GameManager Manager;

    public bool firstNote;

    public AudioSource countDownAudio;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Manager.startPlaying == false)
        {
            firstNote = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "FirstNote")
        {
            if (firstNote == false)
            {
                countDownAudio.Play();
                firstNote = true;
            }
        }
    }
}
