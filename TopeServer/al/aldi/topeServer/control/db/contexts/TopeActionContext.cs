using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using TopeServer.al.aldi.topeServer.model;

namespace TopeServer.al.aldi.topeServer.control.db.contexts
{
    class TopeActionContext : DbContext
    {
        public DbSet<TopeAction> actions { get; set; }
        public DbSet<TopeRequest> requests { get; set; }
    }
}
