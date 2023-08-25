#include "LatinSquare.h"

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
	void solve(Table& output, LatinSquare& input, function<long long()> restMilliSec, int seed) {
		initRand(seed);

		// TODO: implement your own solver which fills the `output` to replace the following trivial solver.
		// sample solver: assign colors randomly (the solution can be infeasible).

		//                      +----[ exit before timeout ]
		//                      |
		for (Num i = 0; (restMilliSec() > 0) && (i < input.n); ++i) {
			for (Num j = 0; j < input.n; ++j) {
				output[i][j] = rand(input.n);
			} //                |
		} //                    +----[ use the random number generator initialized by the given seed ]
		for (auto k = input.fixedNums.begin(); k != input.fixedNums.end(); ++k) {
			output[k->row][k->col] = k->num;
		}

		// TODO: the following code in this function is for illustration only and can be deleted.
		// print some information for debugging.
		cerr << input.n << endl;
		for (Num i = 0; (restMilliSec() > 0) && (i < input.n); ++i) { cerr << i << '\t' << output[i][i] << endl; }
	}
};

// solver.
void solveLatinSquare(Table& output, LatinSquare& input, function<long long()> restMilliSec, int seed) {
	Solver().solve(output, input, restMilliSec, seed);
}

}
