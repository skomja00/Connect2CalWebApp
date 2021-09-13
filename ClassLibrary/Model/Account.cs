using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using Utilities;

namespace CalendarClassLibrary.Model
{
    [Serializable]
    public class Account
    {
        private int accountId;
        private string accountRoleType;
        private string userName;
        private string userAddress;
        private string phoneNumber;
        private string createdEmailAddress;
        private string contactEmailAddress;
        private int avatar;
        private Byte[] accountPassword;
        private DateTime dateTimeStamp;
        private string responseCity;
        private string responsePhone;
        private string responseSchool;
        private string active;
        private int securityPIN;
        private static Byte[] key = { 250, 101, 18, 76, 45, 135, 207, 118, 4, 171, 3, 168, 202, 241, 37, 199 };
        private static Byte[] vector = { 146, 64, 191, 111, 23, 3, 113, 119, 231, 121, 252, 112, 79, 32, 114, 156 };

        // Create an instance of the web service proxy class to send requests 
        // to the Web Service and receive a response from it via SOAP/HTTP.
        AccountService.AccountWS pxyAccount = new AccountService.AccountWS();

        public Account()
        {

        }
        /// <summary>
        /// Encrypt the given password and return an Encrypted Byte[]
        /// </summary>
        /// <param name="thePassword"></param>
        /// <returns></returns>
        public static Byte[] Encrypt(string thePassword)
        {
            UTF8Encoding encoder = new UTF8Encoding();      // used to convert bytes to characters, and back
            Byte[] textBytes;                               // stores the plain text data as bytes

            // Perform Encryption
            //-------------------
            // Convert a string to a byte array, which will be used in the encryption process.
            textBytes = encoder.GetBytes(thePassword);

            // Create an instances of the encryption algorithm (Rinjdael AES) for the encryption to perform,
            // a memory stream used to store the encrypted data temporarily, and
            // a crypto stream that performs the encryption algorithm.
            RijndaelManaged rmEncryption = new RijndaelManaged();
            MemoryStream myMemoryStream = new MemoryStream();
            CryptoStream myEncryptionStream = new CryptoStream(myMemoryStream, rmEncryption.CreateEncryptor(key, vector), CryptoStreamMode.Write);

            // Use the crypto stream to perform the encryption on the plain text byte array.
            myEncryptionStream.Write(textBytes, 0, textBytes.Length);

            // Retrieve the encrypted data from the memory stream, and write it to a separate byte array.
            myMemoryStream.Position = 0;
            Byte[] encryptedBytes = new Byte[myMemoryStream.Length];
            myMemoryStream.Read(encryptedBytes, 0, encryptedBytes.Length);

            // Close all the streams.
            myEncryptionStream.Close();
            myMemoryStream.Close();

            // Convert the bytes to a string and display it.
            return encryptedBytes;
        }
        private static void Decrypt(string thePassword)
        {
            Byte[] encryptedPasswordBytes = Convert.FromBase64String(thePassword);
            Byte[] textBytes;
            UTF8Encoding encoder = new UTF8Encoding();

            // Perform Decryption
            //-------------------
            // Create an instances of the decryption algorithm (Rinjdael AES) for the encryption to perform,
            // a memory stream used to store the decrypted data temporarily, and
            // a crypto stream that performs the decryption algorithm.
            RijndaelManaged rmEncryption = new RijndaelManaged();
            MemoryStream myMemoryStream = new MemoryStream();
            CryptoStream myDecryptionStream = new CryptoStream(myMemoryStream, rmEncryption.CreateDecryptor(key, vector), CryptoStreamMode.Write);

            // Use the crypto stream to perform the decryption on the encrypted data in the byte array.
            myDecryptionStream.Write(encryptedPasswordBytes, 0, encryptedPasswordBytes.Length);
            myDecryptionStream.FlushFinalBlock();

            // Retrieve the decrypted data from the memory stream, and write it to a separate byte array.
            myMemoryStream.Position = 0;
            textBytes = new Byte[myMemoryStream.Length];
            myMemoryStream.Read(textBytes, 0, textBytes.Length);

            // Close all the streams.
            myDecryptionStream.Close();
            myMemoryStream.Close();

            encoder.GetString(textBytes);
        }
        /// <summary>
        /// Create an object of the Account class which is avaialable 
        /// through the web service reference and WSDL
        /// 
        /// Call the AddAccount webmethod using Account service proxy
        /// 
        /// Return the Boolean result returned from the web service.
        /// 
        /// </summary>
        /// <returns>Boolean True if successful update. False otherwise.</returns>
        public Boolean AddAccount()
        {
            AccountService.Account objAccountWS = new AccountService.Account();
            objAccountWS.AccountRoleType = this.AccountRoleType;
            objAccountWS.UserName = this.UserName;
            objAccountWS.UserAddress = this.UserAddress;
            objAccountWS.PhoneNumber = this.PhoneNumber;
            objAccountWS.CreatedEmailAddress = this.CreatedEmailAddress;
            objAccountWS.ContactEmailAddress = this.ContactEmailAddress;
            objAccountWS.Avatar = this.Avatar;
            objAccountWS.AccountPassword = this.AccountPassword;
            objAccountWS.ResponseCity = this.ResponseCity;
            objAccountWS.ResponsePhone = this.ResponsePhone;
            objAccountWS.ResponseSchool = this.ResponseSchool;
            objAccountWS.Active = this.Active;
            objAccountWS.SecurityPIN = GenerateSecurityPIN();

            Boolean result = pxyAccount.AddAccount(objAccountWS);

            if (result == true)
            {
                Email objEmail = new Email();
                String strTO = objAccountWS.ContactEmailAddress;
                String strFROM = "tun49199@temple.edu";
                String strSubject = "Account Activate";
                String strMessage = "SecurityPIN: " + objAccountWS.SecurityPIN.ToString();
                try
                {
                    objEmail.SendMail(strTO, strFROM, strSubject, strMessage);
                    //lblDisplay.Text = "The email was sent.";
                }
                catch (Exception ex)
                {
                    //lblDisplay.Text = "The email wasn't sent because one of the required fields was missing.";
                    return false;
                }
                return true;
            }
            else
            {
                return false;
            }
        }
        public Boolean UpdatePassword()
        {
            AccountService.Account objAccountWS = new AccountService.Account();
            objAccountWS.CreatedEmailAddress = this.CreatedEmailAddress;
            objAccountWS.AccountPassword = this.AccountPassword;
            Boolean result =  pxyAccount.UpdatePassword(objAccountWS);
            return result;
        }
        public Boolean UpdateSecurityPIN()
        {
            AccountService.Account objAccountWS = new AccountService.Account();
            objAccountWS.CreatedEmailAddress = this.CreatedEmailAddress;
            objAccountWS.SecurityPIN = this.SecurityPIN;

            Boolean result = pxyAccount.UpdateSecurityPIN(objAccountWS);

            return result;
        }            
        public int SecurityQuestions()
        {
            AccountService.Account objAccountWS = new AccountService.Account();
            objAccountWS.CreatedEmailAddress = this.CreatedEmailAddress;
            objAccountWS.ResponseCity = this.ResponseCity;
            objAccountWS.ResponsePhone = this.ResponsePhone;
            objAccountWS.ResponseSchool = this.ResponseSchool;
            int numOfCorrectResponses = pxyAccount.SecurityQuestions(objAccountWS);
            return numOfCorrectResponses;
        }
        public DataSet LogIn()
        {
            AccountService.Account objAccountWS = new AccountService.Account();
            objAccountWS.CreatedEmailAddress = this.CreatedEmailAddress;
            objAccountWS.AccountPassword = this.AccountPassword;

            DataSet acctDS = pxyAccount.LogIn(objAccountWS);

            return acctDS;
        }
        public int AccountId
        {
            get { return accountId; }
            set { accountId = value; }
        }
        public string AccountRoleType
        {
            get { return accountRoleType; }
            set { accountRoleType = value; }
        }
        public string UserName
        {
            get { return userName; }
            set { userName = value; }
        }
        public string UserAddress
        {
            get { return userAddress; }
            set { userAddress = value; }
        }
        public string PhoneNumber
        {
            get { return phoneNumber; }
            set { phoneNumber = value; }
        }
        public string CreatedEmailAddress
        {
            get { return createdEmailAddress; }
            set { createdEmailAddress = value; }
        }
        public string ContactEmailAddress
        {
            get { return contactEmailAddress; }
            set { contactEmailAddress = value; }
        }
        public int Avatar
        {
            get { return avatar; }
            set { avatar = value; }
        }
        public Byte[] AccountPassword
        {
            get { return accountPassword; }
            set { accountPassword = value; }
        }
        public string ResponseCity
        {
            get { return responseCity; }
            set { responseCity = value; }
        }
        public string ResponsePhone
        {
            get { return responsePhone; }
            set { responsePhone = value; }
        }
        public string ResponseSchool
        {
            get { return responseSchool; }
            set { responseSchool = value; }
        }
        public string Active
        {
            get { return active; }
            set { active = value; }
        }
        public int SecurityPIN
        {
            get { return securityPIN; }
            set { securityPIN = value; }
        }
        private int GenerateSecurityPIN ()
        {
            Random rand = new Random();
            int pin = rand.Next();
            return pin;
        }
        public DateTime DateTimeStamp
        {
            get { return dateTimeStamp; }
            set { dateTimeStamp = value; }
        }
    }
}
