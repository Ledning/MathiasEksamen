using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eksamensopgaven_2015
{
    
    class User //: IComparable
    {
        static private uint _userIDCounter;
        private uint _userID;
        private string _firstName;
        private string _lastName;
        private string _userName;
        private string _email;
        private decimal _balance;

        //constructor
        public User (string firstName, string lastName, string userNameIn, string email)
        {
            _userID = ++_userIDCounter;
            _firstName = firstName;
            _lastName = lastName;
            _userName = userNameIn;
            _email = email;
            _balance = balance;
        }

        public uint userID { get; set; }
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string userName { get; set; }
        public string email { get; set; }
        public decimal balance { get; set; }

        public override bool Equals(object obj)
        {
            User userObj = obj as User;

            if (obj == null || (Object) userObj == null)
            {
                return false;
            }

            return (this._userID == userObj._userID);
        }

        public override string ToString()
        {
 	        return _firstName + " " + _lastName + " (" + _email + ")";
        }

        public override int GetHashCode()
        {
            return this.userName.GetHashCode() ^ this.firstName.GetHashCode() ^ this.lastName.GetHashCode();
        }
    }
}





//implementer Icomparable.
