using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace FaulhaberPlayer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        #region fields
        private FaulhaberPlayerOperator faulhaberPlayerOperator = new FaulhaberPlayerOperator();
        #endregion

        public MainWindow()
        {
            InitializeComponent();


            DataContext = faulhaberPlayerOperator;
        }

        private FaulhaberPlayerOperator Operator
        {
            get
            {
                return faulhaberPlayerOperator;
            }
        }

        private void ButtonConnect_Click(object sender, RoutedEventArgs e)
        {
            faulhaberPlayerOperator.TryConnect();
        }



        public void OnWindowClosing(object sender, CancelEventArgs e)
        {
            // Handle closing logic, set e.Cancel as needed
            faulhaberPlayerOperator.Dispose();
        }

        private void OutputTextBoxTextChanged(object sender, TextChangedEventArgs e)
        {
            OutputTextBox.ScrollToEnd();
        }

        private void Identity_Click(object sender, RoutedEventArgs e)
        {
            faulhaberPlayerOperator.ReadIdentity1018();

        }

        private void DeviceStatus_Click(object sender, RoutedEventArgs e)
        {
            faulhaberPlayerOperator.ReadDeviceStatus2324();
        }

        private void Status_Click(object sender, RoutedEventArgs e)
        {
            faulhaberPlayerOperator.ReadStatusword6041();
        }

        private void ButtonShutdown_Click(object sender, RoutedEventArgs e)
        {
            faulhaberPlayerOperator.Shutdown();
        }

        private void ButtonSwitchOn_Click(object sender, RoutedEventArgs e)
        {
            faulhaberPlayerOperator.SwitichOn();
        }

        private void ButtonEnableOp_Click(object sender, RoutedEventArgs e)
        {
            faulhaberPlayerOperator.EnableOp();
        }

        private void ButtonFaultReset_Click(object sender, RoutedEventArgs e)
        {
            faulhaberPlayerOperator.FaultReset();
        }

        private void ButtonPPSP1Go_Click(object sender, RoutedEventArgs e)
        {
            faulhaberPlayerOperator.PPSP1Go((bool)radioButtonPPSP1Absolute.IsChecked);
        }

        private void ButtonPPSP2Go_Click(object sender, RoutedEventArgs e)
        {
            faulhaberPlayerOperator.PPSP2Go((bool)radioButtonPPSP2Absolute.IsChecked);
        }

        private void ButtonActivatePP_Click(object sender, RoutedEventArgs e)
        {
            faulhaberPlayerOperator.SetOperationModeProfilePosition();
        }

        private void ButtonModeOperationDisplay_Click(object sender, RoutedEventArgs e)
        {
            faulhaberPlayerOperator.ReadModeOfOperationDisplay6061();
        }
        
        private void ButtonPositionActual_Click(object sender, RoutedEventArgs e)
        {
            faulhaberPlayerOperator.ReadPositionActualValue6064();
        }

        private void ButtonActivateHomingMode_Click(object sender, RoutedEventArgs e)
        {
            faulhaberPlayerOperator.SetOperationModeHoming();
        }

        private void ButtonHomingMethodSet_Click(object sender, RoutedEventArgs e)
        {
            faulhaberPlayerOperator.SetHomngMethod();
        }

        public void ButthonHomingStart_Click(object sender, RoutedEventArgs e)
        {
            faulhaberPlayerOperator.StartHoming();
        }

        public void ButthonHomingReset_Click(object sender, RoutedEventArgs e)
        {
            faulhaberPlayerOperator.ControlWordResetBit4();
        }

        private void ButtonPositionDemand_Click(object sender, RoutedEventArgs e)
        {
            faulhaberPlayerOperator.ReadPositionDemandValue6062();
        }

        private void ButtonTorqueActualValue_Click(object sender, RoutedEventArgs e)
        {
            faulhaberPlayerOperator.ReadTorqueActualValue6077();
        }

        private void ButtonVelocityActualValue_Click(object sender, RoutedEventArgs e)
        {
            faulhaberPlayerOperator.ReadVelocityActualValue606C();
        }

        private void ButtonSetGearRatio_Click(object sender, RoutedEventArgs e)
        {
            faulhaberPlayerOperator.SetGearRatio6091();
        }

        private void ButtonSetFeedConstant_Click(object sender, RoutedEventArgs e)
        {
            faulhaberPlayerOperator.SetFeedConstant6092();
        }

        private void ButtonSetVelocityFactor_Click(object sender, RoutedEventArgs e)
        {
            faulhaberPlayerOperator.SetVelocityFactor6096();
        }

        private void ButtonSetMaxSpeedAccDec_Click(object sender, RoutedEventArgs e)
        {
            faulhaberPlayerOperator.SetMaxSpeedAccelerationDeceleration();
        }

        private void ButtonReversePolarity_Click(object sender, RoutedEventArgs e)
        {
            faulhaberPlayerOperator.ReversePolarity607E();
        }

        private void ButtonSetHomingOffset_Click(object sender, RoutedEventArgs e)
        {
            faulhaberPlayerOperator.SetHomingOffset607C();
        }
    }
}
