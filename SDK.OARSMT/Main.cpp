#include <iostream>
#include <string>
#include <chrono>

#include "OARSteinerMinTree.h"


using namespace std;
using namespace szx;


int main(int argc, char* argv[]) {
	cerr << "load environment." << endl;
	long long secTimeout = atoll(argv[1]);
	int randSeed = atoi(argv[2]);

	cerr << "load input." << endl;
	OARSteinerMinTree oarsmt;
	cin >> oarsmt.nodeNum >> oarsmt.obstacleNum;
	oarsmt.nodes.resize(oarsmt.nodeNum);
	for (auto node = oarsmt.nodes.begin(); node != oarsmt.nodes.end(); ++node) { cin >> (*node)[0] >> (*node)[1]; }
	oarsmt.obstacles.resize(oarsmt.obstacleNum);
	for (auto obstacle = oarsmt.obstacles.begin(); obstacle != oarsmt.obstacles.end(); ++obstacle) {
		cin >> (*obstacle)[0] >> (*obstacle)[1] >> (*obstacle)[2] >> (*obstacle)[3];
	}

	cerr << "init output." << endl;
	Paths paths;

	cerr << "solve." << endl;
	chrono::steady_clock::time_point endTime = chrono::steady_clock::now() + chrono::seconds(secTimeout);
	solveOARSteinerMinTree(paths, oarsmt, [&]() -> bool { return endTime < chrono::steady_clock::now(); }, randSeed);

	cerr << "save output." << endl;
	for (auto p = paths.begin(); p != paths.end(); ++p) {
		cout << (*p)[0][0] << " " << (*p)[0][1];
		for (auto n0 = p->begin(), n1 = n0 + 1; n1 != p->end(); ++n0, ++n1) {
			Coord dx = (*n1)[0] - (*n0)[0];
			if (dx != 0) {
				cout << " x " << dx;
			} else {
				cout << " y " << ((*n1)[1] - (*n0)[1]);
			}
		}
		cout << endl;
	}
	return 0;
}
