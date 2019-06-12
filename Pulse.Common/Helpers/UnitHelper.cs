namespace Pulse.Common.Helpers
{
    using Microsoft.AspNet.Identity;
    using Newtonsoft.Json;
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.IO;
    using System.Linq;
    using System.Net;
    using System.Net.NetworkInformation;
    using System.Net.Sockets;
    using System.Security.Cryptography;
    using System.Text;
    using System.Text.RegularExpressions;

    public static class UnitHelper
    {

        public static string SerializeObject(this object obj)
        {
            return JsonConvert.SerializeObject(obj);
        }
        public static string GetHash(string input)
        {
            HashAlgorithm hashAlgorithm = new SHA256CryptoServiceProvider();
            byte[] byteValue = Encoding.UTF8.GetBytes(input);
            byte[] byteHash = hashAlgorithm.ComputeHash(byteValue);
            return Convert.ToBase64String(byteHash);
        }

        public static string ReplaceString(this object input)
        {
            Regex regEx = new Regex(@"[\W_]+");
            return Regex.Replace(regEx.Replace("" + input, ""), @"\s+", "");
        }

        public static string CountDay(this object date)
        {
            if (date == null) return string.Empty;

            DateTime dt;
            DateTime.TryParse(date.ToString(), out dt);
            TimeSpan duration = DateTime.Now - dt;
            string result = (duration.Minutes == 0 ? (duration.Seconds == 0 ? 1 : duration.Seconds) + " Seconds" : duration.Minutes + " Minutes");
            if (duration.Hours != 0 && duration.Days == 0)
            {
                result = duration.Hours + " Hours";
            }
            else if (duration.Days != 0 && duration.Hours != 0)
            {
                if (duration.Days >= 31)
                {
                    result = MonthDifference(DateTime.Now, dt) + " Months";
                }
                else
                {
                    if (DateTime.Now.Year == dt.Year) result = duration.Days + " Days";
                    else
                    {
                        result = duration.Days + " Years";
                    }
                }
            }
            return result;
        }

        private static int MonthDifference(DateTime lValue, DateTime rValue)
        {
            return Math.Abs((lValue.Month - rValue.Month) + 12 * (lValue.Year - rValue.Year));
        }

        public static string Encrypt(string plainText, string password, string saltKey, string viKey)
        {
            byte[] plainTextBytes = Encoding.UTF8.GetBytes(plainText);
            byte[] keyBytes = new Rfc2898DeriveBytes(password, Encoding.ASCII.GetBytes(saltKey)).GetBytes(256 / 8);
            var symmetricKey = new RijndaelManaged() { Mode = CipherMode.CBC, Padding = PaddingMode.Zeros };
            var encryptor = symmetricKey.CreateEncryptor(keyBytes, Encoding.ASCII.GetBytes(viKey));
            byte[] cipherTextBytes;
            using (var memoryStream = new MemoryStream())
            {
                using (var cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write))
                {
                    cryptoStream.Write(plainTextBytes, 0, plainTextBytes.Length);
                    cryptoStream.FlushFinalBlock();
                    cipherTextBytes = memoryStream.ToArray();
                    cryptoStream.Close();
                }
                memoryStream.Close();
            }
            return Convert.ToBase64String(cipherTextBytes);
        }

        public static string Decrypt(string encryptedText, string password, string saltKey, string viKey)
        {
            byte[] cipherTextBytes = Convert.FromBase64String(encryptedText);
            byte[] keyBytes = new Rfc2898DeriveBytes(password, Encoding.ASCII.GetBytes(saltKey)).GetBytes(256 / 8);
            var symmetricKey = new RijndaelManaged() { Mode = CipherMode.CBC, Padding = PaddingMode.None };
            var decryptor = symmetricKey.CreateDecryptor(keyBytes, Encoding.ASCII.GetBytes(viKey));
            using (var memoryStream = new MemoryStream(cipherTextBytes))
            {
                using (var cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read))
                {
                    byte[] plainTextBytes = new byte[cipherTextBytes.Length];
                    int decryptedByteCount = cryptoStream.Read(plainTextBytes, 0, plainTextBytes.Length);
                    memoryStream.Close();
                    cryptoStream.Close();
                    return Encoding.UTF8.GetString(plainTextBytes, 0, decryptedByteCount).TrimEnd("\0".ToCharArray());
                }
            }
        }

        public static string CreateSalt()
        {
            Random rnd = new Random();
            int size = rnd.Next(8, 16);
            RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();
            byte[] buff = new byte[size];
            rng.GetBytes(buff);
            return Convert.ToBase64String(buff);
        }

        public static string GetMacAddress()
        {
            var macAddr =
            (
                from nic in NetworkInterface.GetAllNetworkInterfaces()
                where nic.OperationalStatus == OperationalStatus.Up
                select nic.GetPhysicalAddress().ToString()
            ).FirstOrDefault();

            return macAddr;
        }  

        public static string GetLocalIPAddress()
        {
            var host = Dns.GetHostEntry(Dns.GetHostName());
            var id = host.AddressList.Where(x => x.AddressFamily == AddressFamily.InterNetwork).FirstOrDefault();
            if(id == null) throw new Exception("Local IP Address Not Found!");
            return id.ToString();
        }

        public static string FormatDate(this DateTime? date, string format = "{0:dd/MM/yyyy hh:mm}", string defaultValue = "")
        {
            if (date == null) return defaultValue;

            return FormatDate(Convert.ToDateTime(date), format);
        }

        public static string FormatDate(this DateTime date, string format = "{0:dd/MM/yyyy hh:mm}")
        {
            return string.Format(format, date);
        }

        public static string GenerateNewGuid()
        {
            return Guid.NewGuid().ToString();
        }
        public static string GeneratePasswordHash(this string password)
        {
            PasswordHasher passwordHasher = new PasswordHasher();

            return passwordHasher.HashPassword(password);
        }
        
        public static T GetValue<T>(this object obj, string key)
        {
            return (T)obj.GetType().GetProperty(key).GetValue(obj);
        }

        public static string RandomString(string key = "Kiosk", int length = 0)
        {
            Random rnd = new Random();
            int yourRandomStringLength = rnd.Next(6, 16);
            return key + Guid.NewGuid().ToString("N").Substring(0, (length == 0 ? yourRandomStringLength: length));
        }

      

        public static bool IsEmail(this string emailaddress)
        {
            return new EmailAddressAttribute().IsValid(emailaddress);
        }

    }
}