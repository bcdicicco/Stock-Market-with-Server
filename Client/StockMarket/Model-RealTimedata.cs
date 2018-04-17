using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;

namespace StockExchangeMarket
{
    public class RealTimedata : StockMarket
    {
        private List<Company> StockCompanies = new List<Company>();

        public RealTimedata(string data, TcpClient cli, ref int comSeq, int sessionNum)
        {
            // Parse data
            JObject controller = JObject.Parse(data);

            // Represent as a list of companies
            List<JToken> companies = controller["companies"].Children().ToList();

            // Add every company
            foreach(JToken company in companies) {
                string name = (string)company["name"];
                string symbol = (string)company["symbol"];
                double openPrice = Convert.ToDouble(((string)company["openPrice"]));
                double closePrice = Convert.ToDouble(((string)company["closedPrice"]));
                double currentPrice = Convert.ToDouble(((string)company["currentPrice"]));
               
                addCompany(symbol, name, openPrice, closePrice, currentPrice, cli, ref comSeq, sessionNum);
            }
        }

        public void addCompany(String symbol, String name, double price, double closePrice, double currentPrice, TcpClient cli, ref int comSeq, int sessionNum)
        {
           Company newCompany = new Company(symbol, name, price, this, closePrice, currentPrice, cli, ref comSeq, sessionNum);
           StockCompanies.Add(newCompany);
        }

        public List<Company> getCompanies()
        {
            return StockCompanies;
        }

        
    }
}
