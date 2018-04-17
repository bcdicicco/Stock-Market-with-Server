namespace StockExchangeMarket
{
    partial class StockStateSummary
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
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(StockStateSummary));
            this.summaryGrid = new System.Windows.Forms.DataGridView();
            this.Company = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Symbol = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.openPrice = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.LastSale = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Net = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.img = new System.Windows.Forms.DataGridViewImageColumn();
            this.Percentage = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Volume = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Icons = new System.Windows.Forms.ImageList(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.summaryGrid)).BeginInit();
            this.SuspendLayout();
            // 
            // summaryGrid
            // 
            this.summaryGrid.AllowDrop = true;
            this.summaryGrid.AllowUserToAddRows = false;
            this.summaryGrid.AllowUserToDeleteRows = false;
            this.summaryGrid.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.summaryGrid.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
            this.summaryGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.summaryGrid.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Company,
            this.Symbol,
            this.openPrice,
            this.LastSale,
            this.Net,
            this.img,
            this.Percentage,
            this.Volume});
            this.summaryGrid.Location = new System.Drawing.Point(12, 12);
            this.summaryGrid.Name = "summaryGrid";
            this.summaryGrid.ReadOnly = true;
            this.summaryGrid.Size = new System.Drawing.Size(592, 153);
            this.summaryGrid.TabIndex = 0;
            this.summaryGrid.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.summaryGrid_CellContentClick);
            // 
            // Company
            // 
            this.Company.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.Company.Frozen = true;
            this.Company.HeaderText = "Company";
            this.Company.MaxInputLength = 50;
            this.Company.Name = "Company";
            this.Company.ReadOnly = true;
            this.Company.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.Company.Width = 150;
            // 
            // Symbol
            // 
            this.Symbol.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.Symbol.FillWeight = 40F;
            this.Symbol.Frozen = true;
            this.Symbol.HeaderText = "Symbol";
            this.Symbol.MaxInputLength = 10;
            this.Symbol.Name = "Symbol";
            this.Symbol.ReadOnly = true;
            this.Symbol.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.Symbol.Width = 60;
            // 
            // openPrice
            // 
            this.openPrice.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            dataGridViewCellStyle1.Format = "C2";
            dataGridViewCellStyle1.NullValue = null;
            this.openPrice.DefaultCellStyle = dataGridViewCellStyle1;
            this.openPrice.HeaderText = "Open Price";
            this.openPrice.Name = "openPrice";
            this.openPrice.ReadOnly = true;
            this.openPrice.Width = 60;
            // 
            // LastSale
            // 
            this.LastSale.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            dataGridViewCellStyle2.Format = "C2";
            dataGridViewCellStyle2.NullValue = null;
            this.LastSale.DefaultCellStyle = dataGridViewCellStyle2;
            this.LastSale.FillWeight = 70F;
            this.LastSale.HeaderText = "Last Sale";
            this.LastSale.Name = "LastSale";
            this.LastSale.ReadOnly = true;
            this.LastSale.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.LastSale.Width = 60;
            // 
            // Net
            // 
            this.Net.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.Net.HeaderText = "Change Net";
            this.Net.Name = "Net";
            this.Net.ReadOnly = true;
            this.Net.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.Net.Width = 60;
            // 
            // img
            // 
            this.img.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.img.HeaderText = "";
            this.img.Name = "img";
            this.img.ReadOnly = true;
            this.img.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.img.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.img.Width = 15;
            // 
            // Percentage
            // 
            this.Percentage.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            dataGridViewCellStyle3.Format = "N3";
            dataGridViewCellStyle3.NullValue = null;
            this.Percentage.DefaultCellStyle = dataGridViewCellStyle3;
            this.Percentage.HeaderText = "Change %";
            this.Percentage.Name = "Percentage";
            this.Percentage.ReadOnly = true;
            this.Percentage.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.Percentage.Width = 60;
            // 
            // Volume
            // 
            this.Volume.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.Volume.HeaderText = "Share Volume";
            this.Volume.Name = "Volume";
            this.Volume.ReadOnly = true;
            this.Volume.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.Volume.Width = 80;
            // 
            // Icons
            // 
            this.Icons.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("Icons.ImageStream")));
            this.Icons.TransparentColor = System.Drawing.Color.Transparent;
            this.Icons.Images.SetKeyName(0, "down.bmp");
            this.Icons.Images.SetKeyName(1, "up.bmp");
            this.Icons.Images.SetKeyName(2, "noChange.bmp");
            this.Icons.Images.SetKeyName(3, "Eng.ico");
            // 
            // StockStateSummary
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(614, 181);
            this.Controls.Add(this.summaryGrid);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "StockStateSummary";
            this.Text = "Stock State Summary";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.StockStateSummary_FormClosed);
            this.Load += new System.EventHandler(this.StockStateSummary_Load);
            ((System.ComponentModel.ISupportInitialize)(this.summaryGrid)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        public System.Windows.Forms.DataGridView summaryGrid;
        private System.Windows.Forms.ImageList Icons;
        private System.Windows.Forms.DataGridViewTextBoxColumn Company;
        private System.Windows.Forms.DataGridViewTextBoxColumn Symbol;
        private System.Windows.Forms.DataGridViewTextBoxColumn openPrice;
        private System.Windows.Forms.DataGridViewTextBoxColumn LastSale;
        private System.Windows.Forms.DataGridViewTextBoxColumn Net;
        private System.Windows.Forms.DataGridViewImageColumn img;
        private System.Windows.Forms.DataGridViewTextBoxColumn Percentage;
        private System.Windows.Forms.DataGridViewTextBoxColumn Volume;
    }
}