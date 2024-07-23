using System;
using System.Drawing;
using System.IO;

namespace FWBS.OMS
{
    /// <summary>
    /// Summary description for Images.
    /// </summary>
    public class Images : CommonObject
	{
		#region Fields
		/// <summary>
		/// The Image Object
		/// </summary>
		private System.Drawing.Image _image = null;
		/// <summary>
		/// The Path of the Image for the ToFile Method
		/// </summary>
		private string _path = "";
		#endregion
		
		#region Constructor
		/// <summary>
		/// Blank Constructor for Creating a Image Object
		/// </summary>
		public Images() : base()
		{
			
		}

		/// <summary>
		/// Sets the Extraction Path for the ToFile Method
		/// </summary>
		/// <param name="ExtractionPath"></param>
		public Images(string ExtractionPath) : this()
		{
			_path = ExtractionPath;
		}
		#endregion

		#region Methods
		/// <summary>
		/// Returns the Image from the Database
		/// </summary>
		/// <param name="ID">The ID of the Image in the Global Image Database</param>
		public void GetImage(Int64 ID)
		{
			base.Fetch(ID);
			_image = null;
		}

		/// <summary>
		/// Create a new Image Record
		/// </summary>
		public override void Create()
		{
			base.Create ();
			_image = null;
		}

		/// <summary>
		/// To File Method
		/// </summary>
		/// <returns></returns>
		public System.IO.FileInfo ToFile()
		{
			if (_path == "") _path = Global.GetCachePath() + @"\images\";
			_path += this.ID + ".bmp";
			System.IO.FileInfo file = new System.IO.FileInfo(_path);
			if (file.Directory.Exists == false)
				file.Directory.Create();
			_image.Save(file.FullName, System.Drawing.Imaging.ImageFormat.Bmp);
			return file;
		}
		#endregion

		#region Common Object Overrides
		protected override string DefaultForm
		{
			get
			{
				return null;
			}
		}

		public override string FieldPrimaryKey
		{
			get
			{
				return "ID";
			}
		}

		public override object Parent
		{
			get
			{
				return null;
			}
		}

		protected override string PrimaryTableName
		{
			get
			{
				return "dbImages";
			}
		}

		protected override string SelectStatement
		{
			get
			{
				return "SELECT * FROM dbImages";
			}
		}
		#endregion

		#region Properties
		/// <summary>
		/// The Image ID in the Global Picture Database
		/// </summary>
		public Int64 ID
		{
			get
			{
				try
				{
					return Convert.ToInt64(GetExtraInfo("ID"));
				}
				catch
				{
					return -1;
				}
			}
		}

		/// <summary>
		/// Get and Set Annotation of the Image
		/// </summary>
		public string Text
		{
			get
			{
				return Convert.ToString(GetExtraInfo("Text"));
			}
			set
			{
				SetExtraInfo("Text",value);
			}
		}

		/// <summary>
		/// Get and Set of The System.Drawing.Image extracted and stored in the Database
		/// </summary>
		public System.Drawing.Image Image
		{
			get
			{
				try
				{
					//then passed to PictureBox.
					Byte[] byteBLOBData =  new Byte[0];
					byteBLOBData = (Byte[])GetExtraInfo("Image");
					MemoryStream stmBLOBData = new MemoryStream(byteBLOBData);
					_image = Image.FromStream(stmBLOBData);
				}
				catch (Exception ex)
				{
					Console.WriteLine(ex);
				}
				return _image;
			}
			set
			{
				_image = value;
				System.IO.MemoryStream stream = new System.IO.MemoryStream();
				_image.Save(stream,System.Drawing.Imaging.ImageFormat.Jpeg);
				try
				{
					// Create a buffer to hold the stream bytes
					byte[] buffer = new byte[stream.Length];
					// Read the bytes from this stream
					stream.Position = 0;
					// Now we can close the stream
					stream.Read(buffer, 0, Convert.ToInt32(stream.Length));
					SetExtraInfo("Image",buffer);
				}
				finally
				{
					if (stream != null) stream.Close();
				}
			}
		}
		#endregion

	}
}
