using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VehicleSpawner : MonoBehaviour {

    // Need to have a object pool for memory performance
    public GameObject Vehicle;

	// Use this for initialization
	void Start () {
		
	}
	
	public void ParseData(VehicleLocation[] vlist, int list_size)
    {
        for(int i = 0; i < list_size; i++)
        {
            VehicleDetector detector = FindObjectOfType<VehicleDetector>();
            LivestreamInfo info = detector.LivestreamList[vlist[i].vid];
            // Need to modify position based on video resolution
            SpawnPoint sp = info.GetClosestSpawnPoint(vlist[i].x);
            if (sp.allowSpawn)
            {
                sp.allowSpawn = false;
                Vector3 pos = sp.transform.position;
                Quaternion rot = sp.transform.rotation;
                SpawnVehicle(pos, rot);
            }
        }
    }

    void SpawnVehicle(Vector3 pos, Quaternion rot)
    {
        // Modify to include position and rotation
        Instantiate(Vehicle, pos, rot);
    }
}
