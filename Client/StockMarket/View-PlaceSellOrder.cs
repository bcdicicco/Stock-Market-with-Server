using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Windows.Forms;

namespace StockExchangeMarket
{
    public partial class PlaceSellOrder : Form
    {
        RealTimedata Subject;
        Company selectedCompany;
        TcpClient tcpClient;
        int comSeq;
        int sessionNum;

        public PlaceSellOrder(Object _subject, TcpClient cli, ref int comSeq, int sessionNum)
        {
            Subject = (RealTimedata)_subject;
            this.tcpClient = cli;
            this.comSeq = comSeq;
            this.sessionNum = sessionNum;

            InitializeComponent();

            foreach (Company company in Subject.getCompanies())
            {
                this.comboBox1.Items.Add(company.Name);   
            }
            comboBox1.SelectedIndex = 0;
        }

        private void btn_Cancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btn_Submit_Click(object sender, EventArgs e)
        {
            // Check to see if both validation checks return true
            if (ValidShareSize() && ValidSharePrice())
            {
                selectedCompany.addSellOrder(Convert.ToDouble(textBox2.Text), Convert.ToInt32(textBox1.Text));
                // Order that has fields in right parsing order
                placeholderOrder tempOrder;

                //Stream to send to server
                NetworkStream stream = tcpClient.GetStream();

                string sellMessage = "SELLORDER SME/TCP-1.0\nCSeq: " + comSeq++ + " Session: " + sessionNum + " Data: ";

                //Make order with user input data
                tempOrder = new placeholderOrder(Convert.ToDouble(textBox2.Text), Convert.ToInt32(textBox1.Text));

                string orders = "";

                // Convert tempOrder to Json
                if (selectedCompany.Symbol == "MSFT")
                {
                    orders = JsonConvert.SerializeObject(new { MSFT = tempOrder });
                }

                else if (selectedCompany.Symbol == "AAPL")
                {
                    orders = JsonConvert.SerializeObject(new { AAPL = tempOrder });
                }

                else if (selectedCompany.Symbol == "FB")
                {
                    orders = JsonConvert.SerializeObject(new { FB = tempOrder });
                }

                sellMessage += orders;

                // Send order
                byte[] messageToSend = ASCIIEncoding.ASCII.GetBytes(sellMessage);
                stream.Write(messageToSend, 0, messageToSend.Length);

                foreach (Control control in this.Controls)
                {
                    if (control is TextBox)
                    {
                        TextBox textBox = (TextBox)control;
                        textBox.Text = null;
                    }

                    if (control is ComboBox)
                    {
                        ComboBox comboBox = (ComboBox)control;
                        if (comboBox.Items.Count > 0)
                            comboBox.SelectedIndex = 0;
                    }
                }
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            int i = 0;
            foreach (Company company in Subject.getCompanies())
            {
                if (comboBox1.SelectedIndex == i)
                {
                    selectedCompany = company;
                    break;
                }
                else i++;
            }
        }

        // Method that validates the shares size
        // The method returns true if the textbox passes the validation checks
        private bool ValidShareSize()
        {

            int num;
            bool isNum = Int32.TryParse(textBox1.Text.Trim(), out num);

            if (!isNum)
            {
                // If not numeric, set the error
                epErrorProvider.SetError(textBox1, "The # of Shares is invalid");
                return false;
            }
            else
            {
                // If it has a value, clear the error
                epErrorProvider.SetError(textBox1, "");
                return true;
            }

        }

        // Method that validates the shares Price
        // The method returns true if the textbox passes the validation checks
        private bool ValidSharePrice()
        {

            Double Doub;
            bool isDoub = Double.TryParse(textBox2.Text.Trim(), out Doub);
            // Check the Name text
            if (!isDoub)
            {
                // If empty, set the error
                epErrorProvider.SetError(textBox2, "The Price of Shares is invalid");
                return false;
            }
            else
            {
                // If it has a value, clear the error
                epErrorProvider.SetError(textBox2, "");
                return true;
            }
        }

        private void textBox1_Validating(object sender, CancelEventArgs e)
        {
            ValidShareSize();
        }

        private void textBox2_Validating(object sender, CancelEventArgs e)
        {
            ValidSharePrice();
        }

        private void PlaceSellOrder_Load(object sender, EventArgs e)
        {

        }
    }
}
