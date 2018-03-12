using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Komisy.Services
{
    class ValidationService
    {
     

        public bool validateValuesForElement(TextBox beuty, TextBox old, TextBox name)
        {
            bool isAllValuesCorrect = false;

            bool isBeutyCorrect = validateNumber(beuty.Text);
            bool isOldCorrect = validateNumber(old.Text);
            bool isNameCorrect = validateName(name.Text);

            if (isBeutyCorrect && isOldCorrect && isNameCorrect)
            {
                isAllValuesCorrect = true;
            }

            return isAllValuesCorrect;
        }

        private bool validateNumber(String number)
        {
            Regex regex = new Regex("^[-]?[0-9]+$");
            Match match = regex.Match(number);
            bool isCorrect = true;

            if (!match.Success)
            {
                isCorrect = false;
            }

            return isCorrect;
        }

        private bool validateName(String name)
        {
            Regex regex = new Regex("^[a-zA-Z]+$");
            Match match = regex.Match(name);
            bool isNumber = true;

            if (!match.Success)
            {
                isNumber = false;
            }

            return isNumber;
        }
    }
}
