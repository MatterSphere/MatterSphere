using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Text;

namespace Fwbs.Documents.Preview.Csv
{
    using Handlers;

    [System.Runtime.InteropServices.Guid(CsvPreviewHandlerFactory.ClassID)]
	internal partial class CSVPreviewHandlerControl : PreviewHandlerFromFile , IPreviewHandlerInfo
	{
		public CSVPreviewHandlerControl()
		{
			InitializeComponent();
		}

		public override void DoPreview()
		{
			DataTable dt = new DataTable();

			List<string[]> lines = new List<string[]>();
			int nooffields = 0;

			using (Stream stream = file.OpenRead())
			using (StreamReader reader = new StreamReader(stream, Encoding.ASCII))
			{
				
				string line;
				while ((line = reader.ReadLine()) != null)
				{
					if (line.Length <= 0)
						continue;

					string[] elements = line.Split(',');
					nooffields = Math.Max(nooffields, elements.Length);
					lines.Add(elements);
				}

			}

			for (int n = 0; n < nooffields; n++)
				dt.Columns.Add(string.Format("{0} {1}",columnName, n+1));

			foreach(string[] line in lines)
				dt.Rows.Add(line);

			this.dataGridView1.DataSource = dt;

		}

		#region IFWBSPreviewer Members

		private string columnName = "Column";
		public void SetCultureData(Dictionary<string, string> cultureData)
		{
			if (cultureData.ContainsKey(columnName))
				columnName = cultureData["Column"];
		}

		public void SetPreviewSupport(bool fullPreviewSupport)
		{
			//Not Required On This Previewer
		}

		public void SetAdditionalProperties(Dictionary<string, string> additionalProperties)
		{
			//Not Required On the Previewer
		}

		#endregion
	   
	}
}
