using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Lab1
{
    public enum NodeType
    {
        Base,
        File,
        Def,
        Cond,
        Expr,
        Fun,
        Sym,
        Val,
        List,
        Vector,
        Map,
        KeyValuePair,
        KeyWord
    }

    [Flags]
    public enum SymType
    {
        Nil = 0,
        Int = 1,
        Float = 2,
        Num = 3,
        Bool = 4,
        Fun = 8,
        Str = 16,
        Char = 32,
        Sym = 64,
        Vec = 128,
        Key = 256,
        Any = 511
    }


    public class BaseNode
    {
        [JsonIgnore]
        protected static int stId = 0;

        [JsonProperty(PropertyName = "Id", Order = -2)]
        protected int id = stId++ ;

        [JsonProperty(PropertyName = "Type", Order = -2)]
        [JsonConverter(typeof(StringEnumConverter))]
        public NodeType type { get; protected set; }
    }

    public class FileNode : BaseNode
    {
        [JsonProperty("Expressions")]
        public List<ExprNode> Expressions { get; }

        public FileNode(List<ExprNode> nodes)
        {
            type = NodeType.File;
            Expressions = nodes;
        }

    }

    public class ExprNode : BaseNode
    {
        public ExprNode()
        {
            type = NodeType.Expr;
        }
    }

    public class DefNode : ExprNode
    {
        [JsonProperty("DefSymbol")]
        protected SymNode sym;

        [JsonProperty("DefExpression")]
        protected ExprNode defExpr;

        public DefNode(SymNode symNode, ExprNode expr)
        {
            type = NodeType.Def;
            sym = symNode;
            defExpr = expr;
        }
    }

    public class CondNode : ExprNode
    {
        [JsonProperty("CondExpr")]
        protected ExprNode cond;

        [JsonProperty("ThenExpr")]
        protected ExprNode thenExpr;

        [JsonProperty("ElseExpr")]
        protected ExprNode elseExpr;

        public CondNode(ExprNode condNode, ExprNode thenNode, ExprNode elseNode = null)
        {
            type = NodeType.Cond;
            cond = condNode;
            thenExpr = thenNode;
            elseExpr = elseNode;
        }
    }

    public class FunNode : ExprNode
    {
        [JsonProperty("Params")]
        protected List<SymNode> parameters;

        [JsonProperty("MidExpressions")]
        protected List<ExprNode> mids;

        [JsonProperty("LastExpression")]
        protected ExprNode last;

        public FunNode(List<SymNode> paramNodes, ExprNode lastNode, List<ExprNode> midNodes = null)
        {
            type = NodeType.Fun;
            parameters = paramNodes;
            last = lastNode;
            mids = midNodes;
        }
    }

    public class ListNode : ExprNode
    {
        [JsonProperty("Expressions")]
        protected List<ExprNode> expressions;

        public ListNode(List<ExprNode> exprs)
        {
            type = NodeType.List;
            expressions = exprs;
        }
    }

    public class VectorNode : ExprNode
    {
        [JsonProperty("Expressions")]
        protected List<ExprNode> expressions;

        public VectorNode(List<ExprNode> exprs)
        {
            type = NodeType.Vector;
            expressions = exprs;
        }
    }

    public class MapNode : ExprNode
    {
        [JsonProperty("Pairs")]
        protected List<KeyValuePairNode> pairs;

        public MapNode(List<KeyValuePairNode> pairNodes)
        {
            type = NodeType.Map;
            pairs = pairNodes;
        }
    }

    public class SymNode : ExprNode
    {
        [JsonProperty("SymName")]
        protected string symName;

        [JsonProperty("SymValue")]
        protected string symValue;

        public SymNode(string name, string val)
        {
            type = NodeType.Sym;
            symName = name;
            symValue = val;
        }
    }

    public class KeyValuePairNode : BaseNode
    {
        [JsonProperty("Key")]
        protected KeyWordNode key;

        [JsonProperty("Value")]
        protected ExprNode value;

        public KeyValuePairNode(KeyWordNode keyNode, ExprNode valueNode)
        {
            type = NodeType.KeyValuePair;
            key = keyNode;
            value = valueNode;
        }
    }

    public class KeyWordNode : ExprNode
    {
        [JsonProperty("KeyName")]
        protected string keyName;

        public KeyWordNode(string name)
        {
            type = NodeType.KeyWord;
            keyName = name;
        }
    }

    public class LitNode : ExprNode
    {
        [JsonProperty("ValType")]
        [JsonConverter(typeof(StringEnumConverter))]
        protected SymType valType;

        [JsonProperty("Value")]
        protected string litValue;

        public LitNode(SymType vtype, string val)
        {
            type = NodeType.Val;
            valType = vtype;
            litValue = val;
        }
    }

    ///MidNodes

    public class ExpressionsNode : BaseNode
    {
        public List<ExprNode> expressions;

        public ExpressionsNode(List<ExprNode> nodes)
        {
            expressions = nodes;
        }
    }
}
