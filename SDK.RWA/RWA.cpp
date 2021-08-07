#include "RWA.h"

#include <chrono>
#include <random>
#include <iostream>


using namespace std;


namespace szx {

class Solver {
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
	void solve(Routes& output, RWA& input, long long secTimeout, int seed) {
		initRand(seed);
		initTimer(secTimeout);

		// TODO: implement your own solver which fills the `output` to replace the following trivial solver.
		// sample solver: assign wavelengths and find routes randomly (the solution can be infeasible).

		//                      +----[ exit before timeout ]
		//                      |
		for (NodeId t = 0; !isTimeout() && (t < input.trafficNum); ++t) { output[t].wavelen = rand(input.trafficNum); }
		//                                                                                      |
		//                 [ use the random number generator initialized by the given seed ]----+

		for (NodeId t = 0; !isTimeout() && (t < input.trafficNum); ++t) {
			output[t].nodes.resize(rand(input.nodeNum));
			for (auto n = output[t].nodes.begin(); n != output[t].nodes.end(); ++n) {
				*n = rand(input.trafficNum);
			}
		}

		// print some information for debugging.
		cerr << input.nodeNum << '\t' << input.arcNum << '\t' << input.trafficNum << endl;
		cerr << "traffic\twavelen" << endl;
		for (NodeId n = 0; !isTimeout() && (n < input.nodeNum); ++n) { cerr << n << '\t' << output[n].wavelen << endl; }
	}
};

// solver.
void solveRWA(Routes& output, RWA& input, long long secTimeout, int seed) {
	Solver().solve(output, input, secTimeout, seed);
}

}
