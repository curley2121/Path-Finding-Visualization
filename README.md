# Path Finding Algorithm Visualizer
Welcome! This is a project I built in Unity, using C#, after finishing my Algorithms course. I wanted to test my knowledge and gain a deeper understanding of the different types of path finding algorithms. The visual aspect of the path finding algorithms was especially interesting and fun to both test and program. I implemented five different path finding algorithms with various efficiencies and desired outcomes. You can learn more about the algorithms below and access the application here: https://curley2121.github.io/Path-Finding-Visualization/

Hope you enjoy!

## Instructions
Simply hold down your mouse button on the grid to add or remove walls. You can click and drag the starting position (white square) and the goal (green square) to move them. Use the drop down menu in the top left corner of the screen to change the algorithm. Simply click "Run" when ready to experience the beauty of path finding.

## Implemented Algorithms

**Dijkstra's algorithm**: 
A path finding algorithm that finds the shortest path from the starting node to every other node on the graph. Dijkstra's algorithm runs in time Θ((|V|+|E|)log|V|) with V and E being the number of vertices and edges respectively. 

**A Star**: 
An informed search algorithm, meaning that it uses a cost function to find the the least costly path in the direction of the goal node. A* is very popular due to its optimal efficiency.

**Breadth First Search**: 
BFS is a search algorithm used for traversing all the nodes of a graph from a given source node. It begins at the starting node and visits each neighboring node at the current depth before visting further nodes. The visualization of BFS looks very similar to Dijkstra's algorithm, but in contrast it doesn't always provide the shortest path.

**Depth-first search**: 
DFS pretty much employs the opposite method as Breadth First Search. It begins at the starting node and explores as far as possible for each given branch. DFS does not provide the shortest path and is very ineffective for Path Finding.

**Best First Search**: 
Much like A*, BFS is another informed search algorithm that visits the most promising neighboring nodes according to a given rule. On average, BFS finds a path quite fast, but does not guarantee a shortest path.

