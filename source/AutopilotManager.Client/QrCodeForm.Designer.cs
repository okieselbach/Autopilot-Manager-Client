namespace AutopilotManager.Client
{
    partial class QrCodeForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.tableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.labelHeading = new System.Windows.Forms.Label();
            this.labelProvisioningInformation = new System.Windows.Forms.Label();
            this.flowLayoutPanelButtons = new System.Windows.Forms.FlowLayoutPanel();
            this.pictureBoxQrCode = new System.Windows.Forms.PictureBox();
            this.attributesTableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.labelId = new System.Windows.Forms.Label();
            this.labelManufacturer = new System.Windows.Forms.Label();
            this.labelModel = new System.Windows.Forms.Label();
            this.labelSerialNumber = new System.Windows.Forms.Label();
            this.labelEndpoints = new System.Windows.Forms.Label();
            this.labelRegistered = new System.Windows.Forms.Label();
            this.labelIdValue = new System.Windows.Forms.Label();
            this.labelManufacturerValue = new System.Windows.Forms.Label();
            this.labelModelValue = new System.Windows.Forms.Label();
            this.labelSerialNumberValue = new System.Windows.Forms.Label();
            this.labelEndpointsValue = new System.Windows.Forms.Label();
            this.labelRegisteredValue = new System.Windows.Forms.Label();
            this.flowLayoutPanelCancel = new System.Windows.Forms.FlowLayoutPanel();
            this.labelCancel = new System.Windows.Forms.Label();
            this.buttonRetry = new AutopilotManager.Client.RoundedButton();
            this.buttonCancel = new AutopilotManager.Client.RoundedButton();
            this.tableLayoutPanel.SuspendLayout();
            this.flowLayoutPanelButtons.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxQrCode)).BeginInit();
            this.attributesTableLayoutPanel.SuspendLayout();
            this.flowLayoutPanelCancel.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel
            // 
            this.tableLayoutPanel.AutoSize = true;
            this.tableLayoutPanel.BackColor = System.Drawing.Color.Transparent;
            this.tableLayoutPanel.BackgroundImage = global::AutopilotManager.Client.Properties.Resources.Win11Background;
            this.tableLayoutPanel.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.tableLayoutPanel.ColumnCount = 2;
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel.Controls.Add(this.labelHeading, 0, 0);
            this.tableLayoutPanel.Controls.Add(this.labelProvisioningInformation, 0, 2);
            this.tableLayoutPanel.Controls.Add(this.flowLayoutPanelButtons, 1, 3);
            this.tableLayoutPanel.Controls.Add(this.pictureBoxQrCode, 0, 1);
            this.tableLayoutPanel.Controls.Add(this.attributesTableLayoutPanel, 1, 1);
            this.tableLayoutPanel.Controls.Add(this.flowLayoutPanelCancel, 0, 3);
            this.tableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel.Name = "tableLayoutPanel";
            this.tableLayoutPanel.RowCount = 4;
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 15F));
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 15.8F));
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 19.2F));
            this.tableLayoutPanel.Size = new System.Drawing.Size(800, 500);
            this.tableLayoutPanel.TabIndex = 0;
            // 
            // labelHeading
            // 
            this.labelHeading.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.labelHeading.AutoSize = true;
            this.tableLayoutPanel.SetColumnSpan(this.labelHeading, 2);
            this.labelHeading.Font = new System.Drawing.Font("Segoe UI", 30F);
            this.labelHeading.ForeColor = System.Drawing.Color.DimGray;
            this.labelHeading.Location = new System.Drawing.Point(73, 16);
            this.labelHeading.Name = "labelHeading";
            this.labelHeading.Padding = new System.Windows.Forms.Padding(0, 0, 0, 5);
            this.labelHeading.Size = new System.Drawing.Size(653, 59);
            this.labelHeading.TabIndex = 0;
            this.labelHeading.Text = "Windows Autopilot Import Provider";
            this.labelHeading.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labelProvisioningInformation
            // 
            this.labelProvisioningInformation.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.labelProvisioningInformation.AutoSize = true;
            this.tableLayoutPanel.SetColumnSpan(this.labelProvisioningInformation, 2);
            this.labelProvisioningInformation.Font = new System.Drawing.Font("Segoe UI", 12.25F, System.Drawing.FontStyle.Bold);
            this.labelProvisioningInformation.ForeColor = System.Drawing.Color.DimGray;
            this.labelProvisioningInformation.Location = new System.Drawing.Point(94, 332);
            this.labelProvisioningInformation.Margin = new System.Windows.Forms.Padding(0, 7, 0, 0);
            this.labelProvisioningInformation.Name = "labelProvisioningInformation";
            this.labelProvisioningInformation.Size = new System.Drawing.Size(612, 23);
            this.labelProvisioningInformation.TabIndex = 1;
            this.labelProvisioningInformation.Text = "Scan QR code and authenticate to publish device provisioning information.";
            this.labelProvisioningInformation.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // flowLayoutPanelButtons
            // 
            this.flowLayoutPanelButtons.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.flowLayoutPanelButtons.AutoSize = true;
            this.flowLayoutPanelButtons.Controls.Add(this.buttonRetry);
            this.flowLayoutPanelButtons.Controls.Add(this.buttonCancel);
            this.flowLayoutPanelButtons.Location = new System.Drawing.Point(432, 412);
            this.flowLayoutPanelButtons.Name = "flowLayoutPanelButtons";
            this.flowLayoutPanelButtons.Size = new System.Drawing.Size(365, 85);
            this.flowLayoutPanelButtons.TabIndex = 2;
            // 
            // pictureBoxQrCode
            // 
            this.pictureBoxQrCode.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.pictureBoxQrCode.Location = new System.Drawing.Point(145, 90);
            this.pictureBoxQrCode.Margin = new System.Windows.Forms.Padding(0, 0, 35, 0);
            this.pictureBoxQrCode.Name = "pictureBoxQrCode";
            this.pictureBoxQrCode.Size = new System.Drawing.Size(220, 220);
            this.pictureBoxQrCode.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBoxQrCode.TabIndex = 3;
            this.pictureBoxQrCode.TabStop = false;
            // 
            // attributesTableLayoutPanel
            // 
            this.attributesTableLayoutPanel.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.attributesTableLayoutPanel.ColumnCount = 2;
            this.attributesTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 30.96447F));
            this.attributesTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 69.03553F));
            this.attributesTableLayoutPanel.Controls.Add(this.labelId, 0, 0);
            this.attributesTableLayoutPanel.Controls.Add(this.labelManufacturer, 0, 1);
            this.attributesTableLayoutPanel.Controls.Add(this.labelModel, 0, 2);
            this.attributesTableLayoutPanel.Controls.Add(this.labelSerialNumber, 0, 3);
            this.attributesTableLayoutPanel.Controls.Add(this.labelEndpoints, 0, 4);
            this.attributesTableLayoutPanel.Controls.Add(this.labelRegistered, 0, 5);
            this.attributesTableLayoutPanel.Controls.Add(this.labelIdValue, 1, 0);
            this.attributesTableLayoutPanel.Controls.Add(this.labelManufacturerValue, 1, 1);
            this.attributesTableLayoutPanel.Controls.Add(this.labelModelValue, 1, 2);
            this.attributesTableLayoutPanel.Controls.Add(this.labelSerialNumberValue, 1, 3);
            this.attributesTableLayoutPanel.Controls.Add(this.labelEndpointsValue, 1, 4);
            this.attributesTableLayoutPanel.Controls.Add(this.labelRegisteredValue, 1, 5);
            this.attributesTableLayoutPanel.Location = new System.Drawing.Point(403, 88);
            this.attributesTableLayoutPanel.Name = "attributesTableLayoutPanel";
            this.attributesTableLayoutPanel.Padding = new System.Windows.Forms.Padding(0, 25, 0, 25);
            this.attributesTableLayoutPanel.RowCount = 6;
            this.attributesTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.attributesTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.attributesTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.attributesTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.attributesTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.attributesTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.attributesTableLayoutPanel.Size = new System.Drawing.Size(394, 224);
            this.attributesTableLayoutPanel.TabIndex = 4;
            // 
            // labelId
            // 
            this.labelId.AutoSize = true;
            this.labelId.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelId.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Bold);
            this.labelId.ForeColor = System.Drawing.Color.DimGray;
            this.labelId.Location = new System.Drawing.Point(3, 25);
            this.labelId.Name = "labelId";
            this.labelId.Size = new System.Drawing.Size(116, 30);
            this.labelId.TabIndex = 0;
            this.labelId.Text = "ID:";
            // 
            // labelManufacturer
            // 
            this.labelManufacturer.AutoSize = true;
            this.labelManufacturer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelManufacturer.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Bold);
            this.labelManufacturer.ForeColor = System.Drawing.Color.DimGray;
            this.labelManufacturer.Location = new System.Drawing.Point(3, 55);
            this.labelManufacturer.Name = "labelManufacturer";
            this.labelManufacturer.Size = new System.Drawing.Size(116, 30);
            this.labelManufacturer.TabIndex = 1;
            this.labelManufacturer.Text = "Manufacturer:";
            // 
            // labelModel
            // 
            this.labelModel.AutoSize = true;
            this.labelModel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelModel.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Bold);
            this.labelModel.ForeColor = System.Drawing.Color.DimGray;
            this.labelModel.Location = new System.Drawing.Point(3, 85);
            this.labelModel.Name = "labelModel";
            this.labelModel.Size = new System.Drawing.Size(116, 30);
            this.labelModel.TabIndex = 2;
            this.labelModel.Text = "Model:";
            // 
            // labelSerialNumber
            // 
            this.labelSerialNumber.AutoSize = true;
            this.labelSerialNumber.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelSerialNumber.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelSerialNumber.ForeColor = System.Drawing.Color.DimGray;
            this.labelSerialNumber.Location = new System.Drawing.Point(3, 115);
            this.labelSerialNumber.Name = "labelSerialNumber";
            this.labelSerialNumber.Size = new System.Drawing.Size(116, 30);
            this.labelSerialNumber.TabIndex = 3;
            this.labelSerialNumber.Text = "Serial Number:";
            this.labelSerialNumber.SizeChanged += new System.EventHandler(this.labelSerialNumber_SizeChanged);
            this.labelSerialNumber.DoubleClick += new System.EventHandler(this.labelSerialNumber_DoubleClick);
            // 
            // labelEndpoints
            // 
            this.labelEndpoints.AutoSize = true;
            this.labelEndpoints.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelEndpoints.ForeColor = System.Drawing.Color.DimGray;
            this.labelEndpoints.Location = new System.Drawing.Point(3, 145);
            this.labelEndpoints.Name = "labelEndpoints";
            this.labelEndpoints.Size = new System.Drawing.Size(102, 15);
            this.labelEndpoints.TabIndex = 12;
            this.labelEndpoints.Text = "Enrollment URLs:";
            // 
            // labelRegistered
            // 
            this.labelRegistered.AutoSize = true;
            this.labelRegistered.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelRegistered.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelRegistered.ForeColor = System.Drawing.Color.DimGray;
            this.labelRegistered.Location = new System.Drawing.Point(3, 175);
            this.labelRegistered.Name = "labelRegistered";
            this.labelRegistered.Size = new System.Drawing.Size(116, 24);
            this.labelRegistered.TabIndex = 4;
            this.labelRegistered.Text = "Device registered:";
            // 
            // labelIdValue
            // 
            this.labelIdValue.AutoSize = true;
            this.labelIdValue.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Bold);
            this.labelIdValue.ForeColor = System.Drawing.Color.DimGray;
            this.labelIdValue.Location = new System.Drawing.Point(125, 25);
            this.labelIdValue.Name = "labelIdValue";
            this.labelIdValue.Size = new System.Drawing.Size(51, 13);
            this.labelIdValue.TabIndex = 5;
            this.labelIdValue.Text = "<value>";
            // 
            // labelManufacturerValue
            // 
            this.labelManufacturerValue.AutoSize = true;
            this.labelManufacturerValue.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelManufacturerValue.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Bold);
            this.labelManufacturerValue.ForeColor = System.Drawing.Color.DimGray;
            this.labelManufacturerValue.Location = new System.Drawing.Point(125, 55);
            this.labelManufacturerValue.Name = "labelManufacturerValue";
            this.labelManufacturerValue.Size = new System.Drawing.Size(266, 30);
            this.labelManufacturerValue.TabIndex = 6;
            this.labelManufacturerValue.Text = "<value>";
            // 
            // labelModelValue
            // 
            this.labelModelValue.AutoSize = true;
            this.labelModelValue.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelModelValue.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Bold);
            this.labelModelValue.ForeColor = System.Drawing.Color.DimGray;
            this.labelModelValue.Location = new System.Drawing.Point(125, 85);
            this.labelModelValue.Name = "labelModelValue";
            this.labelModelValue.Size = new System.Drawing.Size(266, 30);
            this.labelModelValue.TabIndex = 7;
            this.labelModelValue.Text = "<value>";
            // 
            // labelSerialNumberValue
            // 
            this.labelSerialNumberValue.AutoSize = true;
            this.labelSerialNumberValue.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelSerialNumberValue.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Bold);
            this.labelSerialNumberValue.ForeColor = System.Drawing.Color.DimGray;
            this.labelSerialNumberValue.Location = new System.Drawing.Point(125, 115);
            this.labelSerialNumberValue.Name = "labelSerialNumberValue";
            this.labelSerialNumberValue.Size = new System.Drawing.Size(266, 30);
            this.labelSerialNumberValue.TabIndex = 8;
            this.labelSerialNumberValue.Text = "<value>";
            // 
            // labelEndpointsValue
            // 
            this.labelEndpointsValue.AutoSize = true;
            this.labelEndpointsValue.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelEndpointsValue.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Bold);
            this.labelEndpointsValue.ForeColor = System.Drawing.Color.DimGray;
            this.labelEndpointsValue.Location = new System.Drawing.Point(125, 145);
            this.labelEndpointsValue.Name = "labelEndpointsValue";
            this.labelEndpointsValue.Size = new System.Drawing.Size(266, 30);
            this.labelEndpointsValue.TabIndex = 11;
            this.labelEndpointsValue.Text = "<value>";
            // 
            // labelRegisteredValue
            // 
            this.labelRegisteredValue.AutoSize = true;
            this.labelRegisteredValue.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelRegisteredValue.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Bold);
            this.labelRegisteredValue.ForeColor = System.Drawing.Color.DimGray;
            this.labelRegisteredValue.Location = new System.Drawing.Point(125, 175);
            this.labelRegisteredValue.Name = "labelRegisteredValue";
            this.labelRegisteredValue.Size = new System.Drawing.Size(266, 24);
            this.labelRegisteredValue.TabIndex = 9;
            this.labelRegisteredValue.Text = "<value>";
            // 
            // flowLayoutPanelCancel
            // 
            this.flowLayoutPanelCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.flowLayoutPanelCancel.AutoSize = true;
            this.flowLayoutPanelCancel.Controls.Add(this.labelCancel);
            this.flowLayoutPanelCancel.FlowDirection = System.Windows.Forms.FlowDirection.BottomUp;
            this.flowLayoutPanelCancel.Location = new System.Drawing.Point(3, 427);
            this.flowLayoutPanelCancel.Name = "flowLayoutPanelCancel";
            this.flowLayoutPanelCancel.Size = new System.Drawing.Size(101, 70);
            this.flowLayoutPanelCancel.TabIndex = 5;
            // 
            // labelCancel
            // 
            this.labelCancel.AutoSize = true;
            this.labelCancel.Cursor = System.Windows.Forms.Cursors.Hand;
            this.labelCancel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelCancel.ForeColor = System.Drawing.Color.DimGray;
            this.labelCancel.Location = new System.Drawing.Point(50, 0);
            this.labelCancel.Margin = new System.Windows.Forms.Padding(50, 0, 0, 55);
            this.labelCancel.Name = "labelCancel";
            this.labelCancel.Size = new System.Drawing.Size(51, 15);
            this.labelCancel.TabIndex = 0;
            this.labelCancel.Text = "&Cancel";
            this.labelCancel.Click += new System.EventHandler(this.labelCancel_Click);
            // 
            // buttonRetry
            // 
            this.buttonRetry.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(103)))), ((int)(((byte)(192)))));
            this.buttonRetry.CornerRadius = 5;
            this.buttonRetry.FlatAppearance.BorderSize = 0;
            this.buttonRetry.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonRetry.Font = new System.Drawing.Font("Segoe UI", 8.25F);
            this.buttonRetry.Location = new System.Drawing.Point(0, 0);
            this.buttonRetry.Margin = new System.Windows.Forms.Padding(0, 0, 15, 50);
            this.buttonRetry.Name = "buttonRetry";
            this.buttonRetry.Size = new System.Drawing.Size(150, 35);
            this.buttonRetry.TabIndex = 1;
            this.buttonRetry.Text = "&Retry";
            this.buttonRetry.UseVisualStyleBackColor = false;
            this.buttonRetry.Click += new System.EventHandler(this.buttonRetry_Click);
            this.buttonRetry.MouseLeave += new System.EventHandler(this.buttonRetry_MouseLeave);
            this.buttonRetry.MouseHover += new System.EventHandler(this.buttonRetry_MouseHover);
            // 
            // buttonCancel
            // 
            this.buttonCancel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(103)))), ((int)(((byte)(192)))));
            this.buttonCancel.CornerRadius = 5;
            this.buttonCancel.FlatAppearance.BorderSize = 0;
            this.buttonCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonCancel.Font = new System.Drawing.Font("Segoe UI", 8.25F);
            this.buttonCancel.Location = new System.Drawing.Point(165, 0);
            this.buttonCancel.Margin = new System.Windows.Forms.Padding(0, 0, 50, 50);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(150, 35);
            this.buttonCancel.TabIndex = 0;
            this.buttonCancel.Text = "&Cancel";
            this.buttonCancel.UseVisualStyleBackColor = false;
            this.buttonCancel.Click += new System.EventHandler(this.buttonDone_Click);
            this.buttonCancel.MouseLeave += new System.EventHandler(this.buttonCancel_MouseLeave);
            this.buttonCancel.MouseHover += new System.EventHandler(this.buttonCancel_MouseHover);
            // 
            // QrCodeForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(66)))), ((int)(((byte)(117)))));
            this.BackgroundImage = global::AutopilotManager.Client.Properties.Resources.Win11Background;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(800, 500);
            this.Controls.Add(this.tableLayoutPanel);
            this.DoubleBuffered = true;
            this.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "QrCodeForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Autopilot Import Provider";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Shown += new System.EventHandler(this.QrCodeForm_Shown);
            this.tableLayoutPanel.ResumeLayout(false);
            this.tableLayoutPanel.PerformLayout();
            this.flowLayoutPanelButtons.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxQrCode)).EndInit();
            this.attributesTableLayoutPanel.ResumeLayout(false);
            this.attributesTableLayoutPanel.PerformLayout();
            this.flowLayoutPanelCancel.ResumeLayout(false);
            this.flowLayoutPanelCancel.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel;
        private System.Windows.Forms.Label labelHeading;
        private System.Windows.Forms.Label labelProvisioningInformation;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanelButtons;
        private System.Windows.Forms.PictureBox pictureBoxQrCode;
        private System.Windows.Forms.TableLayoutPanel attributesTableLayoutPanel;
        private System.Windows.Forms.Label labelId;
        private System.Windows.Forms.Label labelManufacturer;
        private System.Windows.Forms.Label labelModel;
        private System.Windows.Forms.Label labelSerialNumber;
        private System.Windows.Forms.Label labelRegistered;
        private System.Windows.Forms.Label labelIdValue;
        private System.Windows.Forms.Label labelManufacturerValue;
        private System.Windows.Forms.Label labelModelValue;
        private System.Windows.Forms.Label labelSerialNumberValue;
        private System.Windows.Forms.Label labelRegisteredValue;
        private System.Windows.Forms.Label labelEndpointsValue;
        private System.Windows.Forms.Label labelEndpoints;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanelCancel;
        private System.Windows.Forms.Label labelCancel;
        private RoundedButton buttonCancel;
        private RoundedButton buttonRetry;
    }
}