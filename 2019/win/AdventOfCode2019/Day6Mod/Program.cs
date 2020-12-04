using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Day6Mod
{
    class Program
    {
        static TreeNodeList<string> tree;

        static void Main(string[] args)
        {
            List<string[]> orbitList = new List<string[]>();

            using (Stream stream = File.OpenRead(@"..\..\..\Day06_Input.txt"))
            using (StreamReader reader = new StreamReader(stream))
            {
                bool first = true;
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    string[] s = line.Split(')');

                    if (first)
                    {
                        TreeNode<string> node = new TreeNode<string>(s[0]);
                        tree = new TreeNodeList<string>(node);
                        node.Children.Add(new TreeNode<string>(s[1]));
                        first = false;
                    }

                    else
                    {
                       
                    }
                }

            }
        }
    }

    public class TreeNodeList<T> : List<TreeNode<T>>
    {
        public TreeNode<T> Parent;

        public TreeNodeList(TreeNode<T> Parent)
        {
            this.Parent = Parent;
        }

        public new TreeNode<T> Add(TreeNode<T> Node)
        {
            base.Add(Node);
            Node.Parent = Parent;
            return Node;
        }

        public TreeNode<T> Add(T Value)
        {
            return Add(new TreeNode<T>(Value));
        }

        public override string ToString()
        {
            return "Count =" + Count.ToString();
        }
    }

    public class TreeNode<T> : IDisposable
    {
        private TreeNode<T> _Parent;
        private bool _IsDisposed;
        private TreeTraversalType _DisposeTraversal = TreeTraversalType.BottomUp;

        public TreeTraversalType DisposeTraversal
        {
            get { return _DisposeTraversal; }
            set { _DisposeTraversal = value; }
        }

        public bool IsDisposed
        {
            get { return _IsDisposed; }
        }

        public TreeNode(T Value)
        {
            this.Value = Value;
            Parent = null;
            Children = new TreeNodeList<T>(this);
        }

        public TreeNode(T Value, TreeNode<T> Parent)
        {
            this.Value = Value;
            this.Parent = Parent;
            Children = new TreeNodeList<T>(this);
        }

        public TreeNode<T> Parent
        {
            get { return _Parent; }

            set
            {
                if (value == _Parent)
                {
                    return;
                }

                if (_Parent != null)
                {
                    _Parent.Children.Remove(this);
                }

                if (value != null && !value.Children.Contains(this))
                {
                    value.Children.Add(this);
                }

                _Parent = value;
            }
        }

        public TreeNode<T> Root
        {
            get
            {
                //return (Parent == null) ? this : Parent.Root;

                TreeNode<T> node = this;

                while (node.Parent != null)
                {
                    node = node.Parent;
                }

                return node;
            }
        }

        private TreeNodeList<T> _Children;

        public TreeNodeList<T> Children
        {
            get { return _Children; }
            private set { _Children = value; }
        }

        private T _Value;

        public T Value
        {
            get { return _Value; }

            set
            {
                _Value = value;
            }
        }

        public int Depth
        {
            get
            {
                //return (Parent == null ? -1 : Parent.Depth) + 1;
                int depth = 0;
                TreeNode<T> node = this;
                while (node.Parent != null)
                {
                    node = node.Parent;
                    depth++;
                }

                return depth;
            }
        }

        public void Dispose()
        {
            CheckDisposed();

            OnDisposing();

            // clean up contained objects (in Value property)

            if (Value is IDisposable)
            {
                if (DisposeTraversal == TreeTraversalType.BottomUp)
                {
                    foreach (TreeNode<T> node in Children)
                    {
                        node.Dispose();
                    }
                }

                (Value as IDisposable).Dispose();

                if (DisposeTraversal == TreeTraversalType.TopDown)
                {
                    foreach (TreeNode<T> node in Children)
                    {
                        node.Dispose();
                    }
                }
            }
            _IsDisposed = true;
        }

        public event EventHandler Disposing;

        protected void OnDisposing()
        {
            if (Disposing != null)
            {
                Disposing(this, EventArgs.Empty);
            }
        }

        public void CheckDisposed()
        {
            if (IsDisposed)
            {
                throw new ObjectDisposedException(GetType().Name);
            }
        }

        public override string ToString()
        {
            string Description = string.Empty;

            if (Value != null)
            {
                Description = "[" + Value.ToString() + "] ";
            }

            return Description + "Depth=" + Depth.ToString() + ", Children=" + Children.Count.ToString();

        }
    }

    public enum TreeTraversalType
    {
        TopDown,
        BottomUp
    }
}
