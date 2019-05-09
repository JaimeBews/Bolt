using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrisbeeScript : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision) {
        GetComponent<Rigidbody>().isKinematic = true;
    }
}
