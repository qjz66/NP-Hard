#include <iostream>
#include <string>
#include <chrono>

#include "DARP2d.h"


using namespace std;
using namespace szx;


void loadInput(istream& is, DARP2d& darp) {
	is >> darp.requestNum >> darp.maxVehicleNum >> darp.vehicleCapacity >> darp.maxTravelTime >> darp.maxRideTime;
	darp.nodes.resize(darp.nodeNum());
	for (auto n = darp.nodes.begin(); n != darp.nodes.end(); ++n) {
		is >> n->coords[0] >> n->coords[1] >> n->demand >> n->minStayTime >> n->windowBegin >> n->windowEnd;
		n->coords[0] *= DARP2d::Precision;
		n->coords[1] *= DARP2d::Precision;
		n->minStayTime *= DARP2d::Precision;
		n->windowBegin *= DARP2d::Precision;
		n->windowEnd *= DARP2d::Precision;
	}
}

void saveOutput(ostream& os, Routes& routes) {
	for (auto route = routes.begin(); route != routes.end(); ++route) {
		if (route->nodes.empty()) { continue; }
		for (auto node = route->nodes.begin(); node != route->nodes.end(); ++node) { os << *node << ' '; }
		os << endl;
	}
}

void test(istream& inputStream, ostream& outputStream, long long secTimeout, int randSeed) {
	cerr << "load input." << endl;
	DARP2d darp;
	loadInput(inputStream, darp);

	cerr << "solve." << endl;
	chrono::steady_clock::time_point endTime = chrono::steady_clock::now() + chrono::seconds(secTimeout);
	Routes routes(darp.maxVehicleNum);
	solveDARP2d(routes, darp, [&]() -> bool { return endTime < chrono::steady_clock::now(); }, randSeed);

	cerr << "save output." << endl;
	saveOutput(outputStream, routes);
}
void test(istream& inputStream, ostream& outputStream, long long secTimeout) {
	return test(inputStream, outputStream, secTimeout, static_cast<int>(time(nullptr) + clock()));
}

#include <fstream>
int main(int argc, char* argv[]) {
	ifstream ifs("D:/Workspace/mixed/Training/DARP/Instance/pr01.r24v3c6t480l90.txt");
	ofstream ofs("D:/Workspace/mixed/Training/DARP/Instance/sln.txt");
	test(ifs, ofs, 100); // for self-test.
	//cerr << "load environment." << endl;
	//long long secTimeout = atoll(argv[1]);
	//int randSeed = atoi(argv[2]);

	//ifstream ifs("path/to/instance.txt");
	//ofstream ofs("path/to/solution.txt");
	//test(ifs, ofs, secTimeout); // for self-test.

	//test(cin, cout, secTimeout, randSeed);
	return 0;
}
