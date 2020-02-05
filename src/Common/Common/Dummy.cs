using System.IO;
using System.Reflection;

namespace Common
{
    public class Dummy
    {
        public void LogMessage() => File.AppendAllText("debug-output.txt", $"Common: {Assembly.GetExecutingAssembly().GetName().Version} \n");
    }
}
