﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Eksamensopgaven_2015
{
    class LineSystem 
    {
        public List<User> userList = new List<User>();
        public List<Product> productList = new List<Product>();
        public List<Transaction> transactionList = new List<Transaction>();
        
        //private int _chosenProduct;
        //private int _currentUser;

        public int chosenProduct { get; set; }
        public int currentUser { get; set; }

        public LineSystem()
        {
            productList = LoadProductList();
        }

        public List<Product> LoadProductList()
        {
            List<Product> productsFound = new List<Product>();
            List<string[]> dataFound = new List<string[]>();
            string path = Directory.GetCurrentDirectory() + @"\products.csv";
            if (!(File.Exists(path)))
            {
                Console.WriteLine("The product list is missing. Find it.");
            }
            else
            {
                using (StreamReader reader = new StreamReader(path, Encoding.Default))
                {
                    reader.ReadLine(); //Smider første linje væk.
                    string line;
                    string[] column;
                    while ((line = reader.ReadLine()) != null)
                    {
                        column = line.Split(';');
                        dataFound.Add(column);
                    }
                }

                foreach (var item in dataFound)
                {
                    Product product = new Product();
                    uint iD;
                    UInt32.TryParse(item[0], out iD);
                    product.productID = iD;
                    product.name = item[1];

                    //De næste 4 if-sætninger fjerner alle uønskede dele fra produktnavnene.
                    if (product.name.Contains("<b>"))
                    {
                        product.name = product.name.Replace("<b>", "");
                        product.name = product.name.Replace("</b>", "");
                    }

                    if (product.name.Contains("<h1>"))
                    {
                        product.name = product.name.Replace("<h1>", "");
                        product.name = product.name.Replace("</h1>", "");
                    }

                    if (product.name.Contains("<h2>"))
                    {
                        product.name = product.name.Replace("<h2>", "");
                        product.name = product.name.Replace("</h2>", "");
                    }

                    if (product.name.Contains("\""))
                    {
                        product.name = product.name.Replace("\"", "");
                    }

                    decimal productPrice;
                    Decimal.TryParse((item[2]), out productPrice);
                    product.price = productPrice / 100; //divideres med 100, da prisen er angivet i ører.

                    switch (item[3])
                    {
                        case "0":
                            product.active = false;
                            break;
                        case "1":
                            product.active = true;
                            break;
                    }

                    productsFound.Add(product);

                    //Delen med deactivate_date ignoreres ved ikke at bearbejde item[4].
                    //Console.WriteLine(product.productID + product.name + product.price + product.active);
                }
            }

            return productsFound;
        }

        public void BuyProduct (User currentUser, Product chosenProduct)
        {
            BuyTransaction transaction = new BuyTransaction(currentUser, chosenProduct);
            ExecuteTransaction(transaction);
        }

        public void AddCreditsToAccount (User currentUser, decimal amount)
        {
            InsertCashTransaction insert = new InsertCashTransaction(currentUser, amount);
            ExecuteTransaction(insert);
        }

        public void ExecuteTransaction (Transaction transactionIn)
        {
            if (transactionIn is BuyTransaction)
            {
                var transaction = (BuyTransaction)transactionIn;

                if (transaction.Execute())
                {
                    transactionList.Add(transaction);
                    WriteToFile(transaction);
                }

                else
                    throw new IllegalCashTransactionException();
            }

            if (transactionIn is InsertCashTransaction)
            {
                var transaction = (InsertCashTransaction)transactionIn;

                if (transaction.Execute())
                {
                    transactionList.Add(transaction);
                    WriteToFile(transaction);
                }

                else
                    throw new IllegalCashTransactionException();
            }
        }

        public Product GetProduct(uint iD)
        {
            foreach (Product item in productList)
            {
                if (item.productID == iD)
                {
                    return item;
                }
            }

            throw new ProductNotFoundException();
        }

        

        public User GetUser (string currentUsername)
        {            
            foreach (User item in userList)
            {
                if (item.userName == currentUsername)
                {
                    return item;
                }
            }

            return null;
        }

        public void GetTransactionList ()
        {

        }

        public List<Product> GetActiveProducts ()
        {
            List<Product> activeProductList = new List<Product>();
            foreach (Product item in productList)
            {
                if (item.active)
                {
                    activeProductList.Add(item);
                }
            }

            if (activeProductList == null)
            {
                throw new NoProductsFoundException();
            }

            return activeProductList;
        }

        

        public void WriteToFile(Transaction t)
        {
            //sørg for at skelne mellem de to typer m8 (samme som ExecuteTransaction)
            //using StreamWriter writer;
        }
    }
}
