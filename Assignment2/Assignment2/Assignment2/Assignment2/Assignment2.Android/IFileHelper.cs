using System.IO;
using Xamarin.Forms;


[assembly: Dependency(typeof(Assignment2.Droid.FileHelper))]
namespace Assignment2.Droid
{
    public class FileHelper : IFileHelper
    {
        public string GetLocalFilePath(string filename)
        {
            string path = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
            return Path.Combine(path, filename);
        }
    }
}
