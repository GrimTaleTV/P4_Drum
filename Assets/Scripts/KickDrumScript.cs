using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using UnityEngine.InputSystem;

public class KickDrumScript : MonoBehaviour
{
    public bool canBePressed;

    public InputActionReference KickDrumKey;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (canBePressed == true)
        {
            if (KickDrumKey.action != null && KickDrumKey.action.WasPressedThisFrame())
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
        }

        if(other.tag == "Floor")
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
}
