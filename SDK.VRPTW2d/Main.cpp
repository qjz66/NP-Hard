#include <iostream>
#include <string>

#include "VRPTW2d.h"


using namespace std;
using namespace szx;


int main(int argc, char* argv[]) {
	cerr << "load environment." << endl;
	long long secTimeout = atoll(argv[1]);
	int randSeed = atoi(argv[2]);

	cerr << "load input." << endl;
	VRPTW2d vrp;
	cin >> vrp.nodeNum >> vrp.maxVehicleNum >> vrp.vehicleCapacity;
	vrp.nodes.resize(vrp.nodeNum);
	for (auto n = vrp.nodes.begin(); n != vrp.nodes.end(); ++n) {
		cin >> n->coords[0] >> n->coords[1] >> n->demand >> n->minStayTime >> n->windowBegin >> n->windowEnd;
		n->coords[0] *= VRPTW2d::Precision;
		n->coords[1] *= VRPTW2d::Precision;
		n->minStayTime *= VRPTW2d::Precision;
		n->windowBegin *= VRPTW2d::Precision;
		n->windowEnd *= VRPTW2d::Precision;
	}

	cerr << "init output." << endl;
	Routes routes(vrp.maxVehicleNum);

	cerr << "solve." << endl;
	solveVRPTW2d(routes, vrp, secTimeout, randSeed);

	cerr << "save output." << endl;
	for (auto route = routes.begin(); route != routes.end(); ++route) {
		if (route->nodes.empty()) { continue; }
		for (auto node = route->nodes.begin(); node != route->nodes.end(); ++node) { cout << *node << ' '; }
		cout << endl;
	}
	return 0;
}
