#include <iostream>
#include <string>
#include <chrono>

#include "LatinSquare.h"


using namespace std;
using namespace std::chrono;
using namespace szx;


void loadInput(istream& is, LatinSquare& lsc) {
	is >> lsc.n;
	lsc.fixedNums.reserve(lsc.n * lsc.n);
	for (Assignment a; is >> a.row >> a.col >> a.num; lsc.fixedNums.push_back(a)) {}
}

void saveOutput(ostream& os, Table& assignments) {
	for (auto i = assignments.begin(); i != assignments.end(); ++i) {
		for (auto j = i->begin(); j != i->end(); ++j) { os << *j << '\t'; }
		os << endl;
	}
}

void test(istream& inputStream, ostream& outputStream, long long secTimeout, int randSeed) {
	cerr << "load input." << endl;
	LatinSquare lsc;
	loadInput(inputStream, lsc);

	cerr << "solve." << endl;
	steady_clock::time_point endTime = steady_clock::now() + seconds(secTimeout);
	Table assignments(lsc.n, std::vector<Num>(lsc.n));
	solveLatinSquare(assignments, lsc, [&]() { return duration_cast<milliseconds>(endTime - steady_clock::now()).count(); }, randSeed);

	cerr << "save output." << endl;
	saveOutput(outputStream, assignments);
}
void test(istream& inputStream, ostream& outputStream, long long secTimeout) {
	return test(inputStream, outputStream, secTimeout, static_cast<int>(time(nullptr) + clock()));
}


int main(int argc, char* argv[]) {
	cerr << "load environment." << endl;
	if (argc > 2) {
		long long secTimeout = atoll(argv[1]);
		int randSeed = atoi(argv[2]);
		test(cin, cout, secTimeout, randSeed);
	} else {
		//ifstream ifs("path/to/instance.txt");
		//ofstream ofs("path/to/solution.txt");
		//test(ifs, ofs, 10); // for self-test.
	}
	return 0;
}
