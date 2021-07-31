#include "GraphColoring.h"

#include <chrono>
#include <random>
#include <iostream>


using namespace std;


namespace szx {

class Solver { // sample solver: assign colors randomly (the solution can be infeasible).
	// random number generator.
	mt19937 pseudoRandNumGen;
	void initRand(int seed) { pseudoRandNumGen = mt19937(seed); }
	int rand(int lb, int ub) { return uniform_int_distribution<int>(lb, ub - 1)(pseudoRandNumGen); }
	int rand(int ub) { return uniform_int_distribution<int>(0, ub - 1)(pseudoRandNumGen); }

	// timer.
	using Clock = chrono::steady_clock;
	using TimePoint = Clock::time_point;
	TimePoint endTime;
	void initTimer(long long secTimeout) { endTime = Clock::now() + chrono::seconds(secTimeout); }
	bool isTimeout() { return endTime < Clock::now(); }

public:
	void solve(NodeColors& output, const GraphColoring& input, long long secTimeout, int seed) {
		initRand(seed);
		initTimer(secTimeout);

		//                      +----[ exit before timeout ]
		//                      |
		for (NodeId n = 0; !isTimeout() && (n < input.nodeNum); ++n) { output[n] = rand(input.colorNum); }
		//                                                                           |
		//      [ use the random number generator initialized by the given seed ]----+

		// print some information for debugging.
		cerr << input.nodeNum << '\t' << input.colorNum << endl;
		cerr << "node\tcolor" << endl;
		for (NodeId n = 0; !isTimeout() && (n < input.nodeNum); ++n) { cerr << n << '\t' << output[n] << endl; }
	}
};

// solver.
void solveGraphColoring(NodeColors& output, const GraphColoring& input, long long secTimeout, int seed) {
	// TODO: implement your own solver which fills the `output` to replace the following trivial solver.
	Solver().solve(output, input, secTimeout, seed);
}

}
