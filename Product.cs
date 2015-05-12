using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eksamensopgaven_2015
{
    class Product
    {
        private uint _productID; //er uint, da ID'et ikke må være negativt.
        private string _name;
        private decimal _price;
        private bool _active;
        private bool _canBeBoughtOnCredit;

        public Product() //Tom constructor, da SeasonalProduct vil have en constructor uden parametre.
        {

        }

        public Product (uint productID, string name, decimal price, bool active, bool canBeBoughtOnCredit)
        {
            _productID = productID;
            _name = name;
            _price = price;
            _active = active;
            _canBeBoughtOnCredit = canBeBoughtOnCredit;
        }

        public uint productID { get; set; }
        public string name { get; set; }
        public decimal price { get; set; }
        public bool active { get; set; }
        public bool canBeBoughtOnCredit { get; set; }
    }
}
