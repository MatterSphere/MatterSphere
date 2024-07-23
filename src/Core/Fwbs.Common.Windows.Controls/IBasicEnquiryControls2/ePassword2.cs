using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace FWBS.Common.UI.Windows
{
    public enum EncryptType {None,Encrypt,KeyEncrypt,NewEncrypt,NewKeyEncrypt}
	/// <summary>
	/// A text editor enquiry control which generally holds a caption and a text box 
	/// This particular control sets a '*' as the password character.
	/// </summary>
	public class ePassword2 : eTextBox2
	{
		#region Fields
		private string _encryptiontype = "";
		private object _value = "";
		private bool _first = false;
		
		#endregion
		
		#region Constructors

		/// <summary>
        /// Creates a base text editor control but sets the password character property.
        /// Initializes a new instance of the <see cref="ePassword2"/> class.
        /// </summary>
		public ePassword2() : base()
		{
			((TextBox)_ctrl).PasswordChar = '*';
			((TextBox)_ctrl).MaxLength = 50;
			((TextBox)_ctrl).TextChanged +=new EventHandler(ePassword2_TextChanged);
			((TextBox)_ctrl).KeyPress +=new KeyPressEventHandler(ePassword2_KeyPress);
			this.Changed +=new EventHandler(ePassword2_Changed);
		}
		#endregion

		#region Properties
        /// <summary>
        /// Gets or sets the type of the encryption.
        /// </summary>
        /// <value>The type of the encryption.</value>
		[TypeConverter(typeof(EncryptTypeEditor))]
		public string EncryptionType
		{
			get
			{
				return _encryptiontype;
			}
			set
			{
				_encryptiontype = value;
			}
		}

		public override object Value
		{
			get
			{
				return _value;
			}
			set
			{
				_value = value;
				((TextBox)_ctrl).TextChanged -=new EventHandler(ePassword2_TextChanged);
				if (_value != DBNull.Value && Convert.ToString(_value) != "")
						((TextBox)_ctrl).Text = "....................";
				_first = false;
				((TextBox)_ctrl).TextChanged +=new EventHandler(ePassword2_TextChanged);
				if (this.Parent != null)
				{
					IsDirty=true;
					OnChanged();
				}
			}
		}
        #endregion

        private void ePassword2_Changed(object sender, EventArgs e)
		{
			if (_first)
			{
				string myvalue = ((TextBox)_ctrl).Text;
				if (myvalue == "")
					_value = DBNull.Value;
				else
					switch (_encryptiontype.ToString().ToUpper())
					{
						case "ENCRYPT":
						{
							_value = FWBS.Common.Security.Cryptography.Encryption.Encrypt(myvalue);
							break;
						}
						case "KEYENCRYPT":
						{
                            _value = FWBS.Common.Security.Cryptography.Encryption.KeyEncrypt(myvalue, ((TextBox)_ctrl).MaxLength);
							break;
						}
						case "NEWENCRYPT":
						{
                            _value = FWBS.Common.Security.Cryptography.Encryption.NewEncrypt(myvalue);
							break;
						}
						case "NEWKEYENCRYPT":
						{
                            _value = FWBS.Common.Security.Cryptography.Encryption.NewKeyEncrypt(myvalue, ((TextBox)_ctrl).MaxLength);
							break;
						}
						default:
						{
							_value = myvalue;
							break;
						}
					}
			}
		}

		private void ePassword2_TextChanged(object sender, EventArgs e)
		{
			_first = true;
			OnActiveChanged();
		}

		private void ePassword2_KeyPress(object sender, KeyPressEventArgs e)
		{
			if (_first==false)
				((TextBox)_ctrl).Text = "";
		}

	}

	/// <summary>
	/// Summary description for EncryptTypeEditor.
	/// </summary>
	internal class EncryptTypeEditor : StringConverter
	{
		public override bool GetStandardValuesSupported(ITypeDescriptorContext context) 
		{
			return true;
		}
		public override StandardValuesCollection GetStandardValues(ITypeDescriptorContext context) 
		{
			return new StandardValuesCollection("Encrypt,KeyEncrypt,NewEncrypt,NewKeyEncrypt".Split(",".ToCharArray()));
		}
	}
}
