using System;
using System.Collections.Generic;
using System.Linq;

namespace Tubes_2_Stima_youvegotafriendinme
{
    public class Graph
    {
        private int nodes;
        private List<List<int>> adjacencyList;
        private List<string> nodeNames;
        private Dictionary<string, int> nodeIdx;
        private List<Tuple<string, string>> edges;

        public Graph(string text)
        {
            //Mengambil dahulu semua nama node
            char[] delimiterChars = { ' ', '\r', '\n' };
            List<string> rawSplit = text.Split(delimiterChars).ToList();
            int length = rawSplit.Count;
            List<string> textSplit = new List<string>();
            for(int i=0; i<length; i++)
            {
                if(rawSplit[i] != "")
                {
                    textSplit.Add(rawSplit[i]);
                }
            }
            length = textSplit.Count;
            textSplit.Sort();
            nodeNames = new List<string>();
            nodeNames.Add(textSplit[0]);
            for(int i=1; i<length; i++)
            {
                if(textSplit[i] != textSplit[i - 1])
                {
                    nodeNames.Add(textSplit[i]);
                }
            }
            nodes = nodeNames.Count;
            nodeIdx = new Dictionary<string, int>();
            for(int i=0; i<nodes; i++)
            {
                nodeIdx.Add(nodeNames[i], i);
            }
            //Inisialisasi adjacency
            adjacencyList = new List<List<int>>();
            for (int i = 0; i < nodes; i++)
            {
                List<int> addList = new List<int>();
                adjacencyList.Add(addList);
            }
            //Menambahkan tiap edge pada graph
            length = rawSplit.Count;
            textSplit = new List<string>();
            for (int i = 0; i < length; i++)
            {
                if (rawSplit[i] != "")
                {
                    textSplit.Add(rawSplit[i]);
                }
            }
            length = textSplit.Count;
            edges = new List<Tuple<string, string>>();
            for (int i=0; i<length; i += 2)
            {
                adjacencyList[nodeIdx[textSplit[i]]].Add(nodeIdx[textSplit[i+1]]);
                adjacencyList[nodeIdx[textSplit[i+1]]].Add(nodeIdx[textSplit[i]]);
                Tuple<string, string> toAdd = new Tuple<string, string>(textSplit[i], textSplit[i + 1]);
                edges.Add(toAdd);
            }
            for(int i=0; i<nodes; i++)
            {
                adjacencyList[i].Sort();
            }
        }
        public List<int> getFriendNodes(int from)
        {
            return adjacencyList[from];
        }
        public List<string> getNodeNames()
        {
            return nodeNames;
        }
        public List<Tuple<string, string>> getEdges()
        {
            return edges;
        }
        public int[] BFS(int from, int to)
        {
            int[] backtrack = new int[nodes];
            int[] depth = new int[nodes];
            for (int i = 0; i < nodes; i++)
            {
                backtrack[i] = -1;
            }
            Queue<int> queue = new Queue<int>();
            int head, degree, visitedNode;
            queue.Enqueue(from);
            backtrack[from] = from;
            depth[from] = 0;
            while (backtrack[to] == -1 && queue.Count > 0)
            {
                head = queue.Dequeue();
                degree = adjacencyList[head].Count;
                for (int i = 0; i < degree; i++)
                {
                    visitedNode = adjacencyList[head][i];
                    if (backtrack[visitedNode] == -1)
                    {
                        backtrack[visitedNode] = head;
                        depth[visitedNode] = depth[head] + 1;
                        queue.Enqueue(visitedNode);
                    }
                }

            }

            if (backtrack[to] != -1)
            {
                int[] path = new int[depth[to] + 1];
                visitedNode = to;
                for (int i = depth[to]; i > 0; i--)
                {
                    path[i] = visitedNode;
                    visitedNode = backtrack[visitedNode];
                }
                path[0] = visitedNode;
                return path;
            }
            else
            {
                int[] path = new int[0];
                return path;
            }
        }
        public void DFS(int current, int to, ref bool[] visitedNodes, Stack<int> stack)
        {
            if (!visitedNodes[current])
            {
                visitedNodes[current] = true;
                stack.Push(current);
                int count_adj = adjacencyList[current].Count();
                int temp;

                for (int i=0; i < count_adj; i++)
                {
                    if (!visitedNodes[to])
                    {
                        temp = adjacencyList[current][i];
                        DFS(temp, to, ref visitedNodes, stack);
                    }
                }
                if (!visitedNodes[to])
                {
                    temp = stack.Pop();
                }
            }

        }
        public int[] ExploreFriendDFS(int from, int to)
        {
            bool[] visitedNodes = new bool[nodes];
            for (int i = 0; i < nodes; i++)
            {
                visitedNodes[i] = false;

            }
                int notTaken;
                int[] path = new int[adjacencyList.Count];
                Stack<int> stack = new Stack<int>();
                DFS(from, to, ref visitedNodes, stack);
                if (stack.Count > 0)
                {
                    path = stack.ToArray();
                    Array.Reverse(path);
                    return path;
                }
                else
                {
                    Console.WriteLine("There is no connection");
                    return path;
                }
        }
        public void FriendRecommendationDFS(int from, int depth, List<List<int>> parents)
        {
            int degree = adjacencyList[from].Count;
            if (depth > 1)
            {
                for(int i=0; i<degree; i++)
                {
                    FriendRecommendationDFS(adjacencyList[from][i], depth - 1, parents);
                }
            }
            else
            {
                for(int i=0; i<degree; i++)
                {
                    parents[adjacencyList[from][i]].Add(from);
                }
            }
        }
        public List<Tuple<int, List<int>>> SortedFriendRecommendation(int from, bool isDFS)
        {
            List<Tuple<int, List<int>>> toReturn = new List<Tuple<int, List<int>>>();
            if (isDFS)
            {
                List<List<int>> parents = new List<List<int>>();
                
                for (int i = 0; i < nodes; i++)
                {
                    List<int> addList = new List<int>();
                    parents.Add(addList);
                }
                FriendRecommendationDFS(from, 2, parents);

                List<Tuple<int, int>> toSort = new List<Tuple<int, int>>();
                Tuple<int, int> toAdd;
                for (int i=0; i<nodes; i++)
                {
                    if (i != from)
                    {
                        toAdd = new Tuple<int, int>(parents[i].Count, -i);
                        toSort.Add(toAdd);
                    }
                }
                toSort.Sort();

                for(int i=nodes-2; i>=0 && toSort[i].Item1>0; i--)
                {
                    int node = -toSort[i].Item2;
                    Tuple<int, List<int>> addReturn = new Tuple<int, List<int>>(node, parents[node]);
                    toReturn.Add(addReturn);
                }
            }
            return toReturn;
        }
        public Dictionary<int, List<int>> FriendRecommendationBFS(int from)
        {
            Dictionary<int, List<int>> result = new Dictionary<int, List<int>>();
            List<int> accessed = new List<int>();
            List<int> tempFriend = new List<int>();
            List<int> masterFriend = new List<int>();
            int currentNode;
            masterFriend = getFriendNodes(from);
            List<int> queue = new List<int>(masterFriend);
            while (queue.Count > 0)
            {
                currentNode = queue[0];
                tempFriend = getFriendNodes(currentNode);
                if (masterFriend.Contains(currentNode))
                {
                    foreach(int node in tempFriend)
                    {
                        if (!(accessed.Contains(node)) && !(queue.Contains(node)) && (node != from))
                        {
                            queue.Add(node);
                        }
                    }
                    
                }
                else
                {
                    List<int> recommended = new List<int>();
                    foreach(int node in tempFriend)
                    {
                        if (masterFriend.Contains(node))
                        {
                            recommended.Add(node);
                        }
                        if (!(accessed.Contains(node)) && !(queue.Contains(node)) && (node != from))
                        {
                            queue.Add(node);
                        }
                    }
                    result.Add(currentNode, recommended);
                }
                accessed.Add(currentNode);
                queue.Remove(currentNode);
            }
           
            return result;
        }
    }
    
}
