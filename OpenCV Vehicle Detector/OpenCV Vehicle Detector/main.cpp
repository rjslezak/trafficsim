
#include "stdafx.h"
#include <opencv2/imgcodecs.hpp>
#include <opencv2/imgproc.hpp>
#include <opencv2/videoio.hpp>
#include <opencv2/highgui.hpp>
#include <opencv2/video.hpp>
#include <list>

using namespace std;
using namespace cv;

Ptr<BackgroundSubtractor> pMOG2;

struct Livestream
{
	VideoCapture video;
	int vid;
	int numOfErosions;
	int numOfDilations;
	int x1;
	int y1;
	int x2;
	int y2;
	int min_threshold;
	int max_threshold;
};

struct VehicleLocation
{
	VehicleLocation(int id, int px, int py) : vid(id), x(px), y(py) {}
	int vid, x, y;
};

list<Livestream> livestream_list;
list<VehicleLocation> detected_v_list;
bool initialized = false;
bool debug = false;
string s1 = "Video ";
string s2 = "Processed Video ";

extern "C" void __declspec(dllexport) __stdcall Initialize(float background_thres, bool enableDebug)
{
	pMOG2 = createBackgroundSubtractorMOG2(500, background_thres, false);
	debug = enableDebug;
	initialized = true;
}

extern "C" int __declspec(dllexport) __stdcall AddLivestream(const char* url, int id, int num_erode, 
	int num_dilate, int detect_x1, int detect_y1, int detect_x2, int detect_y2, int color_thres1, int color_thres2)
{
	Livestream ls;
	ls.video.open(url);
	if (!ls.video.isOpened())
		return -1;
	if (num_erode < 0 || num_dilate < 0)
		return -2;
	ls.vid = id;
	ls.numOfErosions = num_erode;
	ls.numOfDilations = num_dilate;
	ls.x1 = detect_x1;
	ls.y1 = detect_y1;
	ls.x2 = detect_x2;
	ls.y2 = detect_y2;
	ls.min_threshold = color_thres1;
	ls.max_threshold = color_thres2;
	livestream_list.push_back(ls);
	return 0;
}

extern "C" void __declspec(dllexport) __stdcall CloseAll()
{
	for (list<Livestream>::iterator it = livestream_list.begin(); it != livestream_list.end(); it++) {
		it->video.release();
	}
	destroyAllWindows();
}

extern "C" void __declspec(dllexport) __stdcall Detect(VehicleLocation* vlist, int maxNumOfVehicles, int& numOfVehicles)
{
	Mat frame;
	Mat mask;
	Mat can_output;
	vector<vector<Point>> contours;
	vector<Moments> mu;
	int count = 0;
	for (list<Livestream>::iterator it = livestream_list.begin(); it != livestream_list.end(); it++) {
		if (!it->video.read(frame)) {
			continue;
		}
		pMOG2->apply(frame, mask);
		for (int i = 0; i < it->numOfErosions; i++)
			erode(mask, mask, Mat::ones(3, 3, CV_8U), Point(1, 1));
		for (int i = 0; i < it->numOfDilations; i++)
			dilate(mask, mask, Mat::ones(3, 3, CV_8U), Point(1, 1));
		Canny(mask, can_output, 128, 255);
		findContours(can_output, contours, RETR_LIST, CHAIN_APPROX_NONE);
		vector<Moments> mu(contours.size());
		int prev_x = -1, prev_y = -1;
		for (int i = 0; i < contours.size(); i++) {
			Rect rect = boundingRect(contours[i]);
			if (debug)
				rectangle(frame, rect, Scalar(255, 0, 0));
			// check to see if the rectangles are in detection area
			// check to see if vehicle has been previously detected
			if (count == maxNumOfVehicles)
				break;
			int x = rect.x + rect.width/2;
			int y = rect.y + rect.height/2;
			if (x == prev_x && y == prev_y)
				continue;
			if (x >= it->x1 && x <= it->x2 && y >= it->y1 && y <= it->y2) {
				vlist[count] = VehicleLocation(it->vid, x, y);
				count++;
				numOfVehicles++;
				prev_x = x;
				prev_y = y;
			}
		}
		if (debug)
			rectangle(frame, Rect(Point(it->x1, it->y1), Point(it->x2, it->y2)), Scalar(0, 0, 255));
		if (debug)
		{
			imshow(s1 + to_string(it->vid), frame);
			imshow(s2 + to_string(it->vid), mask);
		}
	}
}


