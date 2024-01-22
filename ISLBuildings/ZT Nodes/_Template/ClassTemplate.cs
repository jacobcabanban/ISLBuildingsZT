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
    ////Instead of the name of the Method:
    //[NodeName(“NodeName”)]

    ////Instead of the namespace and class names, use a ‘.’ to create a subcategory:
    //[NodeCategory(“NodeName.Category")]

    ////Provide a node description:
    //[NodeDescription(“Node Description")]

    ////Tag design script compatibility:
    //[IsDesignScriptCompatible]

    //[MultiReturn(new[]{"Output1", "Output2", "Output3"})]
    [IsVisibleInDynamoLibrary(false)]
    internal class ClassTemplate
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
