using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Leaderone.Domain.Enums
{
    public static class Enumeration
    {
        public enum AppRole
        {
            Admin = 1,
            User = 2,
            Guest = 3
        }

        public enum TenantRole
        {
            Admin = 1,
            Owner = 2,
            Manager = 3,
            Employee = 4
        }

        public enum CustomerStatus
        {
            Abandoned,
            New,
            Local,
            Subscriber,
            Top
        }


    }
}
