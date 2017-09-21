using cryptography;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace accouting_system_manager.License
{
    [Serializable]
    [XmlType(TypeName = "content")]
    public class LicContent
    {
        public DateTime? validUntil { get; set; }

        public int id { get; set; }

        [XmlArray("computers")]
        public List<Computer> computers { get; set; }

        public bool IsDateValid {
            get {
                return validUntil == null || validUntil >= DateTime.Now;
            }
        }

        public bool IsComputerExists
        {
            get
            {
                return computers.Exists(c => StringCipher.Decrypt(c.serialNumber).Equals(StringCipher.Decrypt(ComputerNumber.GetNumber())));
            }
        }
    }
}
