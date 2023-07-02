namespace Utilities;

public static class DateTools
{
	//*********************************************
	/// <summary>
	/// Order by Descending list date eith format : (1399/02/10)
	/// </summary>
	/// <param name="value"></param>
	/// <returns>System.Collections.Generic.List<string></returns>
	public static List<string>
		OrderByDateDescending(this List<string> value)
	{
		List<string> lstOrderBy
			= new List<string>();

		var lst = value
			.Where(date => date.StrigToDateTimeMiladi().HasValue == true)
			.Select(date => date.StrigToDateTimeMiladi()!.Value)
			.OrderByDescending(x => x.Date)
			.Select(date => date.ToShamsi())
			.ToList();

		lstOrderBy.AddRange(lst);

		return lstOrderBy;
	}
	//*********************************************

	//*********************************************
	/// <summary>
	/// order by Descending year with format : (1399 - 1400)
	/// </summary>
	/// <param name="value"></param>
	/// <returns></returns>
	public static List<string>
		OrderByYearDescending(this List<string> value)
	{
		List<string> lstOrderBy = new List<string>();

		var temp = value.Select(x => Convert.ToInt32(x.Split('-')[0].Trim()))
			.OrderByDescending(x => x)
			.Select(date => date)
			.ToList();

		foreach (var item in temp)
		{
			var itemOrdered =
				value.FirstOrDefault(year => Convert.ToInt32(year.Split('-')[0].Trim()) == item);

			lstOrderBy.Add(item: itemOrdered!);
		}

		return lstOrderBy;
	}
	//*********************************************

	//*********************************************
	/// <summary>
	/// Order by list date eith format : (1399/02/10)
	/// </summary>
	/// <param name="value"></param>
	/// <returns></returns>
	public static List<string>
		OrderByDate(this List<string> value)
	{
		List<string> lstOrderBy
			= new List<string>();

		var lstOrder =
			value
			.Where(date => date.StrigToDateTimeMiladi().HasValue == true)
			.Select(date => date.StrigToDateTimeMiladi()!.Value)
			.OrderBy(x => x.Date)
			.Select(date => date.ToShamsi())
			.ToList();

		lstOrderBy.AddRange(lstOrder);
		return lstOrderBy;
	}
	//*********************************************

	//*********************************************
	/// <summary>
	/// order by year with format : (1399 - 1400)
	/// </summary>
	/// <param name="value"></param>
	/// <returns></returns>
	public static List<string>
		OrderByYear(this List<string> value)
	{
		List<string> lstOrderBy = new List<string>();

		var temp =
			value.Select(x => Convert.ToInt32(x.Split('-')[0].Trim()))
			.OrderBy(x => x).Select(date => date).ToList();

		foreach (var item in temp)
		{
			var itemOrder =
				value.FirstOrDefault(year => Convert.ToInt32(year.Split('-')[0].Trim()) == item);

			lstOrderBy.Add(item: itemOrder!);
		}

		return lstOrderBy;
	}
	//*********************************************

	//*********************************************
	/// <summary>
	/// state Date = 0 [29 آذر 1401]
	/// stateDate != 0 [1401/09/29]
	/// </summary>
	/// <param name="value">DateTime type (DateTime)</param>
	/// <returns>String Shamsi</returns>
	public static string
		ToShamsi(this DateTime value, int stateDate = 0)
	{
		try
		{
			System.Globalization.PersianCalendar pc = new();

			int year = pc.GetYear(value);
			string month = pc.GetMonth(value).ToString("00");
			string monthNumber = pc.GetMonth(value).ToString("00");
			string day = pc.GetDayOfMonth(value).ToString("00");

			switch (month)
			{
				case "01":
					{
						month = "فروردین";
						break;
					}
				case "02":
					{
						month = "اردیبهشت";
						break;
					}
				case "03":
					{
						month = "خرداد";
						break;
					}
				case "04":
					{
						month = "تیر";
						break;
					}
				case "05":
					{
						month = "مرداد";
						break;
					}
				case "06":
					{
						month = "شهریور";
						break;
					}
				case "07":
					{
						month = "مهر";
						break;
					}
				case "08":
					{
						month = "آبان";
						break;
					}
				case "09":
					{
						month = "آذر";
						break;
					}
				case "10":
					{
						month = "دی";
						break;
					}
				case "11":
					{
						month = "بهمن";
						break;
					}
				case "12":
					{
						month = "اسفند";
						break;
					}

				default:
					break;
			}

			string result = day + " " + month + " " + year;

			if (stateDate != 0)
			{
				result = $"{year}/{monthNumber}/{day}";
			}

			return result;
		}
		catch
		{
			return string.Empty;
		}
	}
	//*********************************************

	//*********************************************
	/// <summary>
	/// get;Big Year with format ( 1392 - 1393 )  
	/// </summary>
	/// <param name="ListYear">ListYear With Format ( 1392 - 1393 )</param>
	/// <returns>Search And Find Big Date</returns>
	public static string? GetBigYear(this List<string> ListYear)
	{
		string? year = string.Empty;

		int bigYear = ListYear
			.Select(x => Convert.ToInt32(x.Split('-')[0].Trim())).ToList().Max();

		year = ListYear
			.FirstOrDefault(x => Convert.ToInt32(x.Split('-')[0].Trim()) == bigYear);

		return year;
	}
	//*********************************************

	//*********************************************
	/// <summary>
	/// Change Date Miladi to Shamsi "1399/03/22" => 2020/12/12
	/// </summary>
	/// <param name="YourDate">String Date Time</param>
	/// <returns>date or null</returns>
	public static DateTime? StrigToDateTimeMiladi(this string? YourDate)
	{
		try
		{
			if (string.IsNullOrWhiteSpace(YourDate))
			{
				return null;
			}

			System.Globalization.PersianCalendar persianDate =
				new System.Globalization.PersianCalendar();

			int year = Convert.ToInt32(YourDate.Split('/')[0]),
				Month = Convert.ToInt32(YourDate.Split('/')[1]),
				day = Convert.ToInt32(YourDate.Split('/')[2]);

			DateTime date = persianDate.ToDateTime(year, Month, day, 0, 0, 0, 0);

			return date;
		}
		catch
		{
			return null;
		}
	}
	//*********************************************

	//*********************************************
	/// <summary>
	/// Change Date Miladi to Shamsi "1399/03/22" => 2020/12/12
	/// </summary>
	/// <param name="YourDate">String Date Time</param>
	/// <returns></returns>
	public static DateTime? StrigToDateTimeMiladi_Nullable(this string? YourDate)
	{
		if (string.IsNullOrWhiteSpace(YourDate))
		{
			return null;
		}

		System.Globalization.PersianCalendar persianDate =
			new System.Globalization.PersianCalendar();

		DateTime? date = null;

		try
		{
			int year = Convert.ToInt32(YourDate.Split('/')[0]),
			Month = Convert.ToInt32(YourDate.Split('/')[1]),
			day = Convert.ToInt32(YourDate.Split('/')[2]);
			date = persianDate.ToDateTime(year, Month, day, 0, 0, 0, 0);
		}
		catch
		{
			return null;
		}

		return date;
	}
	//*********************************************

	//*********************************************
	/// <summary>
	/// set date between (start date - end dete)
	/// </summary>
	/// <param name="StartDate">Start Date</param>
	/// <param name="EndDate">End Date</param>
	/// <param name="Dates">List For Search (Load In DataBase)</param>
	/// <returns></returns>
	public static List<string>
		GetDates(string? StartDate, string? EndDate, List<string> Dates)
	{
		List<string>? result = new List<string>();

		List<DateTime>? DatesShamsi =
			Dates
			.Where(dateString => dateString.StrigToDateTimeMiladi().HasValue == true)
			.Select(dateString => dateString.StrigToDateTimeMiladi()!.Value).ToList();

		List<DateTime>? searchDate =
			DatesShamsi
			.Where(date => date >= StartDate!.StrigToDateTimeMiladi()
			&& date <= EndDate!.StrigToDateTimeMiladi())
			.ToList();

		foreach (DateTime date in searchDate)
		{
			result.Add(Dates.Find(x => x.Contains(date.ToShamsi()))!);
		}

		return result;
	}
	//*********************************************

	//*********************************************
	public static DateTime
		NextDateTime(this Random rnd, DateTime dateTimeFrom, DateTime dateTimeTo)
	{
		//Calculate cumulative number of seconds between two DateTimes

		Int32 Days = (dateTimeTo - dateTimeFrom).Days * 60 * 60 * 24;
		Int32 Hours = (dateTimeTo - dateTimeFrom).Hours * 60 * 60;
		Int32 Minutes = (dateTimeTo - dateTimeFrom).Minutes * 60;
		Int32 Seconds = (dateTimeTo - dateTimeFrom).Seconds;

		int range = Days + Hours + Minutes + Seconds;

		//Add random number of seconds to dateTimeFrom
		Int32 NumberOfSecondsToAdd = rnd.Next(range);
		DateTime result = dateTimeFrom.AddSeconds(NumberOfSecondsToAdd);

		//Return the value
		return result;
	}
	//*********************************************

	/// <summary>
	/// if -> conflict -> true
	/// </summary>
	/// <param name="start"></param>
	/// <param name="end"></param>
	/// <returns>it's ok = true</returns>
	public static bool CheckConflictDate(DateTime start, DateTime? end)
	{
		if (end.HasValue == true && end <= start)
		{
			return false;
		}

		return true;
	}

	/// <summary>
	/// if -> conflict -> true
	/// </summary>
	/// <param name="start"></param>
	/// <param name="end"></param>
	/// <returns>it's ok = true</returns>
	public static bool CheckConflictDate(string start, string? end)
	{
		DateTime? startDate = StrigToDateTimeMiladi_Nullable(start);
		DateTime? EndDate = StrigToDateTimeMiladi_Nullable(end);

		if (startDate.HasValue == false)
		{
			return false;
		}

		if (EndDate.HasValue == true && EndDate <= startDate)
		{
			return false;
		}

		return true;
	}
}
