using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace accouting_system_manager.License
{
    [Serializable]
    [XmlType(TypeName = "computer")]
    public class Computer
    {
        public string serialNumber { get; set; }
    }
}
