using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text.RegularExpressions;

namespace Nop.Core
{
	/// <summary>
	/// Created By Mohammad Dayyan, mds_soft@yahoo.com
	/// 1393/09/14
	/// </summary>
	public class PersianDateTime
	{
		#region ctor

		private PersianDateTime(DateTime dateTime, bool englishNumber)
		{
			if (dateTime <= DateTime.MinValue) return;
			_dateTime = dateTime;
			EnglishNumber = englishNumber;
		}

		public PersianDateTime(DateTime dateTime)
		{
			if (dateTime <= DateTime.MinValue) return;
			_dateTime = dateTime;
		}

		public PersianDateTime(DateTime? nullableDateTime)
		{
			if (!nullableDateTime.HasValue) return;
			_dateTime = nullableDateTime.Value;
		}

		public PersianDateTime(int year, int month, int day)
		{
			_dateTime = _persianCalendar.ToDateTime(year, month, day, 0, 0, 0, 0);
		}

		public PersianDateTime(int year, int month, int day, int hour, int minute, int second)
		{
			_dateTime = _persianCalendar.ToDateTime(year, month, day, hour, minute, second, 0);
		}

		public PersianDateTime(int year, int month, int day, int hour, int minute, int second, int miliSecond)
		{
			_dateTime = _persianCalendar.ToDateTime(year, month, day, hour, minute, second, miliSecond);
		}

		#endregion

		#region properties and fields

		readonly PersianCalendar _persianCalendar = new PersianCalendar();
		readonly DateTime _dateTime;

		public bool EnglishNumber { get; set; }

		/// <summary>
		/// سال
		/// </summary>
		public int Year
		{
			get
			{
				return _persianCalendar.GetYear(_dateTime);
			}
		}

		/// <summary>
		/// ماه
		/// </summary>
		public int Month
		{
			get
			{
				return _persianCalendar.GetMonth(_dateTime);
			}
		}

		/// <summary>
		/// نام فارسی ماه
		/// </summary>
		public string MonthName
		{
			get { return ((PersianDateTimeMonthEnum)Month).ToString(); }
		}

		/// <summary>
		/// روز ماه
		/// </summary>
		public int Day
		{
			get
			{
				return _persianCalendar.GetDayOfMonth(_dateTime);
			}
		}

		/// <summary>
		/// روز هفته
		/// </summary>
		public DayOfWeek DayOfWeek
		{
			get
			{
				return _persianCalendar.GetDayOfWeek(_dateTime);
			}
		}

		public int Hour
		{
			get
			{
				return _persianCalendar.GetHour(_dateTime);
			}
		}
		public int ShortHour
		{
			get
			{
				int shortHour;
				if (Hour > 12)
					shortHour = Hour - 12;
				else
					shortHour = Hour;
				return shortHour;
			}
		}

		public int Minute
		{
			get { return _persianCalendar.GetMinute(_dateTime); }
		}

		public int Second
		{
			get { return _persianCalendar.GetSecond(_dateTime); }
		}

		public int MiliSecond
		{
			get { return (int)_persianCalendar.GetMilliseconds(_dateTime); }
		}

		/// <summary>
		/// تعداد روز در ماه
		/// </summary>
		public int GetMonthDays
		{
			get
			{
				int days;
				switch (this.Month)
				{
					case 1:
					case 2:
					case 3:
					case 4:
					case 5:
					case 6:
						days = 31;
						break;

					case 7:
					case 8:
					case 9:
					case 10:
					case 11:
						days = 30;
						break;

					case 12:
						{
							if (IsLeapYear) days = 30;
							else days = 29;
							break;
						}

					default:
						throw new Exception("Month number is wrong !!!");

				}
				return days;
			}
		}

		/// <summary>
		/// هفته چندم سال
		/// </summary>
		public int GetWeekOfYear
		{
			get
			{
				return _persianCalendar.GetWeekOfYear(_dateTime, CalendarWeekRule.FirstDay, DayOfWeek.Saturday);
			}
		}

		/// <summary>
		/// هفته چندم ماه
		/// </summary>
		public int GetWeekOfMonth
		{
			get
			{
				double weekNumberIsMonth = GetMonthDays / 7.0;

				if (weekNumberIsMonth <= 1)
					weekNumberIsMonth = 1;
				else if (weekNumberIsMonth <= 2)
					weekNumberIsMonth = 2;
				else if (weekNumberIsMonth <= 3)
					weekNumberIsMonth = 3;
				else if (weekNumberIsMonth <= 4)
					weekNumberIsMonth = 4;
				else 
					weekNumberIsMonth = 5;

				return (int)weekNumberIsMonth;
			}
		}

		/// <summary>
		/// روز چندم ماه
		/// </summary>
		public int GetDayOfYear
		{
			get
			{
				return _persianCalendar.GetDayOfYear(_dateTime);
			}
		}

		/// <summary>
		/// آیا سال کبیسه است؟
		/// </summary>
		public bool IsLeapYear
		{
			get { return _persianCalendar.IsLeapYear(this.Year); }
		}

		private AmPmEnum PersianAmPm
		{
			get
			{
				return _dateTime.ToString("tt") == "PM" ? AmPmEnum.PM : AmPmEnum.AM;
			}
		}

		public string GetPersianAmPm
		{
			get
			{
				string result = string.Empty;
				switch (PersianAmPm)
				{
					case AmPmEnum.AM:
						result = "ق.ظ";
						break;

					case AmPmEnum.PM:
						result = "ب.ظ";
						break;
				}
				return result;
			}
		}

		/// <summary>
		/// آذر
		/// </summary>
		public string GetLongMonthName
		{
			get
			{
				string monthName = null;
				switch (Month)
				{
					case 1:
						monthName = "فروردین";
						break;

					case 2:
						monthName = "اردیبهشت";
						break;

					case 3:
						monthName = "خرداد";
						break;

					case 4:
						monthName = "تیر";
						break;

					case 5:
						monthName = "مرداد";
						break;

					case 6:
						monthName = "شهریور";
						break;

					case 7:
						monthName = "مهر";
						break;

					case 8:
						monthName = "آبان";
						break;

					case 9:
						monthName = "آذر";
						break;

					case 10:
						monthName = "دی";
						break;

					case 11:
						monthName = "بهمن";
						break;

					case 12:
						monthName = "اسفند";
						break;
				}

				return monthName;
			}
		}

		/// <summary>
		/// 93
		/// </summary>
		public int GetShortYear
		{
			get { return Year / 100; }
		}

		/// <summary>
		/// جمعه
		/// </summary>
		public string GetLongDayOfWeekName
		{
			get
			{
				string weekDayName = null;
				switch (DayOfWeek)
				{
					case DayOfWeek.Sunday:
						weekDayName = PersianWeekDaysStruct.یکشنبه.Value;
						break;

					case DayOfWeek.Monday:
						weekDayName = PersianWeekDaysStruct.دوشنبه.Value;
						break;

					case DayOfWeek.Tuesday:
						weekDayName = PersianWeekDaysStruct.سه_شنبه.Value;
						break;

					case DayOfWeek.Wednesday:
						weekDayName = PersianWeekDaysStruct.چهارشنبه.Value;
						break;

					case DayOfWeek.Thursday:
						weekDayName = PersianWeekDaysStruct.پنج_شنبه.Value;
						break;

					case DayOfWeek.Friday:
						weekDayName = PersianWeekDaysStruct.جمعه.Value;
						break;

					case DayOfWeek.Saturday:
						weekDayName = PersianWeekDaysStruct.شنبه.Value;
						break;
				}
				return weekDayName;
			}
		}

		/// <summary>
		/// ج
		/// </summary>
		public string GetShortDayOfWeekName
		{
			get
			{
				string weekDayName = null;
				switch (DayOfWeek)
				{
					case DayOfWeek.Sunday:
						weekDayName = "ی";
						break;

					case DayOfWeek.Monday:
						weekDayName = "د";
						break;

					case DayOfWeek.Tuesday:
						weekDayName = "س";
						break;

					case DayOfWeek.Wednesday:
						weekDayName = "چ";
						break;

					case DayOfWeek.Thursday:
						weekDayName = "پ";
						break;

					case DayOfWeek.Friday:
						weekDayName = "ج";
						break;

					case DayOfWeek.Saturday:
						weekDayName = "ش";
						break;
				}

				return weekDayName;
			}
		}

		public static PersianDateTime Now
		{
			get
			{
				return new PersianDateTime(DateTime.Now);
			}
		}

		public static PersianDateTime Today
		{
			get
			{
				return new PersianDateTime(DateTime.Today);
			}
		}

		/// <summary>
		/// 13:47:40:530
		/// </summary>
		public string TimeOfDay
		{
			get
			{
				if (_dateTime <= DateTime.MinValue) return null;
				string result = string.Format("{0:00}:{1:00}:{2:00}:{3:000}", Hour, Minute, Second, MiliSecond);
				if (EnglishNumber) return result;
				return ToPersianNumber(result);
			}
		}

		/// <summary>
		/// ساعت 01:47:40:530 ب.ظ
		/// </summary>
		public string LongTimeOfDay
		{
			get
			{
				if (_dateTime <= DateTime.MinValue) return null;
				string result = string.Format("ساعت {0:00}:{1:00}:{2:00}:{3:000} {4}", ShortHour, Minute, Second, MiliSecond, GetPersianAmPm);
				if (EnglishNumber) return result;
				return ToPersianNumber(result);
			}
		}

		/// <summary>
		/// 01:47:40 ب.ظ
		/// </summary>
		public string ShortTimeOfDay
		{
			get
			{
				if (_dateTime <= DateTime.MinValue) return null;
				string result = string.Format("{0:00}:{1:00}:{2:00} {3}", ShortHour, Minute, Second, GetPersianAmPm);
				if (EnglishNumber) return result;
				return ToPersianNumber(result);
			}
		}

		#endregion

		#region Types

		enum AmPmEnum
		{
			AM = 0,
			PM = 1,
			None = 2,
		}

		enum PersianDateTimeMonthEnum
		{
			فروردین = 1,
			اردیبهشت = 2,
			خرداد = 3,
			تیر = 4,
			مرداد = 5,
			شهریور = 6,
			مهر = 7,
			آبان = 8,
			آذر = 9,
			دی = 10,
			بهمن = 11,
			اسفند = 12,
		}

		struct PersianWeekDaysStruct
		{
			public static KeyValuePair<int, string> شنبه
			{
				get { return new KeyValuePair<int, string>((int)DayOfWeek.Saturday, "شنبه"); }
			}

			public static KeyValuePair<int, string> یکشنبه
			{
				get { return new KeyValuePair<int, string>((int)DayOfWeek.Sunday, "یکشنبه"); }
			}

			public static KeyValuePair<int, string> دوشنبه
			{
				get { return new KeyValuePair<int, string>((int)DayOfWeek.Monday, "دوشنبه"); }
			}

			public static KeyValuePair<int, string> سه_شنبه
			{
				get { return new KeyValuePair<int, string>((int)DayOfWeek.Tuesday, "سه شنبه"); }
			}

			public static KeyValuePair<int, string> چهارشنبه
			{
				get { return new KeyValuePair<int, string>((int)DayOfWeek.Thursday, "چهارشنبه"); }
			}

			public static KeyValuePair<int, string> پنج_شنبه
			{
				get { return new KeyValuePair<int, string>((int)DayOfWeek.Wednesday, "پنج شنبه"); }
			}

			public static KeyValuePair<int, string> جمعه
			{
				get { return new KeyValuePair<int, string>((int)DayOfWeek.Friday, "جمعه"); }
			}
		}

		#endregion

		#region override

		/// <summary>
		/// 1393/09/14   13:49:40
		/// </summary>
		public override string ToString()
		{
			if (_dateTime <= DateTime.MinValue) return null;
			string result = string.Format("{0:0000}/{1:00}/{2:00}   {3:00}:{4:00}:{5:00}", Year, Month, Day, Hour, Minute, Second);
			if (EnglishNumber) return result;
			return ToPersianNumber(result);
		}

		public override bool Equals(object obj)
		{
			if (!(obj is PersianDateTime)) return false;
			PersianDateTime persianDateTime = (PersianDateTime)obj;
			return _dateTime == persianDateTime.ToDateTime();
		}

		public override int GetHashCode()
		{
			return _dateTime.GetHashCode();
		}

		#region operators

		public static implicit operator DateTime(PersianDateTime persiandateTime)
		{
			return persiandateTime.ToDateTime();
		}

		public static bool operator ==(PersianDateTime persianDateTime1, PersianDateTime persianDateTime2)
		{
			return persianDateTime1 != null && persianDateTime1.Equals(persianDateTime2);
		}

		public static bool operator !=(PersianDateTime persianDateTime1, PersianDateTime persianDateTime2)
		{
			return persianDateTime1 != null && !persianDateTime1.Equals(persianDateTime2);
		}

		public static bool operator >(PersianDateTime persianDateTime1, PersianDateTime persianDateTime2)
		{
			return persianDateTime1.ToDateTime() > persianDateTime2.ToDateTime();
		}

		public static bool operator <(PersianDateTime persianDateTime1, PersianDateTime persianDateTime2)
		{
			return persianDateTime1.ToDateTime() < persianDateTime2.ToDateTime();
		}

		public static bool operator >=(PersianDateTime persianDateTime1, PersianDateTime persianDateTime2)
		{
			return persianDateTime1.ToDateTime() >= persianDateTime2.ToDateTime();
		}

		public static bool operator <=(PersianDateTime persianDateTime1, PersianDateTime persianDateTime2)
		{
			return persianDateTime1.ToDateTime() <= persianDateTime2.ToDateTime();
		}

		public static PersianDateTime operator +(PersianDateTime persiandateTime1, TimeSpan timeSpanToAdd)
		{
			DateTime dateTime1 = persiandateTime1;
			return new PersianDateTime(dateTime1.Add(timeSpanToAdd));
		}

		public static TimeSpan operator -(PersianDateTime persiandateTime1, PersianDateTime persiandateTime2)
		{
			DateTime dateTime1 = persiandateTime1;
			DateTime dateTime2 = persiandateTime2;
			return dateTime2 - dateTime1;
		}

		#endregion

		#endregion

		#region methods


		/// <summary>
		/// پارس کردن رشته و تبدیل به نوع PersianDateTime
		/// </summary>
		public static PersianDateTime Parse(string persianDateTimeInString)
		{
			string month = "", year, day,
				hour = "0",
				minute = "0",
				second = "0",
				miliSecond = "0";

			AmPmEnum amPmEnum = AmPmEnum.None;

			bool containSlash = Regex.IsMatch(persianDateTimeInString, @"/|\/");

			persianDateTimeInString = ToEnglishNumber(persianDateTimeInString.Replace("&nbsp;", " ").Replace(" ", "-").Replace("\\", "-").Replace("/", "-"));
			persianDateTimeInString = persianDateTimeInString.Replace("ك", "ک").Replace("ي", "ی");

			persianDateTimeInString = string.Format("-{0}-", persianDateTimeInString);

			// بدست آوردن ب.ظ یا ق.ظ
			if (persianDateTimeInString.Contains("ق.ظ"))
				amPmEnum = AmPmEnum.AM;
			else if (persianDateTimeInString.Contains("ب.ظ"))
				amPmEnum = AmPmEnum.PM;

			if (persianDateTimeInString.Contains(":")) // رشته ورودی شامل ساعت نیز هست
			{
				persianDateTimeInString = Regex.Replace(persianDateTimeInString, @"-*:-*", ":");
				hour = Regex.Match(persianDateTimeInString, @"(?<=-)\d{1,2}(?=:)", RegexOptions.IgnoreCase).Value;
				minute = Regex.Match(persianDateTimeInString, @"(?<=-\d{1,2}:)\d{1,2}(?=:?)", RegexOptions.IgnoreCase).Value;
				if (persianDateTimeInString.IndexOf(':') != persianDateTimeInString.LastIndexOf(':'))
				{
					second = Regex.Match(persianDateTimeInString, @"(?<=-\d{1,2}:\d{1,2}:)\d{1,2}(?=(\d{1,2})?)", RegexOptions.IgnoreCase).Value;
					miliSecond = Regex.Match(persianDateTimeInString, @"(?<=-\d{1,2}:\d{1,2}:\d{1,2}:)\d{1,2}(?=(\d{1,2})?)", RegexOptions.IgnoreCase).Value;
					if (string.IsNullOrEmpty(miliSecond)) miliSecond = "0";
				}
			}

			if (containSlash)
			{
				// بدست آوردن ماه
				month = Regex.Match(persianDateTimeInString, @"(?<=\d{2,4}-)\d{1,2}(?=-\d{1,2}[^:])", RegexOptions.IgnoreCase).Value;

				// بدست آوردن روز
				day = Regex.Match(persianDateTimeInString, @"(?<=\d{2,4}-\d{1,2}-)\d{1,2}(?=-)", RegexOptions.IgnoreCase).Value;

				// بدست آوردن سال
				year = Regex.Match(persianDateTimeInString, @"(?<=-)\d{2,4}(?=-\d{1,2}-\d{1,2})", RegexOptions.IgnoreCase).Value;
			}
			else
			{
				foreach (PersianDateTimeMonthEnum item in Enum.GetValues(typeof(PersianDateTimeMonthEnum)))
				{
					string itemValueInString = item.ToString();
					if (!persianDateTimeInString.Contains(itemValueInString)) continue;
					month = ((int)item).ToString();
					break;
				}

				if (string.IsNullOrEmpty(month))
					throw new Exception("عدد یا حرف ماه در رشته ورودی وجود ندارد");

				// بدست آوردن روز
				day = Regex.Match(persianDateTimeInString, @"(?<=-)\d{1,2}(?=-)", RegexOptions.IgnoreCase).Value;

				// بدست آوردن سال
				if (Regex.IsMatch(persianDateTimeInString, @"(?<=-)\d{4}(?=-)"))
					year = Regex.Match(persianDateTimeInString, @"(?<=-)\d{4}(?=-)", RegexOptions.Singleline | RegexOptions.IgnoreCase | RegexOptions.Multiline | RegexOptions.IgnorePatternWhitespace).Value;
				else
					year = Regex.Match(persianDateTimeInString, @"(?<=-)\d{2,4}(?=-)", RegexOptions.Singleline | RegexOptions.IgnoreCase | RegexOptions.Multiline | RegexOptions.IgnorePatternWhitespace).Value;
			}

			if (year.Length <= 2 && year[0] == '9') year = string.Format("13{0}", year);
			else if (year.Length <= 2) year = string.Format("14{0}", year);

			int numericYear = int.Parse(year);
			int numericMonth = int.Parse(month);
			int numericDay = int.Parse(day);
			int numericHour = int.Parse(hour);
			int numericMinute = int.Parse(minute);
			int numericSecond = int.Parse(second);
			int numericMiliSecond = int.Parse(miliSecond);

			switch (amPmEnum)
			{
				case AmPmEnum.PM:
					if (numericHour < 12)
						numericHour = numericHour + 12;
					break;
				case AmPmEnum.AM:
				case AmPmEnum.None:
					break;
			}

			return new PersianDateTime(numericYear, numericMonth, numericDay, numericHour, numericMinute, numericSecond, numericMiliSecond);
		}
		public static bool TryParse(string persianDateTimeInString, out PersianDateTime result)
		{
			if (string.IsNullOrWhiteSpace(persianDateTimeInString))
			{
				result = null;
				return false;
			}
			try
			{
				result = Parse(persianDateTimeInString);
				return true;
			}
			catch
			{
				result = null;
				return false;
			}
		}

		/// <summary>
		/// پارس کردن عددی در فرمت تاریخ شمسی، همانند 13920305
		/// </summary>
		public static PersianDateTime Parse(int numericPersianDate)
		{
			if (numericPersianDate.ToString().Length != 8)
				throw new InvalidCastException("Numeric persian date time must have a format like 13920101.");
			int year = numericPersianDate / 10000;
			int day = numericPersianDate % 100;
			int month = (numericPersianDate / 100) % 100;
			return new PersianDateTime(year, month, day);
		}
		/// <summary>
		/// پارس کردن عددی در فرمت تاریخ شمسی، همانند 13920305
		/// </summary>
		public static bool TryParse(int numericPersianDate, out PersianDateTime result)
		{
			try
			{
				result = Parse(numericPersianDate);
				return true;
			}
			catch
			{
				result = null;
				return false;
			}
		}

		static readonly List<string> GregorianWeekDayNames = new List<string> { "monday", "tuesday", "wednesday", "thursday", "friday", "saturday", "sunday" };
		static readonly List<string> GregorianMonthNames = new List<string> { "january", "february", "march", "april", "may", "june", "july", "august", "september", "october", "november", "december" };
		static readonly List<string> PmAm = new List<string> { "pm", "am" };

		/// <summary>
		/// فرمت های زیر پشتیبانی می شوند
		/// yyyy: سال چهار رقمی
		/// yy: سال دو رقمی
		/// MMMM: نام فارسی ماه
		/// MM: عدد دو رقمی ماه
		/// M: عدد یک رقمی ماه
		/// dddd: نام فارسی روز هفته
		/// dd: عدد دو رقمی روز ماه
		/// d: عدد یک رقمی روز ماه
		/// HH: ساعت دو رقمی با فرمت 00 تا 24
		/// H: ساعت یک رقمی با فرمت 0 تا 24
		/// hh: ساعت دو رقمی با فرمت 00 تا 12
		/// h: ساعت یک رقمی با فرمت 0 تا 12
		/// mm: عدد دو رقمی دقیقه
		/// m: عدد یک رقمی دقیقه
		/// ss: ثانیه دو رقمی
		/// s: ثانیه یک رقمی
		/// fff: میلی ثانیه 3 رقمی
		/// ff: میلی ثانیه 2 رقمی
		/// f: میلی ثانیه یک رقمی
		/// tt: ب.ظ یا ق.ظ
		/// t: حرف اول از ب.ظ یا ق.ظ
		/// </summary>
		public string ToString(string format)
		{
			if (_dateTime <= DateTime.MinValue) return null;

			string dateTimeString = format.Trim();

			dateTimeString = dateTimeString.Replace("yyyy", Year.ToString(CultureInfo.InvariantCulture));
			dateTimeString = dateTimeString.Replace("yy", GetShortYear.ToString("00", CultureInfo.InvariantCulture));
			dateTimeString = dateTimeString.Replace("MMMM", MonthName);
			dateTimeString = dateTimeString.Replace("MM", Month.ToString("00", CultureInfo.InvariantCulture));
			dateTimeString = dateTimeString.Replace("M", Month.ToString(CultureInfo.InvariantCulture));
			dateTimeString = dateTimeString.Replace("dddd", GetLongDayOfWeekName);
			dateTimeString = dateTimeString.Replace("dd", Day.ToString("00", CultureInfo.InvariantCulture));
			dateTimeString = dateTimeString.Replace("d", Day.ToString(CultureInfo.InvariantCulture));
			dateTimeString = dateTimeString.Replace("HH", Hour.ToString("00", CultureInfo.InvariantCulture));
			dateTimeString = dateTimeString.Replace("H", Hour.ToString(CultureInfo.InvariantCulture));
			dateTimeString = dateTimeString.Replace("hh", ShortHour.ToString("00", CultureInfo.InvariantCulture));
			dateTimeString = dateTimeString.Replace("h", ShortHour.ToString(CultureInfo.InvariantCulture));
			dateTimeString = dateTimeString.Replace("mm", Minute.ToString("00", CultureInfo.InvariantCulture));
			dateTimeString = dateTimeString.Replace("m", Minute.ToString(CultureInfo.InvariantCulture));
			dateTimeString = dateTimeString.Replace("ss", Second.ToString("00", CultureInfo.InvariantCulture));
			dateTimeString = dateTimeString.Replace("s", Second.ToString(CultureInfo.InvariantCulture));
			dateTimeString = dateTimeString.Replace("fff", MiliSecond.ToString("000", CultureInfo.InvariantCulture));
			dateTimeString = dateTimeString.Replace("ff", (MiliSecond / 10).ToString("00", CultureInfo.InvariantCulture));
			dateTimeString = dateTimeString.Replace("f", (MiliSecond / 100).ToString(CultureInfo.InvariantCulture));
			dateTimeString = dateTimeString.Replace("tt", GetPersianAmPm);
			dateTimeString = dateTimeString.Replace("t", GetPersianAmPm[0].ToString(CultureInfo.InvariantCulture));

			if (!EnglishNumber)
				dateTimeString = ToPersianNumber(dateTimeString);

			return dateTimeString;
		}

		public static bool IsChristianDate(string inputString)
		{
			inputString = inputString.ToLower();
			bool result;

			foreach (string gregorianWeekDayName in GregorianWeekDayNames)
			{
				result = inputString.Contains(gregorianWeekDayName);
				if (result) return true;
			}

			foreach (string gregorianMonthName in GregorianMonthNames)
			{
				result = inputString.Contains(gregorianMonthName);
				if (result) return true;
			}

			foreach (string item in PmAm)
			{
				result = inputString.Contains(item);
				if (result) return true;
			}

			result = Regex.IsMatch(inputString, @"(1[8-9]|[2-9][0-9])\d{2}", RegexOptions.IgnoreCase);

			return result;
		}

		public static bool IsSqlDateTime(DateTime dateTime)
		{
			DateTime minSqlDateTimeValue = new DateTime(1753, 1, 1);
			return dateTime >= minSqlDateTimeValue;
		}

		/// <summary>
		/// تبدیل نام ماه شمسی به عدد معادل آن
		/// به طور مثال آذر را به 9 تبدیل می کند
		/// </summary>
		public int GetMonthEnum(string longMonthName)
		{
			PersianDateTimeMonthEnum monthEnum = (PersianDateTimeMonthEnum)Enum.Parse(typeof(PersianDateTimeMonthEnum), longMonthName);
			return (int)monthEnum;
		}

		/// <summary>
		/// 1393/09/14
		/// </summary>
		public string ToShortDateString()
		{
			if (_dateTime <= DateTime.MinValue) return null;
			string result = string.Format("{0:0000}/{1:00}/{2:00}", Year, Month, Day);
			if (EnglishNumber) return result;
			return ToPersianNumber(result);
		}

		/// <summary>
		/// ج 14 آذر 93
		/// </summary>
		public string ToShortDate1String()
		{
			if (_dateTime <= DateTime.MinValue) return null;
			string result = string.Format("{0} {1:00} {2} {3}", GetShortDayOfWeekName, Day, GetLongMonthName, GetShortYear);
			if (EnglishNumber) return result;
			return ToPersianNumber(result);
		}

		/// <summary>
		/// 13930914
		/// </summary>
		public int ToShortDateInt()
		{
			string result = string.Format("{0:0000}{1:00}{2:00}", Year, Month, Day);
			return int.Parse(result);
		}

		/// <summary>
		/// جمعه، 14 آذر 1393
		/// </summary>
		public string ToLongDateString()
		{
			if (_dateTime <= DateTime.MinValue) return null;
			string result = string.Format("{0}، {1:00} {2} {3:0000}", GetLongDayOfWeekName, Day, GetLongMonthName, Year);
			if (EnglishNumber) return result;
			return ToPersianNumber(result);
		}

		/// <summary>
		/// جمعه، 14 آذر 1393 ساعت 13:50:27
		/// </summary>
		public string ToLongDateTimeString()
		{
			if (_dateTime <= DateTime.MinValue) return null;
			string result = string.Format("{0}، {1:00} {2} {3:0000} ساعت {4:00}:{5:00}:{6:00}", GetLongDayOfWeekName, Day, GetLongMonthName, Year, Hour, Minute, Second);
			if (EnglishNumber) return result;
			return ToPersianNumber(result);
		}

		/// <summary>
		/// جمعه، 14 آذر 1393 13:50
		/// </summary>
		public string ToShortDateTimeString()
		{
			if (_dateTime <= DateTime.MinValue) return null;
			string result = string.Format("{0}، {1:00} {2} {3:0000} {4:00}:{5:00}", GetLongDayOfWeekName, Day, GetLongMonthName, Year, Hour, Minute);
			if (EnglishNumber) return result;
			return ToPersianNumber(result);
		}

		/// <summary>
		/// 01:50 ب.ظ
		/// </summary>
		public string ToShortTimeString()
		{
			if (_dateTime <= DateTime.MinValue) return null;
			string result = string.Format("{0:00}:{1:00} {2}", ShortHour, Minute, GetPersianAmPm);
			if (EnglishNumber) return result;
			return ToPersianNumber(result);
		}

		/// <summary>
		/// 13:50:20
		/// </summary>
		public string ToLongTimeString()
		{
			if (_dateTime <= DateTime.MinValue) return null;
			string result = string.Format("{0:00}:{1:00}:{2:00}", Hour, Minute, Second);
			if (EnglishNumber) return result;
			return ToPersianNumber(result);
		}

		/// <summary>
		/// 1 دقیقه قبل
		/// </summary>
		public string ElapsedTime()
		{
			if (_dateTime <= DateTime.MinValue) return null;

			PersianDateTime persianDateTimeNow = new PersianDateTime(DateTime.Now);
			TimeSpan timeSpan = this - persianDateTimeNow;
			if (timeSpan.TotalDays > 90)
				return this.ToShortDateTimeString();

			string result = string.Empty;
			if (timeSpan.TotalDays > 30)
			{
				double month = timeSpan.TotalDays / 30;
				if (month > 0)
					result = string.Format("{0:0} ماه قبل", month);
			}
			else if (timeSpan.TotalDays >= 1)
			{
				result = string.Format("{0:0} روز قبل", timeSpan.TotalDays);
			}
			else if (timeSpan.TotalHours >= 1)
			{
				result = string.Format("{0:0} ساعت قبل", timeSpan.TotalHours);
			}
			else
			{
				double minute = timeSpan.TotalMinutes;
				if (minute <= 1) minute = 1;
				result = string.Format("{0:0} دقیقه قبل", minute);
			}
			if (EnglishNumber) return result;
			return ToPersianNumber(result);
		}

		public DateTime ToDateTime()
		{
			return _dateTime;
			//return _persianCalendar.ToDateTime(Year, Month, Day, Hour, Minute, Second, MiliSecond);
		}

		public PersianDateTime Add(TimeSpan timeSpan)
		{
			return new PersianDateTime(_dateTime.Add(timeSpan), EnglishNumber);
		}

		public PersianDateTime AddYears(int years)
		{
			return new PersianDateTime(_dateTime.AddYears(years), EnglishNumber);
		}

		public PersianDateTime AddDays(int days)
		{
			return new PersianDateTime(_dateTime.AddDays(days), EnglishNumber);
		}

		public PersianDateTime AddMonths(int months)
		{
			return new PersianDateTime(_dateTime.AddMonths(months), EnglishNumber);
		}

		public PersianDateTime AddHours(int hours)
		{
			return new PersianDateTime(_dateTime.AddHours(hours), EnglishNumber);
		}

		public PersianDateTime AddMinutes(int minuts)
		{
			return new PersianDateTime(_dateTime.AddMinutes(minuts), EnglishNumber);
		}

		public PersianDateTime AddSeconds(int seconds)
		{
			return new PersianDateTime(_dateTime.AddSeconds(seconds), EnglishNumber);
		}

		public PersianDateTime AddMilliseconds(int miliseconds)
		{
			return new PersianDateTime(_dateTime.AddMilliseconds(miliseconds), EnglishNumber);
		}

		static string ToPersianNumber(string input)
		{
			if (string.IsNullOrEmpty(input)) return null;
			input = input.Replace("ي", "ی").Replace("ك", "ک");

			//۰ ۱ ۲ ۳ ۴ ۵ ۶ ۷ ۸ ۹
			return
				input
					.Replace("0", "۰")
					.Replace("1", "۱")
					.Replace("2", "۲")
					.Replace("3", "۳")
					.Replace("4", "۴")
					.Replace("5", "۵")
					.Replace("6", "۶")
					.Replace("7", "۷")
					.Replace("8", "۸")
					.Replace("9", "۹");
		}
		static string ToEnglishNumber(string input)
		{
			if (string.IsNullOrEmpty(input)) return null;
			input = input.Replace("ي", "ی").Replace("ك", "ک");

			//۰ ۱ ۲ ۳ ۴ ۵ ۶ ۷ ۸ ۹
			return input
				.Replace(",", "")
				.Replace("۰", "0")
				.Replace("۱", "1")
				.Replace("۲", "2")
				.Replace("۳", "3")
				.Replace("۴", "4")
				.Replace("۵", "5")
				.Replace("۶", "6")
				.Replace("۷", "7")
				.Replace("۸", "8")
				.Replace("۹", "9");
		}

		#endregion
	}
}