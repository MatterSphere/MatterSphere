using System;

namespace FWBS.Common
{
	/// <summary>
	/// Base class for file paths and UNC Paths to ensure that the right number of backslashes 
	/// etc.. are entered correctly.
	/// </summary>
	/// 
	public class Directory
	{
        private static readonly char[] _invalidChars;
        static Directory()
        {
            char[] invalidFileNameChars = System.IO.Path.GetInvalidFileNameChars();
            char[] invalidPathChars = System.IO.Path.GetInvalidPathChars();

            _invalidChars = new char[invalidFileNameChars.Length + invalidPathChars.Length];

            invalidFileNameChars.CopyTo(_invalidChars, 0);
            invalidPathChars.CopyTo(_invalidChars, invalidFileNameChars.Length);
        }

		/// <summary>
		/// Raw file path string.
		/// </summary>
		protected string pathData = "";

		/// <summary>
		/// Constructor, passing potential file path.
		/// </summary>
		/// <param name="path">Potential Directory string.</param>
		public Directory(string path)
		{
			pathData = path;
		}
		

		/// <summary>
		/// Implcitly casts this data type into a string for convenience when outputting the file path.
		/// It uses the ToString() method to validate the path and return it.
		/// </summary>
		/// <param name="dir">File path data type parameter.</param>
		/// <returns>File Path</returns>
		public static implicit operator string(Directory dir)
		{
			return dir.ToString();
		}


		/// <summary>
		/// Implicity converts a string into a Directory type, convenient for assigning strings.  This will
		/// force a new instance of a Directory type.
		/// </summary>
		/// <param name="dir">String value assigned.</param>
		/// <returns>File Path</returns>
		public static implicit operator Directory (string dir)
		{
			return new Directory(dir);
		}

		
		/// <summary>
		/// Concatenation operator overloaded so a Directory object can be combined with a string object.
		/// Two Directory objects could be concatenated through the implicit conversion of a string.
		/// </summary>
		/// <param name="a">String file path value.</param>
		/// <param name="b">Directory object value.</param>
		/// <returns>Concatenated File Path</returns>
		public static Directory operator + (string a, Directory b)
		{
			return new Directory(a + b.pathData);
		}

        /// <summary>
        /// Extracts illegal directory and file name characters out of the output replacing the illegal characters with nothing
        /// </summary>
        /// <param name="name">Directory or file name</param>        
        /// <returns>Legal File Name that has been trimmed.</returns>
        public static string ExtractInvalidChars(string name)
        {
            return ExtractInvalidChars(name, string.Empty);
        }

		/// <summary>
		/// Extracts illegal directory and file name characters out of the output.
        /// This function only deals with the components of a path or file name . 
        /// It should not be passed a directory path such as "c:\Documents\TextDocument.txt" as it will removed the colon and backslashes
		/// </summary>
        /// <param name="name">Directory or file name</param>
        /// <param name="replacement">A string to replace the invalid characters with.</param>
		/// <returns>Legal File Name that has been trimmed.</returns>
		public static string ExtractInvalidChars(string name, string replacement)
		{
            if (name == null)
                throw new ArgumentNullException("name");

            if (replacement==null)
                replacement = string.Empty;

            if (replacement.IndexOfAny(_invalidChars) > -1)
                throw new ArgumentException("replacement contains invalid characters");

            return string.Join(replacement, name.Split(_invalidChars, StringSplitOptions.None)).Trim();
		}

		/// <summary>
		/// Output of raw file path data as a leagal file path..
		/// </summary>
		/// <returns>Legal file path with a backslash suffix (being a directory).</returns>
		public override string ToString()
		{
			string ret = "";
			if (pathData.Length >= 3)
			{
				
				string prefix = pathData.Substring(0, 3);
				string suffix = ""; 
 
				if (prefix.EndsWith(@":\"))
					suffix = pathData.Substring(3);
				else if (prefix.StartsWith(@"\\"))
				{
					prefix = @"\\";
					suffix = pathData.Substring(2);
				}
				else
				{
					prefix = @"c:\";
					suffix = pathData;
				}

				while (suffix.IndexOf(@"\\") > -1 )
				{
					suffix = suffix.Replace(@"\\", @"\");
				}
				
				string [] directory = suffix.Split('\\');
				
				
				suffix = "";
				foreach (string iterator in directory)
				{
					suffix += Directory.ExtractInvalidChars(iterator);
					if (iterator != "") suffix += @"\";
				}
				

				ret = prefix + suffix;

				if (!ret.EndsWith(@"\")) 
					ret+=@"\";
				
			}

			return ret;
		}


	}


	/// <summary>
	/// Builds a valid file path string with the correct number of backslashes and legal characters.
	/// </summary>
	public class FilePath : Directory
	{
		/// <summary>
		/// Constructor, passing the path parameter to its base class.
		/// </summary>
		/// <param name="path">Raw file path string.</param>
		public FilePath (string path) : base(path)
		{
		}


		/// <summary>
		/// Implcitly casts this data type into a string for convenience when outputting the file path.
		/// It uses the ToString() method to validate the path and return it.
		/// </summary>
		/// <param name="fp">File path data type parameter.</param>
		/// <returns>File Path</returns>
		public static implicit operator string(FilePath fp)
		{
			return fp.ToString();
		}


		/// <summary>
		/// Implicity converts a string into a FilePath type, convenient for assigning strings.  This will
		/// force a new instance of a Directory type.
		/// </summary>
		/// <param name="fp">String value assigned.</param>
		/// <returns>File Path</returns>
		public static implicit operator FilePath (string fp)
		{
			return new FilePath(fp);
		}

		
		/// <summary>
		/// Concatenation operator overloaded so a FilePath object can be combined with a string object.
		/// Two Directory objects could be concatenated through the implicit conversion of a string.
		/// </summary>
		/// <param name="a">String file path value.</param>
		/// <param name="b">FilePath object value.</param>
		/// <returns>Concatenated File Path</returns>
		public static FilePath operator + (string a, FilePath b)
		{
			return new FilePath(a + b.pathData);
		}



		/// <summary>
		/// Overriden ToString() from base class Directory.  This will return the file path without an ending slash.
		/// </summary>
		/// <returns>File path without an ending slash.</returns>
		public override string ToString()
		{
			string ret = base.ToString();
			if (ret.EndsWith(@"\"))
				ret = ret.Substring(0, ret.Length - 1);

			return ret;
		}
	}
}
