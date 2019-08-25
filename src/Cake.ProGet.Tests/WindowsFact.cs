using System.Runtime.InteropServices;
using Xunit;

namespace Cake.ProGet.Tests
{
    public sealed class WindowsFact : FactAttribute
    {
        public WindowsFact() {
            if(!RuntimeInformation.IsOSPlatform(OSPlatform.Windows)) {
                Skip = "Only valid test on Windows Operating System";
            }
        }
    }
}
