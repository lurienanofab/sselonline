using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace sselOnLine.AppCode.BLL
{
    public static class KioskDB
    {
        public static bool IsKiosk(string IP)
        {
            return IP.StartsWith("192.168.1");
        }
    }
}
