////////////////////////////////
/// usage : 1.	SDK for routing and wavelength assignment solver.
/// 
/// note  : 1.	
////////////////////////////////

#ifndef CN_HUST_SZX_NPBENCHMARK_RWA_H
#define CN_HUST_SZX_NPBENCHMARK_RWA_H


#include <array>
#include <vector>


namespace szx {

using NodeId = int;
using VehicleId = int;
using Coord = double;
using Capacity = int;
using Time = int;

struct Node2d {
	std::array<Coord, 2> coords;
	Capacity demand;
	Time minStayTime;
	Time windowBegin;
	Time windowEnd;
};

struct VRPTW2d {
	NodeId nodeNum;
	VehicleId maxVehicleNum;
	Capacity vehicleCapacity;
	std::vector<Node2d> nodes;
};

struct Route {
	std::vector<NodeId> nodes; // `nodes[n]` is the `n`th node in the route.
};

using Routes = std::vector<Route>; // `Routes[v]` is the route for vehicle `v`.


void solveVRPTW2d(Routes& output, VRPTW2d& input, long long secTimeout, int seed);

}


#endif // CN_HUST_SZX_NPBENCHMARK_RWA_H
