using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPoint : MonoBehaviour
{
    public bool allowSpawn = true;

    void OnEnable()
    {
        allowSpawn = true;
    }

    void OnTriggerEnter(Collider other)
    {
        allowSpawn = false;
        Debug.Log("Enter");
    }

    void OnTriggerStay(Collider other)
    {
        allowSpawn = false;
    }

    void OnTriggerExit(Collider other)
    {
        allowSpawn = true;
        Debug.Log("Exit");
    }
}
