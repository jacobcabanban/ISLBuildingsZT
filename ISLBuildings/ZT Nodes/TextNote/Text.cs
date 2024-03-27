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
    /// Class containing method to create text notes on plan.
    /// </summary>
    public class Text
    {

        #region Private Methods
        /// <summary>
        /// Constructor
        /// </summary>
        private Text()
        {

        }
        #endregion


        #region Public Methods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="viewDyn">The view where the texts will be created.</param>
        /// <param name="positionsDyn">The position (coordinate) of each text.</param>
        /// <param name="text">The text to insert.</param>
        /// <param name="textTypeDyn">Family type of text.</param>
        public static void CreateTextNote(Revit.Elements.Element viewDyn, 
            List<Autodesk.DesignScript.Geometry.Point> positionsDyn, 
            List<string> text, 
            Revit.Elements.Element textTypeDyn)
        {
            // Get current document/UI.
            var doc = DocumentManager.Instance.CurrentDBDocument;

            List<XYZ> pointsXYZ = positionsDyn.Select(x => x.ToXyz()).ToList();

            // Unwrap
            var viewId = viewDyn.UnwrapElement().Id;
            var textTypeId = textTypeDyn.UnwrapElement().Id;

            if (positionsDyn.Count != text.Count)
            {
                throw new ArgumentException("Position and text lists must have the same length.");
            }

            // New transaction
            TransactionManager.Instance.ForceCloseTransaction();
            TransactionManager.Instance.EnsureInTransaction(doc);

            for (int i = 0; i < pointsXYZ.Count(); i++)
            {
                Autodesk.Revit.DB.TextNote.Create(doc, viewId, pointsXYZ[i],
                    text[i], textTypeId);
            }

            // End transaction
            TransactionManager.Instance.TransactionTaskDone();

        }

        #endregion
    }
}
