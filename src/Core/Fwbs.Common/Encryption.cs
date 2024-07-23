using System;

namespace FWBS.Common
{
	/// <summary>
	/// Encryption routines including legacy ones used in older applications.
	/// </summary>
    [Obsolete("Please use the FWBS.OMS.Security.Cryptography.Encrypt class instead.")]
	sealed public class Encryption
	{
		private Encryption() {}
		

		static public string  Decrypt (string inString)
		{
            return Security.Cryptography.Encryption.Decrypt(inString);
		}
		

		static public string Encrypt (string inString)
		{
            return Security.Cryptography.Encryption.Encrypt(inString);
		}

		static public string NewDecrypt(string inString)
		{
            return Security.Cryptography.Encryption.NewDecrypt(inString);
		}

		static public string NewEncrypt(string inString)
		{
            return Security.Cryptography.Encryption.NewEncrypt(inString);
		}

		static public string KeyDecrypt(string inString)
		{
            return Security.Cryptography.Encryption.KeyDecrypt(inString);
		}

		static public string KeyEncrypt(string inString, int length)
		{
            return Security.Cryptography.Encryption.KeyEncrypt(inString, length);
		}

		static public string NewKeyDecrypt(string inString)
		{
            return Security.Cryptography.Encryption.NewKeyDecrypt(inString);
		}

		static public string NewKeyEncrypt(string inString, int length)
		{
            return Security.Cryptography.Encryption.NewKeyEncrypt(inString, length);
		}
	}
}
