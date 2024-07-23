#region References
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;
#endregion

namespace FWBS.Configuration
{
    #region XmlBased class
    public abstract class XmlBased : Configuration
	{
		#region Fields
		private Encoding encoding = Encoding.UTF8;
		internal XmlBuffer buffer;
		#endregion

		#region Constructors
		protected XmlBased()
		{
		}

		protected XmlBased(string fileName) :
			base(fileName)
		{
		}

		protected XmlBased(XmlBased config) :
			base(config)
		{
			this.encoding = config.Encoding;
		}
		#endregion

		#region GetXmlDocument
		protected XmlDocument GetXmlDocument()
		{
			if (this.buffer != null)
			{
				return this.buffer.XmlDocument;
			}

			this.VerifyName();
			if (!File.Exists(Name))
			{
				return null;
			}

			XmlDocument doc = new XmlDocument();
			doc.Load(Name);
			return doc;
		}
		#endregion

		#region Save
		protected void Save(XmlDocument doc)
		{
			if (this.buffer != null)
			{
				this.buffer.needsFlushing = true;
			}
			else
			{
				doc.Save(Name);
			}
		}
		#endregion

		#region Buffer
		public XmlBuffer Buffer(bool lockFile)
		{
			if (this.buffer == null)
			{
				this.buffer = new XmlBuffer(this, lockFile);
			}

			return this.buffer; 
		}

		public XmlBuffer Buffer()
		{
			return this.Buffer(true);
		}
		#endregion

		#region Properties
		public bool Buffering
		{
			get 
			{
				return this.buffer != null;
			}
		}

		public Encoding Encoding
		{
			get 
			{
				return this.encoding; 
			}
			set 
			{
				this.VerifyNotReadOnly();
				if (this.encoding == value)
				{
					return;
				}

				if (!this.RaiseChangeEvent(true, ConfigurationChangeType.Other, null, "Encoding", value))
				{
					return;
				}

				this.encoding = value;
				this.RaiseChangeEvent(false, ConfigurationChangeType.Other, null, "Encoding", value);
			}
		}
		#endregion
	}
	#endregion

	#region XmlBuffer
	public class XmlBuffer : IDisposable
	{
		#region Fields
		private XmlBased configuration;
		private XmlDocument xmlDoc;
		private FileStream file;
		internal bool needsFlushing;
		#endregion

		#region Constructor
		internal XmlBuffer(XmlBased configuration, bool lockFile)
		{
			this.configuration = configuration;

			if (lockFile)
			{
				this.configuration.VerifyName();
				if (File.Exists(this.configuration.Name))
				{
					this.file = new FileStream(this.configuration.Name, FileMode.Open, this.configuration.ReadOnly ? FileAccess.Read : FileAccess.ReadWrite, FileShare.Read);
				}
			}
		}
		#endregion

		#region Load
		internal void Load(XmlTextWriter writer)
		{
			writer.Flush();
			writer.BaseStream.Position = 0;
			this.xmlDoc.Load(writer.BaseStream);

			this.needsFlushing = true;
		}
		#endregion

		#region Properties
		internal XmlDocument XmlDocument
		{
			get
			{
				if (this.xmlDoc == null)
				{
					this.xmlDoc = new XmlDocument();

					if (this.file != null)
					{
						this.file.Position = 0;
						this.xmlDoc.Load(this.file);
					}
					else
					{
						this.configuration.VerifyName();
						if (File.Exists(this.configuration.Name))
						{
							this.xmlDoc.Load(this.configuration.Name);
						}
					}
				}
				return this.xmlDoc;
			}
		}

		internal bool IsEmpty
		{
			get
			{
				return XmlDocument.InnerXml == String.Empty;
			}
		}

		public bool NeedsFlushing
		{
			get
			{
				return this.needsFlushing;
			}
		}

		public bool Locked
		{
			get
			{
				return this.file != null;
			}
		}
		#endregion

		#region Flush
		public void Flush()
		{
			if (this.configuration == null)
			{
				throw new InvalidOperationException("Cannot flush an XmlBuffer object that has been closed.");
			}

			if (this.xmlDoc == null)
			{
				return;
			}

			if (this.file == null)
			{
				this.xmlDoc.Save(this.configuration.Name);
			}
			else
			{
				this.file.SetLength(0);
				this.xmlDoc.Save(this.file);
			}

			this.needsFlushing = false;
		}
		#endregion

		#region Reset
		public void Reset()
		{
			if (this.configuration == null)
			{
				throw new InvalidOperationException("Cannot reset an XmlBuffer object that has been closed.");
			}

			this.xmlDoc = null;
			this.needsFlushing = false;
		}
		#endregion

		#region Close
		public void Close()
		{
			if (this.configuration == null)
			{
				return;
			}

			if (this.needsFlushing)
			{
				this.Flush();
			}

			this.xmlDoc = null;
		
			if (this.file != null)
			{
				this.file.Close();
				this.file = null;
			}

			if (this.configuration != null)
			{
				this.configuration.buffer = null;
			}
			this.configuration = null;
		}
		#endregion

		#region Dispose
		public void Dispose()
		{
			this.Close();
		}
		#endregion
	}
	#endregion

	#region XmlFile class
	public sealed class XmlFile : XmlBased
	{
		//   <?xml version="1.0" encoding="utf-8">;
		//   <configuration>
		//     <section name="A Section">
		//       <entry name="An Entry">Some Value</entry>
		//       <entry name="Another Entry">Another Value</entry>
		//     </section>
		//     <section name="Another Section">
		//       <entry name="This is cool">True</entry>
		//     </section>
		//   </configuration>

		#region Fields
		private string rootName = "configuration";
		#endregion

		#region Constructors
		public XmlFile()
		{
		}

		public XmlFile(string fileName) :
			base(fileName)
		{
		}

		public XmlFile(XmlFile xml) :
			base(xml)
		{
			this.rootName = xml.rootName;
		}
		#endregion

		#region Properties
		public override string DefaultName
		{
			get
			{
				return DefaultNameWithoutExtension + ".xml";
			}
		}

		public string RootName
		{
			get
			{
				return this.rootName;
			}
			set
			{
				this.VerifyNotReadOnly();
				if (this.rootName == value.Trim())
				{
					return;
				}

				if (!this.RaiseChangeEvent(true, ConfigurationChangeType.Other, null, "RootName", value))
				{
					return;
				}

				this.rootName = value.Trim();
				this.RaiseChangeEvent(false, ConfigurationChangeType.Other, null, "RootName", value);
			}
		}
		#endregion

		#region Clone
		public override object Clone()
		{
			return new XmlFile(this);
		}
		#endregion

		#region GetSectionsPath
		private string GetSectionsPath(string section)
		{
			return "section[@name=\"" + section + "\"]";
		}
		#endregion

		#region GetEntryPath
		private string GetEntryPath(string entry)
		{
			return "entry[@name=\"" + entry + "\"]";
		}
		#endregion

		#region SetValue
		public override void SetValue(string section, string entry, object value)
		{
			// If the value is null, remove the entry
			if (value == null)
			{
				this.RemoveEntry(section, entry);
				return;
			}

			this.VerifyNotReadOnly();
			this.VerifyName();
			this.VerifyAndAdjustSection(ref section);
			this.VerifyAndAdjustEntry(ref entry);

			if (!this.RaiseChangeEvent(true, ConfigurationChangeType.SetValue, section, entry, value))
			{
				return;
			}

			string valueString = value.ToString();

			// If the file does not exist, use the writer to quickly create it
			if ((this.buffer == null || this.buffer.IsEmpty) && !File.Exists(Name))
			{	
				XmlTextWriter writer = null;
				
				// If there's a buffer, write to it without creating the file
				if (this.buffer == null)
				{
					writer = new XmlTextWriter(Name, Encoding);
				}
				else
				{
					writer = new XmlTextWriter(new MemoryStream(), Encoding);
				}

				writer.Formatting = Formatting.Indented;
	            
	            writer.WriteStartDocument();
				
	            writer.WriteStartElement(this.rootName);			
					writer.WriteStartElement("section");
					writer.WriteAttributeString("name", null, section);				
						writer.WriteStartElement("entry");
						writer.WriteAttributeString("name", null, entry);				
	            			writer.WriteString(valueString);
	            		writer.WriteEndElement();
	            	writer.WriteEndElement();
	            writer.WriteEndElement();

				if (this.buffer != null)
				{
					this.buffer.Load(writer);
				}
				writer.Close();

				this.RaiseChangeEvent(false, ConfigurationChangeType.SetValue, section, entry, value);
				return;
			}
			
			// The file exists, edit it
			XmlDocument doc = this.GetXmlDocument();
			XmlElement root = doc.DocumentElement;
			
			// Get the section element and add it if it's not there
			XmlNode sectionNode = root.SelectSingleNode(this.GetSectionsPath(section));
			if (sectionNode == null)
			{
				XmlElement element = doc.CreateElement("section");
				XmlAttribute attribute = doc.CreateAttribute("name");
				attribute.Value = section;
				element.Attributes.Append(attribute);			
				sectionNode = root.AppendChild(element);			
			}

			// Get the entry element and add it if it's not there
			XmlNode entryNode = sectionNode.SelectSingleNode(this.GetEntryPath(entry));
			if (entryNode == null)
			{
				XmlElement element = doc.CreateElement("entry");
				XmlAttribute attribute = doc.CreateAttribute("name");
				attribute.Value = entry;
				element.Attributes.Append(attribute);			
				entryNode = sectionNode.AppendChild(element);			
			}

			// Add the value and save the file
			entryNode.InnerText = valueString;
			this.Save(doc);
			this.RaiseChangeEvent(false, ConfigurationChangeType.SetValue, section, entry, value);
		}
		#endregion

		#region GetValue
		public override object GetValue(string section, string entry)
		{
			this.VerifyAndAdjustSection(ref section);
			this.VerifyAndAdjustEntry(ref entry);
			
			try
			{ 	
				XmlDocument doc = GetXmlDocument();
				XmlElement root = doc.DocumentElement;

				XmlNode entryNode = root.SelectSingleNode(this.GetSectionsPath(section) + "/" + this.GetEntryPath(entry));
				return entryNode.InnerText;
			}
			catch
			{	
				return null;
			}			
		}
		#endregion

		#region RemoveEntry
		public override void RemoveEntry(string section, string entry)
		{
			this.VerifyNotReadOnly();
			this.VerifyAndAdjustSection(ref section);
			this.VerifyAndAdjustEntry(ref entry);

			// Verify the document exists
			XmlDocument doc = GetXmlDocument();
			if (doc == null)
			{
				return;
			}

			// Get the entry's node, if it exists
			XmlElement root = doc.DocumentElement;
			XmlNode entryNode = root.SelectSingleNode(this.GetSectionsPath(section) + "/" + this.GetEntryPath(entry));
			if (entryNode == null)
			{
				return;
			}

			if (!this.RaiseChangeEvent(true, ConfigurationChangeType.RemoveEntry, section, entry, null))
				return;
			
			entryNode.ParentNode.RemoveChild(entryNode);
			this.Save(doc);
			this.RaiseChangeEvent(false, ConfigurationChangeType.RemoveEntry, section, entry, null);
		}
		#endregion

		#region RemoveSection
		public override void RemoveSection(string section)
		{
			this.VerifyNotReadOnly();
			this.VerifyAndAdjustSection(ref section);

			// Verify the document exists
			XmlDocument doc = GetXmlDocument();
			if (doc == null)
			{
				return;
			}
			
			// Get the root node, if it exists
			XmlElement root = doc.DocumentElement;
			if (root == null)
			{
				return;
			}

			// Get the section's node, if it exists
			XmlNode sectionNode = root.SelectSingleNode(this.GetSectionsPath(section));
			if (sectionNode == null)
			{
				return;
			}

			if (!this.RaiseChangeEvent(true, ConfigurationChangeType.RemoveSection, section, null, null))
			{
				return;
			}

			root.RemoveChild(sectionNode);
			this.Save(doc);
			this.RaiseChangeEvent(false, ConfigurationChangeType.RemoveSection, section, null, null);
		}
		#endregion

		#region Remove
		public override void Remove()
		{
			this.VerifyNotReadOnly();
			this.VerifyName();

			HashSet<string> sections = this.GetSectionNames();
			foreach (string section in sections)
			{
				this.RemoveSection(section);
			}
		}
		#endregion

		#region GetEntryNames
		public override HashSet<string> GetEntryNames(string section)
		{
			// Verify the section exists
			if (!this.HasSection(section))
			{
				return null;
			}

			this.VerifyAndAdjustSection(ref section);

			XmlDocument doc = this.GetXmlDocument();
			XmlElement root = doc.DocumentElement;
			
			// Get the entry nodes
			XmlNodeList entryNodes = root.SelectNodes(this.GetSectionsPath(section) + "/entry[@name]");
			if (entryNodes == null)
			{
				return null;
			}

			// Add all entry names to the string array			
			HashSet<string> entries = new HashSet<string>();

			foreach (XmlNode node in entryNodes)
			{
				entries.Add(node.Attributes["name"].Value);
			}
			
			return entries;
		}
		#endregion

		#region GetSectionNames
		public override HashSet<string> GetSectionNames()
		{
			// Verify the document exists
			XmlDocument doc = this.GetXmlDocument();
			if (doc == null)
			{
				return null;
			}

			// Get the root node, if it exists
			XmlElement root = doc.DocumentElement;
			if (root == null)
			{
				return null;
			}

			// Get the section nodes
			XmlNodeList sectionNodes = root.SelectNodes("section[@name]");
			if (sectionNodes == null)
			{
				return null;
			}

			// Add all section names to the string array			
			HashSet<string> sections = new HashSet<string>();

			foreach (XmlNode node in sectionNodes)
			{
				sections.Add(node.Attributes["name"].Value);
			}
			
			return sections;
		}
		#endregion
	}
	#endregion
}
