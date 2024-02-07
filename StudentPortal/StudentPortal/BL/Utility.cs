using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StudentPortal.BL
{
    public static class Utility
    {
        private static string IDPrefix = "C";

        public static string GenerateId(int counter)
        {
            string id = $"C{counter:D4}";
            return id;
        }
    }
}