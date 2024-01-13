namespace ISL_ZeroTouch
{
    using System.Collections.Generic;
    using System.Linq;
    using Autodesk.DesignScript.Runtime;

    /// <summary>
    /// Extension methods that unwrap Revit element/s selected in Dynamo.
    /// </summary>
    [IsVisibleInDynamoLibrary(false)]
    public static class Unwrapper
    {

        #region Public Methods

        /// <summary>
        /// Extension method to unwrap a single Dynamo Revit element.
        /// </summary>
        /// <param name="wrappedElement"> A Revit element from Dynamo selection. </param>
        /// <returns> Collection of unwrapped Revit elements. </returns>
        public static Autodesk.Revit.DB.Element UnwrapElement(
            this Revit.Elements.Element wrappedElement) => wrappedElement.InternalElement;
       

        /// <summary>
        /// Extension method to unwrap a list of Dynamo Revit elements.
        /// </summary>
        /// <param name="wrappedList"> A list of Revit element/s from Dynamo selection. </param>
        /// <returns></returns>
        public static IEnumerable<Autodesk.Revit.DB.Element> UnwrapCollection(
            this ICollection<Revit.Elements.Element> wrappedList)
            => wrappedList.Select(x => x.InternalElement);
        
        #endregion

    }
}
