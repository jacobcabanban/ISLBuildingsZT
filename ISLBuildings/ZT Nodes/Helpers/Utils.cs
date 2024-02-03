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
using Revit.Elements;
using Revit.GeometryConversion;
using RevitServices.Transactions;
using RevitServices.Persistence;

namespace ISL_ZeroTouch
{
    /// <summary>
    /// Various utility/helper methods.
    /// </summary>
    [IsVisibleInDynamoLibrary(false)]
    public class Utils
    {
        #region Public Methods

        /// <summary>
        /// Method that collects all elements of a particular Revit DB type
        /// from a Revit DB view element (Inherits from the View class).
        /// </summary>
        /// <typeparam name="T"> Any Revit DB type. </typeparam>
        /// <param name="doc"> Revit model document. </param>
        /// <param name="viewId"> Element Id of view. </param>
        /// <returns></returns>
        public static List<ElementId> GetIdOfElementsInView<T>(Document doc, ElementId viewId) 
                                                            where T : Autodesk.Revit.DB.Element
        {
            var collector = new FilteredElementCollector(doc, viewId);

            return collector.OfClass(typeof(T))
                            .WhereElementIsNotElementType()
                            .Cast<T>()
                            .Select(x => x.Id)
                            .ToList();
        }

        /// <summary>
        /// Convert value in MM to decimal feet.
        /// </summary>
        /// <param name="val">The value to convert.</param>
        /// <returns>Values in decimal feet.</returns>
        public static double LengthFromMM(double val)
            => UnitUtils.ConvertToInternalUnits(val, UnitTypeId.Millimeters);

        /// <summary>
        /// Convert value in decimal feet to MM.
        /// </summary>
        /// <param name="val">The value to convert.</param>
        /// <returns>Values in MM.</returns>
        public static double LengthToMM(double val)
            => UnitUtils.ConvertFromInternalUnits(val, UnitTypeId.Millimeters);


        #endregion
    }
}
