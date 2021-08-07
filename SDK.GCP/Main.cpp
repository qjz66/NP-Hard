#include <iostream>
#include <string>

#include "GraphColoring.h"


using namespace std;
using namespace szx;


int main(int argc, char* argv[]) {
	cerr << "load environment." << endl;
	long long secTimeout = atoll(argv[1]);
	int randSeed = atoi(argv[2]);

	cerr << "load input." << endl;
	GraphColoring gc;
	cin >> gc.nodeNum >> gc.edgeNum >> gc.colorNum;
	gc.edges.resize(gc.edgeNum);
	for (auto edge = gc.edges.begin(); edge != gc.edges.end(); ++edge) { cin >> (*edge)[0] >> (*edge)[1]; }

	cerr << "init output." << endl;
	NodeColors nodeColors(gc.nodeNum);

	cerr << "solve." << endl;
	solveGraphColoring(nodeColors, gc, secTimeout, randSeed);

	cerr << "save output." << endl;
	for (auto color = nodeColors.begin(); color != nodeColors.end(); ++color) { cout << *color << endl; }
	return 0;
}
