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
