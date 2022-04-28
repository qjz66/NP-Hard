#include "DFeedbackVertexSet.h"

#include <random>
#include <iostream>
#include <unordered_set>
#include <unordered_map>


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
	void solve(Nodes& output, DFeedbackVertexSet& input, std::function<bool()> isTimeout, int seed) {
		initRand(seed);

		// TODO: implement your own solver which fills the `output` to replace the following trivial solver.
		// sample solver: remove nodes randomly (the solution can be infeasible).

		//                      +----[ exit before timeout ]
		//                      |
		for (NodeId n = 0; !isTimeout() && (n < 100); ++n) { output.push_back(rand(input.nodeNum)); }
		//                                                                      |
		// [ use the random number generator initialized by the given seed ]----+

		// print some information for debugging.
		cerr << input.nodeNum << '\t' << input.arcNum << endl;
		for (auto n = output.begin(); !isTimeout() && (n != output.end()); ++n) { cerr << *n << endl; }
	}
};

#define MAGIC 1
#if MAGIC
template<typename T,typename IndexType=int>class LoopQueue{public:LoopQueue(){}LoopQueue(IndexType size):len(size),head(0),tail(0),q(size){}void clear(){head=tail=0;}T&front(){return q[head];}T&back(){return q[prevIndex(tail)];}const T&front()const{return q[head];}const T&back()const{return q[prevIndex(tail)];}T&pushBack(){IndexType t=tail;increaseIndex(tail);return q[t];}T&pushFront(){return q[decreaseIndex(head)];}T&pushBack(const T&item){return pushBack()=item;}T&pushFront(const T&item){return pushFront()=item;}void popBack(){decreaseIndex(tail);}void popFront(){increaseIndex(head);}bool empty()const{return(head==tail);}bool full()const{return head==nextIndex(tail);}protected:IndexType&increaseIndex(IndexType&index)const{if((++index)>=len){index=0;}return index;}IndexType&decreaseIndex(IndexType&index)const{if((--index)<0){index+=len;}return index;}IndexType prevIndex(IndexType index)const{return decreaseIndex(index);}IndexType nextIndex(IndexType index)const{return increaseIndex(index);}IndexType len;IndexType head;IndexType tail;std::vector<T>q;};class ConsecutiveIdSet{public:using Index=int;using Item=Index;static constexpr Index InvalidIndex=-1;ConsecutiveIdSet(Index capacity,Item minValue=0):lowerBound(minValue),upperBound(minValue+capacity),itemNum(0),items(capacity),index(capacity,InvalidIndex){}bool isItemExist(Item e)const{return(index[e-lowerBound]!=InvalidIndex);}Item itemAt(Index i)const{return items[i];}Index indexOf(Item e)const{return index[e-lowerBound];}Item back()const{return itemAt(size()-1);}void insert(Item e){items[itemNum]=e;index[e-lowerBound]=itemNum++;}bool tryInsert(Item e){if(isItemExist(e)){return false;}insert(e);return true;}void eraseItem(Item e){Index i=indexOf(e);index[items[--itemNum]-lowerBound]=i;items[i]=items[itemNum];index[e-lowerBound]=InvalidIndex;}void eraseIndex(Index i){Item e=itemAt(i);index[items[--itemNum]-lowerBound]=i;items[i]=items[itemNum];index[e-lowerBound]=InvalidIndex;}Item pop(){Item e=itemAt(--itemNum);index[e-lowerBound]=InvalidIndex;return e;}Index size()const{return itemNum;}bool empty()const{return size()<=0;}void clear(bool sparseData=false){if(sparseData){for(Index i=0;i<itemNum;++i){index[items[i]-lowerBound]=InvalidIndex;}}else{std::fill(index.begin(),index.end(),InvalidIndex);}itemNum=0;}const std::vector<Index>&getIndices()const{return index;}protected:Item lowerBound;Item upperBound;Index itemNum;std::vector<Item>items;std::vector<Index>index;};template<typename Container>void unorderedRemove(Container&container,typename Container::iterator pos){*pos=container.back();	container.pop_back();}template<typename Container>bool containKey(const Container&container,const typename Container::key_type&value){return container.find(value)!=container.end();}template<typename Container,typename T>typename Container::iterator find(Container&container,const T&value){return std::find(container.begin(),container.end(),value);}struct Magic{using ID=long;template<typename T>using Vec=std::vector<T>;template<typename T>using HashSet=std::unordered_set<T>;template<typename Key,typename Value>using HashMap=std::unordered_map<Key,Value>;Vec<ID>mrn;Vec<ID>oni;ID nodeNum;AdjList&adjList;AdjList adjListR;Vec<ID>dominatingNode;Magic(DFeedbackVertexSet&input):nodeNum(input.nodeNum),adjList(input.adjList){}bool shouldSkipNode(ID n)const{return dominatingNode[n]!=n;}ID activeDegree(const AdjList&adjacencyList,ID n)const{return static_cast<ID>(adjacencyList[n].size());}static void reverseGraph(const AdjList&forwardAdjList,AdjList&backwardAdjList){ID nodeNum=static_cast<ID>(forwardAdjList.size());backwardAdjList.resize(nodeNum);for(ID src=0;src<nodeNum;++src){backwardAdjList[src].clear();}for(ID src=0;src<nodeNum;++src){for(auto dst=forwardAdjList[src].begin();dst!=forwardAdjList[src].end();++dst){backwardAdjList[*dst].push_back(src);}}}void eliminateInactiveNodes(){for(ID src=0;src<nodeNum;++src){if(shouldSkipNode(src)){continue;}adjList[src].erase(remove_if(adjList[src].begin(),adjList[src].end(),[this](ID dst){return shouldSkipNode(dst);}),adjList[src].end());}for(ID dst=0;dst<nodeNum;++dst){if(shouldSkipNode(dst)){continue;}adjListR[dst].erase(remove_if(adjListR[dst].begin(),adjListR[dst].end(),[this](ID src){return shouldSkipNode(src);}),adjListR[dst].end());}}ID markZeroDegreeNodes(const AdjList&forwardAdjList,const AdjList&backwardAdjList){ID skipNodeNum=0;Vec<ID>inDegrees(nodeNum,0);for(ID dst=0;dst<nodeNum;++dst){if(!shouldSkipNode(dst)){inDegrees[dst]=activeDegree(backwardAdjList,dst);}}LoopQueue<ID>freeNodes(nodeNum);for(int n=0;n<nodeNum;++n){if(!shouldSkipNode(n)&&(inDegrees[n]<=0)){freeNodes.pushBack(n);}}for(;!freeNodes.empty();++skipNodeNum){int src=freeNodes.front();freeNodes.popFront();dominatingNode[src]=-1;for(auto dst=forwardAdjList[src].begin();dst!=forwardAdjList[src].end();++dst){if(shouldSkipNode(*dst)){continue;}if(--inDegrees[*dst]<=0){freeNodes.pushBack(*dst);}}}return skipNodeNum;}ID mergeOneDegreeNodes(AdjList&forwardAdjList,AdjList&backwardAdjList){ID skipNodeNum=0;struct Merging{HashSet<ID>nodes;bool noLoop;};HashMap<ID,Merging>mergings;for(ID src=0;src<nodeNum;++src){if(shouldSkipNode(src)){continue;}if(containKey(mergings,src)){continue;}HashSet<ID>nodes;ID dst=src;for(;;){if(activeDegree(forwardAdjList,dst)!=1){if(!nodes.empty()){mergings[dst].noLoop=true;}break;}nodes.insert(dst);dst=forwardAdjList[dst][0];if(containKey(nodes,dst)){mergings[dst].noLoop=false;break;}if(containKey(mergings,dst)){break;}}for(auto n=nodes.begin();n!=nodes.end();++n){dominatingNode[*n]=dst;mergings[dst].nodes.insert(*n);}}ConsecutiveIdSet neighbors(nodeNum);for(auto m=mergings.begin();m!=mergings.end();++m){ID dst=m->first;Merging&merging(m->second);if(merging.noLoop){neighbors.clear(true);for(auto i=backwardAdjList[dst].begin();i!=backwardAdjList[dst].end();++i){neighbors.insert(*i);}for(auto d=merging.nodes.begin();d!=merging.nodes.end();++d){for(auto s=backwardAdjList[*d].begin();s!=backwardAdjList[*d].end();++s){if(neighbors.tryInsert(*s)){*find(forwardAdjList[*s],*d)=dst;backwardAdjList[dst].push_back(*s);}else{unorderedRemove(forwardAdjList[*s],find(forwardAdjList[*s],*d));}}}}else{for(auto d=merging.nodes.begin();d!=merging.nodes.end();++d){for(auto s=backwardAdjList[*d].begin();s!=backwardAdjList[*d].end();++s){unorderedRemove(forwardAdjList[*s],find(forwardAdjList[*s],*d));}}dominatingNode[dst]=-2;}skipNodeNum+=static_cast<ID>(merging.nodes.size());}return skipNodeNum;}void rebuild(){static constexpr ID SkippedNode=-1;Vec<ID>newIds(nodeNum,SkippedNode);ID newNodeNum=0;for(ID src=0;src<nodeNum;++src){if(dominatingNode[src]==-2){mrn.push_back(oni[src]);}if(shouldSkipNode(src)){continue;}oni[newNodeNum]=src;newIds[src]=newNodeNum++;}for(ID src=0;src<nodeNum;++src){if(newIds[src]==SkippedNode){continue;}ID s=newIds[src];adjList[s]=move(adjList[src]);auto d=adjList[s].begin();for(auto dst=adjList[s].begin();dst!=adjList[s].end();++dst){if(newIds[*dst]!=SkippedNode){*(d++)=newIds[*dst];}}adjList[s].erase(d,adjList[s].end());}adjList.resize(newNodeNum);nodeNum=newNodeNum;oni.resize(nodeNum);}void play(){reverseGraph(adjList,adjListR);oni.resize(nodeNum);dominatingNode.resize(nodeNum);for(ID n=0;n<nodeNum;++n){oni[n]=n;dominatingNode[n]=n;}ID skipNodeNum=markZeroDegreeNodes(adjList,adjListR);skipNodeNum+=markZeroDegreeNodes(adjListR,adjList);if(skipNodeNum>0){eliminateInactiveNodes();}for(ID d;;){d=mergeOneDegreeNodes(adjList,adjListR);d+=mergeOneDegreeNodes(adjListR,adjList);if(d<=0){break;}skipNodeNum+=d;d=markZeroDegreeNodes(adjList,adjListR);d+=markZeroDegreeNodes(adjListR,adjList);if(d<=0){break;}eliminateInactiveNodes();skipNodeNum+=d;}if(skipNodeNum>0){rebuild();}}};
#endif // MAGIC

// solver.
void solveDFeedbackVertexSet(Nodes& output, DFeedbackVertexSet& input, std::function<bool()> isTimeout, int seed) {
	#if MAGIC
	Magic m(input);
	m.play();
	#endif // MAGIC

	Solver().solve(output, input, isTimeout, seed);

	#if MAGIC
	for (auto n = output.begin(); n != output.end(); ++n) { *n = m.oni[*n]; }
	for (auto n = m.mrn.begin(); n != m.mrn.end(); ++n) { output.push_back(*n); }
	#endif // MAGIC
}

}
