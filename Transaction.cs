using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eksamensopgaven_2015
{
    abstract class Transaction
    {
        static protected uint _transactionIDCounter;
        protected uint _transactionID;
        protected User _currentUser;
        protected DateTime _date;
        protected decimal _amount;
        

        public bool Execute ()
        {
            return true;
        }

        public override string ToString()
        {
            return _transactionID.ToString() + _amount.ToString() + _date.ToString();
        }

        //public Transaction (uint transactionID, User currentUser, DateTime date, decimal amount)
        //{
        //    _transactionID = ++_transactionIDCounter;
        //    _currentUser = currentUser;
        //    _date = date;
        //    _amount = amount;
        //}

        public Guid transactionID { get; set; }
        public User currentUser { get; set; }
        public DateTime date { get; set; }
        public decimal amount { get; set; }
    }
}
