using JetBrains.Annotations;
using System;
using System.IO;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;


namespace PtxBuddy
{
    public class PtxbuddySecurity
    {
        private static readonly byte[] Key = Encoding.UTF8.GetBytes("PtxbuddyIsEncryptionKeyZyxwvutsr");
        private static readonly byte[] IV = Encoding.UTF8.GetBytes("PtxbuddyIsIVDcab");
        public int exists=0;
       public  static string EmbedRequestCount(string guid, int requestCount)
        {
            string valueToAttach = requestCount.ToString();
            int lastDashIndex = guid.LastIndexOf('-');

            if (lastDashIndex != -1)
            {
                string beforeLastSegment = guid.Substring(0, lastDashIndex);
                string lastSegment = guid.Substring(lastDashIndex + 1);

                int secondLastDashIndex = beforeLastSegment.LastIndexOf('-');
                if (secondLastDashIndex != -1)
                {
                    string segmentToModify = beforeLastSegment.Substring(secondLastDashIndex + 1);

                    // Remove any existing trailing number and append new request count
                    string cleanedSegment = Regex.Replace(segmentToModify, @"\d+$", ""); 
                    string newSegment = cleanedSegment + valueToAttach;

                    string modifiedString = beforeLastSegment.Substring(0, secondLastDashIndex + 1) +
                                            newSegment + "-" +
                                            lastSegment;

                    return modifiedString;
                }
            }
            return guid; 
        }

        public static int ExtractRequestCount(string modifiedGuid)
        {

            int lastDashIndex = modifiedGuid.LastIndexOf('-');
            if (lastDashIndex != -1)
            {
                string beforeLastSegment = modifiedGuid.Substring(0, lastDashIndex);

                // Find the second-last dash
                int secondLastDashIndex = beforeLastSegment.LastIndexOf('-');
                if (secondLastDashIndex != -1)
                {
                    string originalSegment = beforeLastSegment.Substring(secondLastDashIndex + 1);

                    // dettach only the last digit
                    Match match = Regex.Match(originalSegment, @"(\d)$");

                    if (match.Success)
                    {
                        return int.Parse(match.Value);  
                    }
                }
            }
            return -1;

        }
  
        public static string Encrypt(string plainText)
        {
            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = Key;
                aesAlg.IV = IV;

                ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

                using (MemoryStream msEncrypt = new MemoryStream())
                {
                    using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                        {
                            swEncrypt.Write(plainText);
                        }
                        return Convert.ToBase64String(msEncrypt.ToArray());
                    }
                }
            }
        }

    
        public static string Decrypt(string cipherText)
        {
            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = Key;
                aesAlg.IV = IV;

                ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

                using (MemoryStream msDecrypt = new MemoryStream(Convert.FromBase64String(cipherText)))
                {
                    using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                        {
                            return srDecrypt.ReadToEnd();
                        }
                    }
                }
            }
        }

        public  void IsCounterExists( string newGuid,int IsCounter)
        {
            try
            {

                string appDataPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "MyApp");
                string filePath = Path.Combine(appDataPath, "encryptedData.txt");
                if (File.Exists(filePath))
                {
                    // Decrypt the data
                    appDataPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "MyApp");
                    filePath = Path.Combine(appDataPath, "encryptedData.txt");
                    string decryptedData = Decrypt(File.ReadAllText(filePath));
                    Console.WriteLine("Decrypted Data: " + decryptedData);

                    // Extract the request count from the decrypted GUID
                    int extractedRequestCount = ExtractRequestCount(decryptedData);
                    Console.WriteLine("Extracted Request Count: " + extractedRequestCount);
                   
                }
                else
                {

                   
                    Directory.CreateDirectory(appDataPath);

                    ////Console.WriteLine("File exists: " + filePath);
                    //string MYGUID = newGuid.ToString();
                    //// Original GUID
                    //string originalGuid = MYGUID;
                    int requestCount = IsCounter; // Request count to embed

                    // attach the request count in the GUID
                    string modifiedGuid = EmbedRequestCount(newGuid, requestCount);
                    Console.WriteLine("Modified GUID: " + modifiedGuid);

                   
                    string encryptedData = Encrypt(modifiedGuid);

              
                    appDataPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "MyApp");
                    Directory.CreateDirectory(appDataPath);

                    filePath = Path.Combine(appDataPath, "encryptedData.txt");
                    File.WriteAllText(filePath, encryptedData);
                }
            }
            catch
            {


            }
        }
        public int IsLimitterExists(string newGuid,int IsCounter)
        {
            try
            {
                int existingCounter = 0;
                string appDirectory = Application.StartupPath;  // Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
                string filePath = Path.Combine(appDirectory, "Limit.txt");
                if (File.Exists(filePath))
                {
                    exists = 1;
                    return 1;
                }
                
                string appDataPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "MyApp");
                string filePathAppData = Path.Combine(appDataPath, "encryptedData.txt");
                if (File.Exists(filePathAppData))
                {

                    existingCounter = ExtractRequestCount(newGuid);
                    if (existingCounter == 3)
                    {
                        MessageBox.Show($"Your free limit has been reached");
                        return 0;
                    }
                    else
                    {
                        string UpdatedGuid = EmbedRequestCount(newGuid, IsCounter);
                        string encryptedData = Encrypt(UpdatedGuid);
                        filePath = Path.Combine(appDataPath, "encryptedData.txt");
                        File.WriteAllText(filePath, encryptedData);
                    }
                }
                else
                {
                    IsCounterExists(newGuid, IsCounter);

                }
            }
            catch
            {


            }
            return 0;   
        }
    }
}

