using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using PerceptronLocalService.DTO;

namespace PerceptronLocalService.Utility
{
    public class StringSearch
    {
        public static int NodeCount;
       
        public class TreeNode
        {

            #region Constructor & Methods

            /// <summary>
            /// Initialize tree node with specified character
            /// </summary>
            /// <param name="parent">Parent node</param>
            /// <param name="c">Character</param>
            public TreeNode(TreeNode parent, char c)
            {
                _char = c; _parent = parent;
                _results = new ArrayList();
                ResultsAr = new string[] { };

                _transitionsAr = new TreeNode[] { };
                TransHash = new Hashtable();
                State = NodeCount;
                NodeCount++;
            }


            /// <summary>
            /// Adds pattern ending in this node
            /// </summary>
            /// <param name="result">Pattern</param>
            public void AddResult(string result)
            {
                if (_results.Contains(result)) return;
                _results.Add(result);
                ResultsAr = (string[])_results.ToArray(typeof(string));
            }

            /// <summary>
            /// Adds trabsition node
            /// </summary>
            /// <param name="node">Node</param>
            public void AddTransition(TreeNode node)
            {
                TransHash.Add(node.Char, node);
                TreeNode[] ar = new TreeNode[TransHash.Values.Count];
                TransHash.Values.CopyTo(ar, 0);
                _transitionsAr = ar;
            }


            /// <summary>
            /// Returns transition to specified character (if exists)
            /// </summary>
            /// <param name="c">Character</param>
            /// <returns>Returns TreeNode or null</returns>
            public TreeNode GetTransition(char c)
            {
                return (TreeNode)TransHash[c];
            }


            /// <summary>
            /// Returns true if node contains transition to specified character
            /// </summary>
            /// <param name="c">Character</param>
            /// <returns>True if transition exists</returns>
            public bool ContainsTransition(char c)
            {
                return GetTransition(c) != null;
            }

            #endregion
            #region Properties

            private readonly char _char;
            private readonly TreeNode _parent;
            public TreeNode _failure;
            public ArrayList _results;
            private TreeNode[] _transitionsAr;
            public string[] ResultsAr;
            public Hashtable TransHash;

            public int State;
            /// <summary>
            /// Character
            /// </summary>
            public char Char
            {
                get { return _char; }
            }


            /// <summary>
            /// Parent tree node
            /// </summary>
            public TreeNode Parent
            {
                get { return _parent; }
            }


            /// <summary>
            /// Failure function - descendant node
            /// </summary>
            public TreeNode Failure
            {
                get { return _failure; }
                set { _failure = value; }
            }


            /// <summary>
            /// Transition function - list of descendant nodes
            /// </summary>
            public TreeNode[] Transitions
            {
                get { return _transitionsAr; }
            }


            /// <summary>
            /// Returns list of patterns ending by this letter
            /// </summary>
            public string[] Results
            {
                get { return ResultsAr; }
            }

            #endregion
        }

        public List<string> Keys;
        public TreeNode Root;       
        private string[] _keywords;
        public string[] Keywords
        {
            get { return _keywords; }
            set
            {
                _keywords = value;
                BuildTree();
                Keys = _keywords.ToList();
            }
        }

        public StringSearch(string[] keywords)
        {
            NodeCount = 0;
            Keywords = keywords;           
        }

        public StringSearch()
        {
        }

        void BuildTree()
        {
            // Build keyword tree and transition function
            Root = new TreeNode(null, ' ');
            foreach (string p in _keywords)
            {
                // add pattern to tree
                TreeNode nd = Root;
                foreach (char c in p)
                {
                    TreeNode ndNew = null;
                    foreach (TreeNode trans in nd.Transitions)
                        if (trans.Char == c) { ndNew = trans; break; }

                    if (ndNew == null)
                    {
                        ndNew = new TreeNode(nd, c);
                        nd.AddTransition(ndNew);
                    }
                    nd = ndNew;
                }
                nd.AddResult(p);
            }

            // Find failure functions
            ArrayList nodes = new ArrayList();
            // level 1 nodes - fail to root node
            foreach (TreeNode nd in Root.Transitions)
            {
                nd.Failure = Root;
                foreach (TreeNode trans in nd.Transitions) nodes.Add(trans);
            }
            // other nodes - using BFS
            while (nodes.Count != 0)
            {
                ArrayList newNodes = new ArrayList();
                foreach (TreeNode nd in nodes)
                {
                    TreeNode r = nd.Parent.Failure;
                    char c = nd.Char;

                    while (r != null && !r.ContainsTransition(c)) r = r.Failure;
                    if (r == null)
                        nd.Failure = Root;
                    else
                    {
                        nd.Failure = r.GetTransition(c);
                        foreach (string result in nd.Failure.Results)
                            nd.AddResult(result);
                    }

                    // add child nodes to BFS list 
                    foreach (TreeNode child in nd.Transitions)
                        newNodes.Add(child);
                }
                nodes = newNodes;
            }
            Root.Failure = Root;
        }
        
        public StringSearchResult[] FindAll(string text)
        {
            ArrayList ret = new ArrayList();
            TreeNode ptr = Root;
            int index = 0;

            while (index < text.Length)
            {
                TreeNode trans = null;
                while (trans == null)
                {
                    trans = ptr.GetTransition(text[index]);
                    if (ptr == Root) break;
                    if (trans == null) ptr = ptr.Failure;
                }
                if (trans != null) ptr = trans;

                foreach (string found in ptr.Results)
                    ret.Add(new StringSearchResult(index - found.Length + 1, found));
                index++;
            }
            return (StringSearchResult[])ret.ToArray(typeof(StringSearchResult));
        }

        public int[] FindAl(string text)
        {
            TreeNode ptr = Root;
            int index = 0;
            int[] ans = new int[Keywords.Length];
            for (int k = 0; k < Keywords.Length; k++)
                ans[k] = 0;

            while (index < text.Length)
            {
                TreeNode trans = null;
                while (trans == null)
                {
                    trans = ptr.GetTransition(text[index]);
                    if (ptr == Root) break;
                    if (trans == null) ptr = ptr.Failure;
                }
                if (trans != null) ptr = trans;

                foreach (string found in ptr.Results)
                {
                    ans[Keys.IndexOf(found)]++;
                }
                //ret.Add(new StringSearchResult(index - found.Length + 1, found));
                index++;
            }
            return (ans);
        }

        public StringSearchResult FindFirst(string text)
        {
            TreeNode ptr = Root;
            int index = 0;

            while (index < text.Length)
            {
                TreeNode trans = null;
                while (trans == null)
                {
                    trans = ptr.GetTransition(text[index]);
                    if (ptr == Root) break;
                    if (trans == null) ptr = ptr.Failure;
                }
                if (trans != null) ptr = trans;

                foreach (string found in ptr.Results)
                    return new StringSearchResult(index - found.Length + 1, found);
                index++;
            }
            return StringSearchResult.Empty;
        }

        public bool ContainsAny(string text)
        {
            TreeNode ptr = Root;
            int index = 0;

            while (index < text.Length)
            {
                TreeNode trans = null;
                while (trans == null)
                {
                    trans = ptr.GetTransition(text[index]);
                    if (ptr == Root) break;
                    if (trans == null) ptr = ptr.Failure;
                }
                if (trans != null) ptr = trans;

                if (ptr.Results.Length > 0) return true;
                index++;
            }
            return false;
        }

        public void build_table(Hashtable[] tab, TreeNode node)
        {
            foreach (DictionaryEntry entry in node.TransHash)
                tab[node.State].Add(entry.Key, ((TreeNode)entry.Value).State);

            foreach (DictionaryEntry entry in node.TransHash)
                build_table(tab, (TreeNode)entry.Value);




        }

        private void PopulateStateTransitionMatrix(int[,] table1, TreeNode node)
        {
            foreach (DictionaryEntry entry in node.TransHash)
                table1[node.State, (Convert.ToInt16(entry.Key) - Convert.ToInt16('A'))] = ((TreeNode)entry.Value).State;

            foreach (DictionaryEntry entry in node.TransHash)
                PopulateStateTransitionMatrix(table1, (TreeNode)entry.Value);

        }

        public int[,] GenerateStateTransitionMatrix(string transitions)
        {
            var stateTransitionMatrix = new int[NodeCount, transitions.Length];
            for (var i = 0; i < NodeCount; i++)
                for (var j = 0; j < transitions.Length; j++)
                {
                    stateTransitionMatrix[i, j] = -1;
                }

            PopulateStateTransitionMatrix(stateTransitionMatrix, Root);
            return stateTransitionMatrix;
        }

        public int[] GenerateStateSuccessVector()
        {
            int[] stateSuccessVector = new int[NodeCount];
            PopulateStateSuccessVector(stateSuccessVector, Root);
            return stateSuccessVector;
        }

        private void PopulateStateSuccessVector(int[] table1, TreeNode node)
        {
            foreach (DictionaryEntry entry in node.TransHash)
            {
                if (((TreeNode)entry.Value)._results.Count > 0)
                {
                    foreach (string s in ((TreeNode)entry.Value).ResultsAr)
                    {
                        table1[((TreeNode)entry.Value).State] = Keys.IndexOf(s) + 1;
                    }
                }
                else
                    table1[node.State] = 0;
            }

            foreach (DictionaryEntry entry in node.TransHash)
                PopulateStateSuccessVector(table1, (TreeNode)entry.Value);
        }
    }
}
