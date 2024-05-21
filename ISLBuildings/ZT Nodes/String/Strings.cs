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
        /// <param name="inputStrings"></param>
        /// <returns> List of formatted numbers as string.</returns>
        public static List<string> TrimStringToTwoFigures(List<string> inputStrings)
        {
            if (!(inputStrings?.Any() ?? false)) return new List<string>();

            List<string> processedStrings = new List<string>();

            foreach (string inputString in inputStrings)
            {
                int periodIndex = inputString.IndexOf(".");

                if (periodIndex != -1)
                {
                    // Get substring from the left of the period up to the period
                    string leftSubstring = inputString.Substring(0, periodIndex + 1);

                    // Get two characters after the period (or pad with 0 if not available)
                    string rightSubstring = periodIndex + 3 <= inputString.Length ?
                        inputString.Substring(periodIndex + 1, 2) :
                        inputString.Substring(periodIndex + 1).PadRight(2, '0');

                    // Concatenateleft and right substrings
                    string processedString = leftSubstring + rightSubstring;
                    processedStrings.Add(processedString);
                }
                else
                {
                    // If period not found, just add the original string to the processed list
                    processedStrings.Add(inputString);
                }
            }
            return processedStrings;

        }

        #endregion
    }
}
