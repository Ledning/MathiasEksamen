using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eksamensopgaven_2015
{
    class LineSystemCLI : ILineSystemUI
    {
        //private List<Product> allProducts = new List<Product>();
        //private List<Product> activeProducts = new List<Product>();
        private LineSystem _lineSystem;
        private List<Product> _activeProducts = new List<Product>();

        public LineSystemCLI (LineSystem lineSystem)
        {
            _lineSystem = lineSystem;
        }

        public void RunProgram (LineSystemCommandParser cmdParser)
        {
            _lineSystem.productList = _lineSystem.LoadProductList();
            bool programOn = true;

            while (programOn)
            {
                DisplayProductList();

                try
                {
                    cmdParser.ParseCmd(Console.ReadLine());
                }
                #region Lang Liste af catch
                catch (IllegalCashTransactionException e)
                {
                    Console.Clear();
                    Console.WriteLine("Der opstod en fejl i overførslen:\n" + e);
                    Console.ReadKey();
                }

                catch (InsufficientCreditsException)
                {
                    Console.Clear();
                    Console.WriteLine("Du har ikke nok penge. Indsæt flere på kontoen og prøv igen");
                    Console.ReadKey();
                }

                catch (NoProductsFoundException)
                {
                    Console.Clear();
                    Console.WriteLine("Listen over produkter er væk.");
                    Console.ReadKey();
                }

                catch (NoUsersFoundException)
                {
                    Console.Clear();
                    Console.WriteLine("Der er i øjeblikket ingen brugere registreret.");
                    Console.ReadKey();
                }

                catch (ProductNotFoundException inputProductID)
                {
                    Console.Clear();
                    Console.WriteLine("Det indtastede ID Blev ikke fundet.", inputProductID);
                    Console.ReadKey();
                }

                catch (UserNotFoundException)
                {
                    Console.Clear();
                    Console.WriteLine("Brugeren med dette navn eksisterer ikke.");
                    Console.ReadKey();
                }

                catch (GeneralErrorException e)
                {
                    Console.Clear();
                    Console.WriteLine("Der opstod en fejl.\n" + e);
                }

                catch (AdminCommandNotFoundException notFoundCommand)
                {
                    Console.Clear();
                    Console.WriteLine("Den indtastede adminkommando {0} findes ikke. De eksisterende kommandoer er: :adduser, :activate (efterfulgt af produktID), :deactivate (Efterfulgt af produktID), :addcash (Efterfulgt af brugernavn og beløb), :boughtoncrediton (efterfulgt af produktID), :boughtoncreditoff (efterfulgt af produktID) og :quit", notFoundCommand);
                    Console.ReadKey();
                }

                catch (ProductIDNotNumberException inputThatIsNotNumber)
                {
                    Console.Clear();
                    Console.WriteLine("Det indtastede produkt ID {0} er ikke et tal", inputThatIsNotNumber);
                    Console.ReadKey();
                }

                catch (TooManyArgumentsErrorException)
                {
                    Console.Clear();
                    Console.WriteLine("Det indtastede input indeholdt for mange argumenter.");
                    Console.ReadKey();
                }

                catch (Exception e)
                {
                    Console.Clear();
                    Console.WriteLine(e.Message);
                    Console.ReadKey();
                }
                #endregion

                Console.ReadKey();
            }
        }

        public void DisplayProductList()
        {
            Console.Clear();

            foreach (Product item in _lineSystem.productList)
            {
                if (item.active == true)
                {
                    Console.Write(item.productID + "\t" + item.name);
                    Console.CursorLeft = Console.BufferWidth - 35;
                    Console.Write(item.price + " kr \n");
                }
            }  
        }

        public User AddUser()
        {

            string firstName = DisplayAddUserExecutor("fornavn");
            string lastName = DisplayAddUserExecutor("efternavn");
            string userName = DisplayAddUserExecutor("brugernavn");
            string email = DisplayAddUserExecutor("email");

            User newUser = new User(firstName, lastName, userName, email);
            return newUser;
        }

        public string DisplayAddUserExecutor(string wantedString)
        {
            string createdString;
            bool validater = true;

            do
            {
                validater = true;
                Console.Clear();
                Console.WriteLine("Indtast venligst brugerens " + wantedString);
                createdString = Console.ReadLine();

                if (wantedString != "email")
                {
                    foreach (char checker in createdString)
                    {
                        if (wantedString == "fornavn" || wantedString == "efternavn")
                        {
                            if (!Char.IsLetter(checker))
                                validater = false;
                        }

                        else if (wantedString == "brugernavn")
                        {
                            if (!Char.IsLower(checker) && !Char.IsDigit(checker) && checker != '_')
                                validater = false;
                        }
                    }
                }

                else
                {
                    string[] emailSplitter = createdString.Split('@');
                    if (emailSplitter.Count() != 2 || !emailSplitter[0].Contains('.') || emailSplitter[1].StartsWith("-") || emailSplitter[1].StartsWith(".") || emailSplitter[1].EndsWith("-") || emailSplitter[1].EndsWith("."))
                    {
                        validater = false;
                    }

                    foreach (char checker in emailSplitter[0])
                    {
                        if (!Char.IsLetterOrDigit(checker) && checker != '.' && checker != '_' && checker != '-')
                            validater = false;
                    }

                    foreach (char checker in emailSplitter[1])
                    {
                        if (!Char.IsLetterOrDigit(checker) && checker != '.' && checker != '-') //tager ikke højde for at ikkeengelske bogstaver ikke må benyttes.
                            validater = false;
                    }
                }
            }
            while (validater == false);

            return createdString;
        }

        public void DisplayUserNotFound(string notFoundUserName)
        {
            throw new UserNotFoundException();
        }

        public void DisplayProductNotFound(uint inputProductID)
        {
            throw new ProductNotFoundException(Convert.ToString(inputProductID));
        }

        public void DisplayUserInfo(string inputUserName)
        {
            Console.Clear();
            Console.WriteLine(_lineSystem.userList[1].userName);
            Console.WriteLine(inputUserName);
            Console.ReadKey();

            foreach (User user in _lineSystem.userList)
	        {
                
                
                if(user.userName == inputUserName)
                {
                    Console.Clear();
                    Console.WriteLine("Brugernavn: " + user.userName);
                    Console.WriteLine("Navn: {0} {1}", user.firstName, user.lastName);
                    Console.WriteLine("Saldo: " + user.balance);

                    if (user.balance < 50)
                    {
                        Console.WriteLine("Din saldo er lav. Overvej venligst at indsætte flere penge på kontoen.");
                    }

                    Console.ReadKey();
                    return; 
                }
	        }

            DisplayUserNotFound(inputUserName);
        }

        public void DisplayTooManyArgumentsError()
        {
            throw new TooManyArgumentsErrorException();
        }

        public void DisplayAdminCommandNotFoundMessage(string notFoundAdminCommand)
        {
            throw new AdminCommandNotFoundException(notFoundAdminCommand);
        }

        public void DisplayUserBuysProduct(BuyTransaction transaction)
        {

        }

        public void DisplayUserBuysProduct(string inputUserName, uint inputProductID)
        {
            foreach (User user in _lineSystem.userList)
	        {
                if(user.userName == inputUserName)
                {
                    foreach (Product product in _lineSystem.productList)
                    {
                        if(product.productID == inputProductID)
                        {
                            _lineSystem.BuyProduct(user, product);
                            return;
                        }
                    }

                    DisplayProductNotFound(inputProductID);
                    return;
                }
            }

            DisplayUserNotFound(inputUserName);
        }

        public void Close()
        {
            Console.Clear();
            Console.WriteLine("Programmet lukkes.");
            Console.ReadKey();
            Environment.Exit(1);
        }

        public void DisplayInsufficientCash()
        {
            throw new InsufficientCreditsException();
        }

        public void DisplayGeneralError(string errorString)
        {
            throw new GeneralErrorException(errorString);
        }

        public void DisplayProductIDNotNumber (string inputThatIsNotNumber)
        {
            throw new ProductIDNotNumberException(inputThatIsNotNumber);
        }

        public void DisplayIllegalCashTransaction (string errorString)
        {
            throw new IllegalCashTransactionException(errorString);
        }

        public void DisplayNoProductsFound()
        {
            throw new NoProductsFoundException();
        }

        public void DisplayNoUsersFound()
        {
            throw new NoUsersFoundException();
        }
    }
}
