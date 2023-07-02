using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
namespace Utilities;

public static class Validations
{
	// **************************************************
	public static bool IsValidImage(this IFormFile image, long MB = 2)
	{
		var volume = Math.Pow(1024, 2) * MB;
		var Extensions = new List<string> { ".jpg", ".img", ".png", ".jpeg", ".gif" };

		if (image != null)
		{
			var ext = System.IO.Path.GetExtension(image.FileName);
			if (image.Length <= volume && Extensions.Contains(ext.ToLower()))
				return true;
		}
		return false;
	}
	// **************************************************

	// **************************************************
	public static bool IsValidFileSize(this IFormFile file, long MB = 2)
	{
		var volume = Math.Pow(1024, 2) * MB;
		return (file.Length <= volume) ? true : false;
	}
	// **************************************************

	// **************************************************
	public static bool IsValidSize(long max, double MB = 2)
	{
		var volume = Math.Pow(1024, 2) * MB;
		return (max <= volume) ? true : false;
	}
	// **************************************************
}
