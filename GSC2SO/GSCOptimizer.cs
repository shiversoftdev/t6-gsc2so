using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Irony.Parsing;
using System.IO;
using System.Reflection;

/* Removed all ghetto fixes
 * Try and perform a dynamic replacement?
 * 
 * 
 * 
 * 
 * 
 * 
 * 
 * 
 */


namespace GSC2SO
{
    public class GSCOptimizer
    {
        #region private
        private string path, finalstring;
        private const string chars = "abcdefghijklmnopqrstuvwxyz_0123456789";
        private bool DEBUG_ = true;
        #endregion 
        #region public
         //This limits the amount of times a string will be used at any point in the program
        
        
        public static int includespaced = 0;
        public List<int> whitespacepositions = new List<int>();

        //New
        private Form1 This;
        public const int STRING_REF_MAX = 250;
        public static int Index;
        public static string StringFromBytes = "";
        public List<KeyValuePair<string, string>> whitelist = new List<KeyValuePair<string, string>>();
        public List<KeyValuePair<string, string>> blacklist = new List<KeyValuePair<string, string>>();
        public List<string> stringblacklist = new List<string>(), functionwhitelist = new List<string>();
        public ParseTree Tree;
        public List<string> FunctionNames;
        public Dictionary<string, List<Token>> FunctionArguments;
        public Dictionary<string, List<Token>> FunctionLocals;
        public Dictionary<string, List<Token>> FunctionRefs;
        public Dictionary<string, string> FunctionRefLexicon;
        public Dictionary<int, int> Uses;
        public List<string> local_excludes;
        public Dictionary<string, Dictionary<string, string>> FunctionLexicon;
        public Dictionary<string, Dictionary<string, int>> FunctionUses;
        public Dictionary<string, string> GlobalLexicon;
        public List<Token> Globals;
        public Dictionary<string, int> GlobalUses;
        public List<ParseTreeNode> IdentifierTokens;
        public List<string> ReplacedStrings = new List<string>();
        public delegate void NodeOperator(ParseTreeNode node, string function, string type);
        public List<string> WhiteListedFunctions;
        public List<string> BlacklistedStrings;
        public Dictionary<string, string> StringLexicon;
        public List<Token> StringLiterals;
        public Dictionary<string, List<string>> Available_keys;
        public Dictionary<Token, int> Foreach_Ref_count;
        public Dictionary<string, int> Function_Foreach_Count;
        /// <summary>
        /// Default Constructor
        /// </summary>
        /// <param name="ths"> The Form1 we are working with outside of this thread</param>
        public GSCOptimizer(Form1 ths)
        {
            This = ths;
            FunctionNames = new List<string>();
            FunctionArguments = new Dictionary<string, List<Token>>();
            FunctionRefs = new Dictionary<string, List<Token>>();
            FunctionLocals = new Dictionary<string, List<Token>>();
            Globals = new List<Token>();
            Uses = new Dictionary<int, int>();
            local_excludes = new List<string>();
            FunctionLexicon = new Dictionary<string, Dictionary<string, string>>();
            GlobalLexicon = new Dictionary<string, string>();
            FunctionUses = new Dictionary<string, Dictionary<string, int>>();
            GlobalUses = new Dictionary<string, int>();
            FunctionRefLexicon = new Dictionary<string, string>();
            IdentifierTokens = new List<ParseTreeNode>();
            WhiteListedFunctions = new List<string>();
            ReplacedStrings = new List<string>();
            BlacklistedStrings = new List<string>();
            StringLexicon = new Dictionary<string, string>();
            StringLiterals = new List<Token>();
            Available_keys = new Dictionary<string, List<string>>();
            Foreach_Ref_count = new Dictionary<Token, int>();
            Function_Foreach_Count = new Dictionary<string, int>();
        }


        /// <summary>
        /// Perform the background operation on a seperate thread and report progress back to console
        /// </summary>
        /// <returns> bool success</returns>
        public bool Main()
        {
            if (!This.IncludedHeaders)
                ResetGlobals();
            try
            {
                if (!This.COMPILE_ONLY)
                    return CheckSyntax() &&
                           Defaults() &&
                           ParseTokens() &&
                           ICollectVariables() &&
                           IReplaceGlobals() &&
                           IAssignLocalTemplates() &&
                           IReplaceLocals() &&
                           IReplaceStrings() &&
                           IReplaceFunctionRefs() &&
                           IReconstructGSC() &&
                           ICompile() &&
                           IObfuscate();
                else
                    return CheckSyntax() &&
                            WriteTemp() &&
                            ICompile();
            }
            catch (Exception ex)
            {
                This.scout("Ran into an unhandled exception when trying to handle the files.", "Error");
                if (This.DEBUG || DEBUG_)
                    This.scout(ex.GetBaseException().ToString());
                return false;
            }
        }

        private bool ParseTokens()
        {
            This.scout("Parsing file...");
            var gameScript = new GSC2Grammar();
            var parser = new Parser(gameScript);
            Tree = parser.Parse(finalstring);
            return Tree.ParserMessages.Count < 1;
        }
		
		private bool IReplaceGlobals() //Iterate and define global lexicon, then replace token object values for terminal identities
		{
            This.scout("Assigning Globals...");
            int i = 0;
            foreach(Token t in Globals)
            {
                t.Value = t.Text.ToLower();
                foreach (KeyValuePair<string,string> kvp in whitelist)
                {
                    if (t.Value as string == kvp.Value)
                    {
                        GlobalLexicon[t.Value as string] = t.Value as string; //Allow to be used for locals and function references
                        if (GlobalUses.Keys.Contains(t.Value as string))
                        {
                            GlobalUses[GlobalLexicon[t.Value as string]]++;
                        }
                        else
                        {
                            GlobalUses[GlobalLexicon[t.Value as string]] = 1;
                        }
                        goto EndOfLoop;
                    }
                }
                if (GlobalLexicon.Keys.Contains(t.Value as string))
                {
                    GlobalUses[GlobalLexicon[t.Value as string]]++;
                    continue;
                }
                if(!This.OP_GLOBALS)
                {
                    GlobalLexicon[t.Value as string] = t.Value as string;
                    if (GlobalUses.Keys.Contains(t.Value as string))
                    {
                        GlobalUses[GlobalLexicon[t.Value as string]]++;
                    }
                    else
                    {
                        GlobalUses[GlobalLexicon[t.Value as string]] = 1;
                    }
                    continue;
                }
                string key = "";
                if (i == 0)
                {
                    key = "serioushd";
                }
                else
                {
                    key = chars[i / (chars.Length * chars.Length)] + "" + chars[i / (chars.Length)] + chars[i % chars.Length];
                    if(key == "abs")
                    {
                        i++;
                        key = chars[i / (chars.Length * chars.Length)] + "" + chars[i / (chars.Length)] + chars[i % chars.Length];
                    }
                }
                GlobalLexicon[t.Value as string] = key;
                ReplacedStrings.Add(key);
                GlobalUses[GlobalLexicon[t.Value as string]] = 1;
                i++;
                EndOfLoop:;
            }
            This.scout("Replacing Globals...");
            foreach (Token t in Globals)
            {
                if(GlobalLexicon.Keys.Contains(t.Value as string))
                {
                    t.Value = GlobalLexicon[t.Value as string];
                }
            }
            return true;
		}

        private bool IAssignLocalTemplates()
        {
            This.scout("Assigning and replacing locals...");
            foreach(string s in FunctionLocals.Keys)
            {
                local_excludes.Clear();
                local_excludes.Add("abs");
                foreach (Token t in FunctionLocals[s])
                {
                    int count = CountToken(FunctionLocals[s], t);
                    t.Value = t.Text.ToLower();
                    if(FunctionLexicon[s].Keys.Contains(t.Value as string))
                    {
                        continue;
                    }
                    string key = null;
                    foreach (string str in GlobalUses.Keys)
                    {
                        if (local_excludes.Contains(str))
                            continue;
                        if (GlobalUses[str] + count <= STRING_REF_MAX)
                        {
                            key = str;
                            break;
                        }
                    }
                    int i = GlobalUses.Count;
                    if(key == null)
                    {
                        GlobalUses["a" + chars[i / chars.Length] + chars[i % chars.Length]] = count;
                        key = "a" + chars[i / chars.Length] + chars[i % chars.Length];
                        while (local_excludes.Contains(key))
                        {
                            i++;
                            GlobalUses["a" + chars[i / chars.Length] + chars[i % chars.Length]] = count;
                            key = "a" + chars[i / chars.Length] + chars[i % chars.Length];
                        }
                    }
                    else
                        GlobalUses[key] += count;
                    local_excludes.Add(key);
                    ReplacedStrings.Add(key);
                    FunctionLexicon[s][t.Value as string] = key;
                }
            }
            if(This.DEBUG)
            {
                foreach(string s in FunctionLexicon.Keys)
                {
                    foreach(string key in FunctionLexicon[s].Keys)
                    {
                        This.scout(key + " -> " + FunctionLexicon[s][key], "Local Replacement " + s);
                    }
                }
            }
            return true;
        }

        private int CountToken(List<Token> tokens, Token t)
        {
            int count = 0;
            foreach (Token ts in tokens)
            {
                if (ts.Text.ToLower() == t.Text.ToLower())
                    count++;
            }
            return count;
        }

        private int CountToken(List<Token> tokens, string t)
        {
            int count = 0;
            foreach (Token ts in tokens)
            {
                if (ts.Text.ToLower() == t.ToLower())
                    count++;
            }
            return count;
        }

        public bool IReplaceLocals()
        {
            foreach(string s in FunctionLocals.Keys) //TODO? Might work. Might not. I dunno. Hopefully it does :)
            {
                Available_keys[s] = new List<string>();
                Available_keys[s].AddRange(GlobalLexicon.Values);
                Available_keys[s].Remove("abs");
                int HighestCount = 0;
                foreach(Token t in Foreach_Ref_count.Keys)
                {
                    if (FunctionLocals[s].Contains(t) && Foreach_Ref_count[t] > HighestCount)
                        HighestCount = Foreach_Ref_count[t];
                }
                foreach (string str in FunctionLexicon[s].Values)
                {
                    Available_keys[s].Remove(str);
                }
                for (int i = 0; i < Available_keys[s].Count; i++)
                {
                    if (GlobalUses[Available_keys[s][i]] + HighestCount > STRING_REF_MAX)
                    {
                        Available_keys[s].RemoveAt(i);
                        i--;
                    }
                }
                while(Available_keys[s].Count < Function_Foreach_Count[s])
                {
                    int i = GlobalUses.Count;
                    GlobalUses["a" + chars[i / chars.Length] + chars[i % chars.Length]] = HighestCount; //Innaccurate but safe :)
                    string key = "a" + chars[i / chars.Length] + chars[i % chars.Length];
                    Available_keys[s].Add(key);
                }
            }
            foreach(string s in FunctionLocals.Keys)
            {
                foreach(Token t in FunctionLocals[s])
                {
                    t.Value = FunctionLexicon[s][t.Text.ToLower()];
                }
            }
            return true;
        }

        public bool IReplaceFunctionRefs()
        {
            This.scout("Replacing Function Refs...");
            local_excludes.Clear();
            local_excludes.Add("abs");
            local_excludes.AddRange(functionwhitelist);
            foreach (string s in FunctionRefs.Keys)
            {
                if (s.ToLower() == "init")
                    continue;
                int count = CountToken(FunctionRefs[s], s);
                count++;
                //FunctionRefLexicon
                string key = null;
                foreach (string str in GlobalUses.Keys)
                {
                    if (local_excludes.Contains(str))
                        continue;
                    if (GlobalUses[str] + count <= STRING_REF_MAX)
                    {
                        key = str;
                        break;
                    }
                }
                int i = GlobalUses.Count;
                if (key == null)
                {
                    GlobalUses["a" + chars[i / chars.Length] + chars[i % chars.Length]] = count;
                    key = "a" + chars[i / chars.Length] + chars[i % chars.Length];
                    while (local_excludes.Contains(key))
                    {
                        i++;
                        GlobalUses["a" + chars[i / chars.Length] + chars[i % chars.Length]] = count;
                        key = "a" + chars[i / chars.Length] + chars[i % chars.Length];
                    }
                }
                else
                    GlobalUses[key] += count;
                local_excludes.Add(key);
                ReplacedStrings.Add(key);
                FunctionRefLexicon[s] = key;
            }
            foreach(string s in FunctionRefs.Keys)
            {
                foreach (Token t in FunctionRefs[s])
                {
                    if (t.Text.ToLower().ToLower() == "init")
                        continue;
                    t.Value = FunctionRefLexicon[t.Text.ToLower()];
                }
            }
            foreach (var function in Tree.Root.ChildNodes[4].ChildNodes)
            {
                if (function.ChildNodes[0].Token.Text.ToLower() == "init")
                    continue;

                Available_keys[FunctionRefLexicon[function.ChildNodes[0].Token.Text.ToLower()]] = Available_keys[function.ChildNodes[0].Token.Text.ToLower()]; //Redefine the pass to point the the right spot
                function.ChildNodes[0].Token.Value = FunctionRefLexicon[function.ChildNodes[0].Token.Text.ToLower()];
                if(This.DEBUG)
                    This.scout(function.ChildNodes[0].Token.Text.ToLower() + " -> " + FunctionRefLexicon[function.ChildNodes[0].Token.Text.ToLower()], "FunctionRef replacement");
            }
            return true;
        }

        public bool IReconstructGSC()
        {
            List<string> uniqueids = new List<string>();
            foreach (var tok in IdentifierTokens)
            {
                if (tok.Term == GSC2Grammar.numberLiteral)
                    continue;
                string val = tok.FindTokenAndGetValue().ToLower();
                if (tok.Term == GSC2Grammar.stringLiteral)
                {
                    val = tok.FindTokenAndGetValue();
                }
                if (uniqueids.Contains(val))
                    continue;
                uniqueids.Add(tok.FindTokenAndGetValue().ToLower());
            }
            This.ReportStrings(uniqueids.Count);
            This.scout("Reconstructing GSC...");
            string FinalText = "";
            FinalText += ReconstructHeader();
            FinalText += ReconstructBody();
            string tpath = path.Substring(0, path.LastIndexOf(".gsc")) + ".txt";
            File.WriteAllText(tpath, FinalText);
            return true;
        }

        public bool WriteTemp()
        {
            string tpath = path.Substring(0, path.LastIndexOf(".gsc")) + ".txt";
            File.WriteAllText(tpath, finalstring);
            return true;
        }

        public string ReconstructHeader()
        {
            string toReturn = "";
            foreach(var node in Tree.Root.ChildNodes[0].ChildNodes)
            {
                toReturn += "#include " + node.ChildNodes[0].Token.Text + ";\n";
            }
            return toReturn;
        }

        public string ReconstructBody()
        {
            string toReturn = HandleNode(Tree.Root.ChildNodes[4]);
            return toReturn;
        }


        public string HandleNode(ParseTreeNode node, int indent = 0)
        {
            string toreturn = "";
                /*
                * 
                * 
                * 
                * 
                */
            switch (node.ToString())
            {
                case "function":
                    {
                        toreturn += node.ChildNodes[0].Token.Value;
                        if(node.ChildNodes[1].ChildNodes[0].ToString() == "parameters")
                        {
                            toreturn += "(";
                            foreach (var nod in node.ChildNodes[1].ChildNodes[0].ChildNodes)
                            {
                                toreturn += nod.ChildNodes[0].Token.Value + ",";
                            }
                            toreturn = toreturn.Substring(0, toreturn.Length - 1) + ")\n{\n";
                        }
                        else
                        {
                            toreturn += "()\n{\n";
                        }
                        if(node.ChildNodes[2].ChildNodes.Count > 0)
                        {
                            foreach(var nod in node.ChildNodes[2].ChildNodes[0].ChildNodes[0].ChildNodes)
                            {
                                toreturn += HandleNode(nod, 1);
                            }
                        }
                        toreturn += "}\n";
                        break;
                    }
                case "animTree":
                    {
                        toreturn += "#animTree";
                        break;
                    }
                case "getAnimation":
                    {
                        toreturn += "%" + node.ChildNodes[1].Token.Value;
                        break;
                    }
                case "conditionalStatement":
                    {
                        toreturn += "(" + HandleNode(node.ChildNodes[0]) + ") ? (" + HandleNode(node.ChildNodes[2]) + ") : (" + HandleNode(node.ChildNodes[4]) + ")";
                        break;
                    }
                case "hashedString":
                    {
                        toreturn += "#" + node.ChildNodes[1].Token.Text + "";
                        break;
                    }
                case "getFunction"://ToTerm("::") + expr | gscForFunction + expr;
                    {
                        if (node.ChildNodes.Count == 1)
                        {
                            toreturn += "::" + HandleNode(node.ChildNodes[0]);
                        }
                        else
                        {
                            toreturn += HandleNode(node.ChildNodes[0]) + HandleNode(node.ChildNodes[1]);
                        }
                        break;
                    }
                case "gscForFunction":
                    {
                        toreturn += node.ChildNodes[0].Token.Text + "::";
                        break;
                    }
                case "isString":
                    {
                        toreturn += "&" + node.ChildNodes[1].Token.Text + "";
                        break;
                    }
                case "directAccess":
                    {
                        toreturn += HandleNode(node.ChildNodes[0]) + "." + node.ChildNodes[1].Token.Value;
                        break;
                    }
                case "relationalExpression":
                case "expression":
                    {
                        toreturn += "(" + HandleNode(node.ChildNodes[0]) + ") " + node.ChildNodes[1].FindTokenAndGetText() + " ("+ HandleNode(node.ChildNodes[2]) + ") ";
                        break;
                    }
                case "boolNot":
                    {
                        toreturn += "!" + HandleNode(node.ChildNodes[1]);
                        break;
                    }
                case "size":
                    {
                        toreturn += HandleNode(node.ChildNodes[0]) + ".size";
                        break;
                    }
                case "expr":
                    {
                        if(node.ChildNodes[0].Term == GSC2Grammar.identifier)
                        {
                            toreturn += node.ChildNodes[0].Token.Value;
                        }
                        else if(node.ChildNodes[0].Term == GSC2Grammar.stringLiteral)
                        {
                            if(StringLiterals.Contains(node.ChildNodes[0].Token))
                                toreturn += node.ChildNodes[0].Token.Value;
                            else
                                toreturn += node.ChildNodes[0].Token.Text;
                        }
                        else if(node.ChildNodes[0].Term == GSC2Grammar.numberLiteral)
                        {
                            toreturn += node.ChildNodes[0].FindTokenAndGetText();
                        }
                        else
                        {
                            toreturn += HandleNode(node.ChildNodes[0]);
                        }
                        break;
                    }
                case "array":
                    {
                        if(node.ChildNodes.Count < 2)
                        {
                            toreturn += "[]";
                        }
                        else
                        {
                            toreturn += HandleNode(node.ChildNodes[0]) + "[ " + HandleNode(node.ChildNodes[1]) + " ]";
                        }
                        break;
                    }
                case "vector":
                    {
                        toreturn += "(" + HandleNode(node.ChildNodes[0]) + ", " + HandleNode(node.ChildNodes[1]) + ", " + HandleNode(node.ChildNodes[2]) + ")";
                        break;
                    }
                case "wait":
                    {
                        toreturn += "wait " + HandleNode(node.ChildNodes[1]) + ";\n";
                        break;
                    }
                case "jumpStatement":
                    {
                        toreturn += node.ChildNodes[0].Token.Text + ";";
                        break;
                    }
                case "waitTillFrameEnd":
                    {
                        toreturn += "waittillframeend;\n";
                        break;
                    }
                case "return":
                    {
                        toreturn += GetTabs(indent) + "return ";
                        if(node.ChildNodes.Count > 1)
                        {
                            toreturn += HandleNode(node.ChildNodes[1]);
                        }
                        toreturn += ";\n";
                        break;
                    }
                case "declaration":
                    {
                        for(int i = 0; i < indent; i++)
                        {
                            toreturn += "\t";
                        }
                        
                        toreturn += HandleNode(node.ChildNodes[0], indent);
                        break;
                    }
                case "switchStatement":
                    {
                        toreturn += "switch(" + HandleNode(node.ChildNodes[1]) + ")\n";
                        toreturn += GetTabs(indent) + "{\n";
                        toreturn += HandleNode(node.ChildNodes[2], indent + 1);
                        toreturn += GetTabs(indent) + "}\n";
                        break;
                    }
                case "switchContent":
                    {
                        toreturn += HandleNode(node.ChildNodes[0]) + "\n" + GetTabs(indent);
                        toreturn += HandleNode(node.ChildNodes[1]);
                        break;
                    }
                case "switchLabel":
                    {
                        toreturn += "case " + HandleNode(node.ChildNodes[1]) + " :";
                        break;
                    }
                case "setVariableField":
                    {
                        toreturn += HandleNode(node.ChildNodes[0]) + node.ChildNodes[1].FindTokenAndGetText(); //Might be null. I dunno
                        if (node.ChildNodes.Count > 2)
                            toreturn += HandleNode(node.ChildNodes[2]);
                        toreturn += ";\n";
                        break;
                    }
                case "developerScript":
                    {
                        toreturn += "/#" + HandleNode(node.ChildNodes[1], indent) + "#/";
                        break;
                    }
                case "foreachStatement":
                    {
                        toreturn += "foreach(";
                        toreturn += node.ChildNodes[1].Token.Value + " in ";
                        toreturn += HandleNode(node.ChildNodes[3]) + ")\n";
                        toreturn += GetTabs(indent) + "{\n";
                        if (node.ChildNodes[4].ChildNodes[0].ToString() == "declaration")
                        {
                            toreturn += HandleNode(node.ChildNodes[4].ChildNodes[0], indent + 1);
                        }
                        else
                        {
                            foreach (var nod in node.ChildNodes[4].ChildNodes[0].ChildNodes)
                            {
                                toreturn += HandleNode(nod, indent + 1);
                            }
                        }
                        toreturn += GetTabs(indent) + "}\n";
                        break;
                    }
                case "ifStatement":
                    {
                        toreturn += "if(";
                        toreturn += HandleNode(node.ChildNodes[1]) + ")\n";
                        toreturn += GetTabs(indent) + "{\n";
                        if(node.ChildNodes[2].ChildNodes[0].ToString() == "declaration")
                        {
                            toreturn += HandleNode(node.ChildNodes[2].ChildNodes[0], indent + 1);
                        }
                        else
                        {
                            foreach(var nod in node.ChildNodes[2].ChildNodes[0].ChildNodes)
                            {
                                toreturn += HandleNode(nod, indent + 1);
                            }
                        }
                        toreturn += GetTabs(indent) + "}\n";
                        if(node.ChildNodes.Count > 3)
                        {
                            toreturn += GetTabs(indent) + "else\n" + GetTabs(indent) + "{\n";
                            if (node.ChildNodes[3].ChildNodes[1].ChildNodes[0].ToString() == "declaration")
                            {
                                toreturn += HandleNode(node.ChildNodes[3].ChildNodes[1].ChildNodes[0], indent + 1);
                            }
                            else
                            {
                                foreach (var nod in node.ChildNodes[3].ChildNodes[1].ChildNodes[0].ChildNodes)
                                {
                                    toreturn += HandleNode(nod, indent + 1);
                                }
                            }
                            toreturn += GetTabs(indent) + "}\n";
                        }
                        break;
                    }
                case "whileStatement":
                    {
                        toreturn += "while(";
                        toreturn += HandleNode(node.ChildNodes[1]) + ")\n";
                        toreturn += GetTabs(indent) + "{\n";
                        if (node.ChildNodes[2].ChildNodes[0].ToString() == "declaration")
                        {
                            toreturn += HandleNode(node.ChildNodes[2].ChildNodes[0], indent + 1);
                        }
                        else
                        {
                            foreach (var nod in node.ChildNodes[2].ChildNodes[0].ChildNodes)
                            {
                                toreturn += HandleNode(nod, indent + 1);
                            }
                        }
                        toreturn += GetTabs(indent) + "}\n";
                        break;
                    }
                case "forStatement":
                    {
                        toreturn += "for(";
                        if(node.ChildNodes[1].ChildNodes.Count > 0)
                        {
                            switch(node.ChildNodes[1].ChildNodes[0].ToString())
                            {
                                case "setVariableField":
                                    {
                                        toreturn += HandleNode(node.ChildNodes[1].ChildNodes[0]).Replace("\n", "");
                                        if(node.ChildNodes[1].ChildNodes.Count > 1)
                                        {
                                            switch(node.ChildNodes[1].ChildNodes[1].ToString())
                                            {
                                                case "booleanExpression":
                                                    {
                                                        toreturn += HandleNode(node.ChildNodes[1].ChildNodes[1]).Replace("\n", "") + ";";
                                                        if(node.ChildNodes[1].ChildNodes.Count > 2)
                                                        {
                                                            toreturn += HandleNode(node.ChildNodes[1].ChildNodes[2]);
                                                        }
                                                        break;
                                                    }
                                                case "forIterate":
                                                    {
                                                        toreturn += ";";
                                                        toreturn += HandleNode(node.ChildNodes[1].ChildNodes[1]);
                                                        break;
                                                    }
                                            }
                                        }
                                        else
                                        {
                                            toreturn += ";";
                                        }
                                        break;
                                    }
                                case "booleanExpression":
                                    {
                                        toreturn += ";";
                                        toreturn += HandleNode(node.ChildNodes[1].ChildNodes[0]).Replace("\n", "") + ";";
                                        if (node.ChildNodes[1].ChildNodes.Count > 1)
                                        {
                                            toreturn += HandleNode(node.ChildNodes[1].ChildNodes[1]);
                                        }
                                        break;
                                    }
                                case "forIterate":
                                    {
                                        toreturn += ";;";
                                        toreturn += HandleNode(node.ChildNodes[1].ChildNodes[0]);
                                        break;
                                    }
                            }
                        }
                        else
                        {
                            toreturn += ";;";
                        }
                        toreturn += ")\n";
                        toreturn += GetTabs(indent) + "{\n";
                        foreach (var nod in node.ChildNodes[2].ChildNodes)
                        {
                            toreturn += HandleNode(nod, indent + 1);
                        }
                        toreturn += GetTabs(indent) + "}\n";
                        break;
                    }
                case "forIterate":
                    {
                        toreturn += HandleNode(node.ChildNodes[0]) + node.ChildNodes[1].FindTokenAndGetText();
                        if(node.ChildNodes.Count > 2)
                            toreturn += HandleNode(node.ChildNodes[2], indent);
                        break;
                    }
                case "boolParen": //We have to ignore because OG compiler cant handle that expression :(
                case "booleanExpression":
                    {
                        if(node.ChildNodes.Count == 1)
                        {
                            toreturn += HandleNode(node.ChildNodes[0], indent);
                        }
                        else
                        {
                            toreturn += HandleNode(node.ChildNodes[0], indent) + " " + node.ChildNodes[1].ChildNodes[0].FindTokenAndGetText() + " " + HandleNode(node.ChildNodes[2], indent);
                        }
                        break;
                    }
                case "simpleCall":
                    {
                        toreturn += HandleNode(node.ChildNodes[0], indent) + ";\n";
                        break;
                    }
                case "baseCall": //gscForFunction + identifier + parenParameters | identifier + parenParameters
                    {
                        if(node.ChildNodes.Count > 2)
                        {
                            toreturn += node.ChildNodes[0].ChildNodes[0].Token.Value + "::" + node.ChildNodes[1].Token.Value + HandleNode(node.ChildNodes[2], indent);
                        }
                        else
                        {
                            toreturn += node.ChildNodes[0].Token.Value + HandleNode(node.ChildNodes[1], indent);
                        }
                        break;
                    }
                case "scriptMethodThreadCallPointer":
                case "scriptMethodThreadCall":
                    {
                        toreturn += HandleNode(node.ChildNodes[0]) + " thread " + HandleNode(node.ChildNodes[2]);
                        break;
                    }
                case "scriptThreadCallPointer":
                case "scriptThreadCall":
                    {
                        toreturn += "thread " + HandleNode(node.ChildNodes[1]);
                        break;
                    }
                case "scriptMethodCallPointer":
                case "scriptMethodCall":
                    {
                        toreturn += HandleNode(node.ChildNodes[0]) + " " + HandleNode(node.ChildNodes[1]);
                        break;
                    }
                case "baseCallPointer":
                    {
                        toreturn += "[[ " + HandleNode(node.ChildNodes[0]) + " ]]" + HandleNode(node.ChildNodes[1]);
                        break;
                    }
                case "parenParameters":
                    {
                        if(node.ChildNodes[0].ToString() == "parameters")
                        {
                            foreach(var nod in node.ChildNodes[0].ChildNodes)
                            {
                                toreturn += HandleNode(nod) + ",";
                            }
                            toreturn = "(" + toreturn.Substring(0, toreturn.Length - 1) + ")";
                        }
                        else
                        {
                            toreturn += "()";
                        }
                        break;
                    }
                case "StringAway":
                case "ParserNotification":
                    break;
                default:
                    foreach(var nod in node.ChildNodes)
                    {
                        toreturn += HandleNode(nod, indent);
                    }
                    break;
            }
            return toreturn;
        }

        private string GetTabs(int indent)
        {
            string toreturn = "";
            for (int i = 0; i < indent; i++)
                toreturn += "\t";
            return toreturn;
        }

        private bool ICollectVariables()
        {
            This.scout("Collecting Variables...");
            CollectIdentifiers(Tree.Root);
            List<string> uniqueids = new List<string>();
            foreach(var tok in IdentifierTokens)
            {
                if (tok.Term == GSC2Grammar.numberLiteral)
                    continue;
                string val = tok.FindTokenAndGetValue().ToLower();
                if (tok.Term == GSC2Grammar.stringLiteral)
                {
                    val = tok.FindTokenAndGetValue();
                }
                if (uniqueids.Contains(val))
                    continue;
                uniqueids.Add(tok.FindTokenAndGetValue().ToLower());
            }
            This.scout(uniqueids.Count + "", "Estimated string usage pre-compile time");
            foreach (var function in Tree.Root.ChildNodes[4].ChildNodes)
            {
                string name = function.ChildNodes[0].ToString().Split(' ')[0].ToLower();
                FunctionNames.Add(name.ToLower());
                FunctionArguments[name] = new List<Token>();
                Function_Foreach_Count[name] = 0;
                FunctionLocals[name] = new List<Token>();
                FunctionRefs[name] = new List<Token>();
                FunctionLexicon[name] = new Dictionary<string, string>();
                FunctionUses[name] = new Dictionary<string, int>();

            }
            IterateNode("undefined", ICollect, Tree.Root.ChildNodes[2]); //Collect preprocessor protections
            IterateNode("undefined", ICollect, Tree.Root.ChildNodes[3]); //Collect preprocessor stringaways
            foreach (var function in Tree.Root.ChildNodes[4].ChildNodes)
            {
                string name = function.ChildNodes[0].ToString().Split(' ')[0].ToLower();
                if (function.ChildNodes[1].ChildNodes[0].ToString().Split(' ')[0] == "parameters")
                {
                    foreach(var arg in function.ChildNodes[1].ChildNodes[0].ChildNodes)
                    {
                        FunctionArguments[name].Add(arg.ChildNodes[0].Token);
                        FunctionLocals[name].Add(arg.ChildNodes[0].Token);
                    }
                }
                if (function.ChildNodes[2].ChildNodes.Count < 1)
                    continue; //We have no block content in this function
                var declarations = function.ChildNodes[2].ChildNodes[0].ChildNodes[0].ChildNodes; //function -> block -> blockcontent -> declarations
                IterateNode(name, ICollect, function.ChildNodes[2].ChildNodes[0].ChildNodes[0]);
            }
            if(This.DEBUG)
            {
                foreach(Token g in Globals)
                {
                    This.scout(g.Text, "Global");
                }
                foreach(string key in FunctionLocals.Keys)
                {
                    foreach(Token s in FunctionLocals[key])
                    {
                        if (s == null)
                        {
                            This.scout(key + " -> " + "NULL_TOKEN", "Local ERROR");
                        }
                        else if (s.Text == null)
                        {
                            This.scout(key + " -> " + "NULL_TEXT", "Local ERROR");
                        }
                        else
                        {
                            This.scout(key + " -> " + s.Text, "Local");
                        }
                        
                    }
                }
                foreach (string key in FunctionRefs.Keys)
                {
                    foreach (Token t in FunctionRefs[key])
                    {
                        This.scout(key + " -> " + t.Text, "Function Ref");
                    }
                }
            }
            return true;
        }

        public void CollectIdentifiers(ParseTreeNode node)
        {
            foreach(var nod in node.ChildNodes)
            {
                CollectIdentifiers(nod);
            }
            if(node.Token != null && (node.Term == GSC2Grammar.identifier  || node.Term == GSC2Grammar.stringLiteral))
            {
                //node.Token.Value = node.Token.ToString();
                IdentifierTokens.Add(node);
            }
        }

        private int IterateNode(string function, NodeOperator INode, ParseTreeNode node, bool HasChild = false, bool IsRef = false)
        {
            try
            {
                switch (node.ToString())
                {
                    case "expr":
                        {
                            if(node.ChildNodes[0].Term == GSC2Grammar.identifier)
                            {
                                if(IsRef)
                                {
                                    INode(node.ChildNodes[0], function, "function");
                                }
                                else
                                {
                                    INode(node.ChildNodes[0], function, "local"); //Always local -- Direct access picks up children
                                }
                                break;
                            }
                            if(node.ChildNodes[0].Term == GSC2Grammar.stringLiteral)
                            {
                                INode(node.ChildNodes[0], function, "string");
                            }
                            foreach (var child in node.ChildNodes)
                            {
                                IterateNode(function, INode, child, HasChild, IsRef);
                            }
                            break;
                        }
                    case "directAccess": //expr + identifier -- this is aka global
                        {
                            IterateNode(function, INode, node.ChildNodes[0], true); //Let our handler work
                            INode(node.ChildNodes[1], function, "global");
                            break;
                        }
                    case "getFunction": //function | external + function -- We only need internals
                        {
                            if(node.ChildNodes.Count < 2)
                                IterateNode(function, INode, node.ChildNodes[0], false, true);
                            break;
                        }
                    case "size": // expr + .size
                        {
                            IterateNode(function, INode, node.ChildNodes[0]);
                            break;
                        }
                    case "boolNot": //! + expr
                        {
                            IterateNode(function, INode, node.ChildNodes[1], HasChild, IsRef);
                            break;
                        }
                    case "conditionalStatement": //bool + ? + expr + : + expr
                        {
                            IterateNode(function, INode, node.ChildNodes[0], HasChild, IsRef);
                            IterateNode(function, INode, node.ChildNodes[2], HasChild, IsRef);
                            IterateNode(function, INode, node.ChildNodes[4], HasChild, IsRef);
                            break;
                        }
                    case "array": //expr + expr | []
                         {
                            if(node.ChildNodes.Count < 2)
                            {
                                break;
                            }
                            foreach (var child in node.ChildNodes)
                            {
                                IterateNode(function, INode, child); //Dont pass because scope changes
                            }
                            break;
                         }
                   
                    case "baseCall": // gscForFunction + identifier + parenParameters | identifier + parenParameters
                        {
                            if(node.ChildNodes.Count < 3)
                            {
                                IterateNode(function, INode, node.ChildNodes[1], HasChild, IsRef);
                                INode(node.ChildNodes[0], function, "function");
                            }
                            else
                            {
                                //INode(node.ChildNodes[1], function, "function"); Dont parse externals
                                IterateNode(function, INode, node.ChildNodes[2], HasChild, IsRef); //Collect parameters, but not the external function name
                            }
                            break;
                        }
                    case "developerScript": // /# + expr + #/
                        {
                            IterateNode(function, INode, node.ChildNodes[1]);
                            break;
                        }
                    case "foreachStatement": //foreach + local + in + expr + statementblock
                        {
                            INode(node.ChildNodes[1], function, "local");
                            Function_Foreach_Count[function] += 2;
                            int count_of_refs = CountForeachRefs(node.ChildNodes[1].FindTokenAndGetText().ToLower(), node.ChildNodes[3]) + CountForeachRefs(node.ChildNodes[1].FindTokenAndGetText().ToLower(), node.ChildNodes[4]);
                            Foreach_Ref_count[node.ChildNodes[1].Token] = count_of_refs;
                            IterateNode(function, INode, node.ChildNodes[3], HasChild, IsRef);
                            IterateNode(function, INode, node.ChildNodes[4], HasChild, IsRef);
                            break;
                        }
                    case "forStatement": //for + forbody + statementblock
                        {
                            IterateNode(function, INode, node.ChildNodes[1], HasChild, IsRef);
                            IterateNode(function, INode, node.ChildNodes[2], HasChild, IsRef);
                            break;
                        }
                    case "ifStatement": //if + boolExpr + statementblock + ?else
                        {
                            IterateNode(function, INode, node.ChildNodes[1], HasChild, IsRef);
                            IterateNode(function, INode, node.ChildNodes[2], HasChild, IsRef);
                            if (node.ChildNodes.Count > 3) //else
                                IterateNode(function, INode, node.ChildNodes[3], HasChild, IsRef);
                            break;
                        }
                    case "whileStatement": //while + boolExpr + statementblock
                        {
                            IterateNode(function, INode, node.ChildNodes[1], HasChild, IsRef);
                            IterateNode(function, INode, node.ChildNodes[2], HasChild, IsRef);
                            break;
                        }
                    case "elseStatement": //else + statementblock
                        {
                            IterateNode(function, INode, node.ChildNodes[1], HasChild, IsRef);
                            break;
                        }
                    case "switchStatement": //switch + parenExpr + switchContents
                        {
                            IterateNode(function, INode, node.ChildNodes[1], HasChild, IsRef);
                            IterateNode(function, INode, node.ChildNodes[2], HasChild, IsRef);
                            break;
                        }
                    case "switchLabel": //keyword + ?id + keyword
                        {
                            if(node.ChildNodes.Count > 2)
                                IterateNode(function, INode, node.ChildNodes[1], HasChild, IsRef);
                            break;
                        }
                    case "boolevaloperator": //Dont care
                        {
                            break;
                        }
                    case "scriptThreadCall": //thread (keyword) + basecall
                        {
                            IterateNode(function, INode, node.ChildNodes[1], HasChild, IsRef);
                            break;
                        }
                    case "scriptThreadCallPointer": //thread (keyword) + baseCallPointer
                        {
                            IterateNode(function, INode, node.ChildNodes[1], HasChild, IsRef);
                            break;
                        }
                    case "scriptMethodThreadCall": //expr + thread + basecall
                        {
                            IterateNode(function, INode, node.ChildNodes[0], HasChild, IsRef);
                            IterateNode(function, INode, node.ChildNodes[2], HasChild, IsRef);
                            break;
                        }
                    case "scriptMethodThreadCallPointer": //expr + thread + baseCallPointer
                        {
                            IterateNode(function, INode, node.ChildNodes[0], HasChild, IsRef);
                            IterateNode(function, INode, node.ChildNodes[2], HasChild, IsRef);
                            break;
                        }
                    case "baseCallPointer": //expr + paren
                        {
                            //INode(node.ChildNodes[0].ChildNodes[0], function, "function"); //expr.identifier
                            IterateNode(function, INode, node.ChildNodes[0], HasChild );
                            IterateNode(function, INode, node.ChildNodes[1], HasChild );
                            break;
                        }
                    case "userprotections":
                        {
                            foreach(var nod in node.ChildNodes)
                            {
                                string text = nod.FindTokenAndGetText();
                                text = text.Replace("/$", "");
                                text = text.Replace("$/", "");
                                string[] lines = text.Split('\n');
                                foreach (string s in lines)
                                {
                                    if (s.Replace(" ", "").Trim().Replace("\r", "").ToLower().Length < 1)
                                        continue;
                                    whitelist.Add(new KeyValuePair<string, string>("*", s.Replace(" ", "").Trim().Replace("\r", "").ToLower()));
                                }
                            }
                            break;
                        }
                    case "userblacklist":
                        {
                            foreach (var nod in node.ChildNodes)
                            {
                                string text = nod.FindTokenAndGetText();
                                text = text.Replace("/!", "");
                                text = text.Replace("!/", "");
                                string[] lines = text.Split('\n');
                                foreach (string s in lines)
                                {
                                    if (s.Replace(" ", "").Trim().Replace("\r", "").Length < 1)
                                        continue;
                                    BlacklistedStrings.Add("\"" + s.Replace(" ", "").Trim().Replace("\r", "") + "\"");
                                }
                            }
                            break;
                        }
                    case "waitTillFrameEnd":
                    case "relationalOperator":
                    case "operator":
                    case "numberLiteral":
                    case "stringLiteral":
                    case "shortExprOperator":
                    case "isString":
                    case "gscForFunction":
                    case "hashedString":
                    case "getAnimation":
                    case "animTree":
                        break; //ignore
                    default:
                        {
                            //if(node.Token != null && node.Term == GSC2Grammar.identifier)
                            //{
                            //    if(IsRef)
                            //    {

                            //    }
                            //    else
                            //    {

                            //    }
                            //}
                            foreach(var child in node.ChildNodes)
                            {
                                IterateNode(function, INode, child, HasChild, IsRef);
                            }
                            break;
                        }
                }
            }
            catch
            {
                This.scout(node.ToString());
                throw new Exception();
            }
            return 0;
        }

        public int CountForeachRefs(string identifier, ParseTreeNode node)
        {
            int count = 0;
            foreach(var nod in node.ChildNodes)
            {
                count += CountForeachRefs(identifier, nod);
            }
            if(node.Token != null)
            {
                if (node.FindTokenAndGetText().ToLower() == identifier.ToLower())
                    count++;
            }
            return count;
        }


        private void ICollect(ParseTreeNode node, string function, string type)
        {
            if (node.Token == null)
                return;
            switch(type)
            {
                case "function":
                    {
                        if(FunctionNames.Contains(node.Token.Text.ToLower()))
                            FunctionRefs[function].Add(node.Token);
                        break;
                    }
                case "local":
                    {
                        if (node.Token.Text.ToLower() != "level" && node.Token.Text.ToLower() != "self" && node.Token.Text.ToLower() != "game" && node.Token.Text.ToLower() != "true" && node.Token.Text.ToLower() != "false" && node.Token.Text.ToLower() != "undefined")
                        {
                            FunctionLocals[function].Add(node.Token);
                        } 
                        break;
                    }
                case "global":
                    {
                        if (node.Token.Text.ToLower() != "level" && node.Token.Text.ToLower() != "self" && node.Token.Text.ToLower() != "game")
                            Globals.Add(node.Token);
                        break;
                    }
                case "string":
                    {
                        node.Token.Value = node.Token.Text;
                        StringLiterals.Add(node.Token);
                        break;
                    }
            }
        }

        public bool ICompile()
        {
            if(This.SHOW_COUNT)
            {
                foreach(string key in GlobalUses.Keys)
                {
                    This.scout(key + " -> " + GlobalUses[key], "Usage Count");
                }
            }
            This.scout("Compiling file...");
            if(!This.PC)
            {
                string result = new GameScriptCompiler_v3.ExternalEntry().Compile(path.Substring(0, path.LastIndexOf(".gsc")) + ".txt", Available_keys, This);
                if (result != "")
                {
                    This.scout(result, "Compilation error");
                    return false;
                }
            }
            else
            {
                string result = new GameScriptCompiler_v3_pc.ExternalEntry().Compile(path.Substring(0, path.LastIndexOf(".gsc")) + ".txt", Available_keys, This);
                if (result != "")
                {
                    This.scout(result, "Compilation error");
                    return false;
                }
            }
            if(This.Clean)
            {
                File.Delete(path.Substring(0, path.LastIndexOf(".gsc")) + ".txt");
            }
            return true;
        }

        public bool IObfuscate()
        {
            if (!This.Symbolize)
                return true;
            foreach(var kvp in whitelist)
            {
                while(ReplacedStrings.Contains(kvp.Value.ToLower()))
                {
                    ReplacedStrings.Remove(kvp.Value.ToLower());
                }
            }
            foreach(string s in functionwhitelist)
            {
                while (ReplacedStrings.Contains(s.ToLower()))
                {
                    ReplacedStrings.Remove(s.ToLower());
                }
            }
            This.scout("Symbolizing GSC Structure...");
            List<byte> bytes = File.ReadAllBytes(path).ToList();
            int index = 0x40;
            index = ReadStringFromByteArray(bytes.ToArray(), index); //Skip name
            int numofincludes = BitConverter.ToInt16(BitConverter.GetBytes(bytes[0x3C]).ToArray<byte>(), 0);
            int EndOfStrings = -1;
            if(This.PC)
            {
                EndOfStrings = BitConverter.ToInt32(bytes.GetRange(0xC, sizeof(int)).ToArray<byte>(), 0);
            }
            else
            {
                EndOfStrings = BitConverter.ToInt32(bytes.GetRange(0xC, sizeof(int)).Reverse<byte>().ToArray<byte>(), 0);
            }
            for (int i = 0; i < numofincludes; i++)
            {
                index = ReadStringFromByteArray(bytes.ToArray(), index); //Skip includes
            }
            while( index < EndOfStrings)
            {
                int ogindex = index;
                index = ReadStringFromByteArray(bytes.ToArray(), index);
                if (StringFromBytes.ToLower() == "serioushd") //Leave the watermark :P
                    continue;
                if(ReplacedStrings.Contains(StringFromBytes))
                {
                    for(int i = 0; i < StringFromBytes.Length; i++)
                    {
                        bytes[ogindex + i] = (byte)(chars.IndexOf((char)bytes[ogindex + i]) + 1);
                    }
                }
            }
            File.WriteAllBytes(path, bytes.ToArray());
            return true;
        }

        public object CountGlobals(ParseTreeNode node, string type)
        {
            if(type == "global")
            {
                if (!GlobalUses.Keys.Contains(node.Token.Text))
                    GlobalUses[node.Token.Text] = 0;
                GlobalUses[node.Token.Text]++;
            }
            return null;
        }

        public int CountStringTokens(List<Token> tokens, string match)
        {
            int count = 0;
            foreach(Token t in tokens)
            {
                if ((string)t.Value == match)
                    count++;
            }
            return count;
        }

        public bool IReplaceStrings()
        {
            This.scout("Replacing string literals...");
            local_excludes.Clear();
            foreach (string s in BlacklistedStrings)
            {
                foreach (Token t in StringLiterals)
                {
                    if (s != t.Text)
                        continue;
                    t.Value = t.Text;
                    if (StringLexicon.Keys.Contains(t.Value as string))
                    {
                        continue;
                    }
                    int count = CountStringTokens(StringLiterals, t.Value as string);
                    string key = null;
                    foreach (string str in GlobalUses.Keys)
                    {
                        if (local_excludes.Contains(str))
                            continue;
                        if (str.Length > 3)
                            continue; //Only use our newly created values to be safe :)
                        if (GlobalUses[str] + count <= STRING_REF_MAX)
                        {
                            key = str;
                            break;
                        }
                    }
                    int i = GlobalUses.Count;
                    if (key == null)
                    {
                        GlobalUses["a" + chars[i / chars.Length] + chars[i % chars.Length]] = count;
                        key = "a" + chars[i / chars.Length] + chars[i % chars.Length];
                        while (local_excludes.Contains(key))
                        {
                            i++;
                            GlobalUses["a" + chars[i / chars.Length] + chars[i % chars.Length]] = count;
                            key = "a" + chars[i / chars.Length] + chars[i % chars.Length];
                        }
                    }
                    else
                        GlobalUses[key] += count;
                    local_excludes.Add(key);
                    ReplacedStrings.Add(key);
                    StringLexicon[t.Value as string] = key;
                }
            }
            foreach(Token t in StringLiterals)
            {
                if (!StringLexicon.Keys.Contains(t.Value as string))
                    continue;
                t.Value = "\"" + StringLexicon[t.Value as string] + "\"";
            }
            return true;
        }

        #endregion

        /// <summary>
        /// Check Syntax Wrapper for this thread
        /// </summary>
        /// <returns>Bool success</returns>
        private bool CheckSyntax()
        {
            This.scout("Checking Syntax...");
            path = This.PATH;
            finalstring = "";
            foreach (string s in This.allfiletext)
                finalstring += s + " ";
            if (!checksyntax(finalstring))
            {
                This.scout("Your GSC seems to have a syntax error. Please check syntax before using this program", "Error");
                return false;
            }
            This.scout("Syntax is correct");
            return true;
        }

        /// <summary>
        /// Process the default protections header if user allows
        /// </summary>
        /// <returns>Bool success</returns>
        private bool Defaults()
        {
            if (This.Defaults)
            {
                This.scout("Inserting Default Protections");
                var assembly = Assembly.GetExecutingAssembly();
                using (Stream stream = assembly.GetManifestResourceStream("GSC2SO.DefaultLists.txt"))
                using (StreamReader reader = new StreamReader(stream))
                {
                    string result = reader.ReadToEnd();
                    List<string> lines = result.Split('\n').ToList();
                    foreach (string line in lines)
                    {
                        line.Replace("\r", "");
                        ProcessPT(line);
                    }
                }
                //WhiteListedFunctions
                using (Stream stream = assembly.GetManifestResourceStream("GSC2SO.functions.txt"))
                using (StreamReader reader = new StreamReader(stream))
                {
                    string result = reader.ReadToEnd();
                    List<string> lines = result.Split('\n').ToList();
                    foreach (string line in lines)
                    {
                        functionwhitelist.Add(line.Replace("\r", ""));
                    }
                }
                This.scout("Insertion successful");
            }
            return true;
        }

        #region Utility

        /// <summary>
        /// Reset the global variables to default values
        /// </summary>
        public void ResetGlobals()
        {
            whitelist = new List<KeyValuePair<string, string>>();
            blacklist = new List<KeyValuePair<string, string>>();
            stringblacklist = new List<string>();
            functionwhitelist = new List<string>();
            FunctionNames = new List<string>();
            FunctionArguments = new Dictionary<string, List<Token>>();
            FunctionLocals = new Dictionary<string, List<Token>>();
            FunctionRefs = new Dictionary<string, List<Token>>();
            FunctionRefLexicon = new Dictionary<string, string>();
            Uses = new Dictionary<int, int>();
            local_excludes = new List<string>();
            FunctionLexicon = new Dictionary<string, Dictionary<string, string>>();
            FunctionUses = new Dictionary<string, Dictionary<string, int>>();
            GlobalLexicon = new Dictionary<string, string>();
            Globals = new List<Token>();
            GlobalUses = new Dictionary<string, int>();
            IdentifierTokens = new List<ParseTreeNode>();
            ReplacedStrings = new List<string>();
            WhiteListedFunctions = new List<string>();
            BlacklistedStrings = new List<string>();
            StringLexicon = new Dictionary<string, string>();
            StringLiterals = new List<Token>();
            Available_keys = new Dictionary<string, List<string>>();
            Foreach_Ref_count = new Dictionary<Token, int>();
            Function_Foreach_Count = new Dictionary<string, int>();
        }

        /// <summary>
        /// Read a string from the given byte array and set the 'StringFromBytes' global to result
        /// </summary>
        /// <param name="array">Byte array to read from</param>
        /// <param name="index">Index starting location</param>
        /// <returns>Final Index Position after NULL</returns>
        public static int ReadStringFromByteArray(byte[] array, int index)
        {
            string toReturn = "";
            if (index < 0)
            {
                throw new Exception();
            }
            for (; index < array.Length; index++)
            {
                if (array[index] == 0x00)
                    break;
                toReturn += (char)array[index];
            }
            StringFromBytes = toReturn;
            return index + 1;
        }

        /// <summary>
        /// Check the file's basic syntax
        /// </summary>
        /// <param name="data">File data</param>
        /// <returns>Bool Success</returns>
        private bool checksyntax(string data)
        {
            var gameScript = new GSCGrammar();
            var parser = new Parser(gameScript);
            ParseTree tree = parser.Parse(data);
            if (tree.ParserMessages.Count > 0)
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// Proccess Forceaway
        /// </summary>
        /// <param name="data">Forceaway parameter</param>
        public void ProcessFA(string data)
        {
            data = data.Trim();
            string dat = data.Split(' ')[data.Split(' ').Length - 1];
            if (dat.IndexOf(".") < 1)
            {
                blacklist.Add(new KeyValuePair<string, string>("*", dat.Substring(dat.IndexOf(".") + 1)));
            }
            else if (dat.IndexOf(".") == dat.Length - 1)
            {
                blacklist.Add(new KeyValuePair<string, string>(dat.Substring(0, dat.IndexOf(".")), "*"));
            }
            else
            {
                blacklist.Add(new KeyValuePair<string, string>(dat.Substring(0, dat.IndexOf(".")), dat.Substring(dat.IndexOf(".") + 1)));
            }
        }

        /// <summary>
        /// Process Stringaway
        /// </summary>
        /// <param name="data">Stringaway parameter</param>
        public void ProcessSA(string data)
        {
            string str = data.Split(' ')[data.Split(' ').Length - 1];
            stringblacklist.Add(str);
        }

        /// <summary>
        /// Process protectfunction
        /// </summary>
        /// <param name="data">Function parameter</param>
        public void ProcessPF(string data)
        {
            string str = data.Split(' ')[data.Split(' ').Length - 1];
            functionwhitelist.Add(str);
        }

        /// <summary>
        /// Process Protect
        /// </summary>
        /// <param name="data">Protection Parameter</param>
        public void ProcessPT(string data)
        {
            data = data.Trim();
            string dat = data.Split(' ')[data.Split(' ').Length - 1];
            if (dat.IndexOf(".") < 1)
            {
                whitelist.Add(new KeyValuePair<string, string>("*", dat.Substring(dat.IndexOf(".") + 1)));
            }
            else if (dat.IndexOf(".") == dat.Length - 1)
            {
                whitelist.Add(new KeyValuePair<string, string>(dat.Substring(0, dat.IndexOf(".")), "*"));
            }
            else
            {
                whitelist.Add(new KeyValuePair<string, string>(dat.Substring(0, dat.IndexOf(".")), dat.Substring(dat.IndexOf(".") + 1)));
            }
        }

    }
        #endregion
}
