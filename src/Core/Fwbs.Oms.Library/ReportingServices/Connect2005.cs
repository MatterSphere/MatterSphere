using System;
using System.Data;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace FWBS.OMS.ReportingServices
{
    using v2005;

    /// <summary>
    /// Summary description for Class1.
    /// </summary>
    internal class SSRSConnect2005 : ISSRSWS
	{
		private const string fldOnWeekofmonth = "On Week of month";
		private const string fldOnCalendarDays = "On Calendar day(s)";
		private const string fldOnTheFollowingDays = "On the Following Days";
		private const string fldEveryWeekday = "Every Weekday";
		private const string fldRepeatAfterThis = "Repeat after this ";
		private const string fldNumberOfDays = "number of days";
		private const string fldRepeatAfterNoWeeks = "Repeat after this number of weeks";
		private const string fldJan = "Jan";
		private const string fldFeb = "Feb";
		private const string fldMar = "Mar";
		private const string fldApr = "Apr";
		private const string fldMay = "May";
		private const string fldJun = "Jun";
		private const string fldJul = "Jul";
		private const string fldAug = "Aug";
		private const string fldSep = "Sep";
		private const string fldOct = "Oct";
		private const string fldNov = "Nov";
		private const string fldDec = "Dec";
		private const string fldSun = "Sun";
		private const string fldMon = "Mon";
		private const string fldTue = "Tue";
		private const string fldWed = "Wed";
		private const string fldThu = "Thu";
		private const string fldFri = "Fri";
		private const string fldSat = "Sat";
		private const string fldWeekOfMonth = "[WeekOfMonth]";
		private const string fldDayOfMonth = "[DayOfMonth]";
		private const string fldWeek1st = "1st";
		private const string fldWeek2nd = "2nd";
		private const string fldWeek3rd = "3rd";
		private const string fldWeek4th = "4th";
		private const string fldWeekLast = "Last";
		private const string fldStartTime = "Start Time";

		private ReportingService rs = null;
		private bool _mangeshared = false;
		
		public SSRSConnect2005(bool ManageShared)
		{
			_mangeshared = ManageShared;
			rs = new ReportingService();
			rs.Credentials = System.Net.CredentialCache.DefaultCredentials;
			rs.Url = this.ReportServerASMX;
		}

		public bool CheckStatus()
		{
			try
			{
				string[] n = rs.GetSystemPermissions();
				return true;
			}
			catch
			{
				return false;
			}
		}

		public DataTable ReportParameters(string Report)
		{
			DataTable dt = new DataTable("Parameters");
			dt.Columns.Add("Name",typeof(string));
			dt.Columns.Add("DisplayName",typeof(string));
			dt.Columns.Add("Type",typeof(string));
			dt.Columns.Add("Row",typeof(int));
			dt.Columns.Add("Column",typeof(int));
			dt.Columns.Add("Required",typeof(bool));
			dt.Columns.Add("ReadOnly",typeof(bool));
			dt.Columns.Add("Value",typeof(object));
			dt.Columns.Add("SelectValues",typeof(DataTable));
			rs.Credentials = System.Net.CredentialCache.DefaultCredentials;
			string historyID = null;
			DataSourceCredentials[] credentials = null;
			ReportParameter[] rparam = rs.GetReportParameters(Report,historyID,true,new ParameterValue[0],credentials);
			// Loops through the Parameters for known invisible types
			int row = 0;
			foreach (ReportParameter rp in rparam)
			{
				if (rp.Prompt != "")
				{
                    AddRecurisveQuestion(dt, row, 0, "", rp.Prompt, SSRSConnect.typCaption, false, "", false);
					if (rp.Type == ParameterTypeEnum.Boolean)
                        AddRecurisveQuestion(dt, row, 1, rp.Name, "", SSRSConnect.typCheckbox, false, ValidValuesToString(rp.DefaultValues), false);
					else if (rp.Type == ParameterTypeEnum.DateTime)
                        AddRecurisveQuestion(dt, row, 1, rp.Name, "", SSRSConnect.typDateTime, false, "", false);
					else if (rp.Name.StartsWith("cds") || rp.Name.StartsWith("ccl") || rp.Name.StartsWith("cmbAutoUser") || rp.Name.StartsWith("cmbAutoFeeEarner")  || rp.Name.StartsWith("cmbAutoDepartment"))
                        AddRecurisveQuestion(dt, row, 1, rp.Name, "", SSRSConnect.typCombobox, !(rp.AllowBlank || rp.Nullable), ValidValuesToString(rp.ValidValues), ValidValuesToDataTable(rp.ValidValues), false);
					else if (rp.ValidValues != null && rp.ValidValues.Length > 0)
                        AddRecurisveQuestion(dt, row, 1, rp.Name, "", SSRSConnect.typCombobox, !(rp.AllowBlank || rp.Nullable), ValidValuesToString(rp.DefaultValues), ValidValuesToDataTable(rp.ValidValues), false);
					else
                        AddRecurisveQuestion(dt, row, 1, rp.Name, "", SSRSConnect.typTextbox, !(rp.AllowBlank || rp.Nullable), ValidValuesToString(rp.DefaultValues), false);
					row++;
				}
			}
			dt.Prefix = "A_2_" + row.ToString();
			return dt;
		}

		public string[] UploadReport(string ReportToUpload, string DestinationPath, out string reportName)
		{
			FileInfo info = new FileInfo(ReportToUpload);
			reportName = info.Name.Replace(info.Extension,"");

			FileStream stream = info.OpenRead();
			byte[] definition = new byte[stream.Length];
			stream.Read(definition, 0, Convert.ToInt32(stream.Length));
			stream.Close();
	        Warning[] wwarnings = rs.CreateReport(reportName, DestinationPath, true, definition, null);
			if (wwarnings != null)
			{
				string[] warnings = new string[wwarnings.Length];
				int i = 0;
				foreach (Warning w in wwarnings)
				{
					warnings[i] = w.Code + ", " + w.Message + ", " + w.ObjectName + ", " + w.ObjectType + ", " + w.Severity;
					i++;
				}
				return warnings;
			}
			else
				return null;
		}

		public void DeleteReport(string ReportPath)
		{
			rs.DeleteItem(ReportPath);
		}
		
		public DataTable Extensions(ExtensionsOptions options)
		{
			DataTable dt = new DataTable("Extensions");
			dt.Columns.Add("Name",typeof(string));
			dt.Columns.Add("DisplayName",typeof(string));
			dt.Columns.Add("Type",typeof(string));
			dt.Columns.Add("Row",typeof(int));
			dt.Columns.Add("Column",typeof(int));
			dt.Columns.Add("Required",typeof(bool));
			dt.Columns.Add("ReadOnly",typeof(bool));
			dt.Columns.Add("Value",typeof(object));
			dt.Columns.Add("SelectValues",typeof(DataTable));
			ExtensionParameter[] extensionParams = null;
			
			if (options == ExtensionsOptions.ReportServerEmail)
			{
				extensionParams = ProcessExtensionsByPermissions(rs.GetExtensionSettings("Report Server Email"));
				dt.Namespace = "Report Server Email";
			}
			else
			{
				extensionParams = rs.GetExtensionSettings("Report Server FileShare");
				dt.Namespace = "Report Server FileShare";
			}

				
			
			
			int row = 0;
			foreach (ExtensionParameter extensionParam in extensionParams)
			{
				if (extensionParam.DisplayName != null)
				{
                    AddRecurisveQuestion(dt, row, 0, "", extensionParam.DisplayName, SSRSConnect.typCaption, false, "", extensionParam.ReadOnly);
					if (ValidValuesToString(extensionParam.ValidValues) == "True,False")
                        AddRecurisveQuestion(dt, row, 1, extensionParam.Name, "", SSRSConnect.typCheckbox, extensionParam.Required, extensionParam.Value, extensionParam.ReadOnly);
					else if (extensionParam.ValidValues != null && extensionParam.ValidValues.Length > 0)
                        AddRecurisveQuestion(dt, row, 1, extensionParam.Name, "", SSRSConnect.typCombobox, extensionParam.Required, extensionParam.Value, ValidValuesToDataTable(extensionParam.ValidValues), extensionParam.ReadOnly);
					else
                        AddRecurisveQuestion(dt, row, 1, extensionParam.Name, "", SSRSConnect.typTextbox, extensionParam.Required, extensionParam.Value, extensionParam.ReadOnly);
					row++;
				}
			}
			dt.Prefix = "A_2_" + row.ToString();
			return dt;
		}

		private ExtensionParameter[] ProcessExtensionsByPermissions(ExtensionParameter[] ext)
		{
			int ic = 0, ii = 0;
			foreach (ExtensionParameter extensionParam in ext)
			{
				if (extensionParam.DisplayName != null)
				{
					if (_mangeshared == false)
					{
						if (extensionParam.Name == "TO")
						{
							extensionParam.Value = Environment.UserName;
							extensionParam.ReadOnly = true;
						}
						else if (extensionParam.Name == "CC" || extensionParam.Name == "ReplyTo" || extensionParam.Name == "BCC")
						{
							extensionParam.DisplayName = "";
						}
					}
				}
			}
			foreach (ExtensionParameter p in ext)
				if (p.DisplayName != "") ic++;

			ExtensionParameter[] results = new ExtensionParameter[ic];
			foreach (ExtensionParameter p in ext)
			{
				if (p.DisplayName != "")
				{
					results[ii] = p;
					ii++;
				}
			}
			return results;
		}

		public object ProcessSubscriptionQuestions(DataTable questions)
		{
			if (questions.TableName == "Parameters")
			{
                DataView d = new DataView(questions, "Type <> '" + SSRSConnect.typCaption + "'", "", System.Data.DataViewRowState.CurrentRows);
				int i = 0;
				ParameterValue[] parameters = new ParameterValue[d.Count];
				foreach (DataRowView row in d)
				{
					parameters[i] = new ParameterValue();
					parameters[i].Name = Convert.ToString(row["Name"]);
					parameters[i].Value = Convert.ToString(row["Value"]);
					i++;
				}	
				return parameters;
			}
			else if (questions.TableName == "Extensions")
			{
                DataView d = new DataView(questions, "Type <> '" + SSRSConnect.typCaption + "'", "", System.Data.DataViewRowState.CurrentRows);
				ParameterValue[] extensionParams = new ParameterValue[d.Count];
				int i = 0;
				foreach (DataRowView rw in d)
				{
					if (Convert.ToString(rw["Value"]) != "")
					{
						extensionParams[i] = new ParameterValue();
						extensionParams[i].Name = Convert.ToString(rw["Name"]);
						extensionParams[i].Value = Convert.ToString(rw["Value"]);
						i++;
					}
				}
				ExtensionSettings extensionSettings = new ExtensionSettings(); 
				extensionSettings.Extension = questions.Namespace;
				extensionSettings.ParameterValues = extensionParams;
				return extensionSettings;
			}
			else if (questions.TableName == "DailySheduleQuestions")
			{
				ScheduleDefinition schedule = new ScheduleDefinition();
				schedule.EndDateSpecified = false;       

				RecurrencePattern pattern = null;
				DataView find = new DataView(questions,"Name = '" + fldOnTheFollowingDays + "'","",DataViewRowState.CurrentRows);
				if (find.Count > 0 && Convert.ToBoolean(find[0]["Value"]))
				{
					pattern = new WeeklyRecurrence();
					DaysOfWeekSelector days = new DaysOfWeekSelector();
					((WeeklyRecurrence)pattern).WeeksIntervalSpecified = true;
					((WeeklyRecurrence)pattern).WeeksInterval = 1;
					((WeeklyRecurrence)pattern).DaysOfWeek = days;
				}

				if (pattern == null)
				{
					find = new DataView(questions,"Name = '" + fldEveryWeekday + "'","",DataViewRowState.CurrentRows);
					if (find.Count > 0 && Convert.ToBoolean(find[0]["Value"]))
					{
						pattern = new WeeklyRecurrence();
						DaysOfWeekSelector days = new DaysOfWeekSelector();
						days.Monday = true;
						days.Tuesday = true;
						days.Wednesday = true;
						days.Thursday = true;
						days.Friday = true;
						((WeeklyRecurrence)pattern).WeeksIntervalSpecified = true;
						((WeeklyRecurrence)pattern).WeeksInterval = 1;
						((WeeklyRecurrence)pattern).DaysOfWeek = days;
						return pattern;
					}
				}
				
				if (pattern == null)
					pattern = new DailyRecurrence();

				foreach (DataRow rw in questions.Rows)
				{
					string field = Convert.ToString(rw["Name"]);
					string value = Convert.ToString(rw["Value"]);
					switch(field)
					{
						case fldNumberOfDays:
						{
							if (pattern is DailyRecurrence)
								((DailyRecurrence)pattern).DaysInterval = Convert.ToInt32(value);
							break;
						}
						case fldSun:
						{
							if (pattern is WeeklyRecurrence)
								((WeeklyRecurrence)pattern).DaysOfWeek.Sunday = Convert.ToBoolean(value);
							break;
						}							
						case fldMon:
						{
							if (pattern is WeeklyRecurrence)
								((WeeklyRecurrence)pattern).DaysOfWeek.Monday = Convert.ToBoolean(value);
							break;
						}							
						case fldTue:
						{
							if (pattern is WeeklyRecurrence)
								((WeeklyRecurrence)pattern).DaysOfWeek.Tuesday = Convert.ToBoolean(value);
							break;
						}							
						case fldWed:
						{
							if (pattern is WeeklyRecurrence)
								((WeeklyRecurrence)pattern).DaysOfWeek.Wednesday = Convert.ToBoolean(value);
							break;
						}							
						case fldThu:
						{
							if (pattern is WeeklyRecurrence)
								((WeeklyRecurrence)pattern).DaysOfWeek.Thursday = Convert.ToBoolean(value);
							break;
						}							
						case fldFri:
						{
							if (pattern is WeeklyRecurrence)
								((WeeklyRecurrence)pattern).DaysOfWeek.Friday = Convert.ToBoolean(value);
							break;
						}							
						case fldSat:
						{
							if (pattern is WeeklyRecurrence)
								((WeeklyRecurrence)pattern).DaysOfWeek.Saturday = Convert.ToBoolean(value);
							break;
						}							
					}
				}
				return pattern;
			}
			else if (questions.TableName == "MonthlySheduleQuestions")
			{
				ScheduleDefinition schedule = new ScheduleDefinition();
				schedule.EndDateSpecified = false;       
				RecurrencePattern pattern = null;
				DataView find = new DataView(questions,"Name = '" + fldOnWeekofmonth + "'","",DataViewRowState.CurrentRows);
				if (find.Count > 0)
				{
					if (Convert.ToBoolean(find[0]["Value"]))
						pattern = new MonthlyDOWRecurrence();
					else
						pattern = new MonthlyRecurrence();
				}
				else
				{
					//exception
					return null;
				}

				MonthsOfYearSelector months = new MonthsOfYearSelector();
				DaysOfWeekSelector days = new DaysOfWeekSelector();

				foreach (DataRow rw in questions.Rows)
				{
					string field = Convert.ToString(rw["Name"]);
					string value = Convert.ToString(rw["Value"]);
					switch(field)
					{
						case fldWeekOfMonth:
						{
							if (pattern is MonthlyDOWRecurrence)
							{
								((MonthlyDOWRecurrence)pattern).WhichWeekSpecified = true;
								switch(value)
								{
									case fldWeek1st:
										((MonthlyDOWRecurrence)pattern).WhichWeek = WeekNumberEnum.FirstWeek;
										break;
									case fldWeek2nd:
										((MonthlyDOWRecurrence)pattern).WhichWeek = WeekNumberEnum.SecondWeek;
										break;
									case fldWeek3rd:
										((MonthlyDOWRecurrence)pattern).WhichWeek = WeekNumberEnum.ThirdWeek;
										break;
									case fldWeek4th:
										((MonthlyDOWRecurrence)pattern).WhichWeek = WeekNumberEnum.FourthWeek;
										break;
									case fldWeekLast:
										((MonthlyDOWRecurrence)pattern).WhichWeek = WeekNumberEnum.LastWeek;
										break;
								}
							}
							break;
						}
						case fldDayOfMonth:
						{
							if (pattern is MonthlyRecurrence)
								((MonthlyRecurrence)pattern).Days = value;
							break;
						}
						case fldJan:
						{
							months.January = Convert.ToBoolean(value);
							break;
						}
						case fldFeb:
						{
							months.February = Convert.ToBoolean(value);
							break;
						}						
						case fldMar:
						{
							months.March = Convert.ToBoolean(value);
							break;
						}						
						case fldApr:
						{
							months.April = Convert.ToBoolean(value);
							break;
						}						
						case fldMay:
						{
							months.May = Convert.ToBoolean(value);
							break;
						}						
						case fldJun:
						{
							months.June = Convert.ToBoolean(value);
							break;
						}	
						case fldJul:
						{
							months.July = Convert.ToBoolean(value);
							break;
						}							
						case fldAug:
						{
							months.August = Convert.ToBoolean(value);
							break;
						}
						case fldSep:
						{
							months.September = Convert.ToBoolean(value);
							break;
						}						
						case fldOct:
						{
							months.October = Convert.ToBoolean(value);
							break;
						}							
						case fldNov:
						{
							months.November = Convert.ToBoolean(value);
							break;
						}							
						case fldDec:
						{
							months.December = Convert.ToBoolean(value);
							break;
						}							
						case fldSun:
						{
							days.Sunday = Convert.ToBoolean(value);
							break;
						}							
						case fldMon:
						{
							days.Monday = Convert.ToBoolean(value);
							break;
						}							
						case fldTue:
						{
							days.Tuesday = Convert.ToBoolean(value);
							break;
						}							
						case fldWed:
						{
							days.Wednesday = Convert.ToBoolean(value);
							break;
						}							
						case fldThu:
						{
							days.Thursday = Convert.ToBoolean(value);
							break;
						}							
						case fldFri:
						{
							days.Friday = Convert.ToBoolean(value);
							break;
						}							
						case fldSat:
						{
							days.Saturday = Convert.ToBoolean(value);
							break;
						}							
					}
				}
				if (pattern is MonthlyDOWRecurrence)
				{
					((MonthlyDOWRecurrence)pattern).MonthsOfYear = months;
					((MonthlyDOWRecurrence)pattern).DaysOfWeek = days;
				}
				else if (pattern is MonthlyDOWRecurrence)
				{
					((MonthlyDOWRecurrence)pattern).MonthsOfYear = months;
				}
				else if (pattern is MonthlyRecurrence)
				{
					((MonthlyRecurrence)pattern).MonthsOfYear = months;
				}
				else if (pattern is WeeklyRecurrence)
				{
					((WeeklyRecurrence)pattern).DaysOfWeek = days;
					((WeeklyRecurrence)pattern).WeeksIntervalSpecified = false;
				}
				return pattern;
			}
			else
				return null;
		}
		

		private DataSet CreateQuestionsFromSubscription(DataSet scheduleproperties, ExtensionSettings extenstions, ScheduleDefinition schedule, ParameterValue[] parameters)
		{

			foreach (ParameterValue p in parameters)
			{
				DataView d = new DataView(scheduleproperties.Tables["Parameters"],"Name = '" + p.Name + "'","",System.Data.DataViewRowState.CurrentRows);
				if (d.Count > 0) d[0]["Value"] = p.Value;
			}
			
			if (scheduleproperties.Tables["DailySheduleQuestions"] != null)
			{
				DataTable tbl = scheduleproperties.Tables["DailySheduleQuestions"];
				SetValue(tbl,fldStartTime,schedule.StartDateTime.ToShortTimeString());
				WeeklyRecurrence weeklyrec = schedule.Item as WeeklyRecurrence;
				if (weeklyrec != null)
				{
					SetValue(tbl,fldMon,weeklyrec.DaysOfWeek.Monday);
					SetValue(tbl,fldTue,weeklyrec.DaysOfWeek.Tuesday);
					SetValue(tbl,fldWed,weeklyrec.DaysOfWeek.Wednesday);
					SetValue(tbl,fldThu,weeklyrec.DaysOfWeek.Thursday);
					SetValue(tbl,fldFri,weeklyrec.DaysOfWeek.Friday);
					SetValue(tbl,fldSat,weeklyrec.DaysOfWeek.Saturday);
					SetValue(tbl,fldSun,weeklyrec.DaysOfWeek.Sunday);
					SetValue(tbl,fldOnTheFollowingDays,true);
				}
				DailyRecurrence dailyrec = schedule.Item as DailyRecurrence;
				if (dailyrec != null)
				{
					SetValue(tbl,fldOnTheFollowingDays,false);
					SetValue(tbl,fldRepeatAfterThis,true);
					SetValue(tbl,fldNumberOfDays,dailyrec.DaysInterval);
				}

			}
			else if (scheduleproperties.Tables["MonthlySheduleQuestions"] != null)
			{
				DataTable tbl = scheduleproperties.Tables["MonthlySheduleQuestions"];
				SetValue(tbl,fldStartTime,schedule.StartDateTime.ToShortTimeString());
				MonthlyRecurrence monrec = schedule.Item as MonthlyRecurrence;
				if (monrec != null)
				{
					SetValue(tbl,fldJan,monrec.MonthsOfYear.January);
					SetValue(tbl,fldFeb,monrec.MonthsOfYear.February);
					SetValue(tbl,fldMar,monrec.MonthsOfYear.March);
					SetValue(tbl,fldApr,monrec.MonthsOfYear.April);
					SetValue(tbl,fldMay,monrec.MonthsOfYear.May);
					SetValue(tbl,fldJun,monrec.MonthsOfYear.June);
					SetValue(tbl,fldJul,monrec.MonthsOfYear.July);
					SetValue(tbl,fldAug,monrec.MonthsOfYear.August);
					SetValue(tbl,fldSep,monrec.MonthsOfYear.September);
					SetValue(tbl,fldOct,monrec.MonthsOfYear.October);
					SetValue(tbl,fldNov,monrec.MonthsOfYear.November);
					SetValue(tbl,fldDec,monrec.MonthsOfYear.December);
					SetValue(tbl,fldDayOfMonth,monrec.Days);
					SetValue(tbl,fldOnWeekofmonth,false);
					SetValue(tbl,fldOnCalendarDays,true);
				}
				MonthlyDOWRecurrence mondrec = schedule.Item as MonthlyDOWRecurrence;
				if (mondrec != null)
				{
					SetValue(tbl,fldJan,mondrec.MonthsOfYear.January);
					SetValue(tbl,fldFeb,mondrec.MonthsOfYear.February);
					SetValue(tbl,fldMar,mondrec.MonthsOfYear.March);
					SetValue(tbl,fldApr,mondrec.MonthsOfYear.April);
					SetValue(tbl,fldMay,mondrec.MonthsOfYear.May);
					SetValue(tbl,fldJun,mondrec.MonthsOfYear.June);
					SetValue(tbl,fldJul,mondrec.MonthsOfYear.July);
					SetValue(tbl,fldAug,mondrec.MonthsOfYear.August);
					SetValue(tbl,fldSep,mondrec.MonthsOfYear.September);
					SetValue(tbl,fldOct,mondrec.MonthsOfYear.October);
					SetValue(tbl,fldNov,mondrec.MonthsOfYear.November);
					SetValue(tbl,fldDec,mondrec.MonthsOfYear.December);
					
					SetValue(tbl,fldMon,mondrec.DaysOfWeek.Monday);
					SetValue(tbl,fldTue,mondrec.DaysOfWeek.Tuesday);
					SetValue(tbl,fldWed,mondrec.DaysOfWeek.Wednesday);
					SetValue(tbl,fldThu,mondrec.DaysOfWeek.Thursday);
					SetValue(tbl,fldFri,mondrec.DaysOfWeek.Friday);
					SetValue(tbl,fldSat,mondrec.DaysOfWeek.Saturday);
					SetValue(tbl,fldSun,mondrec.DaysOfWeek.Sunday);
					SetValue(tbl,fldOnCalendarDays,false);
					SetValue(tbl,fldOnWeekofmonth,true);

					switch(mondrec.WhichWeek)
					{
						case WeekNumberEnum.FirstWeek:
							SetValue(tbl,fldWeekOfMonth,fldWeek1st);
							break;
						case WeekNumberEnum.SecondWeek:
							SetValue(tbl,fldWeekOfMonth,fldWeek2nd);
							break;
						case WeekNumberEnum.ThirdWeek:
							SetValue(tbl,fldWeekOfMonth,fldWeek3rd);
							break;
						case WeekNumberEnum.FourthWeek:
							SetValue(tbl,fldWeekOfMonth,fldWeek4th);
							break;
						case WeekNumberEnum.LastWeek:
							SetValue(tbl,fldWeekOfMonth,fldWeekLast);
							break;
					}
				}

			}
			foreach (ParameterValue e in extenstions.ParameterValues)
			{
				DataView d = new DataView(scheduleproperties.Tables["Extensions"],"Name = '" + e.Name + "'","",System.Data.DataViewRowState.CurrentRows);
				d[0]["Value"] = e.Value;
			}

			return scheduleproperties;
		}

		private void SetValue(DataTable table, string field, object value)
		{
			DataView d = new DataView(table,"Name = '" + field + "'","",System.Data.DataViewRowState.CurrentRows);
			d[0]["Value"] = value;
		}

		public DataTable MonthlySheduleQuestions()
		{
			DataTable dt = new DataTable("MonthlySheduleQuestions");
			dt.Prefix = "A_9_7";
			dt.Columns.Add("Name",typeof(string));
			dt.Columns.Add("DisplayName",typeof(string));
			dt.Columns.Add("Type",typeof(string));
			dt.Columns.Add("Row",typeof(int));
			dt.Columns.Add("Column",typeof(int));
			dt.Columns.Add("Required",typeof(bool));
			dt.Columns.Add("ReadOnly",typeof(bool));
			dt.Columns.Add("Value",typeof(object));
			dt.Columns.Add("SelectValues",typeof(DataTable));

            AddRecurisveQuestion(dt, 0, 0, "", "Start Time", SSRSConnect.typCaption, false, "", false);
			AddRecurisveQuestion(dt,0,1,fldStartTime,fldStartTime,SSRSConnect.typTimebox,false,"09:00",false);
			AddRecurisveQuestion(dt,1,0,"","Months:",SSRSConnect.typCaption,false,"",false);
			AddRecurisveQuestion(dt,1,2,fldJan,fldJan,SSRSConnect.typCheckbox,false,true,false);
			AddRecurisveQuestion(dt,1,3,fldFeb,fldFeb,SSRSConnect.typCheckbox,false,true,false);
			AddRecurisveQuestion(dt,1,4,fldMar,fldMar,SSRSConnect.typCheckbox,false,true,false);
			AddRecurisveQuestion(dt,1,5,fldApr,fldApr,SSRSConnect.typCheckbox,false,true,false);
			AddRecurisveQuestion(dt,1,6,fldMay,fldMay,SSRSConnect.typCheckbox,false,true,false);
			AddRecurisveQuestion(dt,1,7,fldJun,fldJun,SSRSConnect.typCheckbox,false,true,false);
			AddRecurisveQuestion(dt,2,2,fldJul,fldJul,SSRSConnect.typCheckbox,false,true,false);
			AddRecurisveQuestion(dt,2,3,fldAug,fldAug,SSRSConnect.typCheckbox,false,true,false);
			AddRecurisveQuestion(dt,2,4,fldSep,fldSep,SSRSConnect.typCheckbox,false,true,false);
			AddRecurisveQuestion(dt,2,5,fldOct,fldOct,SSRSConnect.typCheckbox,false,true,false);
			AddRecurisveQuestion(dt,2,6,fldNov,fldNov,SSRSConnect.typCheckbox,false,true,false);
			AddRecurisveQuestion(dt,2,7,fldDec,fldDec,SSRSConnect.typCheckbox,false,true,false);
			AddRecurisveQuestion(dt,3,0,fldOnWeekofmonth,fldOnWeekofmonth,SSRSConnect.typOptions,false,true,false);
			AddRecurisveQuestion(dt,3,1,fldWeekOfMonth,fldWeekOfMonth,SSRSConnect.typCombobox,true,"1st",ValidValuesToDataTable(new string[5]{"1st","2nd","3rd","4th","Last"}),false);
			AddRecurisveQuestion(dt,4,0,"","     On day of week",SSRSConnect.typCaption,false,"",false);
			AddRecurisveQuestion(dt,4,2,fldSun,fldSun,SSRSConnect.typCheckbox,false,true,false);
			AddRecurisveQuestion(dt,4,3,fldMon,fldMon,SSRSConnect.typCheckbox,false,true,false);
			AddRecurisveQuestion(dt,4,4,fldTue,fldTue,SSRSConnect.typCheckbox,false,false,false);
			AddRecurisveQuestion(dt,4,5,fldWed,fldWed,SSRSConnect.typCheckbox,false,false,false);
			AddRecurisveQuestion(dt,4,6,fldThu,fldThu,SSRSConnect.typCheckbox,false,false,false);
			AddRecurisveQuestion(dt,4,7,fldFri,fldFri,SSRSConnect.typCheckbox,false,false,false);
			AddRecurisveQuestion(dt,5,2,fldSat,fldSat,SSRSConnect.typCheckbox,false,false,false);
			AddRecurisveQuestion(dt,6,0,fldOnCalendarDays,fldOnCalendarDays,SSRSConnect.typOptions,false,false,false);
			AddRecurisveQuestion(dt,6,1,fldDayOfMonth,fldDayOfMonth,SSRSConnect.typTextbox,false,"1, 3-5",false);
			return dt;
		}

		public DataTable DailySheduleQuestions()
		{
			DataTable dt = new DataTable("DailySheduleQuestions");
			dt.Prefix = "A_5_5";
			dt.Columns.Add("Name",typeof(string));
			dt.Columns.Add("DisplayName",typeof(string));
			dt.Columns.Add("Type",typeof(string));
			dt.Columns.Add("Row",typeof(int));
			dt.Columns.Add("Column",typeof(int));
			dt.Columns.Add("Required",typeof(bool));
			dt.Columns.Add("ReadOnly",typeof(bool));
			dt.Columns.Add("Value",typeof(object));
			dt.Columns.Add("SelectValues",typeof(DataTable));

			AddRecurisveQuestion(dt,0,0,"","Start Time",SSRSConnect.typCaption,false,"",false);
			AddRecurisveQuestion(dt,0,1,fldStartTime,fldStartTime,SSRSConnect.typTimebox,false,"09:00",false);
			AddRecurisveQuestion(dt,1,0,fldOnTheFollowingDays,fldOnTheFollowingDays,SSRSConnect.typOptions,false,"true",false);
			AddRecurisveQuestion(dt,1,1,fldSun,fldSun,SSRSConnect.typCheckbox,false,true,false);
			AddRecurisveQuestion(dt,1,2,fldMon,fldMon,SSRSConnect.typCheckbox,false,true,false);
			AddRecurisveQuestion(dt,1,3,fldTue,fldTue,SSRSConnect.typCheckbox,false,false,false);
			AddRecurisveQuestion(dt,1,4,fldWed,fldWed,SSRSConnect.typCheckbox,false,false,false);
			AddRecurisveQuestion(dt,2,1,fldThu,fldThu,SSRSConnect.typCheckbox,false,false,false);
			AddRecurisveQuestion(dt,2,2,fldFri,fldFri,SSRSConnect.typCheckbox,false,false,false);
			AddRecurisveQuestion(dt,2,3,fldSat,fldSat,SSRSConnect.typCheckbox,false,false,false);			
			AddRecurisveQuestion(dt,3,0,fldEveryWeekday,fldEveryWeekday,SSRSConnect.typOptions,false,false,false);
			AddRecurisveQuestion(dt,4,0,fldRepeatAfterThis,fldRepeatAfterThis,SSRSConnect.typOptions,false,false,false);
			AddRecurisveQuestion(dt,4,1,fldNumberOfDays,fldNumberOfDays,SSRSConnect.typTextbox,true,"1",false);
			return dt;
		}

		public DataTable WeeklySheduleQuestions()
		{
			DataTable dt = new DataTable("WeeklySheduleQuestions");
			dt.Prefix = "A_4_3";
			dt.Columns.Add("Name",typeof(string));
			dt.Columns.Add("DisplayName",typeof(string));
			dt.Columns.Add("Type",typeof(string));
			dt.Columns.Add("Row",typeof(int));
			dt.Columns.Add("Column",typeof(int));
			dt.Columns.Add("Required",typeof(bool));
			dt.Columns.Add("ReadOnly",typeof(bool));
			dt.Columns.Add("Value",typeof(object));
			dt.Columns.Add("SelectValues",typeof(DataTable));

			AddRecurisveQuestion(dt,0,0,fldRepeatAfterNoWeeks,fldRepeatAfterNoWeeks,SSRSConnect.typTextbox,false,"1",false);
			AddRecurisveQuestion(dt,1,0,"","On day(s)",SSRSConnect.typCaption,false,"",false);
			AddRecurisveQuestion(dt,1,1,fldSun,fldSun,SSRSConnect.typCheckbox,false,true,false);
			AddRecurisveQuestion(dt,1,2,fldMon,fldMon,SSRSConnect.typCheckbox,false,true,false);
			AddRecurisveQuestion(dt,1,3,fldTue,fldTue,SSRSConnect.typCheckbox,false,false,false);
			AddRecurisveQuestion(dt,1,4,fldWed,fldWed,SSRSConnect.typCheckbox,false,false,false);
			AddRecurisveQuestion(dt,2,1,fldThu,fldThu,SSRSConnect.typCheckbox,false,false,false);
			AddRecurisveQuestion(dt,2,2,fldFri,fldFri,SSRSConnect.typCheckbox,false,false,false);
			AddRecurisveQuestion(dt,2,3,fldSat,fldSat,SSRSConnect.typCheckbox,false,false,false);			
			return dt;
		}

		private XmlDocument GetScheduleAsXml(ScheduleDefinition schedule, bool Fix)
		{
			XmlDocument doc = new XmlDocument();
			using (MemoryStream buffer = new MemoryStream())
			{
				XmlSerializer xmlSerializer = new XmlSerializer(typeof(ScheduleDefinition));
				xmlSerializer.Serialize(buffer, schedule);
				buffer.Seek(0, SeekOrigin.Begin);
				doc.Load(buffer);
			}
			if (Fix)
			{
				// patch up WhichWeek
				XmlNamespaceManager ns = new XmlNamespaceManager(doc.NameTable);
				ns.AddNamespace("rs", 
					"http://schemas.microsoft.com/sqlserver/2003/12/reporting/reportingservices");

				XmlNode node = 
					doc.SelectSingleNode(
					"/ScheduleDefinition/rs:MonthlyDOWRecurrence/rs:WhichWeek", ns
					);
				if(node != null)
				{
					switch (node.InnerXml)
					{
						case "FirstWeek":
							node.InnerXml = "FIRST_WEEK"; break;
						case "SecondWeek":
							node.InnerXml = "SECOND_WEEK"; break;
						case "ThirdWeek":
							node.InnerXml = "THIRD_WEEK"; break;
						case "FourthWeek":
							node.InnerXml = "FOURTH_WEEK"; break;
						case "LastWeek":
							node.InnerXml = "LAST_WEEK"; break;
					}
				}
			}

			return doc;
		}

		private ScheduleDefinition GetXmlAsSchedule(XmlDocument xmlschedule)
		{
			// patch up WhichWeek
			XmlNamespaceManager ns = new XmlNamespaceManager(xmlschedule.NameTable);
			ns.AddNamespace("rs", 
				"http://schemas.microsoft.com/sqlserver/2003/12/reporting/reportingservices");

			XmlNode node = 
				xmlschedule.SelectSingleNode(
				"/ScheduleDefinition/rs:MonthlyDOWRecurrence/rs:WhichWeek", ns
				);
			if(node != null)
			{
				switch (node.InnerXml)
				{
					case "FIRST_WEEK":
						node.InnerXml = "FirstWeek"; break;
					case "SECOND_WEEK":
						node.InnerXml = "SecondWeek"; break;
					case "THIRD_WEEK":
						node.InnerXml = "ThirdWeek"; break;
					case "FOURTH_WEEK":
						node.InnerXml = "FourthWeek"; break;
					case "LAST_WEEK":
						node.InnerXml = "LastWeek"; break;
				}
			}

			StringReader ts = new StringReader(xmlschedule.OuterXml);
			XmlSerializer xmlSerializer = new XmlSerializer(typeof(ScheduleDefinition));
			ScheduleDefinition schedule = xmlSerializer.Deserialize(ts) as ScheduleDefinition;
			return schedule;
		}

		private DataTable ValidValuesToDataTable(ValidValue[] validValues)
		{
			DataTable data = new DataTable("Values");
			data.Columns.Add("Value",typeof(string));
			data.Columns.Add("Label",typeof(string));

			if(validValues != null)
			{
				for(int i = 0; i < validValues.Length; i++)
				{
					DataRow r = data.NewRow();
					r["Value"] = validValues[i].Value;
					r["Label"] = validValues[i].Label;
					data.Rows.Add(r);
				}           
			}
			return data;
		}  

		private DataTable ValidValuesToDataTable(string[] validValues)
		{
			DataTable data = new DataTable("Values");
			data.Columns.Add("Value",typeof(string));
			data.Columns.Add("Label",typeof(string));

			if(validValues != null)
			{
				for(int i = 0; i < validValues.Length; i++)
				{
					DataRow r = data.NewRow();
					r["Value"] = validValues[i];
					r["Label"] = validValues[i];
					data.Rows.Add(r);
				}           
			}
			return data;
		} 
		
		private string ValidValuesToString(ValidValue[] validValues)
		{
			string result = " ";

			if(validValues != null)
			{
				StringBuilder sb = new StringBuilder();

				for(int i = 0; i < validValues.Length; i++)
				{
					sb.Append(validValues[i].Label);
					if(i < validValues.Length - 1)
					{
						sb.Append(",");
					}
				}           
				result = sb.ToString();
			}

			return result;
		}       
		
		private string ValidValuesToString(string[] validValues)
		{
			string result = "";

			if(validValues != null)
			{
				StringBuilder sb = new StringBuilder();

				for(int c = 0; c < validValues.Length; c++)
				{
					sb.Append(validValues[c]);
                }           
				result = sb.ToString();
			}

			return result;
		}       

		private void AddRecurisveQuestion(DataTable dt, int Row, int Column, string Name, string DisplayName, string Type, bool Required, object Value, bool ReadOnly)
		{
			AddRecurisveQuestion(dt,Row,Column,Name,DisplayName,Type,Required,Value,null,ReadOnly);
		}

		private void AddRecurisveQuestion(DataTable dt, int Row, int Column, string Name, string DisplayName, string Type, bool Required, object Value, DataTable SelectValues, bool ReadOnly)
		{
			DataRow dr = dt.NewRow();
			dr["Name"] = Name;
			dr["DisplayName"] = DisplayName;
			dr["Row"] = Row;
			dr["Column"] = Column;
			dr["Type"] = Type;
			dr["Required"] = Required;
			dr["ReadOnly"] = ReadOnly;
			if (Value == null)
				dr["Value"] = DBNull.Value;
			else
				dr["Value"] = Value;
			if (SelectValues == null)
				dr["SelectValues"] = DBNull.Value;
			else
				dr["SelectValues"] = SelectValues;
			dt.Rows.Add(dr);
		}

		public DataSet GetSubscription(string SubscriptionID, string ReportName)
		{
			ExtensionSettings extenstions;
			string subscriptiondescription = "";
			string eventtype = "";
			ActiveState active;
			string status = "";
			string matchdata = "";
			ParameterValue[] parameters;


			string id = rs.GetSubscriptionProperties(SubscriptionID, out extenstions, out subscriptiondescription, out active, out status, out eventtype, out matchdata, out parameters);
			
			XmlDocument buffer = new XmlDocument();
			buffer.LoadXml(matchdata);
			ScheduleDefinition schedule = GetXmlAsSchedule(buffer);

			DataSet subsdataset = new DataSet("UPDATE");
			if (schedule.Item is MonthlyRecurrence || schedule.Item is MonthlyDOWRecurrence)
				subsdataset.Tables.Add(MonthlySheduleQuestions());
			else 
				subsdataset.Tables.Add(DailySheduleQuestions());

			subsdataset.Tables.Add(Extensions(ExtensionsOptions.ReportServerEmail));
			subsdataset.Tables.Add(ReportParameters(ReportName));

			return CreateQuestionsFromSubscription(subsdataset,extenstions,schedule,parameters);
		}
		
		public void UpdateSubscription(string SubscriptionID, string SubscriptionDescription, DataTable extensions , DataTable Schedule, DataTable Parameters)
		{
			ScheduleDefinition schedule = new ScheduleDefinition();
			schedule.StartDateTime = DateTime.Now;
			schedule.EndDateSpecified = false;       
			RecurrencePattern pattern = ProcessSubscriptionQuestions(Schedule) as RecurrencePattern;
			if (pattern != null)
				schedule.Item = pattern;
			else
			{
				// throw exception
			}
			XmlDocument xmlSchedule = GetScheduleAsXml(schedule,false);

			string eventType = "TimedSubscription";
			string matchData = xmlSchedule.OuterXml;      
			ExtensionSettings extSettings = ProcessSubscriptionQuestions(extensions) as ExtensionSettings;
			if (extSettings == null)
			{
				// throw exception
			}

			DataView d = new DataView(Parameters,"Type <> '" + SSRSConnect.typCaption + "'","",System.Data.DataViewRowState.CurrentRows);
			int i = 0;
			ParameterValue[] parameters = new ParameterValue[d.Count];
			foreach (DataRowView row in d)
			{
				parameters[i] = new ParameterValue();
				parameters[i].Name = Convert.ToString(row["Name"]);
				parameters[i].Value = Convert.ToString(row["Value"]);
				i++;
			}

			rs.SetSubscriptionProperties(SubscriptionID, extSettings, SubscriptionDescription, eventType, matchData, parameters);
		}

		public void CreateSharedDataSource()
		{
			string name = "FWBS";
			string parent ="/";

			// Define the data source definition.
			DataSourceDefinition definition = new DataSourceDefinition();
			definition.CredentialRetrieval = CredentialRetrievalEnum.Integrated;
			definition.ConnectString = "data source=(local);initial catalog=AdventureWorks";
			definition.Enabled = true;
			definition.EnabledSpecified = true;
			definition.Extension = "SQL";
			definition.ImpersonateUserSpecified = false;
			//Use the default prompt string.
			definition.Prompt = null;
			definition.WindowsCredentials = false;
			rs.CreateDataSource(name, parent, false, definition, null);
		}

		public string CreateSubscription(string Report, string SubscriptionDescription, DataTable extensions , DataTable Schedule, DataTable Parameters, string StartTime)
		{
			ScheduleDefinition schedule = new ScheduleDefinition();

			
			DataView find = new DataView(Schedule,"Name = '" + fldStartTime + "'","",DataViewRowState.CurrentRows);
			if (find.Count > 0)
			{
				string[] timesplit = Convert.ToString(find[0]["Value"]).Split(":".ToCharArray());
				if (timesplit.Length != 2)
					throw new Exception("Invalid Time Format {" + Convert.ToString(find[0]["Value"]) + "}");
				schedule.StartDateTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month,DateTime.Now.Day, Convert.ToInt32(timesplit[0]),Convert.ToInt32(timesplit[1]),0,0);
			}
			else 
				schedule.StartDateTime = new DateTime(DateTime.Now.Year,DateTime.Now.Month,DateTime.Now.Day,9,0,0,0);


			schedule.EndDateSpecified = false;       
			RecurrencePattern pattern = ProcessSubscriptionQuestions(Schedule) as RecurrencePattern;
			if (pattern != null)
				schedule.Item = pattern;
			else
			{
				// throw exception
			}
			XmlDocument xmlSchedule = GetScheduleAsXml(schedule,false);

			string eventType = "TimedSubscription";
			string matchData = xmlSchedule.OuterXml;      
			ExtensionSettings extSettings = ProcessSubscriptionQuestions(extensions) as ExtensionSettings;
			if (extSettings == null)
			{
				// throw exception
			}

			ParameterValue[] parameters = ProcessSubscriptionQuestions(Parameters) as ParameterValue[];
			if (parameters == null)
			{
				// throw exception
			}

			return rs.CreateSubscription(Report, extSettings, SubscriptionDescription, eventType, matchData, parameters);
		}

		public void DeleteSubscription(string ID)
		{
			rs.DeleteSubscription(ID);
		}

		public DataTable ListSubscriptions()
		{
			ExtensionSettings extSettings;
			string desc;
			ActiveState active;
			string status;
			string eventType;
			string matchData;
			ParameterValue[] values = null;
			Subscription[] subscriptions = null;

			DataTable dt = new DataTable("Subscriptions");
			dt.Columns.Add("ID",typeof(string));
			dt.Columns.Add("Description",typeof(string));
			dt.Columns.Add("Status",typeof(string));
			dt.Columns.Add("Report",typeof(string));
			dt.Columns.Add("Path",typeof(string));
			dt.Columns.Add("MatchData",typeof(string));
			dt.Columns.Add("Extension",typeof(string));
			dt.Columns.Add("LastExecuted",typeof(DateTime));

			subscriptions = rs.ListSubscriptions(null,null);
			foreach (Subscription s in subscriptions)
			{
				DataRow dr = dt.NewRow();
				dr["ID"] = s.SubscriptionID;
				// Retrieve properties for the first subscription in the list.
				rs.GetSubscriptionProperties(s.SubscriptionID, out extSettings, out desc, out active, 
					out status, out eventType, out matchData, out values);

				dr["Description"] = desc;
				dr["Status"] = status;
				dr["Report"] = s.Report;
				dr["Path"] = s.Path;
				dr["MatchData"] = matchData;
				dr["Extension"] = extSettings.Extension;
				if (s.LastExecutedSpecified) dr["LastExecuted"] = s.LastExecuted;
				dt.Rows.Add(dr);
			}
			return dt;
		}
		
		public DataTable ListReports(string Folder)
		{
			CatalogItem[] catalogItems;
			catalogItems = rs.ListChildren(Folder, true);

			DataTable dt = new DataTable("REPORTS");
			dt.Columns.Add("ID",typeof(string));
			dt.Columns.Add("CreatedBy",typeof(string));
			dt.Columns.Add("CreationDate",typeof(DateTime));
			dt.Columns.Add("Description",typeof(string));
			dt.Columns.Add("ExecutionDate",typeof(DateTime));
			dt.Columns.Add("Hidden",typeof(bool));
			dt.Columns.Add("MimeType",typeof(string));
			dt.Columns.Add("ModifiedBy",typeof(string));
			dt.Columns.Add("ModifiedDate",typeof(DateTime));
			dt.Columns.Add("Name",typeof(string));
			dt.Columns.Add("Path",typeof(string));
			dt.Columns.Add("Size",typeof(int));
			dt.Columns.Add("Type",typeof(string));
			dt.Columns.Add("VirtualPath",typeof(string));

			foreach(CatalogItem item in catalogItems)
			{
				DataRow dr = dt.NewRow();
				dr["ID"] = item.ID;
				dr["CreatedBy"] = item.CreatedBy;
				dr["CreationDate"] = item.CreationDate;
				dr["Description"] = item.Description;
				dr["ExecutionDate"] = item.ExecutionDate;
				dr["Hidden"] = item.Hidden;
				dr["MimeType"] = item.MimeType;
				dr["ModifiedBy"] = item.ModifiedBy;
				dr["ModifiedDate"] = item.ModifiedDate;
				dr["Name"] = item.Name;
				dr["Path"] = item.Path;
				dr["Size"] = item.Size;
				dr["Type"] = item.Type;
				dr["VirtualPath"] = item.VirtualPath;
				dt.Rows.Add(dr);
			}
			return dt;
		}


		// Report Server defines the URL to root of the 
		// Reporting Services home page. 
		public string ReportServerASMX
		{
            get { return SSRSConnect.WebServer; }
		}
		
		// Report Server defines the URL to root of the 
		// Reporting Services home page. 
		public string ReportServer
		{
            get { return SSRSConnect.Server; }
		}

		/// <summary>
		/// Returns the WebUrl to the Report Item
		/// </summary>
		public string ReportWebUrlPath
		{
			get
			{
                return SSRSConnect.Server + "/Pages/Report.aspx?ItemPath=";
			}
		}

		// ReportPath, when appended to the ReportServer property, 
		// will define the root of the report search. 
		// For example, to view all available reports on the report 
		// server, use "/" as the ReportPath. Setting the value to 
		// "/MyDemoReports", would only show reports and subdirectories
		// under http://ReportServer/MyDemoReports. 
		public string ReportPath
		{
			get { return "/OMS Reports"; }
		}

		// There are a couple places where we need to do string 
		// manipulation to add and remove path seperators - 
		// sometimes the seperator needs to be passed as an array
		// of char, othertimes as a string. These two properties
		// simply define both so the code is a little cleaner
		// when we do the string munging. 
		public char[] PathSeparatorArray
		{
			get { return "/".ToCharArray();}
		}

		public string PathSeparatorString
		{
			get { return "/"; }
		}
	}
}
