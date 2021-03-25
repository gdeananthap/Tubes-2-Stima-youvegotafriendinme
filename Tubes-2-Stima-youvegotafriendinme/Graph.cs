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
            for (int i = 0; i < length; i++)
            {
                if (rawSplit[i] != "")
                {
                    textSplit.Add(rawSplit[i]);
                }
            }
            length = textSplit.Count;
            textSplit.Sort();
            nodeNames = new List<string>();
            nodeNames.Add(textSplit[0]);
            for (int i = 1; i < length; i++)
            {
                if (textSplit[i] != textSplit[i - 1])
                {
                    nodeNames.Add(textSplit[i]);
                }
            }
            nodes = nodeNames.Count;
            nodeIdx = new Dictionary<string, int>();
            for (int i = 0; i < nodes; i++)
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
            for (int i = 0; i < length; i += 2)
            {
                adjacencyList[nodeIdx[textSplit[i]]].Add(nodeIdx[textSplit[i + 1]]);
                adjacencyList[nodeIdx[textSplit[i + 1]]].Add(nodeIdx[textSplit[i]]);
                Tuple<string, string> toAdd = new Tuple<string, string>(textSplit[i], textSplit[i + 1]);
                edges.Add(toAdd);
            }
            for (int i = 0; i < nodes; i++)
            {
                adjacencyList[i].Sort();
            }
        }
        public List<int> getFriendNodes(int from)
        {
            // I.S. from adalah salah satu nodes dari graf
            // F.S. mengembalikan list of ID account yang berhubungan langsung dengan from
            return adjacencyList[from];
        }
        
        public List<string> getNodeNames()
        {
            // I.S. Graph terdefinisi
            // F.S. Mengembalikan list of nama account yang ada di graph
            return nodeNames;
        }

        public List<Tuple<string, string>> getEdges()
        {
            // I.S. Graph terdefinisi
            // F.S. Mengembalikan list of tuple akun-akun yang bersisian di graph
            return edges;
        }
        
        public string[] ExploreFriendBFS(string from, string to)
        {
            int[] path = BFS(nodeIdx[from], nodeIdx[to]);
            int length = path.Length;
            string[] toReturn = new string[length];
            for (int i = 0; i < length; i++)
            {
                toReturn[i] = nodeNames[path[i]];
            }
            return toReturn;
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

                for (int i = 0; i < count_adj; i++)
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
        public string[] ExploreFriendDFS(string from, string to)
        {
            bool[] visitedNodes = new bool[nodes];
            for (int i = 0; i < nodes; i++)
            {
                visitedNodes[i] = false;

            }
            Stack<int> stack = new Stack<int>();
            DFS(nodeIdx[from], nodeIdx[to], ref visitedNodes, stack);
            int[] path = new int[stack.Count];
            string[] path_string = new string[stack.Count];
            if (stack.Count > 0)
            {
                path = stack.ToArray();
                Array.Reverse(path);
                for (int i = 0; i < path.Count(); i++)
                {
                    path_string[i] = nodeNames[path[i]];
                }
                return path_string;
            }
            else
            {
                Console.WriteLine("Tidak ada jalur koneksi yang tersedia \n Anda harus memulai koneksi baru itu sendiri.");
                return path_string;
            }
        }
        public void FriendRecommendationDFS(int from, int depth, List<List<int>> parents)
        {
            int degree = adjacencyList[from].Count;
            if (depth > 1)
            {
                for (int i = 0; i < degree; i++)
                {
                    FriendRecommendationDFS(adjacencyList[from][i], depth - 1, parents);
                }
            }
            else
            {
                for (int i = 0; i < degree; i++)
                {
                    parents[adjacencyList[from][i]].Add(from);
                }
            }
        }
        public string SortedFriendRecommendation(string from, bool isDFS)
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
                FriendRecommendationDFS(nodeIdx[from], 2, parents);

                List<Tuple<int, int>> toSort = new List<Tuple<int, int>>();
                Tuple<int, int> toAdd;
                for (int i = 0; i < nodes; i++)
                {
                    if (i != nodeIdx[from])
                    {
                        toAdd = new Tuple<int, int>(parents[i].Count, -i);
                        toSort.Add(toAdd);
                    }
                }
                toSort.Sort();

                for (int i = nodes - 2; i >= 0 && toSort[i].Item1 > 0; i--)
                {
                    int node = -toSort[i].Item2;
                    Tuple<int, List<int>> addReturn = new Tuple<int, List<int>>(node, parents[node]);
                    toReturn.Add(addReturn);
                }

                int NBRecommend = toReturn.Count;
                int countFriends = 0;
                string toReturn_string = ("Friends recommendation for " + from + " with DFS Algorithm\n");
                foreach (Tuple<int, List<int>> f in toReturn)
                {
                    if (f.Item2.Count > 0 && !adjacencyList[nodeIdx[from]].Contains(f.Item1))
                    {
                        countFriends++;
                        toReturn_string += "Account name: " + nodeNames[f.Item1] + "\n";
                        toReturn_string += f.Item2.Count + " mutual friends:" + "\n";
                        foreach (int acc in f.Item2)
                        {
                            toReturn_string += nodeNames[acc] + "\n";
                        }
                        toReturn_string += "\n";
                    }
                }
                if (countFriends == 0)
                {
                    toReturn_string += "There is no friends recommendation for " + from + " \n";
                }
                return toReturn_string;
            }
            else
            {
                return FriendRecommendationBFS(from);
            }
        }
        public string FriendRecommendationBFS(string from)
        {
            // I.S. from adalah salah satu akun yang ada di graf
            // F.S. Mengembalikan string yang merepresentasikan hasil pencarian Friend Recommendation jika ditemukan.
            // Jika tidak ditemukan, akan mengembalikan pesan kesalahan


            // Inisialisasi variabel-variabel
            Dictionary<string, List<string>> result = new Dictionary<string, List<string>>(); //Map dengan key adalah akun yang memiliki mutual friends dengan from dan valuenya adalah List of mutual friends
            string printResult = "";  // Variabel untuk menyimpan 
            List<int> accessed = new List<int>(); // List untuk menyimpan akun mana saja yang sudah dikunjungi
            List<int> tempFriend = new List<int>(); // List untuk menyimpan akun mana saja yang berteman dengan currentNode
            List<int> masterFriend = new List<int>(); // List untuk menyimpan akun apa saja yang berteman dengan from
            int currentNode; // Akun yang sedang di cek

            masterFriend = getFriendNodes(nodeIdx[from]); 
            List<int> queue = new List<int>(masterFriend); // List untuk menyimpan daftar kandidat akun yang dijadikan friend recommendation
            
            // Iterasi selama antrian tidak kosong
            while (queue.Count > 0)
            {
                // Ambil akun pertama dari antrian
                currentNode = queue[0];
                // Cari teman dari akun currentNode
                tempFriend = getFriendNodes(currentNode);
                if (masterFriend.Contains(currentNode))
                {
                    // Jika currentNode merupakan teman dari from maka tambahkan semua teman currentNode ke queue
                    // jika belum terdaftar pada antrian, belum pernah dikunjungi, dan tidak sama dengan from
                    foreach (int node in tempFriend)
                    {
                        if (!(accessed.Contains(node)) && !(queue.Contains(node)) && (node != nodeIdx[from]))
                        {
                            queue.Add(node);
                        }
                    }

                }
                else
                {
                    // Jika currentNode bukan teman
                    List<string> recommended = new List<string>(); // List untuk menyimpan mutual friends dari currentNode
                    foreach (int node in tempFriend)
                    {
                        if (masterFriend.Contains(node))
                        {
                            // Mutual Friends
                            recommended.Add(nodeNames[node]);
                        }
                        if (!(accessed.Contains(node)) && !(queue.Contains(node)) && (node != nodeIdx[from]))
                        {
                            // Tambahkan ke antrian jika belum terdaftar pada antrian, belum pernah dikunjungi, dan tidak sama dengan from
                            queue.Add(node);
                        }
                    }
                    // Tambahkan ke Map result dengan key currentNode dan value mutual friends
                    result.Add(nodeNames[currentNode], recommended);
                }
                // Tandai bahwa currentNode sudah pernah dikunjungi dan hilangkan dari antrian
                accessed.Add(currentNode);
                queue.Remove(currentNode);
            }
            // {endwhile}

            // Urutkan hasil pencarian berdasarkan banyaknya mutual friends
            var sortedResult = (from val in result orderby val.Value.Count descending select val);
            int countFriends = 0; // Variabel yang menyimpan total teman yang direkomendasikan

            // Formatting result untuk friends recommendation
            printResult += ("Friends recommendation for " + from +" with BFS Algorithm\n");
            foreach (KeyValuePair<string, List<string>> f in sortedResult)
            {
                if (f.Value.Count > 0)
                {
                    countFriends++;
                    printResult += ("Account name: " + f.Key + " \n");
                    printResult += (f.Value.Count.ToString() + " mutual friends:\n");
                    foreach (string acc in f.Value)
                    {
                        printResult += (acc + "\n");
                    }
                    printResult += "\n";
                }
            }
            if (countFriends == 0)
            {
                // Tidak ada rekomendasi teman untuk from
                printResult += "There is no friends recommendation for  " + from + " \n";
            }
            return printResult;
        }
    }
}