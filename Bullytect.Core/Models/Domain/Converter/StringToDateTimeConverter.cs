using System;
using System.Diagnostics;
using AutoMapper;

namespace Bullytect.Core.Models.Domain.Converter
{
	public class StringToDateTimeConverter : ITypeConverter<string, DateTime>
	{
        
        public DateTime Convert(string source, DateTime destination, ResolutionContext context)
        {
            Debug.WriteLine("Convert String to DateTime ...");

			if (source == null)
			{
				return default(DateTime);
			}

			if (DateTime.TryParse(source, out destination))
			{
				return destination;
			}

			return default(DateTime);
        }
    }
}
