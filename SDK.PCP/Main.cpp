#include <iostream>
#include <string>

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
	for (auto n = pc.coverages.begin(); n != pc.coverages.end(); ++n) {
		NodeId coveredNodeNum;
		cin >> coveredNodeNum;
		n->resize(coveredNodeNum);
		for (auto i = n->begin(); i != n->end(); ++i) { cin >> *i; }
	}

	EdgeId minEdgeLenRank;
	EdgeId maxEdgeLenRank;
	cin >> maxEdgeLenRank >> minEdgeLenRank;
	pc.nodesWithDrops.resize(maxEdgeLenRank - minEdgeLenRank);
	for (auto r = pc.nodesWithDrops.begin(); r != pc.nodesWithDrops.end(); ++r) {
		NodeId nodeNumToDrop;
		cin >> nodeNumToDrop;
		r->resize(nodeNumToDrop);
		for (auto i = r->begin(); i != r->end(); ++i) { cin >> *i; }
	}

	cerr << "init output." << endl;
	Centers cs(pc.centerNum);

	cerr << "solve." << endl;
	solvePCenter(cs, pc, secTimeout, randSeed);

	cerr << "save output." << endl;
	for (auto c = cs.begin(); c != cs.end(); ++c) { cout << *c << endl; }
	return 0;
}
