#include "FJSP.h"

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
	void solve(Schedule& output, FJSP& input, long long secTimeout, int seed) {
		initRand(seed);
		initTimer(secTimeout);

		// TODO: implement your own solver which fills the `output` to replace the following trivial solver.
		// sample solver: assign tasks randomly (the solution can be infeasible).

		//                      +----[ exit before timeout ]
		//                      |
		for (JobId j = 0; !isTimeout() && (j < input.jobNum); ++j) {
			OperationId opNum = static_cast<OperationId>(input.jobs[j].size());
			for (OperationId o = 0; o < opNum; ++o) {
				WorkerId candidateNum = static_cast<WorkerId>(input.jobs[j][o].size());
				//                                   +----[ use the random number generator initialized by the given seed ]
				//                                   |
				WorkerId w = input.jobs[j][o][rand(candidateNum)].worker;
				output[w].push_back({ j, o });
			}
		}

		// print some information for debugging.
		cerr << input.jobNum << '\t' << input.workerNum << '\t' << input.maxCandidateNum << endl;
		cerr << "job\toperation\tworker" << endl;
		for (WorkerId w = 0; !isTimeout() && (w < input.workerNum); ++w) {
			for (auto t = output[w].begin(); t != output[w].end(); ++t) {
				cerr << t->job << '\t' << t->operation << '\t' << w << endl;
			}
		}
	}
};

// solver.
void solveFJSP(Schedule& output, FJSP& input, long long secTimeout, int seed) {
	Solver().solve(output, input, secTimeout, seed);
}

}
