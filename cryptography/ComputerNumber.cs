using System;
using System.Collections.Generic;
using System.Linq;
using System.Management;
using System.Text;
using System.Threading.Tasks;

namespace cryptography
{
    public static class ComputerNumber
    {
        public static string GetNumber()
        {
            string Key = "Win32_Processor";

            ManagementObjectSearcher searcher = new ManagementObjectSearcher("select * from " + Key);
            
            foreach (ManagementObject share in searcher.Get())
            {
                foreach (PropertyData pc in share.Properties)
                {
                    if (pc.Name.Equals("ProcessorId"))
                    {
                        return StringCipher.Encrypt(pc.Value?.ToString());
                    }

                }
            }

            return string.Empty;
        }
    }
}
