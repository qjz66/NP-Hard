#include "DFeedbackVertexSet.h"

#include <random>
#include <iostream>
#include <unordered_set>
#include <unordered_map>


using namespace std;


namespace szx {

class Solver {
	// random number generator.
	mt19937 pseudoRandNumGen;
	void initRand(int seed) { pseudoRandNumGen = mt19937(seed); }
	int fastRand(int lb, int ub) { return (pseudoRandNumGen() % (ub - lb)) + lb; }
	int fastRand(int ub) { return pseudoRandNumGen() % ub; }
	int rand(int lb, int ub) { return uniform_int_distribution<int>(lb, ub - 1)(pseudoRandNumGen); }
	int rand(int ub) { return uniform_int_distribution<int>(0, ub - 1)(pseudoRandNumGen); }

public:
	void solve(Nodes& output, DFeedbackVertexSet& input, function<long long()> restMilliSec, int seed) {
		initRand(seed);

		// TODO: implement your own solver which fills the `output` to replace the following trivial solver.
		// sample solver: remove nodes randomly (the solution can be infeasible).

		//                      +----[ exit before timeout ]
		//                      |
		for (NodeId n = 0; (restMilliSec() > 0) && (n < 100); ++n) { output.push_back(rand(input.nodeNum)); }
		//                                                                             |
		//        [ use the random number generator initialized by the given seed ]----+

		// TODO: the following code in this function is for illustration only and can be deleted.
		// print some information for debugging.
		cerr << input.nodeNum << '\t' << input.arcNum << endl;
		for (auto n = output.begin(); (restMilliSec() > 0) && (n != output.end()); ++n) { cerr << *n << endl; }
	}
};

// solver.
void solveDFeedbackVertexSet(Nodes& output, DFeedbackVertexSet& input, function<long long()> restMilliSec, int seed) {
	Solver().solve(output, input, restMilliSec, seed);
}

}
