using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Linq.Expressions;

namespace Day20
{
    class Program
    {
        static void Main(string[] args)
        {
            List<PathString> paths = new List<PathString>();
            string pathstring = "NENNW(SE|N(SSSW|)EESSESW(NNNNNW|NNWNWW)S)WSS";  //3ms.
            int depth = 0;
            int marker = 0;
            int[] position = new int[500];
            List<string> enumeratedPaths = new List<string>();

            using (Stream stream = File.OpenRead(@"day20.in"))
            using (StreamReader reader = new StreamReader(stream))
            {
                pathstring = reader.ReadLine();
            }

            Stopwatch sw = new Stopwatch();
            sw.Start();
            PathString p0 = new PathString();
            p0.Depth = depth;
            p0.Marker = marker;
            p0.Position = position[depth];
            paths.Add(p0);
            foreach (char c in pathstring)
            {
                switch (c)
                {
                    case 'N':
                    case 'S':
                    case 'E':
                    case 'W':
                        foreach (PathString ps in paths.Where(w => w.Depth == depth && w.Position == position[depth]))
                            ps.Path += c.ToString();
                        break;
                    case '(':
                        marker += 1;
                        foreach (PathString ps in paths.Where(w => w.Depth == depth && w.Position == position[depth]))
                            ps.Path += "["+marker+"]";
                        depth += 1;
                        PathString openParen = new PathString();
                        openParen.Depth = depth;
                        openParen.Marker = marker;
                        openParen.Position = position[depth];
                        paths.Add(openParen);
                        break;
                    case '|':
                        position[depth] += 1;
                        PathString pipe = new PathString();
                        pipe.Depth = depth;
                        pipe.Marker = marker;
                        pipe.Position = position[depth];
                        paths.Add(pipe);
                        break;
                    case ')':
                        position[depth] += 1;
                        depth -= 1;
                        break;
                    case '^':
                    case '$':
                        break;
                }
            }

            depth = 0;
            enumeratedPaths = EnumeratedPaths(paths);
            sw.Stop();
            Console.WriteLine("Possible paths:" + Environment.NewLine);
            foreach (string pathString in enumeratedPaths)
                Console.WriteLine(pathString + Environment.NewLine);
            Console.WriteLine("Completed in " + sw.ElapsedMilliseconds + "ms.");
            Console.ReadKey();
            Environment.Exit(0);
        }

        public static List<string> EnumeratedPaths(List<PathString> paths)
        {
            List<string> _ret = new List<string>();
            int marker = 1;
            string path = paths.Single(w => w.Depth == 0).Path;
            _ret.Add(path);
            paths.Remove(paths.Single(w => w.Path == path));
            List<string> stringsToRemove = new List<string>();
            List<string> stringsToAdd = new List<string>();

            while (paths.Count() > 0)
            {
                foreach (string str in _ret.Where(w => !String.IsNullOrEmpty(w) && w.Contains("["+marker+"]")))
                {
                    stringsToRemove.Add(str);
                    foreach (PathString psi in paths.Where(w => w.Marker == marker))
                    {
                        string s = str.Replace("[" + marker + "]", psi.Path);
                        stringsToAdd.Add(s);
                        //_ret.Add(s);
                    }
                }
                paths.RemoveAll(w => w.Marker == marker);
                marker++;
                
                foreach (string rem in stringsToRemove)
                {
                    _ret.Remove(rem);
                }
                foreach (string add in stringsToAdd)
                {
                    _ret.Add(add);
                }

                stringsToRemove.Clear();
                stringsToAdd.Clear();
                
            }

            return _ret;
        }
    }

    public class PathString
    {
        public int Marker { get; set; }
        public int Depth { get; set; }
        public string Path { get; set; }
        public int Position { get; set; }

        public static implicit operator string(PathString p)
        {
            return p.Path;
        }
    }
}
