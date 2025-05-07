using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Domain
{
    public class Log
    {
        public string Message { get; set; }
        public TraceLevel TraceLevel { get; set; }
        public DateTime Date { get; set; }

        public Log() { }
        public Log(string message, TraceLevel traceLevel = TraceLevel.Info, DateTime date = default)
        {
            Message = message;
            TraceLevel = traceLevel;
            Date = (date == default) ? DateTime.Now : date;
        }
    }
}
