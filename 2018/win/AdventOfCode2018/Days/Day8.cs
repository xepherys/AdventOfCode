using System;
using System.IO;
using System.Windows.Forms;
using AdventOfCode2018.Core;

namespace AdventOfCode2018
{
    class Day8
    {
        static System.Reflection.Assembly thisExe = System.Reflection.Assembly.GetExecutingAssembly();

        public static BinaryTree Day8a(object sender = null)
        {
            string[] svalues;
            using (Stream stream = thisExe.GetManifestResourceStream("AdventOfCode2018._data.AdventOfCode_Day8.txt"))
            using (StreamReader reader = new StreamReader(stream))
            {
                svalues = reader.ReadToEnd().Split(' ');
            }

            int[] values = new int[svalues.Length];

            values = Array.ConvertAll(svalues, int.Parse);

            BinaryTree tree = new BinaryTree(values);

            tree.RunTree();

            int day8aValue = tree.CalculateMetadataSum();

            if (sender != null)
                MessageBox.Show(day8aValue.ToString());

            return tree;
        }

        public static void Day8b()
        {
            BinaryTree tree = Day8a();

            int day8bValue = tree.CalculateNodeValue(tree.GetRootNode());

            MessageBox.Show(day8bValue.ToString());
        }
    }
}