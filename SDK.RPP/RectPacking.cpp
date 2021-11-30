#include "RectPacking.h"

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
	void solve(Layout& output, RectPacking& input, long long secTimeout, int seed) {
		initRand(seed);
		initTimer(secTimeout);

		// TODO: implement your own solver which fills the `output` to replace the following trivial solver.
		// sample solver: place rectangles randomly (the solution can be infeasible).

		Coord x = 0;
		//                      +----[ exit before timeout ]
		//                      |
		for (RectId r = 0; !isTimeout() && (r < input.rectNum); ++r) {
			//                    +----[ use the random number generator initialized by the given seed ]
			//                    |
			output[r].rotated = rand(2) & 1;
			output[r].pos[0] = x;
			x += (output[r].rotated ? input.rects[r][1] : input.rects[r][0]);
			output[r].pos[1] = 0;
		}

		// print some information for debugging.
		cerr << input.rectNum << endl;
		cerr << "x\ty\trotated" << endl;
		for (RectId r = 0; !isTimeout() && (r < input.rectNum); ++r) { cerr << output[r].pos[0] << '\t' << output[r].pos[1] << '\t' << output[r].rotated << endl; }
	}
};

// solver.
void solveRectPacking(Layout& output, RectPacking& input, long long secTimeout, int seed) {
	Solver().solve(output, input, secTimeout, seed);
}

}
