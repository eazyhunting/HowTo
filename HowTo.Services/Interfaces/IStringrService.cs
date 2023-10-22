using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HowTo.Services.Interfaces
{
    public interface IStringService
    {
        Task<string> StuffStringAsync(string input, int interval, string value);
    }
}
