using System;

namespace FWBS.Common.Security.Cryptography
{
	/// <summary>
	/// Encryption routines including legacy ones used in older applications.
	/// </summary>
	sealed public class Encryption
	{
		private Encryption() {}
		

		static public string  Decrypt (string inString)
		{	
			string ret = "";
			try
			{
				for (int ctr = inString.Length; ctr > 0; ctr--)
				{
					char c = inString[ctr - 1];
					int a = c;
					a += - (13 - ctr + inString.Length + 1);
					c = (char)a;
					ret += c;				
				}
			}
			catch{}
			return ret;
		}
		

		static public string Encrypt (string inString)
		{
			string ret = "";
			try
			{
				for (int ctr = inString.Length; ctr > 0; ctr--)
				{
					char c = inString[ctr - 1];
					int a = c;
					a += (13 + ctr);
					if (a > 255) a = 255;
					c = (char)a;
					ret += c;
				}
			}
			catch{}
			return ret;
		}

		static public string NewDecrypt(string inString)
		{
			string ret = "";
			try
			{
				for (int ctr = inString.Length; ctr > 0; ctr--)
				{
					char c = inString[ctr - 1];
					int a = c;
					a += (13 - ctr + inString.Length + 1);
					c = (char)a;
					ret += c;				
				}
			}
			catch{}
			return ret;
		}

		static public string NewEncrypt(string inString)
		{
			string ret = "";
			try
			{
				for (int ctr = inString.Length; ctr > 0; ctr--)
				{
					char c = inString[ctr - 1];
					int a = c;
					a += - (13 + ctr);
					if (a > 255) a = 255;
					c = (char)a;
					ret += c;
				}
			}
			catch{}
			return ret;
		}

		static public string KeyDecrypt(string inString)
		{
            if (String.IsNullOrEmpty(inString))
                return String.Empty;
            
            string ret = "";
			try
			{
				int a = inString[0];
				int key = inString[inString.Length - 1];
				inString = inString.Substring(1, key);
				key += a;
				for (int ctr = inString.Length - 1; ctr >= 0; ctr--)
				{
					int ach = inString[ctr];
					ach -= key;
					ret += (char)ach;
				}
			}
			catch{}
			return ret;
		}

		static public string KeyEncrypt(string inString, int length)
		{
			string ret = "";
			try
			{
				Random rnd = new Random(System.DateTime.Now.Millisecond);
				int r = (int)(80D * rnd.NextDouble() + 32D);
				int key = r + inString.Length;
				ret = ((char)r).ToString();
				for (int ctr = inString.Length - 1; ctr>=0; ctr--)
				{
					int ach = inString[ctr];
					ach += key;
					if (ach > 255) ach = 255;
					ret += (char)ach;
				}
				for (int ctr = ret.Length; ctr <= length - 2; ctr++)
				{
					r = (int)((255D-1D) * rnd.NextDouble() + 1D);
					ret += (char)r;
				}
				ret += (char) inString.Length;
			}
			catch{}
			return ret;
		}

		static public string NewKeyDecrypt(string inString)
		{
            if (String.IsNullOrEmpty(inString))
                return String.Empty;

            string ret = "";
			try
			{
				int a = inString[0];
				int key = inString[inString.Length - 1];
				inString = inString.Substring(1, key);
				key += a;
				for (int ctr = inString.Length - 1; ctr >= 0; ctr--)
				{
					int ach = inString[ctr];
					ach -= key;
					if (ach < 1) 
					{
						do 
							ach += 255;
						while (ach < 1);

					}
					ret += (char)ach;
				}	
			}
			catch{}
			return ret;
		}

		static public string NewKeyEncrypt(string inString, int length)
		{
			string ret = "";
			try
			{
				Random rnd = new Random(System.DateTime.Now.Millisecond);
				int r = (int)(150D * rnd.NextDouble() + 32D);
				int key = r + inString.Length;
				ret = ((char)r).ToString();
				for (int ctr = inString.Length - 1; ctr>=0; ctr--)
				{
					int ach = inString[ctr];
					ach += key;
					if (ach > 255)
					{
						do
							ach -= 255;
						while(ach > 255);
					}
					ret += (char)ach;
				}
				for (int ctr = ret.Length; ctr <= length - 2; ctr++)
				{
					r = (int)((255D-1D) * rnd.NextDouble() + 1D);
					ret += (char)r;
				}
				ret += (char) inString.Length;
			}
			catch{}
			return ret;
		}
	}
}
