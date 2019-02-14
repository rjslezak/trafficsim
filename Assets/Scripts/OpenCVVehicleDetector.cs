using System.Collections.Generic;
using System.Runtime.InteropServices;

internal class OpenCVVehicleDetector
{
    [DllImport("OpenCV Vehicle Detector")]
    internal static extern void Initialize(float background_thres, bool enableDebug);

    [DllImport("OpenCV Vehicle Detector")]
    internal static extern int AddLivestream(string url, int id, int num_erode,
        int num_dilate, int detect_x1, int detect_y1, int detect_x2, int detect_y2,
        int color_thres1, int color_thres2);

    [DllImport("OpenCV Vehicle Detector")]
    internal static extern void CloseAll();

    [DllImport("OpenCV Vehicle Detector")]
    internal unsafe static extern void Detect(VehicleLocation* vlist, int maxNumOfVehicles, ref int numOfVehicles);
}

[StructLayout(LayoutKind.Sequential, Size = 12)]
public struct VehicleLocation
{
    public int vid, x, y;
}
