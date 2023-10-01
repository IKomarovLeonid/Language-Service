using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WordsApp
{
    internal class AttemptInfo
    {
        public int ExpectedCount { get; set; }
        public int Attempts { get; set; }
        public int Errors { get; set; }
        public double Percent { get; set; }
        public DateTime DateTime { get; set; }

        public override string ToString()
        {
            return $"{ExpectedCount},{Attempts},{Errors},{Percent},{DateTime:yyyy-MM-dd HH:mm:ss}";
        }

        public string Print()
        {
            return $"Correct count: {ExpectedCount}, Attempts: {Attempts},Errors: {Errors},Percent: {Percent}%,Time: {DateTime:yyyy-MM-dd HH:mm:ss}";
        }
    }
}
