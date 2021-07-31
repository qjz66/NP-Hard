#include <iostream>
#include <string>

#include "GraphColoring.h"


using namespace std;
using namespace szx;


int main(int argc, char *argv[]) {
	cerr << "load environment." << endl;
	long long secTimeout = atoll(argv[1]);
	int randSeed = atoi(argv[2]);

	cerr << "load input." << endl;
	GraphColoring gc;
	cin >> gc.nodeNum >> gc.edgeNum >> gc.colorNum;
	gc.edges.resize(gc.edgeNum);
	for (EdgeId e = 0; e < gc.edgeNum; ++e) { cin >> gc.edges[e][0] >> gc.edges[e][1]; }

	cerr << "init output." << endl;
	NodeColors nc(gc.nodeNum);

	cerr << "solve." << endl;
	solveGraphColoring(nc, gc, secTimeout, randSeed);

	cerr << "save output." << endl;
	for (NodeId n = 0; n < gc.nodeNum; ++n) { cout << nc[n] << ' '; }
	return 0;
}
