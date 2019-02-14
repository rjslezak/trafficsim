using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LivestreamInfo : MonoBehaviour {

    public string URL;
    [Range(0, 6)]
    public int numOfErosions = 1;
    [Range(0, 6)]
    public int numOfDilations = 1;
    public Vector2 DetectionAreaPoint1;
    public Vector2 DetectionAreaPoint2;
    [Range(0, 255)]
    public int FirstColorThreshold = 128;
    [Range(0, 255)]
    public int SecondColorThreshold = 255;
    public List<GameObject> SpawnPoints;
    public List<float> VideoResToVector3Pos;

    void Start()
    {
        if (SpawnPoints.Count != VideoResToVector3Pos.Count)
            Debug.LogError("Size of SpawnPoints and VideoResToVector3Pos must be identical!");
    }

    public SpawnPoint GetClosestSpawnPoint(int x)
    {
        GameObject go = null;
        float min = 0f;
        for(int i = 0; i < SpawnPoints.Count; i++)
        {
            float current = Math.Abs(x / SpawnPoints[i].transform.position.x - VideoResToVector3Pos[i]);
            if (i == 0)
            {
                min = current;
                go = SpawnPoints[i];
            }
            else
            {
                if (current < min)
                {
                    min = current;
                    go = SpawnPoints[i];
                }
            }
        }
        return go.GetComponent<SpawnPoint>();
    }
}
