﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POSIntegratorV2.Entities
{
    public class System
    {
        public String Date { get; set; }
        public String Domain { get; set; }
        //public Boolean IsDefaultDate { get; set; }
        public String LocalConnection { get; set; }
        public String LogFileLocation { get; set; }
        public String FoldertoMonitor { get; set; }
    }
}
