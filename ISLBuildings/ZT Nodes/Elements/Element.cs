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
    /// Methods that manipulate both Dynamo and Revit Elements.
    /// </summary>
    public class Element
    {

        #region Private Methods
        private Element()
        {
                
        }
        #endregion

        #region Public Methods

        /// <summary>
        /// Convert Dynamo points to Revit points.
        /// </summary>
        /// <param name="dynPts"></param>
        /// <returns>A list of Revit XYZ objects (points).</returns>
        public static ICollection<Autodesk.Revit.DB.XYZ> DynamoPtToRevitPt(
            List<Autodesk.DesignScript.Geometry.Point> dynPts)
        {
            return dynPts.Select(dPt => new Autodesk.Revit.DB.XYZ(dPt.X, dPt.Y, dPt.Z)).ToList();
        }


        /// <summary>
        /// Create reference plane in views.
        /// </summary>
        /// <param name="bubbleEnds">XYZ pt at bubbleEnd.</param>
        /// <param name="freeEnds">XYZ pt at freeEnd.</param>
        /// <param name="viewsDynamo">View to insert reference plane.</param>
        /// <returns></returns>
        public static ICollection<Autodesk.Revit.DB.ReferencePlane> NewReferencePlaneIn2DView(
            List<XYZ> bubbleEnds, List<XYZ> freeEnds, List<Revit.Elements.Element> viewsDynamo)
        {
            Autodesk.Revit.DB.Document doc = DocumentManager.Instance.CurrentDBDocument;

            var draftingViews = viewsDynamo.UnwrapCollection()
                                                   .Select(x => x as View)
                                                   .ToList();

            var result = new List<Autodesk.Revit.DB.ReferencePlane>();

            TransactionManager.Instance.ForceCloseTransaction();
            TransactionManager.Instance.EnsureInTransaction(doc);

            for (int i = 0; i < bubbleEnds.Count(); i++)
            {
                var freeEnd = freeEnds[i];
                var bubbleEnd = bubbleEnds[i];
                var draftingView = draftingViews[i];

                result.Add(doc.Create.NewReferencePlane(freeEnd, bubbleEnd, new XYZ(0, 0, 1), draftingView));
            }

            TransactionManager.Instance.TransactionTaskDone();

            return result;
        }

        #endregion
    }
}
