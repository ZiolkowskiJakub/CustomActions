using System.IO;

namespace CustomActions
{

    internal static partial class Modify
    {
        internal static string CreateDirectory(string directory, params string[] names)
        {
            string directory_Temp = directory;
            foreach (string name in names)
            {
                string aDirectory_Temp = Path.Combine(directory_Temp, name);
                if (!Directory.Exists(aDirectory_Temp))
                    Directory.CreateDirectory(aDirectory_Temp);
                directory_Temp = aDirectory_Temp;
            }

            return directory_Temp;
        }
    }

}
