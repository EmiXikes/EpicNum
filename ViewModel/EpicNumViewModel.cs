using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Input;
using CADHelper;
using EpicNum.View;
using EpicNum.View.Floaters;
using Gma.System.MouseKeyHook;
using GongSolutions.Wpf.DragDrop;

namespace EpicNum.ViewModel
{
    public class EpicNumViewModel : BasePropertyChanged, GongSolutions.Wpf.DragDrop.IDropTarget
    {
        public EpicNumViewModel()
        {
            // Default generation
            TopMost = true;

            Nodes = NumNodesDefault();


            RemoveNode = new RCommand(removeNode);
            IncrAdderNode = new RCommand(incrAdderNode);
            DecrAdderNode = new RCommand(decrAdderNode, canDecrAdderNode);
            ResetAdderNumber = new RCommand(resetAdderNumber);
            NumSequenceStart = new RCommand(numSequenceStart);

            AddAdderNode = new RCommand(addAdderNode);
            AddTextNode = new RCommand(addTextNode);

            CloseApplication = new RCommand(closeApplication);

            IncremenetSequenceTest = new RCommand(incremenetSequenceTest);

            CircleButton01 = new RCommand(circelButton01);
            CircleButton02 = new RCommand(circelButton02);
            CircleButton03 = new RCommand(circelButton03);
            CircleButton04 = new RCommand(circelButton04);
            CircleButton05 = new RCommand(circelButton05);

            MouseHookUnsubscribe();
            MouseHookSubscribe(Hook.GlobalEvents());

            FloaterNextValue = new FloaterNextValue() {DataContext = this };
            FloaterNextValue.Hide();

            FloaterDraggedNode = new FloaterDummy();
   

        }



        FloaterDummy FloaterDraggedNode;

        #region Bindable items

        public ObservableCollection<object> Nodes { get; set; }

        public string NextValue
        {
            get { return getNextValue(); }
        }

        private bool topMost;
        public bool TopMost
        {
            get => topMost; set
            {
                topMost = value;
                MyPropertyChanged(nameof(TopMost));
            }
        }

        // Bindable Actions
        public ICommand RemoveNode { get; set; }
        public ICommand IncrAdderNode { get; set; }
        public ICommand DecrAdderNode { get; set; }
        public ICommand ResetAdderNumber { get; set; }
        public ICommand NumSequenceStart { get; set; }
        public ICommand AddAdderNode { get; set; }
        public ICommand AddTextNode { get; set; }
        public ICommand CloseApplication { get; set; }

        // Circle Buttons
        public ICommand CircleButton01 { get; set; }
        public ICommand CircleButton02 { get; set; }
        public ICommand CircleButton03 { get; set; }
        public ICommand CircleButton04 { get; set; }
        public ICommand CircleButton05 { get; set; }


        //testing
        public ICommand IncremenetSequenceTest { get; set; }


        #endregion

        #region Helpers

        // Button Command Methods
        private void closeApplication(object obj)
        {
            MouseHookUnsubscribe();
            App.Current.Shutdown();
        }

        private void incrAdderNode(object param)
        {
            NodeAdder Node = (NodeAdder)param;
            //Model.NodeAdder AdderNode = (Model.NodeAdder)Node.Node;
            Node.AdderNumberVM = Node.AdderNumberVM + Node.AdderIncrement;
            UpdateProperties();
        }
        private void decrAdderNode(object param)
        {
            NodeAdder Node = (NodeAdder)param;
            //Model.NodeAdder AdderNode = (Model.NodeAdder)Node.Node;
            Node.AdderNumberVM = Node.AdderNumberVM - Node.AdderIncrement;
            UpdateProperties();
        }
        private bool canDecrAdderNode(object param)
        {
            if (param == null) { return false; }

            NodeAdder Node = (NodeAdder)param;
            //Model.NodeAdder AdderNode = (Model.NodeAdder)Node.Node;
            if (Node.AdderNumberVM <= 0)
            {
                return false;
            }
            return true;
        }
        private void resetAdderNumber(object param)
        {
            NodeAdder Node = (NodeAdder)param;
            //Model.NodeAdder AdderNode = (Model.NodeAdder)Node.Node;
            Node.AdderNumberVM = SharedVariables.AdderStartingNumber;
            UpdateProperties();
        }
        private void removeNode(object param)
        {
            //Lol solution
            try { Nodes.Remove((NodeAdder)param); }
            catch { }
            try { Nodes.Remove((NodeText)param); }
            catch { }

            //NodesVM.Remove((NodeVM)param);
            UpdateProperties();
        }

        // Add new Nodes
        private void addAdderNode(object param)
        {
            Nodes.Add(new NodeAdder
            {
                AdderNumberVM = 1,
                AdderIncrement = 1,
                AdderIncrementToogle = false,
                AdderResetToogle = false,
                AdderPadStringVM = "00",
                AdderResetNumber = 0,
                ValueChanged = UpdateProperties
            });
            UpdateProperties();
        }
        private void addTextNode(object param)
        {
            Nodes.Add(new NodeText
            {
                TextStringVM = ".",
                ValueChanged = UpdateProperties
            });
            UpdateProperties();
        }

        // Sequence
        private void numSequenceStart(object param)
        {
            //FloaterNextValue.Show();

            //Commands.SendDDEString(0, 
            //    "LOGFILEPATH\n"
            //    + "c:\\epic\\log\\\n"
            //    );
            //ACADinfo.ActivateLastAutoCAD();
            //Commands.SeqAttributeEdit(0, incrementMethod, NumFail, NewValue);

            
            TopMost = false;

            ShadowEngineGlobal.InitShadowEngine();
    
            var P = xWinHelper.xWinHelper.xLastProcByName("acad");

            if (P == null)
            {
                System.Windows.MessageBox.Show("Nav atrasts AutoCAD.");
                TopMost = true;
                ShadowEngineGlobal.ShadowEngine = null;
                return;
            }

            var ddeT = ShadowEngineGlobal.xDDEcontainedInProcess(P);

            Commands.SendDDEString(ddeT,"LOGFILEON\n");
            Commands.SendDDEString(ddeT,
                "LOGFILEPATH\n"
                + "c:\\epic\\log\\\n"
                );

            TopMost = true;

            FloaterNextValue.Show();
            xWinHelper.xWinHelper.xActivateWin(P.MainWindowHandle);
            Commands.SeqAttributeEdit(ddeT, incrementMethod, NumFail, NewValue);
        }
        private string NewValue()

        {
            return NextValue;
        }
        private string NumFail(MacroFailState FailSeverity, string Message)
        {
            return "";
        }
        private int incrementMethod()
        {
            DoIncrement();
            return 0;
        }

        // Circle Buttons

        private void circelButton01(object param) 
        {
            var Dicts = System.Windows.Application.Current.Resources.MergedDictionaries;
            Dicts[0].Source = new Uri("/Resources/Epic.Resources.Colors.EpicLight.xaml", UriKind.RelativeOrAbsolute);
        }
        private void circelButton02(object param) 
        {
            var Dicts = System.Windows.Application.Current.Resources.MergedDictionaries;
            Dicts[0].Source = new Uri("/Resources/Epic.Resources.Colors.EpicDark.xaml", UriKind.RelativeOrAbsolute);
        }
        private void circelButton03(object param) 
        {
            var Dicts = System.Windows.Application.Current.Resources.MergedDictionaries;
            Dicts[0].Source = new Uri("/Resources/Epic.Resources.Colors.EpicSolarize.xaml", UriKind.RelativeOrAbsolute);
        }
        private void circelButton04(object param) 
        {
            var Dicts = System.Windows.Application.Current.Resources.MergedDictionaries;
            Dicts[0].Source = new Uri("/Resources/Epic.Resources.Colors.EpicMetro.xaml", UriKind.RelativeOrAbsolute);
        }
        private void circelButton05(object param) 
        {
            
        }

        //testing
        private void incremenetSequenceTest(object param)
        {
            DoIncrement();
        }

        // Calcuations..
        private string getNextValue()
        {
            string Result = "";

            foreach (object Node in Nodes)
            {
                if (Node.GetType() == typeof(NodeAdder))
                {
                    Model.NodeAdder ThisNode = (NodeAdder)Node;
                    Result = Result + ThisNode.NodeResult;
                }
                else if (Node.GetType() == typeof(NodeText))
                {
                    Model.NodeText ThisNode = (NodeText)Node;
                    Result = Result + ThisNode.NodeResult;
                }
            }
            return Result;
        }
        public void UpdateProperties()
        {
            MyPropertyChanged(nameof(NextValue));
        }
        public void DoIncrement()
        {
            bool NextNodeIncrementFlag = false;

            for (int i = Nodes.Count - 1; i > -1; i--)
            {
                object Node = Nodes[i];
                if (Node.GetType() == typeof(NodeAdder))
                {
                    NodeAdder ThisNode = (NodeAdder)Node;

                    //  Basic increment

                    if (ThisNode.AdderIncrementToogle == true)
                    {
                        ThisNode.AdderNumberVM = ThisNode.AdderNumberVM + ThisNode.AdderIncrement;
                    }

                    //  Reset Inrcrement
                    if (NextNodeIncrementFlag == true)
                    {
                        NextNodeIncrementFlag = false;
                        ThisNode.AdderNumberVM = ThisNode.AdderNumberVM + ThisNode.AdderIncrement;
                    }

                    //  Set reset increment                                    

                    if (ThisNode.AdderResetToogle == true)
                    {
                        if (ThisNode.AdderResetNumber < ThisNode.AdderNumberVM)
                        {
                            int overflow = ThisNode.AdderNumberVM - ThisNode.AdderResetNumber;
                            ThisNode.AdderNumberVM = overflow;
                            NextNodeIncrementFlag = true;
                        }
                    }
                }
            }
            MyPropertyChanged(nameof(NextValue));
            // NextValue = GetNextValue();
        }

        #endregion
        

        // Default collection
        public ObservableCollection<object> NumNodesDefault()
        {
            ObservableCollection<object> Result = new ObservableCollection<object>();

            Result.Add(new NodeAdder
            {
                AdderNumberVM = 1,
                AdderIncrement = 1,
                AdderIncrementToogle = false,
                AdderResetToogle = false,
                AdderPadStringVM = "00",
                AdderResetNumber = 0,
                ValueChanged = UpdateProperties
            });

            Result.Add(new NodeText
            {
                TextStringVM = ".",
                ValueChanged = UpdateProperties
            });

            Result.Add(new NodeAdder
            {
                AdderNumberVM = 1,
                AdderIncrement = 1,
                AdderIncrementToogle = true,
                AdderResetToogle = true,
                AdderPadStringVM = "00",
                AdderResetNumber = 24,
                ValueChanged = UpdateProperties
            });

            return Result;
        }



        #region Drag and Drop

        

        public void DragOver(IDropInfo dropInfo)
        {
            object sourceItem = dropInfo.Data;
            object targetItem = dropInfo.TargetItem;

            if (sourceItem != null && targetItem != null)
            {
                dropInfo.DropTargetAdorner = DropTargetAdorners.Insert;
                dropInfo.Effects = System.Windows.DragDropEffects.Copy;

                //FloaterDraggedNode.DataContext = null;
                //FloaterDraggedNode.DataContext = sourceItem;
                FloaterDraggedNode.Show();



            }

        }

        public void Drop(IDropInfo dropInfo)
        {
            object sourceItem = dropInfo.Data;
            object targetItem = dropInfo.TargetItem;

            int indexSource = Nodes.IndexOf(sourceItem);
            int indexTarget = Nodes.IndexOf(targetItem);

            Nodes.Remove(sourceItem);
            Nodes.Insert(indexTarget, sourceItem);
            FloaterDraggedNode.Hide();
            UpdateProperties();

        }


        #endregion


        #region Floating Stuff

        private IKeyboardMouseEvents m_Events;

        FloaterNextValue FloaterNextValue;

        private void MouseHookSubscribe(IKeyboardMouseEvents events)
        {
            m_Events = events;
            m_Events.MouseMove += HookManager_MouseMove;
            m_Events.KeyPress += HookManager_KeyPress;
        }

        private void HookManager_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)27)
            {
                if (FloaterNextValue!= null)
                {
                    FloaterNextValue.Hide();
                }

            }
        }

        private void MouseHookUnsubscribe()
        {
            if (m_Events == null) return;
            m_Events.MouseMove -= HookManager_MouseMove;
            m_Events.KeyPress -= HookManager_KeyPress;
            m_Events.Dispose();
            m_Events = null;
        }

        private void HookManager_MouseMove(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if (FloaterNextValue != null)
            {
                FloaterNextValue.Top = e.Y + 10;
                FloaterNextValue.Left = e.X + 10;

                FloaterDraggedNode.Top = e.Y + 10;
                FloaterDraggedNode.Left = e.X + 10;

            }
        }


        #endregion

    }

}
