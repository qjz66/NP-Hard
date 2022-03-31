#include <iostream>
#include <string>
#include <chrono>

#include "PCenter.h"


using namespace std;
using namespace szx;


int main(int argc, char* argv[]) {
	cerr << "load environment." << endl;
	long long secTimeout = atoll(argv[1]);
	int randSeed = atoi(argv[2]);

	cerr << "load input." << endl;
	PCenter pc;
	cin >> pc.nodeNum >> pc.centerNum;
	pc.coverages.resize(pc.nodeNum);
	for (auto coverage = pc.coverages.begin(); coverage != pc.coverages.end(); ++coverage) {
		NodeId coveredNodeNum;
		cin >> coveredNodeNum;
		coverage->resize(coveredNodeNum);
		for (auto node = coverage->begin(); node != coverage->end(); ++node) { cin >> *node; }
	}

	EdgeId minEdgeLenRank;
	EdgeId maxEdgeLenRank;
	cin >> maxEdgeLenRank >> minEdgeLenRank;
	pc.nodesWithDrops.resize(maxEdgeLenRank - minEdgeLenRank);
	for (auto r = pc.nodesWithDrops.begin(); r != pc.nodesWithDrops.end(); ++r) {
		NodeId nodeNumToDrop;
		cin >> nodeNumToDrop;
		r->resize(nodeNumToDrop);
		for (auto node = r->begin(); node != r->end(); ++node) { cin >> *node; }
	}

	cerr << "init output." << endl;
	Centers centers(pc.centerNum);

	cerr << "solve." << endl;
	chrono::steady_clock::time_point endTime = chrono::steady_clock::now() + chrono::seconds(secTimeout);
	solvePCenter(centers, pc, [&]() -> bool { return endTime < chrono::steady_clock::now(); }, randSeed);

	cerr << "save output." << endl;
	for (auto center = centers.begin(); center != centers.end(); ++center) { cout << *center << endl; }
	return 0;
}
