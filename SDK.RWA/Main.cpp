#include <iostream>
#include <string>

#include "RWA.h"


using namespace std;
using namespace szx;


int main(int argc, char* argv[]) {
	cerr << "load environment." << endl;
	long long secTimeout = atoll(argv[1]);
	int randSeed = atoi(argv[2]);

	cerr << "load input." << endl;
	RWA rwa;
	cin >> rwa.nodeNum >> rwa.arcNum >> rwa.trafficNum;
	rwa.arcs.resize(rwa.arcNum);
	for (auto arc = rwa.arcs.begin(); arc != rwa.arcs.end(); ++arc) { cin >> (*arc)[0] >> (*arc)[1]; }
	rwa.traffics.resize(rwa.trafficNum);
	for (auto traffic = rwa.traffics.begin(); traffic != rwa.traffics.end(); ++traffic) { cin >> (*traffic)[0] >> (*traffic)[1]; }

	cerr << "init output." << endl;
	Routes routes(rwa.trafficNum);

	cerr << "solve." << endl;
	solveRWA(routes, rwa, secTimeout, randSeed);

	cerr << "save output." << endl;
	for (auto route = routes.begin(); route != routes.end(); ++route) {
		cout << route->wavelen << ' ' << route->nodes.size();
		for (auto node = route->nodes.begin(); node != route->nodes.end(); ++node) { cout << ' ' << *node; }
		cout << endl;
	}
	return 0;
}
