using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace HW3
{
    class Program
    {
        private const string InPath = @"F:\input.TXT";
        private const string OutPath = @"F:\output.TXT";
        private static Node[] _nodes;
        private static readonly Stack<int> VisitedNodesStack = new Stack<int>();
        private static bool _isFinished;

        static void Main(string[] args)
        {
            InputHandling();
        }

        private static void InputHandling()
        {
            var inputData = File.ReadAllLines(InPath);
            var nodesCount = int.Parse(inputData[0]);
            _nodes = new Node[nodesCount+1];
            var matrix = inputData[1..].ToList()
                .Select(el => el.Split(" ").Select(int.Parse).ToList()).ToList();
            FillNodesList(matrix);
            RecursiveDfs(_nodes[1], null);
            if (!_isFinished)
                using (var sw = new StreamWriter(OutPath))
                {
                    sw.Write("A");
                }
        }

        private static void FillNodesList(List<List<int>> matrix)
        {
            for(var i =0; i < matrix.Count; i++)
            {
                var currentLine = matrix[i];
                var childNodes = new List<int>();
                for (var j = 0; j < currentLine.Count; j++)
                    if (currentLine[j] != 0)
                        childNodes.Add(j+1);
                _nodes[i + 1] = new Node(i + 1, childNodes);
            }
        }

        private static void WriteOutAnswer(int startOfCycle)
        {
            var result = new List<string>();
            while (VisitedNodesStack.Count > 0)
            {
                var currentNode = VisitedNodesStack.Pop();
                result.Add(currentNode.ToString());
                if (currentNode == startOfCycle)
                    break;
            }
            result.Sort();
            using (var sw = new StreamWriter(OutPath))
            {
                sw.Write("N\n");
                sw.Write(string.Join(" ", result));
            }
        }

        private static void RecursiveDfs(Node currentNode, int? previousNodeNumber)
        {
            if (currentNode == null)
                return;
            if (currentNode.IsVisit)
            {
                WriteOutAnswer(currentNode.NodeNumber);
                _isFinished = true;
                return;
            }
            currentNode.IsVisit = true;
            VisitedNodesStack.Push(currentNode.NodeNumber);
            foreach (var childNodeNumber in currentNode.ChildNodes)
            {
                if (previousNodeNumber == childNodeNumber)
                    continue;
                var childNode = _nodes[childNodeNumber];
                RecursiveDfs(childNode, currentNode.NodeNumber);
                if (_isFinished)
                    return;
            }
            VisitedNodesStack.Pop();
        }
    }

    public class Node
    {
        public int NodeNumber
        { get;}
        public List<int> ChildNodes
        { get;}
        public bool IsVisit
        { get; set; }

        public Node(int nodeNumber, List<int> childNodes)
        {
            NodeNumber = nodeNumber;
            ChildNodes = childNodes;
            IsVisit = false;
        }
    }
}
