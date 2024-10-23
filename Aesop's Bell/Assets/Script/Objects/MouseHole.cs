using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseHole : MonoBehaviour
{
    public GameObject gObject;

    private bool _isInside = false;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player")) { 
            gObject.GetComponent<Collider>().enabled = _isInside;
        }
    }

}
