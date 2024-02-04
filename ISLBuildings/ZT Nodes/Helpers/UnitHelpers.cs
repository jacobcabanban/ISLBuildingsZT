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
        /// Convert MM to equivalent length in project units.
        /// </summary>
        /// <param name="vals">List of values in MM.</param>
        /// <returns>Values converted to project units.</returns>
        public static ICollection<double> ConvertMMtoProjectUnits(List<double> vals)
        {
            if (!(vals?.Any() ?? false))
            {
                throw new ArgumentException($"Invalid argument.", nameof(vals));
            }

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


        /// <summary>
        /// Convert value in MM to decimal feet.
        /// </summary>
        /// <param name="vals">The value to convert.</param>
        /// <returns>Values in decimal feet.</returns>
        public static List<double> LengthFromMM(List<double> vals)
            => vals.Select(val => UnitUtils.ConvertToInternalUnits(val, UnitTypeId.Millimeters)).ToList();


        /// <summary>
        /// Convert value in decimal feet to MM.
        /// </summary>
        /// <param name="vals">The value to convert.</param>
        /// <returns>Values in MM.</returns>
        public static List<double> LengthToMM(List<double> vals)
            => vals.Select(val => UnitUtils.ConvertFromInternalUnits(val, UnitTypeId.Millimeters)).ToList();


        /// <summary>
        /// Converts a list of values (double) from MM to length equivalent in the project's unit settings.
        /// </summary>
        /// <param name="vals">List of values in MM.</param>
        /// <returns>Values converted.</returns>
        public static ICollection<double> ForceMMToProject(List<double> vals)
        {
            if (!(vals?.Any() ?? false))
            {
                throw new ArgumentException($"Invalid argument.", nameof(vals));
            }

            var lengthUnitString = ProjectInfo.LengthUnitDisplay();

            if (lengthUnitString != "Millimeters" && lengthUnitString != "Feet and fractional inches")
            {
                throw new ArgumentException("Invalid unit settings in ISL template.", nameof(lengthUnitString));
            }

            if (lengthUnitString == "Millimeters")
            {
                return vals;
            }
            else
            {
                return vals.Select(x => UnitUtils.ConvertToInternalUnits(x, UnitTypeId.Millimeters)).ToList();         
            }
            
        }
        #endregion
    }
}
