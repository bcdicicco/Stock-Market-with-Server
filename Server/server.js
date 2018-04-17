// Connection setup
{
    var net = require('net');
    var moment = require('moment');
    var port = 8050;

    var sessionNum = 0;
    var SSeq = 0;
}

// Setup stock market companies
{
    var users = [];
    var stockCompanies = {
        "companies": []
    };

    var micro ={
        "name": "Microsoft Corporation",
        "symbol": "MSFT",
        "openPrice": 46.13,
        "currentPrice": 0,
        "closedPrice" : 0
    };

    var apple = {
        "name": "Apple Inc.",
        "symbol": "AAPL",
        "openPrice": 105.22,
        "currentPrice": 0,
        "closedPrice" : 0
    };

    var face = {
        "name": "Facebook, Inc.",
        "symbol": "FB",
        "openPrice": 80.67,
        "currentPrice": 0,
        "closedPrice" : 0
    };

    // Setup orders for each company
    var sOrder = {
        "MSFT": [],
        "AAPL": [],
        "FB": [],
    };

    var bOrder = {
        "MSFT": [],
        "AAPL": [],
        "FB": [],
    };

    stockCompanies.companies.push(micro);
    stockCompanies.companies.push(apple);
    stockCompanies.companies.push(face);
}

// Functionality for orders 
{
    function compareBuy (first, second) {
        if (first.price > second.price)
            return -1;

        if (first.price < second.price)
            return 1;

        return 0;
    }

    function compareSell (first, second) {
        if (first.price < second.price)
            return -1;

        if (second.price > second.price)
            return 1;

        return 0;
    }

    // Same method as previous assignment
    function newBuyOrder(sellList, buyList, newOrder, company) {
        if (sellList.length > 0) {

            sellList.forEach(function (sOrder) {

                if (newOrder.price >= sOrder.price) {

                    if (sOrder.size === newOrder.size) {
                        // Find where sOrder is and remove
                        if (sellList.indexOf(sOrder) >= 0) {
                            sellList.splice(sellList.indexOf(sOrder), 1);
                        }

                        company.currentPrice = newOrder.price;
                    }

                    else if (sOrder.size > newOrder.size) {
                        sOrder.size -= newOrder.size;
                        company.currentPrice = newOrder.price;
                    }

                    else if (sOrder.size < newOrder.size) {
                        newOrder.size -= sOrder.size;

                        if (sellList.indexOf(sOrder) >= 0) {
                            sellList.splice(sellList.indexOf(sOrder), 1);
                        }

                        company.currentPrice = newOrder.price;

                        newBuyOrder(sellList, buyList, newOrder, company);
                    }
                }

                else {
                    buyList.push(newOrder);
                    buyList.sort(compareBuy);
                }
            });
        }

        else

        {
            buyList.push(newOrder);
            buyList.sort(compareBuy);
        }
    }
    
    function newSellOrder(sellList, buyList, newOrder, company) {
        if (buyList.length > 0) {

            buyList.forEach(function (bOrder) {

                if (newOrder.price >= bOrder.price) {

                    if (bOrder.size === newOrder.size) {

                        if (buyList.indexOf(bOrder) >= 0) {
                            buyList.splice(buyList.indexOf(bOrder), 1);
                        }

                        company.currentPrice = newOrder.price;
                    }

                    else if (bOrder.size > newOrder.size) {
                        bOrder.size -= newOrder.size;
                        company.currentPrice = newOrder.price;
                    }

                    else if (bOrder.size < newOrder.size) {
                        newOrder.size -= bOrder.size;

                        if (buyList.indexOf(bOrder) >= 0) {
                            buyList.splice(buyList.indexOf(bOrder), 1);
                        }

                        company.currentPrice = newOrder.price;

                        newSellOrder(sellList, buyList, newOrder, company);
                    }
                }

                else {
                    sellList.push(newOrder);
                    sellList.sort(compareSell);
                }
            });
        }

        else

        {
            sellList.push(newOrder);
            sellList.sort(compareSell);
        }
    }
}

// Server and functionality for server 
{
    var server = net.createServer();

    // On connection
    server.on("connection", function (sock) {
        var session = sessionNum++;
        var comSeq;
        var SSeq = this.SSeq++;
        var remAddress = sock.remoteAddress + ":" + sock.remotePort;
        let ID = null;

        // When data is received, check it
        sock.on("data", function (data) {
            let dataA = data.toString('ascii').split(" ");

            if (ID == null) {
                ID = dataA[2];
            }
    
            console.log("\nTrader %s from is connected from %s.", ID, remAddress);
            console.log("\n%s requests:\n%s" , ID, data);

            switch (dataA[0]) {
                case "REGISTER":
                    // Register user
                    comSeq = parseInt(dataA[4]);
                    // Unique socket
                    users.push(sock);
                
                    sock.write("SME/TCP-1.0 OK \nCSeq: " + comSeq++ + " Session: " + session);
                    console.log("\nServer responds:\nSME/TCP-1.0 OK\nCSeq: " + comSeq + " Session: " + session + " ");
    
                    break;

                case "UNREGISTER":
                    sock.write("SME/TCP-1.0 OK \nCSeq: " + comSeq++ + " Session: " + session);
                    console.log("\nServer responds:\nSME/TCP-1.0 OK \nCSeq: " + comSeq + " Session: " + session + "\n\n" + ID + "'s session has closed");

                    //Get rid of socket of the user
                    if (users.indexOf(sock) >= 0) {
                        users.splice(users.indexOf(sock), 1);
                    }
    
                    sock.end();
    
                    break;

                case "LISTCOMPANIES":
                    sock.write("SME/TCP-1.0 OK \nCSeq: " + comSeq++ + " Session: " + session + " Data: " + JSON.stringify(stockCompanies));
                    console.log("\nServer responds:\nSME/TCP-1.0 OK \nCSeq: " + comSeq + " Session: " + session + " Data: " + JSON.stringify(stockCompanies));
    
                    break;

                case "LISTSELLORDERS":
                    sock.write("SME/TCP-1.0 OK \nCSeq: " + comSeq++ + " Session: " + session + " Data: " + JSON.stringify(sOrder));
                    console.log("\nServer responds:\nSME/TCP-1.0 OK \nCSeq: " + comSeq + " Session: " + session + " Data: " + JSON.stringify(sOrder));
    
                    break;
    
                case "LISTBUYORDERS":
                    sock.write("SME/TCP-1.0 OK \nCSeq: " + comSeq++ + " Session: " + session + " Data: " + JSON.stringify(bOrder));
                    console.log("\nServer responds:\nSME/TCP-1.0 OK \nCSeq: " + comSeq + " Session: " + session + " Data: " + JSON.stringify(bOrder));
    
                    break;

                case "SELLORDER":
                    // Parse data 
                    let sData = data.toString('ascii').split('Data: ');
                    let dOrder = JSON.parse(sData[1]);
                    let orderCompany = Object.keys(JSON.parse(sData[1]));

                    // Create new order
                    if (orderCompany[0] === 'MSFT') {
                        let newSOrder = {
                            "size": dOrder.MSFT.size,
                            "price": dOrder.MSFT.price,
                            "timestamp" : moment().format("DD/MM/YYYY hh:mm:ss A")
                        };
        
                        users.forEach(function (user) {
                            if (user !== sock) {
                                console.log("Notified user: " + ID);
                                user.write('NOTIFY SELL MSFT SME/TCP-1.0 OK \nData: ' + JSON.stringify(newSOrder));
                            }
                        });

                        newSellOrder(sOrder.MSFT, bOrder.MSFT, newSOrder, micro)
                    }

                    else if (orderCompany[0] === 'AAPL') {
                        let newSOrder = {
                            "size": dOrder.AAPL.size,
                            "price": dOrder.AAPL.price,
                            "timestamp" : moment().format("DD/MM/YYYY hh:mm:ss A")
                        };
        
                        users.forEach(function (user) {
                            if (user !== sock) {
                                console.log("Notified user: " + ID);
                                user.write('NOTIFY SELL AAPL SME/TCP-1.0 OK \nData: ' + JSON.stringify(newSOrder));
                            }
                        });

                        newSellOrder(sOrder.AAPL, bOrder.AAPL, newSOrder, apple)
                    }

                    else if (orderCompany[0] === 'FB') {
                        let newSOrder = {
                            "size": dOrder.FB.size,
                            "price": dOrder.FB.price,
                            "timestamp" : moment().format("DD/MM/YYYY hh:mm:ss A")
                        };
        
                        users.forEach(function (user) {
                            if (user !== sock) {
                                console.log("Notified user: " + ID);
                                user.write('NOTIFY SELL FB SME/TCP-1.0 OK \nData: ' + JSON.stringify(newSOrder));
                            }
                        });

                        newSellOrder(sOrder.FB, bOrder.FB, newSOrder, face)
                    }

                    //Output response
                    sock.write("SME/TCP-1.0 OK \nCSeq: " + comSeq++ + " Session: " + session);
                    console.log("\nServer responds:\nSME/TCP-1.0 OK \nCSeq: " + comSeq + " Session: " + session);

                    break;

                case "BUYORDER":
                    let bData = data.toString('ascii').split('Data: ');
                    let bdOrder = JSON.parse(bData[1]);
                    let bOrderCompany = Object.keys(JSON.parse(bData[1]));

                    if (bOrderCompany[0] === 'MSFT') {
                        let newBOrder = {
                            "size": bdOrder.MSFT.size,
                            "price": bdOrder.MSFT.price,
                            "timestamp" : moment().format("DD/MM/YYYY hh:mm:ss A")
                        };
        
                        users.forEach(function (user) {
                            if (user !== sock) {
                                console.log("Notified user: " + ID);
                                user.write('NOTIFY BUY MSFT SME/TCP-1.0 OK \nData: ' + JSON.stringify(newBOrder));
                            }
                        });
    
                        newBuyOrder(sOrder.MSFT, bOrder.MSFT, newBOrder, micro)
                    }
    
                    else if (bOrderCompany[0] === 'AAPL') {
                        let newBOrder = {
                            "size": bdOrder.AAPL.size,
                            "price": bdOrder.AAPL.price,
                            "timestamp" : moment().format("DD/MM/YYYY hh:mm:ss A")
                        };

                        users.forEach(function (user) {
                            if (user !== sock) {
                                console.log("Notified user: " + ID);
                                user.write('NOTIFY BUY AAPL SME/TCP-1.0 OK \nData: ' + JSON.stringify(newBOrder));
                            }
                        });

                        newBuyOrder(sOrder.AAPL, bOrder.AAPL, newBOrder, apple)
                    }
    
                    else if (bOrderCompany[0] === 'FB') {
                        let newBOrder = {
                            "size": bdOrder.FB.size,
                            "price": bdOrder.FB.price,
                            "timestamp" : moment().format("DD/MM/YYYY hh:mm:ss A")
                        };

                        users.forEach(function (user) {
                            if (user !== sock) {
                                console.log("Notified user: " + ID);
                                user.write('NOTIFY BUY FB SME/TCP-1.0 OK \nData: ' + JSON.stringify(newBOrder));
                            }
                        });

                        newBuyOrder(sOrder.FB, bOrder.FB, newBOrder, face)
                    }
    
                    sock.write("SME/TCP-1.0 OK \nCSeq: " + comSeq++ + " Session: " + session);
                    console.log("\nServer responds:\nSME/TCP-1.0 OK \nCSeq: " + comSeq + " Session: " + session);
    
                    break;

                //Default is data sent is supported
                default:
                    sock.write('SME/TCP-1.0 FAIL');
                    sock.end();
                    console.log(ID + " has been closed with an error");
                    break;
            }
        })

        sock.on("error", function (error) {
            console.log("Connection %s error : %s", remAddress, error.message);
         });
    });
}

// Start server
server.listen(port, function () {
    console.log("Stock Security Exchange Server is now available on %j\n", server.address());
});