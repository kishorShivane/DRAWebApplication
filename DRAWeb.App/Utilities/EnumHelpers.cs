using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DRAWeb.App.Utilities
{
    public class EnumHelpers
    {
        public enum NotificationType
        { 
            Success = 0,
            Information = 1,
            Warning = 2,
            Failure = 4            
        }
    }
}
