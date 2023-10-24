using AutopilotManager.Clients;
using System;
using System.Diagnostics;
using System.Drawing;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AutopilotManager.Client
{
    public partial class QrCodeForm : Form
    {
        private readonly Bitmap _idQrCodeImage;
        private readonly Bitmap _qrCodeNoSelfServiceImage;
        private readonly Models.SystemInformation _systemInformation;
        private readonly BackendClient _backendClient;
        private readonly string _backendUrl;
        private readonly string _preCheckErrorMessage;
        private readonly Stopwatch _stopWatch;
        private readonly bool _endpointsValidationResult;
        private bool _quietMode;
        private bool _noWait;

        public QrCodeForm(Bitmap idQrCodeImage, 
            Bitmap qrCodeNoSelfServiceImage,
            Models.SystemInformation systemInformation, 
            BackendClient backendClient, 
            string backendUrl, 
            string preCheckErrorMessage, 
            bool endpointsValidationResult)
        {
            _idQrCodeImage = idQrCodeImage;
            _qrCodeNoSelfServiceImage = qrCodeNoSelfServiceImage;
            _systemInformation = systemInformation;
            _backendClient = backendClient;
            _backendUrl = backendUrl;
            _stopWatch = new Stopwatch();
            _preCheckErrorMessage = preCheckErrorMessage;
            _endpointsValidationResult = endpointsValidationResult;
            _quietMode = false;

            _backendClient.ResultReceived += backendClient_ResultReceived;

            InitializeComponent();
        }

        public void DisplayData(bool quietMode, bool noWait)
        {
            _quietMode = quietMode;
            _noWait = noWait;

            pictureBoxQrCode.Image = _idQrCodeImage;
            labelIdValue.Text = _systemInformation.Id.ToString();
            labelManufacturerValue.Text = _systemInformation.Manufacturer;
            labelModelValue.Text = _systemInformation.Model;
            labelSerialNumberValue.Text = _systemInformation.SerialNumber;
            labelRegisteredValue.Text = _preCheckErrorMessage;
            labelCancel.Visible = false;
            buttonRetry.Visible = false;

            if (_systemInformation.Action == "DELETE")
            {
                labelRegistered.Text = "Device deleted:";
            }

            if (_preCheckErrorMessage.ToLower().Contains("not allowed"))
            {
                Bitmap qrPlaceHolderImage = new Bitmap(80, 80);
                using (Graphics g = Graphics.FromImage(qrPlaceHolderImage))
                {
                    g.Clear(Color.White);
                }
                Pen blackPen = new Pen(Color.Black, 2);

                // draw a black cross
                using (var graphics = Graphics.FromImage(qrPlaceHolderImage))
                {
                    graphics.DrawLine(blackPen, 5, 5, 75, 75);
                    graphics.DrawLine(blackPen, 75, 5, 5, 75);
                }
                pictureBoxQrCode.Image = qrPlaceHolderImage;
            }

            if (_quietMode)
            {                
                Location = new Point(0, 0);
                Size = new Size(1, 1);
                Opacity = 0;
                ShowIcon = false;
                ShowInTaskbar = false;
            }

            ShowDialog();
        }

        private void buttonDone_Click(object sender, EventArgs e)
        {
            if (buttonCancel.Text.StartsWith("&Reboot", StringComparison.OrdinalIgnoreCase) ||
                buttonCancel.Text.StartsWith("Reboot", StringComparison.OrdinalIgnoreCase))
            {
                Process.Start("shutdown.exe", "-r -t 0");
            }
            Close();
            Dispose();
        }

        private async void QrCodeForm_Shown(object sender, EventArgs e)
        {
            var success = false;

            if (_endpointsValidationResult)
            {
                labelEndpointsValue.Text = "Reachable";
            }
            else
            {
                labelEndpointsValue.Text = "Not all reachable!";
            }

            if (string.IsNullOrEmpty(_preCheckErrorMessage))
            {
                var firstRun = true;
                do
                {
                    if (!firstRun)
                    {
                        await Task.Delay(TimeSpan.FromSeconds(10));
                    }

                    await _backendClient.GetResultAsync(_systemInformation, _backendUrl);

                    //if (labelRegisteredValue.Text.StartsWith("Queued", StringComparison.OrdinalIgnoreCase) ||
                    //    string.IsNullOrEmpty(labelRegisteredValue.Text))
                    if (firstRun)
                    {
                        _stopWatch.Start();
                        firstRun = false;
                    }
                    if (labelRegisteredValue.Text.StartsWith("Success", StringComparison.OrdinalIgnoreCase) ||
                        labelRegisteredValue.Text.StartsWith("Already registered", StringComparison.OrdinalIgnoreCase))
                    {
                        // SUCCESS - change colors
                        success = true;

                        BackColor = Color.FromArgb(0, 117, 51);

                        if (_systemInformation.Action.StartsWith("import", StringComparison.OrdinalIgnoreCase))
                        {
                            buttonCancel.Text = "&Reboot";
                        }
                        else // Deletion request processed
                        {
                            buttonCancel.Text = "&Finish";
                        }
                        buttonCancel.BackColor = Color.FromArgb(0, 92, 40);
                        
                        labelCancel.ForeColor = Color.White;
                        labelCancel.Show();

                        if (_quietMode)
                        {
                            Application.Exit();
                        }
                    }

                    if ((labelRegisteredValue.Text.StartsWith("Queued", StringComparison.OrdinalIgnoreCase) && _noWait) ||
                        (labelRegisteredValue.Text.StartsWith("Processing", StringComparison.OrdinalIgnoreCase) && _noWait))
                    {
                        break;
                    }

                } while (labelRegisteredValue.Text.StartsWith("Queued", StringComparison.OrdinalIgnoreCase) ||
                        labelRegisteredValue.Text.StartsWith("Processing", StringComparison.OrdinalIgnoreCase) ||
                        string.IsNullOrEmpty(labelRegisteredValue.Text));

                _stopWatch.Stop();
                labelRegisteredValue.Text += $" (elapsed time: {Math.Ceiling(_stopWatch.Elapsed.TotalMinutes)} minutes)";
                labelRegisteredValue.Refresh();

                if (_quietMode || _noWait)
                {
                    Environment.ExitCode = 1;
                    Application.Exit();
                }
            }

            if (!string.IsNullOrEmpty(_preCheckErrorMessage) || !success)
            {
                // FAILURE - change colors

                // pre-check went wrong like model/manufacturer or import failed
                BackColor = Color.FromArgb(91, 0, 7);

                if (_systemInformation.Action.StartsWith("import", StringComparison.OrdinalIgnoreCase))
                {
                    buttonCancel.Text = "&Reboot";
                }
                else // Deletion request processed
                {
                    buttonCancel.Text = "&Finish";
                }
                buttonCancel.BackColor = Color.FromArgb(119, 0, 8);

                buttonRetry.BackColor = Color.FromArgb(119, 0, 8);
                buttonRetry.Show();

                labelCancel.ForeColor = Color.White;
                labelCancel.Show();

                labelProvisioningInformation.ForeColor = Color.FromArgb(254, 197, 114);
                if (!string.IsNullOrEmpty(_preCheckErrorMessage))
                {
                    if (_preCheckErrorMessage.StartsWith("delete", StringComparison.OrdinalIgnoreCase))
                    {
                        labelProvisioningInformation.Text = "The request is not allowed.";
                    }
                    else
                    {
                        labelProvisioningInformation.Text = "Your device is not allowed to be provisioned.";
                    }
                    
                }
                else if (!success)
                {
                    labelProvisioningInformation.Text = $"Something went wrong, we couldn't {_systemInformation.Action.ToLower()} your device. Contact the IT admin to troubleshoot.";
                }

                if (_quietMode)
                {
                    Environment.ExitCode = 1;
                    Application.Exit();
                }
            }
        }

        private void backendClient_ResultReceived(object sender, Models.ResultEventArgs e)
        {
            if (e.Message == "Processing")
            {
                // poor man's animation :-) to signal we are working...
                var processingMaxAnimation = "Processing.....";

                if (labelRegisteredValue.Text.StartsWith("Queued") ||
                    string.IsNullOrEmpty(labelRegisteredValue.Text))
                {
                    labelRegisteredValue.Text = "Processing";
                    labelRegisteredValue.Refresh();
                }
                else if (labelRegisteredValue.Text.StartsWith("Processing") && 
                    !labelRegisteredValue.Text.StartsWith(processingMaxAnimation))
                {
                    labelRegisteredValue.Text += ".";
                    labelRegisteredValue.Refresh();
                }
                else if (labelRegisteredValue.Text.StartsWith(processingMaxAnimation))
                {
                    labelRegisteredValue.Text = "Processing.";
                    labelRegisteredValue.Refresh();
                }
            }
            else if (e.Message == "ApprovalMode")
            {
                pictureBoxQrCode.Image = _qrCodeNoSelfServiceImage;
                pictureBoxQrCode.Refresh();
                labelProvisioningInformation.Text = "Call your helpdesk to approve your request.";
            }
            else
            {
                labelRegisteredValue.Text = e.Message;
            }
        }

        private void labelCancel_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void buttonRetry_Click(object sender, EventArgs e)
        {
            Close();
            DialogResult = DialogResult.Retry;
        }

        private void labelSerialNumber_DoubleClick(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Maximized)
            {
                this.Size = new Size(800, 500);
                this.WindowState = FormWindowState.Normal;
                this.FormBorderStyle = FormBorderStyle.FixedToolWindow;
            }
            else
            {
                this.WindowState = FormWindowState.Maximized;
                this.FormBorderStyle = FormBorderStyle.None;
            }
        }

        private void labelSerialNumber_SizeChanged(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Maximized)
            {
                this.FormBorderStyle = FormBorderStyle.None;
            }
        }
    }
}
