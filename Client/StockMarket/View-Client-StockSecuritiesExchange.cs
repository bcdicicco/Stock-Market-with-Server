using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace StockExchangeMarket
{
    

    public partial class StockSecuritiesExchange : Form
    {
        TcpClient tcpClient;
        RealTimedata Subject;
        int sessionNum;
        int comSeq;
        // Used for comSeq
        static Random randomNumber = new Random();

        public StockSecuritiesExchange()
        {
            InitializeComponent();          
        }

        private void beginTradingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            
            // Open server
            if (tcpClient == null)
            {
                tcpClient = new TcpClient();
                try {
                    tcpClient.Connect(this.serverIPToolStripMenuItem.Text, Convert.ToInt32(this.serverPortToolStripMenuItem.Text));
                }

                // Can't open if already open
                catch (Exception error) {
                    tcpClient = null;
                    Console.Write(error.Message);
                }
            }

            // Increment comSeq
            comSeq = randomNumber.Next();

            // Register message
            string register = "REGISTER SME/TCP-1.0\nID: " + this.userNameToolStripMenuItem.Text.ToString() + " CSeq: " + comSeq++ + " Notification Port: ";

            // Pathway to server
            NetworkStream stream = tcpClient.GetStream();

            //Convert message to byte array
            byte[] clientMessage = ASCIIEncoding.ASCII.GetBytes(register);

            // Send to server
            stream.Write(clientMessage, 0, clientMessage.Length);

            //Reponse
            byte[] bSize = new byte[tcpClient.ReceiveBufferSize];
            int bytes = stream.Read(bSize, 0, tcpClient.ReceiveBufferSize);
            string eMessage = Encoding.ASCII.GetString(bSize, 0, bytes);
            string[] message = eMessage.Split(' ');

            sessionNum = Convert.ToInt32(message[5]);

            // If response isn't OK, stop connection
            if (message[1] != "OK") {
             tcpClient.Close();
            }

            // Else begin trading
            else {
                this.beginTradingToolStripMenuItem.Enabled = false;
                this.stopTradingToolStripMenuItem.Enabled = true;

                // Get list of companies
                string listMessage = "LISTCOMPANIES SME/TCP-1.0\nCSeq: " + comSeq++ + " Session: " + sessionNum;        
                clientMessage = ASCIIEncoding.ASCII.GetBytes(listMessage);
                stream.Write(clientMessage, 0, clientMessage.Length);
                bytes = stream.Read(bSize, 0, tcpClient.ReceiveBufferSize);
                eMessage = Encoding.ASCII.GetString(bSize, 0, bytes);
                message = eMessage.Split(new string[] { "Data: " }, StringSplitOptions.None);

                // Get stock companies from server and create locally
                Subject = new RealTimedata(message[1], tcpClient, ref comSeq, sessionNum);

                this.watchToolStripMenuItem.Visible = true;
                this.beginTradingToolStripMenuItem.Enabled = false;
                this.ordersToolStripMenuItem.Visible = true;
                this.serverIPToolStripMenuItem.Enabled = false;
                this.serverPortToolStripMenuItem.Enabled = false;
                this.clientPortToolStripMenuItem.Enabled = false;
                this.marketToolStripMenuItem.Text = "Join <<Connected>>";
                this.userNameToolStripMenuItem.Enabled = false;
                this.clientIPToolStripMenuItem.Enabled = false;

                MarketDepthSubMenu(this.marketByPriceToolStripMenuItem1);
                MarketDepthSubMenu(this.marketByOrderToolStripMenuItem1);

                // Get current buy orders
                listMessage = "LISTBUYORDERS SME/TCP-1.0\nCSeq: " + comSeq++ + " Session: " + sessionNum;
                clientMessage = ASCIIEncoding.ASCII.GetBytes(listMessage);
                stream.Write(clientMessage, 0, clientMessage.Length);
                bytes = stream.Read(bSize, 0, tcpClient.ReceiveBufferSize);
                eMessage = Encoding.ASCII.GetString(bSize, 0, bytes);
                message = eMessage.Split(new string[] { "Data: " }, StringSplitOptions.None);

                // Parse message into 3 lists of buy orders
                JObject parsed = JObject.Parse(message[1]);
                List<JToken> MSFTbuys = parsed["MSFT"].Children().ToList();
                List<JToken> AAPLbuys = parsed["AAPL"].Children().ToList();
                List<JToken> FBbuys = parsed["FB"].Children().ToList();
  
                // For each one, parse each section and add to their respective client-side buy order lists
                foreach (Company company in Subject.getCompanies())
                {
                    if (company.Symbol == "MSFT") {
                        foreach (JToken buy in MSFTbuys) {
                            int size = Convert.ToInt32(((string)buy["size"]));
                            double price = Convert.ToDouble(((string)buy["price"]));
                            DateTime date = Convert.ToDateTime(((string)buy["closedPrice"]));
                            company.addBuyOrder(price, size, date);
                        }
                    }

                    else if (company.Symbol == "FB") {
                        foreach (JToken buy in FBbuys)
                        {
                            int size = Convert.ToInt32(((string)buy["size"]));
                            double price = Convert.ToDouble(((string)buy["price"]));
                            DateTime date = Convert.ToDateTime(((string)buy["closedPrice"]));
                            company.addBuyOrder(price, size, date);
                        }
                    }

                    else if (company.Symbol == "AAPL") {
                        foreach (JToken buy in AAPLbuys)
                        {
                            int size = Convert.ToInt32(((string)buy["size"]));
                            double price = Convert.ToDouble(((string)buy["price"]));
                            DateTime date = Convert.ToDateTime(((string)buy["closedPrice"]));
                            company.addBuyOrder(price, size, date);
                        }
                    }
                }

                // Get sell orders and do the same process as buy orders
                listMessage = "LISTSELLORDERS SME/TCP-1.0\nCSeq: " + comSeq++ + " Session: " + sessionNum;
                clientMessage = ASCIIEncoding.ASCII.GetBytes(listMessage);
                stream.Write(clientMessage, 0, clientMessage.Length);
                bytes = stream.Read(bSize, 0, tcpClient.ReceiveBufferSize);
                eMessage = Encoding.ASCII.GetString(bSize, 0, bytes);
                message = eMessage.Split(new string[] { "Data: " }, StringSplitOptions.None);

                parsed = JObject.Parse(message[1]);
                List<JToken> MSFTsells = parsed["MSFT"].Children().ToList();
                List<JToken> FBsells = parsed["FB"].Children().ToList();
                List<JToken> AAPLsells = parsed["AAPL"].Children().ToList();

                foreach (Company company in Subject.getCompanies())
                {
                    if (company.Symbol == "MSFT")
                    {
                        foreach (JToken sell in MSFTsells)
                        {
                            int size = Convert.ToInt32(((string)sell["size"]));
                            double price = Convert.ToDouble(((string)sell["price"]));
                            DateTime date = Convert.ToDateTime(((string)sell["closedPrice"]));
                            company.addSellOrder(price, size, date);
                        }
                    }

                    else if (company.Symbol == "FB")
                    {
                        foreach (JToken sell in FBsells)
                        {
                            int size = Convert.ToInt32(((string)sell["size"]));
                            double price = Convert.ToDouble(((string)sell["price"]));
                            DateTime date = Convert.ToDateTime(((string)sell["closedPrice"]));
                            company.addSellOrder(price, size, date);
                        }
                    }

                    else if (company.Symbol == "AAPL")
                    {
                        foreach (JToken sell in AAPLsells)
                        {
                            int size = Convert.ToInt32(((string)sell["size"]));
                            double price = Convert.ToDouble(((string)sell["price"]));
                            DateTime date = Convert.ToDateTime(((string)sell["closedPrice"]));
                            company.addSellOrder(price, size, date);
                        }
                    }
                }

                // New thread to allow for multiple users
                Thread listener = new Thread(new ThreadStart(serverListen));
                listener.Start();
            }
        }

        public void serverListen()
        {
            // Always listen for other users' actions
            while (true)
            {
                NetworkStream stream = tcpClient.GetStream();
                byte[] bSize = new byte[tcpClient.ReceiveBufferSize];
                int bytes = stream.Read(bSize, 0, tcpClient.ReceiveBufferSize);
                string eMessage = Encoding.ASCII.GetString(bSize, 0, bytes);
                string[] message = eMessage.Split(' ');

                // If notified of something by another user
                if (message[0] == "NOTIFY")
                {
                    // Parse the buy or sell order
                    string[] data = eMessage.Split(new string[] { "Data: " }, StringSplitOptions.None);

                    JObject parsed = JObject.Parse(data[1]);

                    double price = Convert.ToDouble((string)parsed.SelectToken("price"));
                    int size = Convert.ToInt32((string)parsed.SelectToken("size"));
                    DateTime date = DateTime.Now;

                    // Add to buy orders
                    if (message[1] == "BUY") {
                        foreach (Company company in Subject.getCompanies())
                        {
                            if (message[2] == company.Symbol)
                            {
                                company.addBuyOrder(price, size, date);
                            }
                        }
                    }

                    // Add to sell orders
                    else {                   
                        foreach (Company company in Subject.getCompanies())
                        {
                            if (message[2] == company.Symbol)
                            {
                                company.addSellOrder(price, size, date);
                            }
                        }
                    }
                }
            
                // Should never happen
                else if (message[0] == "FAIL") {
                    break;
                }
            }
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (tcpClient != null)
                tcpClient.Close();

            tcpClient = null;
            this.Close();
        }

        private void StockStateSummaryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            StockStateSummary summaryObserver = new StockStateSummary(Subject);
            summaryObserver.MdiParent = this;

            // Display the new form.
            summaryObserver.Show();

        }
        private void cascadeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Cascade all MDI child windows.
            this.LayoutMdi(MdiLayout.Cascade);
        }



        private void arrangeIconsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Tile all child forms vertically.
            this.LayoutMdi(MdiLayout.ArrangeIcons);

        }

        private void horizontalTileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Tile all child forms horizontally.
            this.LayoutMdi(MdiLayout.TileHorizontal);
        }

        private void verticalTileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Tile all child forms vertically.
            this.LayoutMdi(MdiLayout.TileVertical);

        }

        private void stopTradingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //close connection
            if (tcpClient != null)
                tcpClient.Close();

            tcpClient = null;

            this.beginTradingToolStripMenuItem.Enabled = true;
            this.stopTradingToolStripMenuItem.Enabled = false;
            this.marketToolStripMenuItem.Text = "Join <<Disconnected>>";
            this.watchToolStripMenuItem.Visible = false;
            this.ordersToolStripMenuItem.Visible = false;     
            this.userNameToolStripMenuItem.Enabled = true;
            this.clientIPToolStripMenuItem.Enabled = true;
            this.clientPortToolStripMenuItem.Enabled = true;
            this.serverIPToolStripMenuItem.Enabled = true;
            this.serverPortToolStripMenuItem.Enabled = true;

            foreach (Form form in this.MdiChildren)
            {
                form.Dispose();
                form.Close();
            }
        }



        public void MarketDepthSubMenu(ToolStripMenuItem MnuItems)
        {
            ToolStripMenuItem SSSMenu;
            List<Company> StockCompanies = Subject.getCompanies();
            foreach (Company company in StockCompanies)
            {
                if (MnuItems.Name == "marketByPriceToolStripMenuItem1")
                    SSSMenu = new ToolStripMenuItem(company.Name, null, marketByPriceToolStripMenuItem_Click);
                else
                    SSSMenu = new ToolStripMenuItem(company.Name, null, marketByOrderToolStripMenuItem_Click);
                MnuItems.DropDownItems.Add(SSSMenu);
            }
        }

        public void marketByOrderToolStripMenuItem_Click(object sender, System.EventArgs e)
        {
           
            MarketByOrder newMDIChild = new MarketByOrder(Subject, sender.ToString());
            // Set the parent form of the child window.
            newMDIChild.Text = "Market Depth By Order (" + sender.ToString() + ")";
            newMDIChild.MdiParent = this;
            // Display the new form.
            newMDIChild.Show();
        }
        private void marketByPriceToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MarketByPrice newMDIChild = new MarketByPrice(Subject, sender.ToString());
            // Set the parent form of the child window.

            newMDIChild.Text = "Market Depth By Price (" + sender.ToString() + ")";

            newMDIChild.MdiParent = this;
            // Display the new form.
            newMDIChild.Show();
        }

        private void bidToolStripMenuItem_Click(object sender, EventArgs e)
        {
            PlaceBidOrder newMDIChild = new PlaceBidOrder(Subject, tcpClient, ref comSeq, sessionNum);
            // Set the parent form of the child window.
            newMDIChild.MdiParent = this;
            // Display the new form.
            newMDIChild.Show();
        }

        private void askToolStripMenuItem_Click(object sender, EventArgs e)
        {
            PlaceSellOrder newMDIChild = new PlaceSellOrder(Subject, tcpClient, ref comSeq, sessionNum);
            // Set the parent form of the child window.
            newMDIChild.MdiParent = this;
            // Display the new form.
            newMDIChild.Show();
        }

        private void StockSecuritiesExchange_Load(object sender, EventArgs e)
        {

        }
    }
}
