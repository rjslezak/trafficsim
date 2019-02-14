using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VehicleDetector : MonoBehaviour {

    public List<LivestreamInfo> LivestreamList;
    public VehicleSpawner VehicleInstantiator;
    public float BackgroundThreshold = 16f;
    public int MaxNumberOfDetectedVehicles = 9;
    public bool EnableDebug = false;
    private bool isReady = false;
    private VehicleLocation[] VL_List;
    private float SecondsPerFrame = 1 / 30f;
    private float Timer = 0f;

	// Use this for initialization
	void Start () {
        VL_List = new VehicleLocation[MaxNumberOfDetectedVehicles];
        OpenCVVehicleDetector.Initialize(BackgroundThreshold, EnableDebug);
        int i = 0;
        foreach (LivestreamInfo info in LivestreamList)
        {
            int code = OpenCVVehicleDetector.AddLivestream(info.URL, i, info.numOfErosions, info.numOfDilations,
                (int)info.DetectionAreaPoint1.x, (int)info.DetectionAreaPoint1.y, (int)info.DetectionAreaPoint2.x,
                (int)info.DetectionAreaPoint2.y, info.FirstColorThreshold, info.SecondColorThreshold);
            i++;
            if (code == -1)
                Debug.LogError("Failed to connect to: " + info.URL);
        }
        isReady = true;
	}
	
	// Update is called once per frame
	void Update () {
        /*if (Timer >= SecondsPerFrame)
        {
            if (isReady)
            {
                // Get data from the DLL
                int NumberOfVehicles = 0;
                VL_List = new VehicleLocation[MaxNumberOfDetectedVehicles];
                unsafe
                {
                    fixed (VehicleLocation* vlist = VL_List)
                    {
                        OpenCVVehicleDetector.Detect(vlist, MaxNumberOfDetectedVehicles, ref NumberOfVehicles);
                    }
                }
                if (NumberOfVehicles > 0)
                    VehicleInstantiator.ParseData(VL_List, NumberOfVehicles);
                Timer = 0;
            }
        }
        else
            Timer += Time.deltaTime;*/

        if (isReady)
        {
            // Get data from the DLL
            int NumberOfVehicles = 0;
            VL_List = new VehicleLocation[MaxNumberOfDetectedVehicles];
            unsafe
            {
                fixed (VehicleLocation* vlist = VL_List)
                {
                    OpenCVVehicleDetector.Detect(vlist, MaxNumberOfDetectedVehicles, ref NumberOfVehicles);
                }
            }
            if (NumberOfVehicles > 0)
                VehicleInstantiator.ParseData(VL_List, NumberOfVehicles);
        }
    }

    private void OnApplicationQuit()
    {
        isReady = false;
        OpenCVVehicleDetector.CloseAll();
    }
}
