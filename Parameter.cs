using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Xml.Serialization;

namespace PyLauncher
{
    public class Parameter
    {
        public class Test
        {
            public string id;//test name
            public string filename;//"pyhton.exe"
            public string workingdirectory;
            public string arguments;//"path of to .py"
            [XmlElement("Parsstring")]
            public List<string> Parsstring;//5 for smoke test
            public string interceptio;

            public Test()
            {
                Parsstring = new List<string>();
            }
        }

        [XmlElement("Test")]
        public List<Test> tests;

        public Parameter()
        {
            tests = new List<Test>();
        }
    }
}
