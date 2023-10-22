using HowTo.Services.Extensions;
using HowTo.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HowTo.Services
{
    public class StringService : IStringService
    {
        public async Task<string> StuffStringAsync(string input, int interval, string value)
        {
            return await Task.Run(() => input.Stuff(interval, value));
        }
    }
}
