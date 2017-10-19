using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace accouting_system_manager.DB.Entities
{
    public class Artran
    {
        public string invno { get; set; }
        
        public string qtyorder { get; set; }

        public double totalprice { get; set; }

        public string itemcode { get; set; }

        public string fprice { get; set; }

        public string invdate { get; set; }

        public string cost { get; set; }

        public string deletedat { get; set; }

        public int GetInvnoToInt() {
            int no = 0;

            int.TryParse(invno, out no);

            return no;
        }

        public DateTime GetInvdateToDateTime() {
            DateTime date;

            DateTime.TryParse(invdate, out date);

            return date;
        }

    }
}
