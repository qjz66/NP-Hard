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

using RectId = int;
using Coord = int;

using Rect = std::array<Coord, 2>; // `Rect[0]` is width (dx) and `Rect[1]` is height (dy).
using Coords = std::array<Coord, 2>;

struct RectPacking {
	RectId rectNum;
	std::vector<Rect> rects;
};

struct Placement {
	Coords pos; // coords of the left bottom corner.
	bool rotated; // rotate 90 degrees.
};
using Layout = std::vector<Placement>; // `Layout[r]` is the placement of rectangle `r`.


void solveRectPacking(Layout& output, RectPacking& input, long long secTimeout, int seed);

}


#endif // CN_HUST_SZX_NPBENCHMARK_GRAPH_COLORING_H
