#include "GraphColoring.h"

#include <random>
#include <iostream>


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
	void solve(NodeColors& output, GraphColoring& input, function<long long()> restMilliSec, int seed) {
		initRand(seed);

		// TODO: implement your own solver which fills the `output` to replace the following trivial solver.
		// sample solver: assign colors randomly (the solution can be infeasible).

		//                      +----[ exit before timeout ]
		//                      |
		for (NodeId n = 0; (restMilliSec() > 0) && (n < input.nodeNum); ++n) { output[n] = rand(input.colorNum); }
		//                                                                                  |
		//             [ use the random number generator initialized by the given seed ]----+

		// TODO: the following code in this function is for illustration only and can be deleted.
		// print some information for debugging.
		cerr << input.nodeNum << '\t' << input.colorNum << endl;
		cerr << "node\tcolor" << endl;
		for (NodeId n = 0; (restMilliSec() > 0) && (n < input.nodeNum); ++n) { cerr << n << '\t' << output[n] << endl; }
	}
};

// solver.
void solveGraphColoring(NodeColors& output, GraphColoring& input, function<long long()> restMilliSec, int seed) {
	Solver().solve(output, input, restMilliSec, seed);
}

}
