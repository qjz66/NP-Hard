#include <iostream>
#include <string>
#include <chrono>

#include "FJSP.h"


using namespace std;
using namespace szx;


int main(int argc, char* argv[]) {
	cerr << "load environment." << endl;
	long long secTimeout = atoll(argv[1]);
	int randSeed = atoi(argv[2]);

	cerr << "load input." << endl;
	FJSP fjsp;
	cin >> fjsp.jobNum >> fjsp.workerNum >> fjsp.maxCandidateNum;
	fjsp.jobs.resize(fjsp.jobNum);
	for (auto job = fjsp.jobs.begin(); job != fjsp.jobs.end(); ++job) {
		OperationId operationNum;
		cin >> operationNum;
		job->resize(operationNum);
		for (auto op = job->begin(); op != job->end(); ++op) {
			WorkerId candidateNum;
			cin >> candidateNum;
			op->resize(candidateNum);
			for (auto candidate = op->begin(); candidate != op->end(); ++candidate) {
				cin >> candidate->worker >> candidate->duration;
			}
		}
	}

	cerr << "init output." << endl;
	Schedule schedule(fjsp.workerNum);

	cerr << "solve." << endl;
	chrono::steady_clock::time_point endTime = chrono::steady_clock::now() + chrono::seconds(secTimeout);
	solveFJSP(schedule, fjsp, [&]() -> bool { return endTime < chrono::steady_clock::now(); }, randSeed);

	cerr << "save output." << endl;
	for (auto worker = schedule.begin(); worker != schedule.end(); ++worker) {
		cout << worker->size();
		for (auto task = worker->begin(); task != worker->end(); ++task) {
			cout << ' ' << task->job << ' ' << task->operation;
		}
		cout << endl;
	}
	return 0;
}
