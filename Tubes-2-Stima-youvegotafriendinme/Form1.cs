using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Tubes_2_Stima_youvegotafriendinme
{

    
    public partial class Form1 : Form
    {
        string filename;
        string filecontent;
        string accountPicked="";
        Graph Friends;
        Microsoft.Msagl.Drawing.Graph graph;
        Dictionary<string, Dictionary<string, Microsoft.Msagl.Drawing.Edge>> graphEdges;
        Microsoft.Msagl.GraphViewerGdi.GViewer viewer;
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            if(openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                label11.Text = openFileDialog1.SafeFileName;
                filename = openFileDialog1.FileName;
                filecontent = File.ReadAllText(filename);
                richTextBox1.Text = "";
                Friends = new Graph(filecontent);
                List<string> nodeNames = Friends.getNodeNames();
                for(int i=0; i<nodeNames.Count; i++)
                {
                    comboBox1.Items.Add(nodeNames[i]);
                    comboBox2.Items.Add(nodeNames[i]);
                }
                viewer = new Microsoft.Msagl.GraphViewerGdi.GViewer();
                //create a graph object 
                //create the graph content 
                graph = new Microsoft.Msagl.Drawing.Graph("graph");
                List<Tuple<string, string>> edges = Friends.getEdges();
                graphEdges = new Dictionary<string, Dictionary<string, Microsoft.Msagl.Drawing.Edge>>();
                for(int i=0; i<nodeNames.Count; i++)
                {
                    Dictionary<string, Microsoft.Msagl.Drawing.Edge> toAdd = new Dictionary<string, Microsoft.Msagl.Drawing.Edge>();
                    graphEdges.Add(nodeNames[i], toAdd);
                }
                string node1, node2;
                for(int i=0; i<edges.Count; i++)
                {
                    node1 = edges[i].Item1;
                    node2 = edges[i].Item2;
                    graphEdges[node1].Add(node2, graph.AddEdge(node1, node2));
                    graphEdges[node1][node2].Attr.ArrowheadAtTarget = Microsoft.Msagl.Drawing.ArrowStyle.None;
                }
                /*graph.AddEdge("B", "C");
                graph.AddEdge("A", C).Attr.Color = Microsoft.Msagl.Drawing.Color.Green;

                graph.FindNode("A").Attr.FillColor = Microsoft.Msagl.Drawing.Color.Magenta;
                graph.FindNode("B").Attr.FillColor = Microsoft.Msagl.Drawing.Color.MistyRose;
                Microsoft.Msagl.Drawing.Node c = graph.FindNode("C");
                c.Attr.FillColor = Microsoft.Msagl.Drawing.Color.PaleGreen;
                c.Attr.Shape = Microsoft.Msagl.Drawing.Shape.Diamond;
                */
                //bind the graph to the viewer 
                viewer.Graph = graph;
                //associate the viewer with the form 
                //form.SuspendLayout();
                viewer.Dock = System.Windows.Forms.DockStyle.Fill;
                panel11.Controls.Add(viewer);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            RadioButton features = panel8.Controls.OfType<RadioButton>().FirstOrDefault(r => r.Checked);
            RadioButton algorithm = panel12.Controls.OfType<RadioButton>().FirstOrDefault(r => r.Checked);
            if(features.Text == "Friend Recommendation")
            {
                if(comboBox1.Text != "")
                {
                    if(algorithm.Text == "Depth First Search (DFS)")
                    {
                        richTextBox1.Text = Friends.SortedFriendRecommendation(comboBox1.Text,true);
                    }
                    else
                    {
                        richTextBox1.Text = Friends.FriendRecommendationBFS(comboBox1.Text);
                    }
                }
            }
            else { 
                if(comboBox1.Text != "" && comboBox2.Text != "")
                {
                    if (algorithm.Text == "Depth First Search (DFS)")
                    {
                        panel11.Controls.Remove(viewer);
                        richTextBox1.Text = "Explore friends with DFS from account " + comboBox1.Text + " to account " + comboBox2.Text + "\r\n";
                        refreshGraph();
                        string[] path = Friends.ExploreFriendDFS(comboBox1.Text, comboBox2.Text);
                        if (path.Length > 0)
                        {
                            int degree = path.Length - 2;
                            if (degree == 0)
                            {
                                richTextBox1.Text += "Both of them are already friends\n";
                                panel11.Controls.Add(viewer);
                            }
                            else
                            {
                                if (degree % 10 == 1 && !(degree > 10 && degree < 20))
                                {
                                    richTextBox1.Text += degree.ToString() + "st-degree-connection\n";
                                }
                                else if (degree % 10 == 2 && !(degree > 10 && degree < 20))
                                {
                                    richTextBox1.Text += degree.ToString() + "nd-degree-connection\n";
                                }
                                else if (degree % 10 == 3 && !(degree > 10 && degree < 20))
                                {
                                    richTextBox1.Text += degree.ToString() + "rd-degree-connection\n";
                                }
                                else
                                {
                                    richTextBox1.Text += degree.ToString() + "th-degree-connection\n";
                                }
                                richTextBox1.Text += path[0] + " -> ";
                                for (int i = 1; i < path.Length; i++)
                                {
                                    richTextBox1.Text += path[i];
                                    if (i != path.Length - 1)
                                    {
                                        richTextBox1.Text += " -> ";
                                    }
                                    if (graphEdges[path[i - 1]].ContainsKey(path[i]))
                                    {
                                        graphEdges[path[i - 1]][path[i]].Attr.Color = Microsoft.Msagl.Drawing.Color.Red;
                                        graphEdges[path[i - 1]][path[i]].Attr.ArrowheadAtTarget = Microsoft.Msagl.Drawing.ArrowStyle.Normal;
                                    }
                                    else
                                    {
                                        graphEdges[path[i]][path[i - 1]].Attr.Color = Microsoft.Msagl.Drawing.Color.Red;
                                        graphEdges[path[i]][path[i - 1]].Attr.ArrowheadAtSource = Microsoft.Msagl.Drawing.ArrowStyle.Normal;
                                    }
                                }
                                viewer = new Microsoft.Msagl.GraphViewerGdi.GViewer();
                                viewer.Graph = graph;
                                viewer.Dock = System.Windows.Forms.DockStyle.Fill;
                                panel11.Controls.Add(viewer);
                            }
                        }
                        else
                        {
                            panel11.Controls.Add(viewer);
                            richTextBox1.Text += "There isn't any connection available\nYou have to start the new connection itself";
                        }
                    }
                    else
                    {
                        panel11.Controls.Remove(viewer);
                        richTextBox1.Text = "Explore friends with BFS from account " + comboBox1.Text + " to account " + comboBox2.Text + "\r\n";
                        refreshGraph();
                        string[] path = Friends.ExploreFriendBFS(comboBox1.Text, comboBox2.Text);
                        if (path.Length > 0)
                        {
                            int degree = path.Length - 2;
                            if (degree == 0)
                            {
                                richTextBox1.Text += "Both of them are already friends\n";
                                panel11.Controls.Add(viewer);
                            }
                            else
                            {
                                if (degree % 10 == 1 && !(degree > 10 && degree < 20))
                                {
                                    richTextBox1.Text += degree.ToString() + "st-degree-connection\n";
                                }
                                else if (degree % 10 == 2 && !(degree > 10 && degree < 20))
                                {
                                    richTextBox1.Text += degree.ToString() + "nd-degree-connection\n";
                                }
                                else if (degree % 10 == 3 && !(degree > 10 && degree < 20))
                                {
                                    richTextBox1.Text += degree.ToString() + "rd-degree-connection\n";
                                }
                                else
                                {
                                    richTextBox1.Text += degree.ToString() + "th-degree-connection\n";
                                }
                                richTextBox1.Text += path[0] + " -> ";
                                for (int i = 1; i < path.Length; i++)
                                {
                                    richTextBox1.Text += path[i];
                                    if (i != path.Length - 1)
                                    {
                                        richTextBox1.Text += " -> ";
                                    }
                                    if (graphEdges[path[i - 1]].ContainsKey(path[i]))
                                    {
                                        graphEdges[path[i - 1]][path[i]].Attr.Color = Microsoft.Msagl.Drawing.Color.Red;
                                        graphEdges[path[i - 1]][path[i]].Attr.ArrowheadAtTarget = Microsoft.Msagl.Drawing.ArrowStyle.Normal;
                                    }
                                    else
                                    {
                                        graphEdges[path[i]][path[i - 1]].Attr.Color = Microsoft.Msagl.Drawing.Color.Red;
                                        graphEdges[path[i]][path[i - 1]].Attr.ArrowheadAtSource = Microsoft.Msagl.Drawing.ArrowStyle.Normal;
                                    }
                                }
                                viewer = new Microsoft.Msagl.GraphViewerGdi.GViewer();
                                viewer.Graph = graph;
                                viewer.Dock = System.Windows.Forms.DockStyle.Fill;
                                panel11.Controls.Add(viewer);
                            }
                        }
                        else
                        {
                            panel11.Controls.Add(viewer);
                            richTextBox1.Text += "There isn't any connection available\nYou have to start the new connection itself";
                        }
                    }
                }
            }
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            panel18.Hide();
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            panel18.Show();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (accountPicked != "") {
                comboBox2.Items.Add(accountPicked);
            }
            accountPicked = comboBox1.Text;
            comboBox2.Items.Remove(accountPicked);
        }
        private void refreshGraph()
        {
            List<Tuple<string, string>> edges = Friends.getEdges();
            string node1, node2;
            for (int i = 0; i < edges.Count; i++)
            {
                node1 = edges[i].Item1;
                node2 = edges[i].Item2;
                graphEdges[node1][node2].Attr.ArrowheadAtTarget = Microsoft.Msagl.Drawing.ArrowStyle.None;
                graphEdges[node1][node2].Attr.ArrowheadAtSource = Microsoft.Msagl.Drawing.ArrowStyle.None;
                graphEdges[node1][node2].Attr.Color = Microsoft.Msagl.Drawing.Color.Black;
            }
            
        }
    }
}
