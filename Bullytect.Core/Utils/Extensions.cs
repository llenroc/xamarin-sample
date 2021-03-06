﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Bullytect.Core.Models.Domain;
using SkiaSharp;

namespace Bullytect.Utils.Helpers
{
    public static partial class Extensions
    {
		public static string Fmt(this string s, params object[] args)
		{
			return string.Format(s, args);
		}

		public static Dictionary<string, string> ToKeyValuePair(this string querystring)
		{
			return Regex.Matches(querystring, "([^?=&]+)(=([^&]*))?").Cast<Match>().ToDictionary(x => x.Groups[1].Value, x => x.Groups[3].Value);
		}

		public static string ToOrdinal(this DateTime date)
		{
			return "{0}, {1} {2}".Fmt(date.ToString("dddd"), date.ToString("MMM"), date.Day.ToOrdinal());
		}

		public static bool IsEmpty(this string s)
		{
			return string.IsNullOrWhiteSpace(s);
		}

		public static T ToEnum<T>(this string value)
		{
			return (T)Enum.Parse(typeof(T), value, true);
		}

        public static SKColor ToColor (this AlertLevelEnum level)
        {
            SKColor Color;

            switch (level)
            {
                case AlertLevelEnum.INFO:
                    Color = SKColor.Parse("#6781c0");
                    break;
                case AlertLevelEnum.WARNING:
                    Color = SKColor.Parse("#dbc66e");
                    break;
                case AlertLevelEnum.DANGER:
                    Color = SKColor.Parse("#dd6a6b");
                    break;
                case AlertLevelEnum.SUCCESS:
                    Color = SKColor.Parse("#6bdd9b");
                    break;
            }

            return Color;
        
        }

        public static string ToColorString(this AlertLevelEnum level)
        {
            string Color = string.Empty;

            switch (level)
            {
                case AlertLevelEnum.INFO:
                    Color = "#6781c0";
                    break;
                case AlertLevelEnum.WARNING:
                    Color = "#dbc66e";
                    break;
                case AlertLevelEnum.DANGER:
                    Color = "#dd6a6b";
                    break;
                case AlertLevelEnum.SUCCESS:
                    Color = "#6bdd9b";
                    break;
            }

            return Color;

        }

		public static string ToOrdinal(this int num)
		{
			if (num <= 0)
				return num.ToString();

			switch (num % 100)
			{
				case 11:
				case 12:
				case 13:
					return num + "th";
			}

			switch (num % 10)
			{
				case 1:
					return num + "st";
				case 2:
					return num + "nd";
				case 3:
					return num + "rd";
				default:
					return num + "th";
			}

		}


		public static void Track(this Exception ex)
		{
			var baseException = ex.GetBaseException();
			var stack = baseException.StackTrace;

			if (stack.Length > 101)
				stack = stack.Substring(0, 100);
		}


		public static void TrackToConsole(this string Data)
		{
            Debug.WriteLine(Data);
		}

		public static void TrackToConsole(this StringBuilder Data)
		{
			Debug.WriteLine(Data);
		}
    
    }
}
