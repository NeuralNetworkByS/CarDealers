using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Komisy.Models
{
    class DirectionCorrection
    {
        public Dictionary<string, bool> firstPointFlow = new Dictionary<string, bool>();
        public Dictionary<string, bool> secondPointFlow = new Dictionary<string, bool>();

        public Dictionary<string, bool> secondPointInvertedFlow = new Dictionary<string, bool>();

        public DirectionCorrection()
        {
            firstPointFlow.Add("4", true);
            firstPointFlow.Add("3", false);
            firstPointFlow.Add("2", false);
            firstPointFlow.Add("1", true);

            secondPointFlow.Add("44", false);
            secondPointFlow.Add("43", true);
            secondPointFlow.Add("42", true);
            secondPointFlow.Add("41", false);

            secondPointFlow.Add("33", true);
            secondPointFlow.Add("32", true);
            secondPointFlow.Add("31", false);

            secondPointFlow.Add("22", true);
            secondPointFlow.Add("21", false);

            secondPointFlow.Add("11", false);

            // inverted flow

            secondPointInvertedFlow.Add("14", false);
            secondPointInvertedFlow.Add("13", true);
            secondPointInvertedFlow.Add("12", true);
            secondPointInvertedFlow.Add("11", false);

            secondPointInvertedFlow.Add("24", false);
            secondPointInvertedFlow.Add("23", true);
            secondPointInvertedFlow.Add("22", true);

            secondPointInvertedFlow.Add("34", false);
            secondPointInvertedFlow.Add("33", true);

            secondPointInvertedFlow.Add("44", false);

        }
    }
}
