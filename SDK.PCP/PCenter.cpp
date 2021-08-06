#include "PCenter.h"

#include <chrono>
#include <random>
#include <iostream>


using namespace std;


namespace szx {

class Solver { // sample solver: pick center randomly (the solution can be infeasible).
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
	void solve(Centers& output, PCenter& input, long long secTimeout, int seed) {
		initRand(seed);
		initTimer(secTimeout);

		coverAllNodesUnderFixedRadius(output, input, secTimeout, seed);
		for (auto r = input.nodesWithDrops.begin(); !isTimeout() && (r != input.nodesWithDrops.end()); ++r) {
			reduceRadius(input, *r);
			coverAllNodesUnderFixedRadius(output, input, secTimeout, seed);
		}
	}

	void coverAllNodesUnderFixedRadius(Centers& output, PCenter& input, long long secTimeout, int seed) {
		//                      +----[ exit before timeout ]
		//                      |
		for (NodeId n = 0; !isTimeout() && (n < input.centerNum); ++n) { output[n] = rand(input.nodeNum); }
		//                                                                             |
		//        [ use the random number generator initialized by the given seed ]----+

		// print some information for debugging.
		cerr << input.nodeNum << '\t' << input.centerNum << endl;
		for (NodeId n = 0; !isTimeout() && (n < input.centerNum); ++n) { cerr << n << '\t' << output[n] << endl; }
	}

	void reduceRadius(PCenter& input, Nodes nodesWithDrop) {
		for (auto n = nodesWithDrop.begin(); n != nodesWithDrop.end(); ++n) {
			input.coverages[*n].pop_back();
		}
	}
};

// solver.
void solvePCenter(Centers& output, PCenter& input, long long secTimeout, int seed) {
	// TODO: implement your own solver which fills the `output` to replace the following trivial solver.
	Solver().solve(output, input, secTimeout, seed);
}

}
