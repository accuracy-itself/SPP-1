using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tests.TestClasses
{
    public class CodeIdentity
    {

        public string CodeName;
        public int CodeNumber;
        private double EfficiencyIndex;

        public CodeIdentity(string name, double efficiencyIndex)
        {
            CodeName = name;
            EfficiencyIndex = efficiencyIndex;
        }

        public void showInfo()
        {
            Console.WriteLine($"Code Name: {CodeName}, Code Number: {CodeNumber}, EfficiencyIndex: {EfficiencyIndex}");
        }
    }
}
