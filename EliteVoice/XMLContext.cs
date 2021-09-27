using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.XPath;



namespace EliteVoice
{
    class XMLContext
    {
        public static XMLContext instance { get; } = new XMLContext();

        private IDictionary<string, XPathExpression> expressions = new Dictionary<string, XPathExpression>();

        //
        public XPathExpression getXPathExpression(string expression)
        {
            XPathExpression result = null;
            if (!expressions.ContainsKey(expression))
            {
                result = XPathExpression.Compile(expression);
                expressions.Add(expression, result);
            } else
            {
                result = expressions[expression];
            }
            return result;
        }


        public string XEvaluateSting(XmlNode node,  String expression)
        {
            string result = null;
            /*
            switch (expression.ReturnType)
            {
                case XPathResultType.Number:
                case XPathResultType.String:
                    result = "" + navigator.Evaluate(expression);
                    break;
                case XPathResultType.Boolean:
                    result = ((bool)navigator.Evaluate(expression)) ? "true" : "false";
                    break;
                case XPathResultType.NodeSet:
                    XPathNodeIterator iter = (XPathNodeIterator)navigator.Select(expression);
                    result = null;
                    break;

            }
            */
            return result;
        }

        public string EvaluateSting(XPathExpression expression, XPathNavigator navigator)
        {
            string result = null;
            switch (expression.ReturnType)
            {
                case XPathResultType.Number:
                case XPathResultType.String:
                    result = "" + navigator.Evaluate(expression);
                    break;
                case XPathResultType.Boolean:
                    result = ((bool)navigator.Evaluate(expression))?"true":"false";
                    break;
                case XPathResultType.NodeSet:
                    XPathNodeIterator iter = (XPathNodeIterator)navigator.Select(expression);
                    result = null;
                    break;

            }
            return result;
        }
        public XPathNodeIterator EvaluateNodeSet(XPathExpression expression, XPathNavigator navigator)
        {
            XPathNodeIterator result = null;
            if (expression.ReturnType == XPathResultType.NodeSet)
            {
                result = navigator.Select(expression);
            }
            return result;
        }
        public bool EvaluateBoolean(XPathExpression expression, XPathNavigator navigator)
        {
            bool result = false;
            if (expression.ReturnType == XPathResultType.Boolean)
            {
                result = (bool)navigator.Evaluate(expression);
            }
            return result;
        }
        /*
        public XPathNavigator getCurrentNavigator()
        {

        }
        */
    }
}


