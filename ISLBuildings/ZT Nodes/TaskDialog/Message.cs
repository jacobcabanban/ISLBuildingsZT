namespace ISL_ZeroTouch
{
    using Autodesk.Revit.UI;

    /// <summary>
    /// Provide functionality for custom messages through a TaskDialog.
    /// </summary>
    public class Message
    {

        #region Private Methods

        /// <summary>
        /// Constructor
        /// </summary>
        private Message()
        {

        }

        #endregion


        #region Public Methods

        /// <summary>
        /// Displays a Taskdialog (Revit) showing email contacts for technical support.
        /// </summary>
        /// <param name="message">The message to be displayed on the Taskdialog.</param>
        /// <param name="showDialog">Boolean to run node.</param>
        public static void ShowMessage(string message, bool showDialog)
        {
            if (showDialog)
            {
                TaskDialog.Show("ISL Revit", message, TaskDialogCommonButtons.Ok);
            }
        }


        /// <summary>
        /// Returns contact information for technical support.
        /// </summary>
        /// <returns>Contact information for technical support.</returns>
        public static string ISLRevitContact()
        {
            var contactInfo = "For technical inquiries or help, please contact:\napym@islengineering.com\njcabanban@islengineering.com\njperras@islengineering.com";
            return contactInfo;
        }

        #endregion

    }


}
