#include <iostream>
#include <string>

#include "RectPacking.h"


using namespace std;
using namespace szx;


int main(int argc, char* argv[]) {
	cerr << "load environment." << endl;
	long long secTimeout = atoll(argv[1]);
	int randSeed = atoi(argv[2]);

	cerr << "load input." << endl;
	RectPacking rp;
	cin >> rp.rectNum;
	rp.rects.resize(rp.rectNum);
	for (auto rect = rp.rects.begin(); rect != rp.rects.end(); ++rect) { cin >> (*rect)[0] >> (*rect)[1]; }

	cerr << "init output." << endl;
	Layout layout(rp.rectNum);

	cerr << "solve." << endl;
	solveRectPacking(layout, rp, secTimeout, randSeed);

	cerr << "save output." << endl;
	for (auto placement = layout.begin(); placement != layout.end(); ++placement) {
		cout << placement->pos[0] << ' ' << placement->pos[1] << ' ' << (placement->rotated ? "90" : "0") << endl;
	}
	return 0;
}
