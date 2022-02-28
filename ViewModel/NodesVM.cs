using System;
using System.Windows.Input;


namespace EpicNum.ViewModel
{

    public class NodesVM 
    { }

    public class NodeAdder : Model.NodeAdder
    {

        public Action ValueChanged;
        public int AdderNumberVM
        {
            get { return AdderNumber; }
            set
            {
                if (AdderNumber != value)
                {
                    AdderNumber = value;
                    MyPropertyChanged(nameof(AdderNumberVM));
                    ValueChanged?.Invoke();
                }

            }
        }
        public string AdderPadStringVM
        {
            get { return AdderPadString; }
            set
            {
                if (AdderPadString != value)
                {
                    AdderPadString = value;
                    MyPropertyChanged(nameof(AdderPadStringVM));
                    ValueChanged?.Invoke();
                }

            }
        }


    }
    public class NodeText : Model.NodeText
    {
        public Action ValueChanged;
        public string TextStringVM
        {
            get
            {
                return TextString;
            }

            set
            {
                TextString = value;
                MyPropertyChanged(nameof(TextStringVM));
                ValueChanged?.Invoke();
            }
        }
    }



}
