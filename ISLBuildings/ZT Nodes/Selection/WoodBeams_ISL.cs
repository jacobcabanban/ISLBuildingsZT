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
using Revit.Elements;
using Revit.GeometryConversion;
using RevitServices.Persistence;
using RevitServices.Transactions;

namespace ISL_ZeroTouch
{
    /// <summary>
    /// 
    /// </summary>
    [NodeName("WoodBeams_ISL")]
    [NodeCategory("ISL_ZeroTouch.Selection")]
    [NodeDescription("Drop down list for all framing with containing 'b' and 'd' as parameters.")]
    [IsDesignScriptCompatible]
    public class WoodBeams_ISL
    {

        #region Public Methods
        public static void SampleMethod()
        {
            // Get current document
            Autodesk.Revit.DB.Document doc = DocumentManager.Instance.CurrentDBDocument;

            // New transaction
            TransactionManager.Instance.ForceCloseTransaction();
            TransactionManager.Instance.EnsureInTransaction(doc);

            // End transaction
            TransactionManager.Instance.TransactionTaskDone();

        }

        #endregion
    }
}
