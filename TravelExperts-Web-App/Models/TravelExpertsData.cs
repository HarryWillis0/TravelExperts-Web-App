using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TravelExperts_Web_App.Models
{
    /// <summary>
    /// operations on Travel experts database
    /// </summary>
    /// @author - Harry
    public static class TravelExpertsData
    {
        /// <summary>
        /// See if a customer exists in Customer table of Travel Experts Database
        ///     check against business phone number (assume unique)
        /// </summary>
        /// <param name="customer">Customer object to check</param>
        /// <returns>True if customer exists, false otherwise</returns>
        public static bool CustomerExists(Customer customer)
        {
            using (TravelExpertsEntities db = new TravelExpertsEntities())
            {
                // find customer by phone number
                var result = db.Customers.SingleOrDefault(cust => cust.CustBusPhone == customer.CustBusPhone);

                return result != null;
            }
        }

        /// <summary>
        /// See if a customer that's in the database, a login account
        ///     check against phone number in Customer table AND email in AspNetUsers table
        /// </summary>
        /// <param name="customer">Customer object to check</param>
        /// <returns>True if account exists, false otherwise</returns>
        public static bool AccountExists(Customer customer)
        {
            if(CustomerExists(customer)) // customer exists in Customer table - if customer is not in customer table, they can't be in accounts table
            {
                using (AccountEntities accntDB = new AccountEntities())
                {
                    // find account by email
                    var accntResult = accntDB.AspNetUsers.SingleOrDefault(accnt => accnt.Email == customer.CustEmail);

                    return accntResult != null; // customer exists in customer table and has a login account in AspNetUsers table
                }
            }
            return false;
        }

        /// <summary>
        /// Insert a customer into the Customer table of Travel Experts database
        /// </summary>
        /// <param name="customer"></param>
        public static void InsertCustomer(Customer customer)
        {
            using (TravelExpertsEntities db = new TravelExpertsEntities())
            {
                db.Customers.Add(customer);
                db.SaveChanges();
            }
        }

        /// <summary>
        /// Update a customer's user name in AspNetUsers table AND Customers table
        /// </summary>
        /// <param name="newCustomer">Customer object to update in database</param>
        public static void UpdateUserName(Customer newCustomer)
        {
            using (AccountEntities db = new AccountEntities())
            {
                // get account from AspNetUsers table by email
                var account = db.AspNetUsers.SingleOrDefault(accnt => accnt.Email == newCustomer.CustEmail);
                if (account != null) // found account
                {
                    account.UserName = newCustomer.UserName;
                    db.SaveChanges();
                }
            }

            using (TravelExpertsEntities db = new TravelExpertsEntities())
            {
                // get customer from Customer table by phone number
                var customer = db.Customers.SingleOrDefault(cust => cust.CustBusPhone == newCustomer.CustBusPhone);
                if(customer != null) // found customer
                {
                    customer.UserName = newCustomer.UserName;
                    db.SaveChanges();
                }
            }
        }
        
        /// <summary>
        /// Update a customer's email in AspNetUsers table and Customers table
        /// </summary>
        /// <param name="customer"></param>
        public static void UpdateEmail(Customer customer)
        {
            using (AccountEntities db = new AccountEntities())
            {
                // get account from AspNetUsers table by email
                var account = db.AspNetUsers.SingleOrDefault(accnt => accnt.Email == customer.CustEmail);
                if (account != null) // found account
                {
                    account.Email = customer.CustEmail;
                    db.SaveChanges();
                }
            }

            using (TravelExpertsEntities db = new TravelExpertsEntities())
            {
                // get customer from Customer table by phone number
                var cust = db.Customers.SingleOrDefault(c => c.CustBusPhone == customer.CustBusPhone);
                if (customer != null) // found customer
                {
                    cust.CustEmail = customer.CustEmail;
                    db.SaveChanges();
                }
            }
        }
        
        /// <summary>
        /// See if user name is free to use
        ///     case insensitive
        /// </summary>
        /// <param name="userName">user name to check</param>
        /// <returns>True if free to use, otherwise false</returns>
        public static bool IsUniqueUserName(string userName)
        {
            using (AccountEntities db = new AccountEntities())
            {
                var taken = db.AspNetUsers.SingleOrDefault(accnt => accnt.UserName.ToLower() == userName.ToLower());
                return taken == null;
            }

        }

        /// <summary>
        /// See if there's already an account linked to this email, case insensitive
        /// </summary>
        /// <param name="custEmail"></param>
        /// <returns>true if no account is linked with email, false otherwise</returns>
        public static bool IsUniqueEmail(string custEmail, out string error)
        {
            using (AccountEntities db = new AccountEntities())
            {
                var taken = db.AspNetUsers.SingleOrDefault(accnt => accnt.Email.ToLower() == custEmail.ToLower());
                if (taken == null)
                {
                    error = "";
                    return true;
                }
                error = "An account already exists with this email.";
                return false;
            }
        }
    }
}