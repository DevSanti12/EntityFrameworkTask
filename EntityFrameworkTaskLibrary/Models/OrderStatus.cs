using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityFrameworkTaskLibrary.Models;
public enum OrderStatus
{
    NotStarted,
    Loading,
    InProgress,
    Arrived,
    Unloading,
    Cancelled,
    Done
}
