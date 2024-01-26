using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.DesignScript.Runtime;
using Autodesk.DesignScript.Geometry;
using Autodesk.DesignScript.Interfaces;
using CoreNodeModels;
using DSRevitNodesUI;
using Dynamo.Graph.Nodes;
using Newtonsoft.Json;
using ProtoCore.AST.AssociativeAST;
using Revit.Elements;
using Revit.GeometryConversion;
using RevitServices.Persistence;
using RevitServices.Transactions;

namespace ISL_ZeroTouch
{
   /// <summary>
   /// Custom string manipulation methods.
   /// </summary>
    public class Strings
    {

        #region Private Methods
        private Strings()
        {
                
        }
        #endregion


        #region Public Methods

        /// <summary>
        /// String node that trims a string representation of a number to 2 figures.
        /// </summary>
        /// <param name="stringOfNumbers"></param>
        /// <returns> List of formatted numbers as string.</returns>
        public static List<string> TrimStringToTwoFigures(List<string> stringOfNumbers)
        {
            if (!(stringOfNumbers?.Any() ?? false)) return new List<string>();

            var trimmedStrings = new List<string>();

            int startIndex = 0;
            int charsToKeep = 2;

            foreach (var str in stringOfNumbers)
            {
                if (str.Length <= 2)
                {
                    trimmedStrings.Add(str);
                }
                else
                {
                    trimmedStrings.Add(str.Substring(startIndex, charsToKeep));
                }
            }
            return trimmedStrings;
        }

        #endregion
    }
}
