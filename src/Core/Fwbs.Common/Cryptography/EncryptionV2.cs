using System;
using System.IO;
using System.Security.Cryptography;

public class EncryptionV2
{
    public static byte[] Encrypt(byte[] clearData, byte[] Key, byte[] IV)
    {
        byte[] encryptedData;
        using (Rijndael alg = Rijndael.Create())
        {
            alg.Key = Key;
            alg.IV = IV;
            MemoryStream ms = new MemoryStream();
            using (CryptoStream cs = new CryptoStream(ms, alg.CreateEncryptor(), CryptoStreamMode.Write))
            {
                cs.Write(clearData, 0, clearData.Length);
                cs.FlushFinalBlock();
                encryptedData = ms.ToArray();
            }
        }
        return encryptedData;
    }

    public static string Encrypt(string clearText, string Password)
    {
        byte[] clearBytes =
          System.Text.Encoding.Unicode.GetBytes(clearText);

        PasswordDeriveBytes pdb = new PasswordDeriveBytes(Password,
            new byte[] {0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 
            0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76});

        byte[] encryptedData = Encrypt(clearBytes,
                 pdb.GetBytes(32), pdb.GetBytes(16));

        return Convert.ToBase64String(encryptedData);

    }

    public static byte[] Encrypt(byte[] clearData, string Password)
    {
        PasswordDeriveBytes pdb = new PasswordDeriveBytes(Password,
            new byte[] {0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 
            0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76});
        return Encrypt(clearData, pdb.GetBytes(32), pdb.GetBytes(16));
    }

    public static void Encrypt(string fileIn,
                string fileOut, string Password) 
    { 

        FileStream fsIn = new FileStream(fileIn, 
            FileMode.Open, FileAccess.Read); 
        FileStream fsOut = new FileStream(fileOut, 
            FileMode.OpenOrCreate, FileAccess.Write); 

        PasswordDeriveBytes pdb = new PasswordDeriveBytes(Password, 
            new byte[] {0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 
            0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76}); 

        Rijndael alg = Rijndael.Create(); 
        alg.Key = pdb.GetBytes(32); 
        alg.IV = pdb.GetBytes(16); 

        CryptoStream cs = new CryptoStream(fsOut, 
            alg.CreateEncryptor(), CryptoStreamMode.Write); 

        int bufferLen = 4096; 
        byte[] buffer = new byte[bufferLen]; 
        int bytesRead; 

        do { 
            // read a chunk of data from the input file 
            bytesRead = fsIn.Read(buffer, 0, bufferLen); 

            // encrypt it 
            cs.Write(buffer, 0, bytesRead); 
        } while(bytesRead != 0); 

        // close everything 

        // this will also close the unrelying fsOut stream
        cs.Close(); 
        fsIn.Close();     
    }

    // Decrypt a byte array into a byte array using a key and an IV 
    public static byte[] Decrypt(byte[] cipherData, byte[] Key, byte[] IV)
    {
        byte[] decryptedData;
        using (Rijndael alg = Rijndael.Create())
        {
            alg.Key = Key;
            alg.IV = IV;
            MemoryStream ms = new MemoryStream();
            using (CryptoStream cs = new CryptoStream(ms, alg.CreateDecryptor(), CryptoStreamMode.Write))
            {
                cs.Write(cipherData, 0, cipherData.Length);
                cs.FlushFinalBlock();
                decryptedData = ms.ToArray();
            }
        }
        return decryptedData;
    }

    // Decrypt a string into a string using a password 
    //    Uses Decrypt(byte[], byte[], byte[]) 

    public static string Decrypt(string cipherText, string Password)
    {
        byte[] cipherBytes = Convert.FromBase64String(cipherText);

        PasswordDeriveBytes pdb = new PasswordDeriveBytes(Password,
            new byte[] {0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 
            0x64, 0x76, 0x65, 0x64, 0x65, 0x76});

        byte[] decryptedData = Decrypt(cipherBytes,
            pdb.GetBytes(32), pdb.GetBytes(16));

        return System.Text.Encoding.Unicode.GetString(decryptedData);
    }

    public static byte[] Decrypt(byte[] cipherData, string Password)
    {
        PasswordDeriveBytes pdb = new PasswordDeriveBytes(Password,
            new byte[] {0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 
            0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76});

        return Decrypt(cipherData, pdb.GetBytes(32), pdb.GetBytes(16));
    }

    public static void Decrypt(string fileIn,
                string fileOut, string Password) 
    { 
    
        FileStream fsIn = new FileStream(fileIn,
                    FileMode.Open, FileAccess.Read); 
        FileStream fsOut = new FileStream(fileOut,
                    FileMode.OpenOrCreate, FileAccess.Write); 
  
        PasswordDeriveBytes pdb = new PasswordDeriveBytes(Password, 
            new byte[] {0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 
            0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76}); 
        Rijndael alg = Rijndael.Create(); 

        alg.Key = pdb.GetBytes(32); 
        alg.IV = pdb.GetBytes(16); 

        CryptoStream cs = new CryptoStream(fsOut, 
            alg.CreateDecryptor(), CryptoStreamMode.Write); 
  
        int bufferLen = 4096;
        byte[] buffer = new byte[bufferLen]; 
        int bytesRead; 

        do { 
            bytesRead = fsIn.Read(buffer, 0, bufferLen); 
            cs.Write(buffer, 0, bytesRead); 

        } while(bytesRead != 0); 

        cs.Close(); // this will also close the unrelying fsOut stream 
        fsIn.Close();     
    }
}