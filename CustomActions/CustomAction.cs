using Microsoft.Deployment.WindowsInstaller;
using System.Windows.Forms;

namespace CustomActions
{
    public partial class CustomActions
    {
        [CustomAction]
        public static ActionResult UpdateRevitManifestFiles(Session session)
        {
            session.Log("Begin CustomAction1");
            MessageBox.Show("EEE");

            return ActionResult.Success;
        }
    }
}
