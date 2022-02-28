using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EpicNum.Model
{
    class Nodes
    {
    }

    public class NodeAdder : BasePropertyChanged
    {
        private int adderNumber;
        private int adderResetNumber;
        private string adderPadString;

        private bool adderResetToogle;
        private bool adderIncrementToogle;
        private int adderIncrement;

        public int AdderNumber
        {
            get { return adderNumber; }
            set
            {
                if (adderNumber != value)
                {
                    adderNumber = value;
                    MyPropertyChanged(nameof(AdderNumber));
                    MyPropertyChanged(nameof(NodeResult));
                }

            }
        }
        public int AdderResetNumber
        {
            get { return adderResetNumber; }
            set
            {
                if (adderResetNumber != value)
                {
                    adderResetNumber = value;
                    MyPropertyChanged(nameof(AdderResetNumber));
                }

            }
        }
        public string AdderPadString
        {
            get { return adderPadString; }
            set
            {
                if (adderPadString != value)
                {
                    adderPadString = value;
                    MyPropertyChanged(nameof(AdderPadString));
                    MyPropertyChanged(nameof(NodeResult));
                }

            }
        }
        public bool AdderResetToogle
        {
            get { return adderResetToogle; }
            set
            {
                if (adderResetToogle != value)
                {
                    adderResetToogle = value;
                    MyPropertyChanged(nameof(AdderResetToogle));
                }

            }
        }
        public bool AdderIncrementToogle
        {
            get { return adderIncrementToogle; }
            set
            {
                if (adderIncrementToogle != value)
                {
                    adderIncrementToogle = value;
                    MyPropertyChanged(nameof(AdderIncrementToogle));
                }

            }
        }

        public int AdderIncrement
        {
            get => adderIncrement; set
            {
                if (adderIncrement != value)
                {
                    adderIncrement = value;
                    MyPropertyChanged(nameof(AdderIncrement));
                }
                
            }
        }
        /// <summary>
        /// NodeResult. Returns the current result of the node
        /// </summary>
        public string NodeResult
        {
            get
            {
                if (adderNumber.ToString().Length > adderPadString.Length)
                {
                    return adderNumber.ToString();
                }
                else
                {
                    return adderPadString.Substring(0, adderPadString.Length - adderNumber.ToString().Length) +
                        adderNumber.ToString();
                }
            }
        }
    }

    public class NodeText : BasePropertyChanged
    {
        private string textString;
        public string TextString
        {
            get { return textString; }
            set
            {
                if (textString != value)
                {
                    textString = value;
                    MyPropertyChanged(nameof(TextString));
                    MyPropertyChanged(nameof(NodeResult));
                }
            }
        }

        /// <summary>
        /// NodeResult. Returns the current result of the node
        /// </summary>
        public string NodeResult
        {
            get { return textString; }
        }
    }

}
