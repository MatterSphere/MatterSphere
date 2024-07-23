#region References
using System;
using FWBS.OMS.DocumentManagement;
#endregion

namespace FWBS.OMS
{
	public interface IContext
	{
		Associate Associate { get; }

		Client Client { get; }

		OMSFile File { get; }

		Session Session { get; }

		Contact Contact { get; }

		object Data { get; }

		OMSDocument Document { get; }

		System.Globalization.DateTimeFormatInfo DateTimeFormat { get; }

		DocumentVersion DocumentVersion { get; }

		FeeEarner FeeEarner { get; }

		System.Globalization.NumberFormatInfo NumberFormat { get; }

		FilePhase Phase { get; }

		Precedent Precedent { get; }


		User User { get; }

		IContext Parent { get; }

		T Get<T>();

		T Get<T>(Type type);

		T Get<T>(string name);

		T Get<T>(string name, Type type);

		void Set<T>(T value);

		void Set<T>(string name, T value);

		void Set<T>(string name, T value, Type type);

		IContext Clone();
	}
}
