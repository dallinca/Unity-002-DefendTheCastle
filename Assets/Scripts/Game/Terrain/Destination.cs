using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destination : MonoBehaviour
{

    public Transform[] NextDestination;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public bool HasNextDestination() {
        if (NextDestination.Length > 0) {
            return true;
        }

        return false;
    }

    public Transform GetNextDestination() {
        if (NextDestination.Length == 0) {
            return null;
        }

        return NextDestination[Random.Range(0, NextDestination.Length)];
    }
}
