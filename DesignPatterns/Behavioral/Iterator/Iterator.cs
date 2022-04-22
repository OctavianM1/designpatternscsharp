namespace DesignPatterns.Behavioral.Iterator;

public static class Iterator
{
    public class Node<T>
    {
        public T Value { get; set; }
        public Node<T> Left { get; set; }
        public Node<T> Right { get; set; }
        public Node<T> Parent { get; set; }

        public Node(T value)
        {
            Value = value;
        }

        public Node(T value, Node<T> left, Node<T> right) : this(value)
        {
            Left = left;
            Right = right;
            left.Parent = right.Parent = this;
        }
    }

    public class InOrderIterator<T>
    {
        private readonly Node<T> _root;
        private bool yieldStart;
        public Node<T> Current { get; set; }

        public InOrderIterator(Node<T> root)
        {
            _root = root;
            Current = root;
            while (Current.Left != null)
                Current = Current.Left;
        }

        public bool MoveNext()
        {
            if (!yieldStart)
            {
                yieldStart = true;
                return true;
            }

            if (Current.Right != null)
            {
                Current = Current.Right;
                while (Current.Left != null)
                    Current = Current.Left;
                return true;
            }
            else
            {
                var p = Current.Parent;
                while (p != null && Current == p.Right)
                {
                    Current = p;
                    p = p.Parent;
                }
                Current = p;
                return Current != null;
            }
        }

    }

    public class BinaryTree<T>
    {
        private readonly Node<T> _root;

        public BinaryTree(Node<T> root)
        {
            _root = root;
        }

        public IEnumerable<Node<T>> InOrder
        {
            get
            {
                IEnumerable<Node<T>> Traverse(Node<T> current)
                {
                    if (current.Left != null)
                    {
                        foreach (var left in Traverse(current.Left))
                            yield return left;
                    }
                    yield return current;
                    if (current.Right != null)
                    {
                        foreach (var right in Traverse(current.Right))
                            yield return right;
                    }
                }

                foreach (var node in Traverse(_root))
                    yield return node;
            }
        }
    }

    public class BinaryTree2<T>
    {
        private readonly Node<T> _root;

        public BinaryTree2(Node<T> root)
        {
            _root = root;
        }

        public InOrderIterator<T> GetEnumerator()
        {
            return new InOrderIterator<T>(_root);
        }

    }

    //public static void DFS<T>(Node<T> node)
    //{
    //    if (node == null) return;
    //    var left = node.Left;
    //    var right = node.Right;
    //    DFS(left);
    //    Console.WriteLine(node.Value);
    //    DFS(right);
    //}

    public static IEnumerable<Node<T>> DFS<T>(Node<T> node)
    {
        if (node.Left != null)
        {
            foreach (var left in DFS(node.Left))
                yield return left;
        }
        yield return node;
        if (node.Right != null)
        {
            foreach (var right in DFS(node.Right))
                yield return right;
        }
    }

    public static void Render()
    {
        //    1
        //   /  \
        //  2    3

        // in-order 2 1 3

        var root = new Node<int>(1, new Node<int>(2), new Node<int>(3));
        //DFS(root);

        //var it = new InOrderIterator<int>(root);
        //while (it.MoveNext())
        //{
        //    Console.WriteLine(it.Current.Value);;
        //}

        var tree = new BinaryTree<int>(root);
        var tree2 = new BinaryTree2<int>(root);
        foreach (var node in tree2)
        {
            Console.WriteLine(node.Value);
        }

        //Console.WriteLine(string.Join(",", tree.InOrder.Select(x => x.Value)));
        //Console.WriteLine(string.Join(",", DFS(root).Select(x => x.Value)));
    }
}
