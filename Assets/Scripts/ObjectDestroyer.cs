using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectDestroyer : MonoBehaviour {

	void OnTriggerEnter(Collider other)
    {
        if (other.transform.parent.parent.gameObject.tag == "Car")
            Destroy(other.transform.parent.parent.gameObject);
    }
}
