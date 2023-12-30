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
using Autodesk.Revit.DB.Analysis;

namespace ISL_ZeroTouch
{
    /// <summary>
    /// Provides functionality for various ViewSheet methods and properties.
    /// </summary>
    public class Sheets
    {
        #region Private Methods

        /// <summary>
        /// Constructor
        /// </summary>
        private Sheets()
        {

        }

        #endregion        

        #region Public Methods

        #region AddRevisionToSheet

        /// <summary>
        /// Add selected revision to a collection of sheets.
        /// </summary>
        /// <param name="sheets">The sheet/s from which the revision is added.</param>
        /// <param name="revision">The revision to add.</param>
        /// /// <param name="runNode">Boolean to run node.</param>
        public static void AddRevisionToSheet(
            ICollection<Revit.Elements.Element> sheets,
            Revit.Elements.Element revision,
            bool runNode = false)
        {
            // Get current document
            Document doc = DocumentManager.Instance.CurrentDBDocument;
            
            // Unwrap elements
            var unwrappedSheets = sheets.UnwrapCollection();
            var unwrappedRevision = revision.UnwrapElement();

            // Get element Id of selected revision
            var revisionId = unwrappedRevision.Id;

            foreach (var element in unwrappedSheets)
            {
                // Cast unwrapped element as sheet
                var sheet = element as ViewSheet;

                // Get additional revision Id's
                var additionalRevisionIds = sheet.GetAdditionalRevisionIds();

                if (!additionalRevisionIds.Contains(revisionId) && runNode)
                {
                    // New transaction
                    TransactionManager.Instance.ForceCloseTransaction();
                    TransactionManager.Instance.EnsureInTransaction(doc);

                    // Add revision to existing list
                    additionalRevisionIds.Add(revisionId);

                    // Update revision schedule of current sheet
                    sheet.SetAdditionalRevisionIds(additionalRevisionIds);

                    // End transaction
                    TransactionManager.Instance.TransactionTaskDone();
                }
            }
        }

        #endregion

        #region RemoveRevisionFromSheet

        /// <summary>
        /// Remove selected revision from a collection of sheets.
        /// </summary>
        /// <param name="sheets">The sheet/s from which the revision is removed..</param>
        /// <param name="revision">The revision to remove.</param>
        /// /// <param name="runNode">Boolean to run node.</param>
        public static void DeleteRevisionFromSheet(
            ICollection<Revit.Elements.Element> sheets,
            Revit.Elements.Element revision,
            bool runNode = false)
        {
            // Get current document
            Document doc = DocumentManager.Instance.CurrentDBDocument;

            // Unwrap elements
            var unwrappedSheets = sheets.UnwrapCollection();
            var unwrappedRevision = revision.UnwrapElement();

            // Get element Id of selected revision
            var revisionId = unwrappedRevision.Id;

            foreach (var element in unwrappedSheets)
            {
                // Cast unwrapped element as sheet
                var sheet = element as ViewSheet;

                // Get additional revision Id's
                var allRevisionIds = sheet.GetAllRevisionIds();

                if (allRevisionIds.Contains(revisionId) && runNode)
                {
                    // New transaction
                    TransactionManager.Instance.ForceCloseTransaction();
                    TransactionManager.Instance.EnsureInTransaction(doc);

                    // Add revision to existing list
                    allRevisionIds.Remove(revisionId);

                    // Update revision schedule of current sheet
                    sheet.SetAdditionalRevisionIds(allRevisionIds);

                    // End transaction
                    TransactionManager.Instance.TransactionTaskDone();
                }
            }
        }

        #endregion

        #endregion

    }
}
