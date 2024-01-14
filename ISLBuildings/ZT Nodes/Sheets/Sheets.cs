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

        #region Add Revision to Sheet

        /// <summary>
        /// Add selected revision to a collection of unwrappedSheets.
        /// </summary>
        /// <param name="sheets">The sheet/s from which the revision is added.</param>
        /// <param name="revision">The revision to add.</param>
        /// /// <param name="isRun">Boolean to run node.</param>
        public static void AddRevisionToSheet(
            ICollection<Revit.Elements.Element> sheets,
            Revit.Elements.Element revision,
            bool isRun = false)
        {
            // Get current document
            Document doc = DocumentManager.Instance.CurrentDBDocument;

            // Unwrap elements
            var unwrappedSheets = sheets.UnwrapCollection();
            var unwrappedRevision = revision.UnwrapElement();

            // Get element Id of selected revision
            var revisionId = unwrappedRevision.Id;

            if (!(unwrappedSheets?.Any() ?? false)) return;
            if (unwrappedRevision == null) return;

            try
            {
                TransactionManager.Instance.EnsureInTransaction(doc);
                foreach (var element in unwrappedSheets)
                {
                    // Cast unwrapped element as sheet
                    var sheet = element as ViewSheet;

                    // Get additional revision Id's
                    var additionalRevisionIds = sheet.GetAdditionalRevisionIds();

                    if (!additionalRevisionIds.Contains(revisionId) && isRun)
                    {
                        // Add revision to existing list
                        additionalRevisionIds.Add(revisionId);

                        // Update revision schedule of current sheet
                        sheet.SetAdditionalRevisionIds(additionalRevisionIds);
                    }
                }

                TransactionManager.Instance.TransactionTaskDone();
            }
            catch (Exception ex)
            {
                TransactionManager.Instance.ForceCloseTransaction();
                throw new Exception($"ISL_ZeroTouch.AddRevisionToSheet: {ex.Message}");
            }
            
        }

        #endregion


        #region Remove Revision from Sheet

        /// <summary>
        /// Remove selected revision from a collection of unwrappedSheets.
        /// </summary>
        /// <param name="_sheets">The sheet/s from which the revision is removed..</param>
        /// <param name="_revision">The revision to remove.</param>
        /// /// <param name="isRun">Boolean to run node.</param>
        public static void DeleteRevisionFromSheet(
            ICollection<Revit.Elements.Element> _sheets,
            Revit.Elements.Element _revision,
            bool isRun = false)
        {
            // Get current document
            Document doc = DocumentManager.Instance.CurrentDBDocument;

            // Unwrap sheet elements and cast as a Revit DB ViewSheet
            var unwrappedSheets = _sheets.UnwrapCollection()
                                         .Cast<ViewSheet>()
                                         .ToList();

            // Unwrap revision element to delete and cast as a Revit DB Revision
            var unwrappedRevision = _revision.UnwrapElement() as Autodesk.Revit.DB.Revision;

            // Revision to delete casted as Revit DB Element Id
            var revisionIdToDelete = unwrappedRevision.Id;

            // Empty list containining all revisions elements (revisions or clouds) to delete 
            var revisionIDsToDelete = new List<ElementId>();

            if (!(unwrappedSheets?.Any() ?? false)) return;
            if (unwrappedRevision == null) return;

            try
            {
                TransactionManager.Instance.EnsureInTransaction(doc);

                // Update revision properties
                unwrappedRevision.Visibility = RevisionVisibility.CloudAndTagVisible;
                unwrappedRevision.Issued = false;

                doc.Regenerate(); // Regenerate the document to update changes

                // Deletes revision clouds on views placed on unwrappedSheets
                #region Revision Clouds from Other Views 

                // Remove revision clouds from the views that are on the current sheet, if any
                foreach (var sheet in unwrappedSheets)
                {
                    // Empty list of revision cloud Ids (to delete)
                    var revisionCloudIDsToDeleteFromViews = new List<ElementId>();

                    // Get view Id's of views currently placed on sheet (if any)
                    var viewIdsOnSheet = sheet.GetAllPlacedViews().ToList();

                    if (!(viewIdsOnSheet?.Any() ?? false)) continue;

                    // Get view elements from their Id's
                    var viewElementsOnSheet = viewIdsOnSheet.Select(x => doc.GetElement(x)).ToList();

                    foreach (var viewId in viewIdsOnSheet)
                    {
                        // Get all revision cloud Id's in current view
                        var revisionCloudIDsOnSheet = Utils.GetIdOfElementsInView
                            <Autodesk.Revit.DB.RevisionCloud>(doc, viewId);

                        if (!(revisionCloudIDsOnSheet?.Any() ?? false)) continue;

                        // Get all  revision cloud Id's that match the revision to delete
                        revisionCloudIDsToDeleteFromViews.AddRange(revisionCloudIDsOnSheet
                                                         .Select(x => doc.GetElement(x))
                                                         .Cast<Autodesk.Revit.DB.RevisionCloud>()
                                                         .Where(x => x.RevisionId == revisionIdToDelete)
                                                         .Select(x => x.Id));
                    }

                    if (revisionCloudIDsToDeleteFromViews?.Any() ?? false)
                    {
                        // Append element Id's
                        revisionIDsToDelete.AddRange(revisionCloudIDsToDeleteFromViews);
                    }

                }

                #endregion

                // Deletes revision clouds placed directly on unwrappedSheets
                #region Revision Clouds On Sheets 

                foreach (var sheet in unwrappedSheets)
                {
                    // Empty list of revision cloud Ids (to delete)
                    var revisionCloudIdsOnSheetToDelete = new List<ElementId>();

                    // Get all revision cloud Id's that match revision to delete
                    revisionCloudIdsOnSheetToDelete = new FilteredElementCollector(doc, sheet.Id)
                                                            .OfCategory(BuiltInCategory.OST_RevisionClouds)
                                                            .WhereElementIsNotElementType()
                                                            .Cast<Autodesk.Revit.DB.RevisionCloud>()
                                                            .Where(x => x.RevisionId == revisionIdToDelete)
                                                            .Select(x => x.Id)
                                                            .ToList();

                    // Append element Id's
                    if (revisionCloudIdsOnSheetToDelete?.Any() ?? false)
                    {
                        revisionIDsToDelete.AddRange(revisionCloudIdsOnSheetToDelete);
                    }

                }

                #endregion

                // Delete revision numbers added explicitly to unwrappedSheets
                #region Additional Revisions on Sheet

                foreach (var sheet in unwrappedSheets)
                {
                    // Get additional revisions on sheet, if any
                    var extraRevisionIdsOnSheet = sheet.GetAdditionalRevisionIds()?
                                                       .ToList() ?? new List<ElementId>();

                    if (!extraRevisionIdsOnSheet.Any()) continue;

                    // Create a new list of revisions without the selected revision 
                    var newExtraRevisionsOnSheet = extraRevisionIdsOnSheet
                                                       .Except(new List<ElementId> { revisionIdToDelete })
                                                       .ToList();

                    // Set the updated additional revision ids
                    if (!extraRevisionIdsOnSheet.SequenceEqual(newExtraRevisionsOnSheet) && isRun)
                    {
                        sheet.SetAdditionalRevisionIds(newExtraRevisionsOnSheet);
                    }

                }

                #endregion

                // Delete revision elements from unwrappedSheets
                if (revisionIDsToDelete?.Any() ?? false && isRun)
                {
                    foreach (var revIds in revisionIDsToDelete)
                    {
                        doc.Delete(revIds);
                    }
                }

                TransactionManager.Instance.TransactionTaskDone();
            }
            catch (Exception ex)
            {
                TransactionManager.Instance.ForceCloseTransaction();
                throw new Exception($"An error occured in ISL_ZeroTouch.DeleteRevisionFromSheet: {ex.Message}");
            }
        }

        #endregion

        #endregion

    }
}
