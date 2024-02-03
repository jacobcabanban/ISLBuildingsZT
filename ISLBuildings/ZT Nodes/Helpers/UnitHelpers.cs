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
    /// A Class that contains methods for unit conversion operations.
    /// </summary>
    public class UnitHelpers
    {

        #region Private Methods
        private UnitHelpers()
        {
            
        }
        #endregion

        #region Public Methods

        /// <summary>
        /// Convert MM to project units.
        /// </summary>
        /// <param name="vals">List of values in MM.</param>
        /// <returns>Values converted to project units.</returns>
        public static ICollection<double> ConvertMMtoProjectUnits(List<double> vals)
        {
            var doc = DocumentManager.Instance.CurrentDBDocument;

            // Get the Units object from the document
            Units projectUnits = doc.GetUnits();

            // Get the FormatOptions for length from the Units object
            FormatOptions lengthFormatOptions = projectUnits.GetFormatOptions(SpecTypeId.Length);

            // Get the ForgeTypeId representing the unit system used for lengths
            ForgeTypeId lengthUnitsTypeId = lengthFormatOptions.GetUnitTypeId();

            // Convert MM to project units
            return vals.Select(val => UnitUtils.Convert(val, UnitTypeId.Millimeters, lengthUnitsTypeId)).ToList();

        }

        #endregion
    }
}
