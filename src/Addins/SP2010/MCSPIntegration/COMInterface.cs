using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using System.Xml;

namespace FWBS.Sharepoint
{
    [ComVisible(true)]
	[ProgId("FWBS.Sharepoint.VBA.Helper")]
	[Guid("F17841E6-E78E-4591-9198-37E647EA4928")]
	[ClassInterface(ClassInterfaceType.AutoDual)]

	public class COMInterface
	{
		#region Constants
		//
		// Properties and property separators
		//
		// Format is	<property name>\=<value>[\;<next property>]
		private const string PROPERTIES_SEPARATOR = @"\;";	// separates properties
		private const string PROPERTY_SEPARATOR = @"\=";	// separates a property and its value

		// Schema generation namespace
		private const string MA_URI = "http://schemas.microsoft.com/office/2006/metadata/properties/metaAttributes";

		private const string CORE_PROPERTIES_SCHEMA = 
			"<xsd:schema targetNamespace=\"http://schemas.openxmlformats.org/package/2006/metadata/core-properties\" elementFormDefault=\"qualified\" attributeFormDefault=\"unqualified\" blockDefault=\"#all\" xmlns=\"http://schemas.openxmlformats.org/package/2006/metadata/core-properties\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:dc=\"http://purl.org/dc/elements/1.1/\" xmlns:dcterms=\"http://purl.org/dc/terms/\" xmlns:odoc=\"http://schemas.microsoft.com/internal/obd\">" +
				"<xsd:import namespace=\"http://purl.org/dc/elements/1.1/\" schemaLocation=\"http://dublincore.org/schemas/xmls/qdc/2003/04/02/dc.xsd\"/>" +
				"<xsd:import namespace=\"http://purl.org/dc/terms/\" schemaLocation=\"http://dublincore.org/schemas/xmls/qdc/2003/04/02/dcterms.xsd\"/>" +
				"<xsd:element name=\"coreProperties\" type=\"CT_coreProperties\"/>" +
				"<xsd:complexType name=\"CT_coreProperties\">" +
					"<xsd:all>" +
						"<xsd:element ref=\"dc:creator\" minOccurs=\"0\" maxOccurs=\"1\"/>" +
						"<xsd:element ref=\"dcterms:created\" minOccurs=\"0\" maxOccurs=\"1\"/>" +
						"<xsd:element ref=\"dc:identifier\" minOccurs=\"0\" maxOccurs=\"1\"/>" +
						"<xsd:element name=\"contentType\" minOccurs=\"0\" maxOccurs=\"1\" type=\"xsd:string\" ma:index=\"0\" ma:displayName=\"Content Type\"/>" +
						"<xsd:element ref=\"dc:title\" minOccurs=\"0\" maxOccurs=\"1\" ma:index=\"4\" ma:displayName=\"Title\"/>" +
						"<xsd:element ref=\"dc:subject\" minOccurs=\"0\" maxOccurs=\"1\"/>" +
						"<xsd:element ref=\"dc:description\" minOccurs=\"0\" maxOccurs=\"1\"/>" +
						"<xsd:element name=\"keywords\" minOccurs=\"0\" maxOccurs=\"1\" type=\"xsd:string\"/>" +
						"<xsd:element ref=\"dc:language\" minOccurs=\"0\" maxOccurs=\"1\"/>" +
						"<xsd:element name=\"category\" minOccurs=\"0\" maxOccurs=\"1\" type=\"xsd:string\"/>" +
						"<xsd:element name=\"version\" minOccurs=\"0\" maxOccurs=\"1\" type=\"xsd:string\"/>" +
						"<xsd:element name=\"revision\" minOccurs=\"0\" maxOccurs=\"1\" type=\"xsd:string\">" +
							"<xsd:annotation>" +
								"<xsd:documentation>" +
									"This value indicates the number of saves or revisions. The application is responsible for updating this value after each revision." +
								"</xsd:documentation>" +
							"</xsd:annotation>" +
						"</xsd:element>" +
						"<xsd:element name=\"lastModifiedBy\" minOccurs=\"0\" maxOccurs=\"1\" type=\"xsd:string\"/>" +
						"<xsd:element ref=\"dcterms:modified\" minOccurs=\"0\" maxOccurs=\"1\"/>" +
						"<xsd:element name=\"contentStatus\" minOccurs=\"0\" maxOccurs=\"1\" type=\"xsd:string\"/>" +
					"</xsd:all>" +
				"</xsd:complexType>" +
			"</xsd:schema>";
		#endregion

		#region Fields
		#endregion

		#region Constructor
		public COMInterface()
		{
			// Create empty dictionary
			this.PropertyNameValuePairs = new Dictionary<string, string>();
			this.RetrievedPropertyNameValuePairs = new Dictionary<string, string>();
		}
		#endregion

		#region Properties
		#region PropertyNameValuePairs
		protected Dictionary<string, string> PropertyNameValuePairs
		{
			get;
			set;
		}

		protected Dictionary<string, string> RetrievedPropertyNameValuePairs
		{
			get;
			set;
		}
		#endregion
		#endregion

		#region Compute MD5 hash
		protected string ComputeMD5Hash(string input)
		{
			// Create a new instance of the MD5CryptoServiceProvider object.
			MD5 md5Hasher = MD5.Create();

			// Convert the input string to a byte array and compute the hash.
			byte[] data = md5Hasher.ComputeHash(Encoding.Default.GetBytes(input));

			// Create a new Stringbuilder to collect the bytes and create a string.
			StringBuilder sBuilder = new StringBuilder();

			// Loop through each byte of the hashed data and format each one as a hexadecimal string.
			for (int i = 0; i < data.Length; i++)
			{
				sBuilder.Append(data[i].ToString("x2"));
			}

			// Return the hexadecimal string.
			return sBuilder.ToString();
		}
		#endregion

		#region PUBLIC - COMInterface Implementation
		#region AddPropertyValuePair
		/// <summary>
		/// This routine adds a property with its value to the property list.
		/// If the property does not exists then it is added, otherwise its value is changed.
		/// </summary>
		/// <param name="name">Property name</param>
		/// <param name="value">Property value</param>
		/// <returns></returns>
		[DispId(1)]
		public bool AddPropertyValuePair(string name, string value)
		{
			bool retValue = false;

			try
			{
				// if exists then overwrite, otherwise add...
				this.PropertyNameValuePairs[name] = value;
				retValue = true;
			}
			catch (Exception)
			{
				// TODO: handle error
			}

			return retValue;
		}
		#endregion

		#region InsertProperties
		/// <summary>
		/// This routine will search the xml for the 'documentManagement' properties which contains the 
		/// values for SharePoint column values. If a property is found then its value is set and a
		/// new xml string is returned. If no changes are made or an error occurs then an empty string is returned.
		/// </summary>
		/// <param name="customXmlPart"></param>
		/// <returns>If no changes occurred an empty string, otherwise a new XML string with the property values</returns>
		[DispId(2)]
		public string InsertProperties(string customXmlPart)
		{
			bool retValue = false;

			try
			{
				XmlDocument xmlDoc = new XmlDocument();
				xmlDoc.LoadXml(customXmlPart);

				XmlNode node = xmlDoc.SelectSingleNode("//documentManagement");
				if (node != null)
				{
					for (int i = 0; i < node.ChildNodes.Count; i++)
					{
						XmlNode childNode = node.ChildNodes[i];
						string name = childNode.Name;
						if (this.PropertyNameValuePairs.ContainsKey(name))
						{
							// remove the nil attribute
							for (int j = 0; j < childNode.Attributes.Count; j++)
							{
								if (childNode.Attributes[j].Name == "xsi:nil")
								{
									childNode.Attributes.RemoveAt(j);
									break;
								}
							}
							// set value
							childNode.InnerXml = this.PropertyNameValuePairs[name];
							retValue = true;
						}
					}
					if (retValue)
					{
						return xmlDoc.InnerXml;
					}
				}
			}
			catch (Exception)
			{
				;
			}

			return string.Empty;
		}
		#endregion

		#region RetrieveProperties

		[DispId(3)]
		public bool RetrieveProperties(string customXmlPart)
		{
			bool foundValues = false;

			try
			{
				XmlDocument xmlDoc = new XmlDocument();
				xmlDoc.LoadXml(customXmlPart);

				XmlNode node = xmlDoc.SelectSingleNode("//documentManagement");
				if (node == null)
					return foundValues;


				for (int i = 0; i < node.ChildNodes.Count; i++)
				{
					XmlNode childNode = node.ChildNodes[i];

					RetrievedPropertyNameValuePairs.Add(childNode.Name, childNode.InnerXml);
					foundValues = true;
				}

			}
			catch (Exception)
			{
				;
			}

			return foundValues;
		}

		[DispId(4)]
		public string GetPropertyValue(string property)
		{
			if (RetrievedPropertyNameValuePairs.ContainsKey(property))
				return RetrievedPropertyNameValuePairs[property];

			return string.Empty;

		}

		#endregion
		#endregion
	}
}
