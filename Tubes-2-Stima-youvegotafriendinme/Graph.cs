using System;
using System.Collections.Generic;
using System.Linq;

namespace Tubes_2_Stima
{
    public class Graph
    {
        private int nodes;
        private List<List<int>> adjacencyList;
        private List<string> nodeNames;
        private Dictionary<string, int> nodeIdx;
        public Graph(string text)
        {
            //Mengambil dahulu semua nama node
            char[] delimiterChars = { ' ', '\n' };
            List<string> textSplit = text.Split(delimiterChars).ToList();
            textSplit.Sort();
            int length = textSplit.Count;
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
            textSplit = text.Split(delimiterChars).ToList();
            for(int i=0; i<length; i += 2)
            {
                adjacencyList[nodeIdx[textSplit[i]]].Add(nodeIdx[textSplit[i+1]]);
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
        public void AddEdge(int node1, int node2)
        {
            adjacencyList[node1].Add(node2);
            adjacencyList[node2].Add(node1);
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
    class Program
    {
        static void Main(string[] args)
        {
            Graph Friend = new Graph("A B\nA C\nA D\nB C\nB E\nB F\nC F\nC G\nD G\nD F\nE H\nE F\nF H");
            int[] path = Friend.BFS(0, 7);
            Console.WriteLine("[{0}]", string.Join(", ", path));
            path = Friend.ExploreFriendDFS(0, 7);
            Console.WriteLine("[{0}]", string.Join(", ", path));
            /*
            Graph Friend = new Graph(8);
            Friend.AddEdge(0, 1);
            Friend.AddEdge(0, 2);
            Friend.AddEdge(0, 3);
            Friend.AddEdge(1, 2);
            Friend.AddEdge(1, 4);
            Friend.AddEdge(1, 5);
            Friend.AddEdge(2, 5);
            Friend.AddEdge(2, 6);
            Friend.AddEdge(3, 5);
            Friend.AddEdge(3, 6);
            Friend.AddEdge(4, 5);
            Friend.AddEdge(4, 7);
            Friend.AddEdge(5, 7);
            Dictionary<int, List<int>> friends = Friend.FriendRecommendationBFS(0);
            var sortedResult = (from val in friends orderby val.Value.Count descending select val);
            Console.WriteLine("Daftar rekomendasi teman untuk {0}", 0);
            foreach (KeyValuePair<int, List<int>> f in sortedResult)
            {
                if (f.Value.Count > 0)
                {
                    Console.WriteLine("Nama Akun: {0}", f.Key);
                    Console.WriteLine("{0} mutual friends:", f.Value.Count);
                    foreach(int acc in f.Value)
                    {
                        Console.WriteLine("{0}", acc);
                    }
                   
                }
            }
            int[] path = Friend.BFS(0, 7);
            Console.WriteLine("[{0}]", string.Join(", ", path));
            List<Tuple<int, List<int>>> FriendRecommendation = Friend.SortedFriendRecommendation(0, true);
            int NBRecommend = FriendRecommendation.Count;
            for (int i=0; i<NBRecommend; i++)
            {
                Console.WriteLine("Nama Akun {0}: [{1}]", FriendRecommendation[i].Item1, string.Join(", ", FriendRecommendation[i].Item2));
            }
            */
        }
    }
}
