# Big O Analysis

## Challenges (Binary Search Tree)

- ### Search / Insert / Delete:

        All of those operations have a time complexity of O(log n) on average when the tree is balanced. For example, in the search method, the algorithm will divide the amount of nodes it has to search by around half, comparing if the searched value is higher or lower than the current node. When the tree is balanced, the algorithm is able to cut off more and more nodes (until half of the nodes) in each operation. But with an unbalanced tree, their operations are closer to O(n) since the algorithm is not able to cut the nodes as much and will have to go through each of them in the worst case.

- ### Rotations

        All rotations in an AVL tree (as the one implemented) have a time complexity of O(1) because the operation is just replacing the relationship of 3 nodes, meaning that regardless of the size of the tree, these operations will take the same amount of time.

## Map (Directed Graph)

- ### Traversal
The time complexity of the traversal of a graph is O(V + E). This is for the BFS, or breadth-first search, but it is the same for the DFS depth-first search. This is because it has to visit all the vertices in a level and then check the edges of that vertex. In other words, the algorithm goes through all vertices once as well as all the edges. O(V + E) is also on the functions for finding the shortest path as it performs a BFS. Also for the display map and remove room because it has to go through each vertex and each node within it.

## Inventory Queue / RoomHistory Stack

- ### Queue
All basic operations in a normal Queues have a time complexity of O(1). Enqueue adds an element to the end of the queue, and it is the last element to come out. Also dequeue and peek only deal with the first element of the qeuue. A queue uses FIFO (First in ,First out)

- ### Stack
Stacks are just different in that they use the LIFO model (Last in, First out). All of its operations are also O(1) because they just involve dealing with the first item. The stack is used to track the room history because the LIFO structure allows you to go back to the previous room by popping and then
