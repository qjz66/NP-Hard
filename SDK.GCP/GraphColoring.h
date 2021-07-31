////////////////////////////////
/// usage : 1.	SDK for graph coloring solver.
/// 
/// note  : 1.	
////////////////////////////////

#ifndef CN_HUST_SZX_NPBENCHMARK_GRAPH_COLORING_H
#define CN_HUST_SZX_NPBENCHMARK_GRAPH_COLORING_H


#include <array>
#include <vector>


namespace szx {

using NodeId = int;
using EdgeId = NodeId;
using ColorId = NodeId;

using Edge = std::array<NodeId, 2>;

struct GraphColoring {
	NodeId nodeNum;
	EdgeId edgeNum;
	ColorId colorNum;
	std::vector<Edge> edges;
};

using NodeColors = std::vector<ColorId>; // `nodeColors[n]` is the color of node `n`.


void solveGraphColoring(NodeColors& output, const GraphColoring& input, long long secTimeout, int seed);

}


#endif // CN_HUST_SZX_NPBENCHMARK_GRAPH_COLORING_H
