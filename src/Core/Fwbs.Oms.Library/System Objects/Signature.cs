using System;

namespace FWBS.OMS
{
	/// <summary>
	/// Contains a signature that could be used by other objects within the system.
	/// </summary>
	public class Signature
	{
		private System.Drawing.Bitmap _bmap = null;
		private string _id;

		public Signature()
		{
			_id = "empty";
		}

		public Signature(System.Drawing.Bitmap signature)
		{
			_bmap = signature;
		}

		public Signature(System.IO.FileInfo filepath)
		{
			_bmap = (System.Drawing.Bitmap)System.Drawing.Bitmap.FromFile(filepath.FullName);
		}

		internal Signature(byte[] bytes, string id)
		{
			// Open a stream for the image and write the bytes into it
			System.IO.MemoryStream stream = new System.IO.MemoryStream(bytes, true);
			stream.Write(bytes, 0, bytes.Length);

			System.IO.MemoryStream strm = null;
			System.Runtime.Serialization.Formatters.Binary.BinaryFormatter bf = null;
			strm = new System.IO.MemoryStream(bytes, 0, bytes.Length);
			bf = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
			_bmap = (System.Drawing.Bitmap)bf.Deserialize(strm);
			strm.Close();

			_id = id;
		}

		public System.IO.FileInfo ToFile()
		{
			if (_bmap == null) return null;
			string path = Global.GetCachePath() + @"\signatures&logos\";
			path += _id + ".bmp";
			System.IO.FileInfo file = new System.IO.FileInfo(path);
			if (file.Directory.Exists == false)
				file.Directory.Create();
			_bmap.Save(file.FullName, System.Drawing.Imaging.ImageFormat.Bmp);
			return file;
		}

		public System.Drawing.Bitmap ToBitmap()
		{ 
			return _bmap;
		}

		internal byte[] ToByteArray()
		{
			System.IO.MemoryStream stream = new System.IO.MemoryStream();
			System.Runtime.Serialization.Formatters.Binary.BinaryFormatter formatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
			formatter.Serialize(stream, _bmap);
			return stream.ToArray();
		}

		/// <summary>
		/// Empty Signature Object
		/// </summary>
		static readonly public Signature Empty = new Signature();
		

		public override bool Equals(object obj)
		{
			if (obj is Signature && this._id == ((Signature)obj)._id)
				return true;
			else
				return false;
		}

		public override int GetHashCode()
		{
			return _id.GetHashCode();
		}


	}
}
