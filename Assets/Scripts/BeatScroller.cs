using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class BeatScroller : MonoBehaviour
{

    public float beatTempo;

    public bool hasStarted;

    //public InputActionReference StartKey;

    // Start is called before the first frame update
    void Start()
    {
        beatTempo = beatTempo / 60f;
    }

    // Update is called once per frame
    void Update()
    {
        if (!hasStarted)
        {
            /*if (StartKey.action != null && StartKey.action.WasPressedThisFrame())
            {
                hasStarted = true;
            } */
        }
        else
        {
            transform.position -= new Vector3(0f, beatTempo * Time.deltaTime, -beatTempo * Time.deltaTime);
        }
    }
}
