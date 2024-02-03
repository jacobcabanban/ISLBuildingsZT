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
    /// Methods that extract information from the project.
    /// </summary>
    public class ProjectInfo
    {
        #region Private Methods
        private ProjectInfo()
        {
                
        }
        #endregion


        #region Public Methods
        /// <summary>
        /// Displays the unit system for length used in the current project.
        /// </summary>
        /// <returns></returns>
        public static string LengthUnitDisplay()
        {
            var doc = DocumentManager.Instance.CurrentDBDocument;

            // Get the Units object from the document
            Units projectUnits = doc.GetUnits();

            // Get the FormatOptions for length from the Units object
            FormatOptions lengthFormatOptions = projectUnits.GetFormatOptions(SpecTypeId.Length);

            // Get the ForgeTypeId representing the unit system used for lengths
            ForgeTypeId lengthUnitsTypeId = lengthFormatOptions.GetUnitTypeId();

            // Convert the ForgeTypeId to a readable string
            string unitsAsString = LabelUtils.GetLabelForUnit(lengthUnitsTypeId);

            return unitsAsString;
        }

        #endregion
    }
}
