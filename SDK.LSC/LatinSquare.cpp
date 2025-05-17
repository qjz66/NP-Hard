#include "LatinSquare.h"
#include <unordered_set>
#include <random>
#include <iostream>
#include <cstring>


using namespace std;


namespace szx {

class Solver {
	// variables
	typedef pair<int,bool> P;
	vector<vector<P>> Graph; 
	int bestSol = -1;
	int alpha = 20;
	int poolSize = 10;
	int minCL = INT32_MAX;//record min CL of solutions
	int start = 0;
	struct minMax{
		int min;
		int max;
	};
	float theta = 0.2;

	// random number generator.
	mt19937 pseudoRandNumGen;
	void initRand(int seed) { pseudoRandNumGen = mt19937(seed); }
	int fastRand(int lb, int ub) { return (pseudoRandNumGen() % (ub - lb)) + lb; }
	int fastRand(int ub) { return pseudoRandNumGen() % ub; }
	int rand(int lb, int ub) { return uniform_int_distribution<int>(lb, ub - 1)(pseudoRandNumGen); }
	int rand(int ub) { return uniform_int_distribution<int>(0, ub - 1)(pseudoRandNumGen); }
	
	// color class available for v
	vector<vector<unordered_set<int>>> colorClass; 
	// vertexs
	unordered_set<int> vertexs;
	// solutions
	vector<vector<vector<int>>> solutions;
	// state
	vector<vector<vector<int>>> states;
	// conficts record CL of solutions
	vector<int> Cl;
	//tabuList 
	vector<vector<int>> tabuList;

public:
	void init(const int& n, LatinSquare& input) {
		int len = n*n;
		Graph.resize(n);
		for (int i = 0; i < n; ++i) {
			Graph[i].resize(n);
			for (int j = 0; j < n; ++j) {
				Graph[i][j].first = -1;
				Graph[i][j].second = false;
			}
		}
		//init vertexs
		for(int i = 0;i < n;i++){
			for(int j = 0;j < n;j++){
				vertexs.insert(i*n + j);
			}
		}
		colorClass.resize(n);
		for(int i = 0;i < n;i++){
			for(int j = 0;j < n;j++){
				colorClass[i].resize(n);
				
				for(int k = 0;k < n;k++){
					colorClass[i][j].insert(k);
				}
			}
			
		}
		for(auto k = input.fixedNums.begin(); k != input.fixedNums.end(); ++k) {
			Num row = k->row;
			Num col = k->col;
			vertexs.erase(row*n+col);
			Graph[row][col].first = k->num;
			Graph[row][col].second = true;
			for(int i = 0;i < n;i++){
				
				colorClass[i][col].erase(k->num);
				colorClass[row][i].erase(k->num);
			}
		}
		tabuList.resize(len);
		for(int i = 0;i < len;i++){
			tabuList[i].resize(n);
			for(int j = 0;j < n;j++){
				tabuList[i][j] = 0;
			}
		}
		
	}

	// matchRule: return corlor if v match reduction rule, if not return -1
	int matchRule(const int& v, const int& n){
		unordered_set<int> colorSet;
		int row = v/n, col = v % n;
		//judge rule1
		if(colorClass[row][col].size() == 1){
			return *colorClass[row][col].begin();
		}
		//judge rule2
		colorSet = colorClass[row][col];
				for(int k = 0;k < n;k++){
					if(row == k){
						continue;
					}
					unordered_set<int> tmpSet = colorClass[k][col];
					auto it = tmpSet.begin();
					while(it != tmpSet.end()){
						colorSet.erase(*it);
						it++;
					}
				}
		if(colorSet.size() == 1){
			return *colorSet.begin();
		}
		//judge rule3
		colorSet = colorClass[row][col];
				for(int k = 0;k < n;k++){
					if(col == k){
						continue;
					}
					unordered_set<int> tmpSet = colorClass[row][k];
					auto it = tmpSet.begin();
					while(it != tmpSet.end()){
						colorSet.erase(*it);
						it++;
					}
				}
		if(colorSet.size() == 1){
			return *colorSet.begin();
		}
		return -1;
	}

	// construct return a reducted initial solution
	void construct(vector<vector<int>>& sol, int n, vector<Assignment>& fixedNums){
		/*修改逻辑为，每次从candSet抽取一个随机点，如果满足reduction规则分配颜色*/
		/*不满足则从colorClass随机选取*/

		unordered_set<int> candSet = vertexs;
		
		for(int i = 0;i < fixedNums.size();i++){
			int row = fixedNums[i].row, col = fixedNums[i].col;
			sol[row][col] = fixedNums[i].num;
		}
		while(candSet.size()){
			auto it = candSet.begin();
			// select a random v from candSet
			advance(it,fastRand(candSet.size()));
			int v = *it, row = v/n, col = v%n, color;
			color = matchRule(v,n);
			if(color != -1){
				// remove v from candSet
				candSet.erase(it);
				sol[row][col] = color;
				// remove color from neighbors' color class
				for(int i = 0;i < n;i++){
					if(i != row)
					colorClass[i][col].erase(color);
					if(i != col)
					colorClass[row][i].erase(color);
				}
			}else{
				// select a random color from color class for v
				auto iter = colorClass[row][col].begin();
				advance(iter, fastRand(colorClass[row][col].size()));
				int color1 = *iter;
				sol[row][col] = color1;
				candSet.erase(it);
				// remove color from neighbors' color class
				for(int i = 0;i < n;i++){
					if(i != row)
					colorClass[i][col].erase(color);
					if(i != col)
					colorClass[row][i].erase(color);
				}
			}
		}
	}

	// move find and make a move
	int move(vector<vector<int>>& sol, vector<vector<int>>& cScore, vector<vector<int>>& state,vector<vector<int>>& nbColor, vector<vector<int>>& tabuList,int n, int iter, const int& CL){
		//TODO: find best pScore and cScore
		int changeV = -1, changeC = -1;//changeV.color -> changeC
		int bestP = INT32_MIN;
		//traverse all non-fixed vertexs
		for(int i = 0;i < n*n;i++){
			int row = i/n, col = i%n, cCorlor = sol[row][col];
			if(Graph[row][col].second){
				continue;
			}
			//traverse all colors in colorClass
			for(int j = 0;j < n;j++){
				if(cCorlor == j || colorClass[row][col].find(j) == colorClass[row][col].end() || tabuList[i][j] > iter){
					continue;
				}
				//compute pScore
				int tmpDelta;
				tmpDelta = nbColor[i][cCorlor] - nbColor[i][j];
				if(tmpDelta > bestP){
					bestP = tmpDelta;
					changeV = i;
					changeC = j;
				}else if(tmpDelta == bestP){
					if(cScore[row][col] > cScore[changeV/n][changeV%n]){
						// pScore is same, by cScore
						changeV = i;
						changeC = j;
					}
				}
			}
		}
		//TODO: make move by changing nbCorlor、cScore、tabuList、state
		if(changeV == -1 || changeC == -1){
			return 0;
		}
		
		int row = changeV/n, col = changeV%n;
		int color = sol[row][col];

		for(int i = 0;i < n;i++){
			if(i == col){
				continue;
			}
			int tmpV = row*n + i;
			nbColor[tmpV][color]--;
			nbColor[tmpV][changeC]++;
		}
		for(int i = 0;i < n;i++){
			if(i == row){
				continue;
			}
			int tmpV = i*n + col;
			nbColor[tmpV][color]--;
			nbColor[tmpV][changeC]++;
		}

		sol[row][col] = changeC;

		//change cScore
		for(int i = 0;i < n;i++){
			for(int j = 0;j < n;j++){
				int color1 = sol[i][j];
				// change cScore of the same row
				for(int col1 = 0;col1 < n;col1++){
					if(col1 == j){
						continue;
					}
					if(sol[i][col1] == sol[i][j]){
						cScore[i][col1]++;
						cScore[i][j]++;
						
					}
				}
				// change cScore of the same col
				for(int row1 = 0;row1 < n;row1++){
					if(row1 == i){
						continue;
					}
					if(sol[row1][j] == sol[i][j]){
						cScore[row1][j]++;
						cScore[i][j]++;
						
					}
				}
			}
		}
		cScore[row][col] = 0;
		for(int i = 0;i < n;i++){
			int v1 = row*n + i;
			int v2 = i*n + col;
			if(!nbColor[v1][color] && sol[row][i] == color){
				cScore[row][i] = 0;
			}
			if(!nbColor[v2][color] && sol[i][col] == color){
				cScore[i][col] = 0;
			}
		}
		tabuList[changeV][color] = iter + fastRand(10) + 0.6 * CL;
		state[row][col] = iter;
		iter++;
		return bestP;
	}
	
	// addSol: add a solution to pool
	void addSol(const vector<vector<int>>& sol, const int& CL, const vector<vector<int>>& state){
		solutions.push_back(sol);
		Cl.push_back(CL);
		states.push_back(state);
		return ;
	}
	
	// clearPool: remove all solutions in pool
	void clearPool(){
		solutions.clear();
		Cl.clear();
		states.clear();
		return ;
	}

	//copyState: copy state from src to dest
	void copyState(const vector<vector<int>>& src, vector<vector<int>>& dest){
	
		for(int i = 0;i < dest.size();i++){
			for(int j = 0;j < dest[i].size();j++){
				dest[i][j] = src[i][j];
			}
		}
	}

	//removeOldest: remove the oldest solution in pool
	void removeOldest(){
		solutions.erase(solutions.begin());
		Cl.erase(Cl.begin());
		states.erase(states.begin());
	}

	minMax getBound(){
		minMax item;
		item.min = INT32_MAX;
		item.max = INT32_MIN;
		return item;
	}

	// perturb :get a solution from pool
	void perturb(const vector<vector<int>>& sol, const int& CL, const vector<vector<int>>& state, vector<vector<int>>& cSol, int& cCL, vector<vector<int>>& cState){
		int len = solutions.size(), solIndex, n = state.size();
		vector<vector<int>> res(n, vector<int>(n));
		if(len == 0){
			//add solution to pool
			addSol(sol, CL, state);
			copySol(sol, cSol);
			copyState(state, cState);
			cCL = CL;
		}else if(CL < minCL){
			//remove all solutions in pool and add sol to pool
			clearPool();
			addSol(sol, CL, state);
		}else{
			if((solIndex = isSimilar(sol)) != -1){
			//copy state from pool
			copyState(state, states[solIndex]);
			int index = fastRand(len);
			copySol(solutions[index], cSol);
			cCL = Cl[index];
			copyState(states[index], cState);
		}else {
			if(len < poolSize){
			//add solution to pool
			addSol(sol, CL, state);
			}else{
				//remove the oldest solution in pool and add sol to pool'
				removeOldest();
				addSol(sol, CL, state);
			}
			copySol(sol, cSol);
			copyState(state, cState);
			cCL = CL;
		}
			
		}
		minMax s = getBound();
		int min = s.min, max = s.max;
		float RCL = (max - min) * theta + min;
		unordered_set<int> C;
		for(int i = 0;i < n;i++){
			for(int j = 0;j < n;j++){
				if(state[i][j] < int(RCL)){
					C.insert(i*n+j);
				}
			}
		}
		//update color class
		updateCorlorClass(cSol);
		int cnt = C.size()/2;
		while(cnt){
			// select a random v from C
			int index = fastRand(C.size());
			C.erase(index);
			int row = index/n, col = index%n;
			int color = fastRand(colorClass[row][col].size());
			cSol[row][col] = color;
			cnt--;
		}
	}

	//isSimilar return solution's index if csol is similar to a solution in pool,if not return -1
	int isSimilar(const vector<vector<int>>& bSol){
		for(int i = 0;i < solutions.size();i++){
			vector<vector<int>> sol = solutions[i];
			//judge whether two solutions are similiar
			if(judge(sol, bSol)){
				return i;
			}
		}
		return -1;
	}

	bool judge(vector<vector<int>>& sol, const vector<vector<int>>& bSol){
		for(int j = 0;j < sol.size();j++){
				int n = sol[j].size();
				for(int k = 0;k < n;k++){
					for(int l = 0;l < n;l++){
						if(sol[j][k] == sol[j][l] && k != l){
							if(bSol[j][k] != bSol[j][l] || bSol[j][k] != sol[j][k]){
								return false;
							}
						}
					}
				}
			}
			return true;
	}


	// getResult
	void copySol(const vector<vector<int>>& src, vector<vector<int>>& dest){
		for(int i = 0;i < src.size();i++){
			for(int j = 0;j < src[i].size();j++){
				dest[i][j] = src[i][j];
			}
		}
	}

	void updateCorlorClass(const vector<vector<int>>& sol){
		int n = sol.size();
		for(int i = 0;i < n;i++){
			for(int j = 0;j < n;j++){
				colorClass[i][j].clear();
				for(int k = 0;k < n;k++){
					colorClass[i][j].insert(k);
				}
			}
		}
		for(int i = 0;i < n;i++){
			for(int j = 0;j < n;j++){
				for(int k = 0;k < n;k++){
					int color = sol[i][k];
					colorClass[i][j].erase(color);
					color = sol[k][j];
					colorClass[i][j].erase(color);
				}
			}
		}
	}

	// calCL calculate CL of solution
	int calCL(const vector<vector<int>>& sol){
		int CL = 0, len = sol.size();
		for(int i = 0;i < len;i++){
			for(int j = 0;j < len;j++){
				for(int k = 0;k < len;k++){
					
					if(sol[i][j] == sol[i][k] && j != k){
						CL++;
					}
					if(sol[i][j] == sol[k][j] && i!= k){
						CL++;
					}
				}
			}
		}
		return CL;
	}

	//printSol print solution
	void printSol(const vector<vector<int>>& sol){
		cout << "/***********************move********************/" << endl;
		for(int i = 0;i < sol.size();i++){
			for(int j = 0;j < sol[i].size();j++){
				cout << sol[i][j] << " ";
			}
			cout << endl;
		}
	}

	void solve(Table& output, LatinSquare& input, function<long long()> restMilliSec, int seed) {
		int n = input.n, iter = 0, CLB, CLC;
		vector<vector<int>> cSol(n, vector<int>(n)), bSol(n, vector<int>(n)), cState(n, vector<int>(n, 0)), bState(n, vector<int>(n, 0)), cScore(n, vector<int>(n,0));
		vector<vector<int>> nbColor(n*n, vector<int>(n,0)); //nbColor record neibours' color num
		initRand(seed);
		init(input.n, input);
		construct(cSol,n,input.fixedNums);

		//printSol(cSol);

		for(int i = 0;i < n*n;i++){
			int row = i / n, col = i % n;
			int color = cSol[row][col];
			
			for(int j = 0;j < n;j++){
				if(j == col){
					continue;
				}
				int tmpV = row*n + j;
				nbColor[tmpV][color]++;
				if(j == row){
					continue;
				}
				tmpV = j*n + col;
				nbColor[tmpV][color]++;
			}
		}

		//calculate CL of b and c
		CLC = CLB = calCL(cSol);

		// TODO: implement your own solver which fills the `output` to replace the following trivial solver.
		// sample solver: assign colors randomly (the solution can be infeasible).

		//                      +----[ exit before timeout ]
		//					    |
		while(restMilliSec() > 0){
			int depth = 0, cCL = INT32_MAX;
			while(depth < alpha){
				//TODO: select move by pscore and cscore
				int delta = move(cSol, cScore, cState, nbColor, tabuList, n, iter, CLC);//TODO: change move function,get CL of c, return delta
				//printSol(cSol);
				CLC -= delta;
				if(CLC < CLB){
					//copy cSol to bSol
					copySol(cSol, bSol);
					//copy state
					copyState(cState, bState);
					CLB = CLC;
				}
			
				if(!CLB){
					copySol(output, cSol);
					return;
				}
				//TODO: implement Perturb
				depth++;
			}
			perturb(bSol, CLB, bState, cSol, cCL, cState);
			printSol(cSol);
		}//                     |
		//                    	+----[ use the random number generator initialized by the given seed ]
		//for (auto k = input.fixedNums.begin(); k != input.fixedNums.end(); ++k) {
		//	output[k->row][k->col] = k->num;
		//}

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
