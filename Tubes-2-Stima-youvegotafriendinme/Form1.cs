﻿using System;
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
        Microsoft.Msagl.GraphViewerGdi.GViewer viewer;
        public Form1()
        {
            InitializeComponent();
        }

        //private void button1_Click(object sender, EventArgs e)
        //{
        //    openFileDialog1.ShowDialog();
        //    string filename = openFileDialog1.FileName;
        //    string readfile = File.ReadAllText(filename);
        //    //richTextBox1.Text = readfile;
        //}

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        //private void button2_Click(object sender, EventArgs e)
        //{
        //    Microsoft.Msagl.GraphViewerGdi.GViewer viewer = new Microsoft.Msagl.GraphViewerGdi.GViewer();
        //    //create a graph object 
        //    Microsoft.Msagl.Drawing.Graph graph = new Microsoft.Msagl.Drawing.Graph("graph");
        //    //create the graph content 
        //    graph.AddEdge("A", "B").Attr.ArrowheadAtTarget = Microsoft.Msagl.Drawing.ArrowStyle.None;
        //    graph.AddEdge("B", "C");
        //    graph.AddEdge("A", "C").Attr.Color = Microsoft.Msagl.Drawing.Color.Green;
        //    graph.FindNode("A").Attr.FillColor = Microsoft.Msagl.Drawing.Color.Magenta;
        //    graph.FindNode("B").Attr.FillColor = Microsoft.Msagl.Drawing.Color.MistyRose;
        //    Microsoft.Msagl.Drawing.Node c = graph.FindNode("C");
        //    c.Attr.FillColor = Microsoft.Msagl.Drawing.Color.PaleGreen;
        //    c.Attr.Shape = Microsoft.Msagl.Drawing.Shape.Diamond;
        //    //bind the graph to the viewer 
        //    viewer.Graph = graph;
        //    //associate the viewer with the form 
        //    //form.SuspendLayout();
        //    viewer.Dock = System.Windows.Forms.DockStyle.Fill;
        //    panel1.Controls.Add(viewer);
        //}

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
                label12.Text = "";
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
                for(int i=0; i<edges.Count; i++)
                {
                    label12.Text += edges[i].Item1;
                    label12.Text += edges[i].Item2;
                    label12.Text += ";";
                    graph.AddEdge(edges[i].Item1, edges[i].Item2).Attr.ArrowheadAtTarget = Microsoft.Msagl.Drawing.ArrowStyle.None;
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
                        label12.Text = "Friend Recommendation with DFS from account " + comboBox1.Text;
                    }
                    else
                    {
                        label12.Text = "Friend Recommendation with BFS from account " + comboBox1.Text;
                    }
                }
            }
            else { 
                if(comboBox1.Text != "" && comboBox2.Text != "")
                {
                    if (algorithm.Text == "Depth First Search (DFS)")
                    {
                        label12.Text = "Explore friends with DFS from account " + comboBox1.Text+ " to account " + comboBox2.Text;
                    }
                    else
                    {
                        label12.Text = "Explore friends with BFS from account " + comboBox1.Text + " to account " + comboBox2.Text;
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
    }
}
