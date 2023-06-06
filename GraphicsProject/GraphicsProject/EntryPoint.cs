using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphicsProject
{
    internal class EntryPoint
    {

        [STAThread]
        private static void Main()
        {
            new Client.Program().Run();
        }


    }
}
