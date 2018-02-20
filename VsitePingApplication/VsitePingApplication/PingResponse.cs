using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VsitePingApplication
{
    class PingResponse
    {
        bool status;
        long tripTime;

        public bool Status
        {
            get { return status; }
            set { this.status = value; }
        }

         public long TripTime
        {
            get { return tripTime; }
            set { this.tripTime = value; }
        }
    }
}
