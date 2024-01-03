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
    /// Retrieves Revit link instance information.
    /// </summary>
    public class LinkedInstance
    {

        #region Private Methods

        /// <summary>
        ///  Constructor
        /// </summary>
        private LinkedInstance()
        {

        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Retrieves a Revit link instance from the current document.
        /// </summary>
        /// <returns>Link Name, Link Instance (Wrapped), Link Instance Document (Unwrapped)</returns>
        [MultiReturn(new[]{"Link Name", "Link Instance", "Link Document"})]
        public static Dictionary<string, object> CollectLinkInstance()
        {
            // Get current document
            Autodesk.Revit.DB.Document doc = DocumentManager.Instance.CurrentDBDocument;

            // Retrieve the link instance
            var linkInstance = new FilteredElementCollector(doc)
                .OfCategory(BuiltInCategory.OST_RvtLinks)
                .WhereElementIsNotElementType()
                .Cast<RevitLinkInstance>()
                .FirstOrDefault();

            // Wrap link instance 
            var wrappedLinkInstance = linkInstance.ToDSType(false);

            // Retrieve name of link instance
            var linkName = linkInstance.Name;

            // Retrieve document from link instance
            var linkDocument = linkInstance.GetLinkDocument();

            return new Dictionary<string, object>
            {
                {"Link Name", linkName },
                {"Link Instance", wrappedLinkInstance },
                {"Link Document", linkDocument}
            };
         
        }


        /// <summary>
        /// Retrieves the elements from a linked instance of a given category.
        /// </summary>
        /// <param name="linkInstance">The linked instance (RevitLinkInstance Class).</param>
        /// <param name="category">Category of element to collect.</param>
        /// <returns>Elements of the linked instance belonging to specified category.</returns>
        public static IEnumerable<Revit.Elements.Element> CollectLinkInstanceElements(
            Revit.Elements.Element linkInstance, 
            Revit.Elements.Category category)
        {
            // Get current document
            Autodesk.Revit.DB.Document doc = DocumentManager.Instance.CurrentDBDocument;

            // Unwrap linked instance and cast 
            var unwrappedLinkInstance = linkInstance.UnwrapElement() as RevitLinkInstance;
       
            // Element Id of link instance
            var unwrappedLinkInstanceID = unwrappedLinkInstance.Id;

            // Unwrap category
            var categoryDB = Autodesk.Revit.DB.Category.GetCategory(doc, (BuiltInCategory)category.Id);

            // Retrieve BuiltInCategory from category
            var builtInCategory = categoryDB.BuiltInCategory;

            // Retrieve document of linked instance
            var linkDocument = unwrappedLinkInstance.GetLinkDocument();

            // Collect all elements by category and wrap
            var elementCategory = new FilteredElementCollector(linkDocument)
                .OfCategory(builtInCategory)
                .WhereElementIsNotElementType()
                .Select(x => x.ToDSType(false))
                .ToList();

            return elementCategory;

        }

        #endregion

    }
}
