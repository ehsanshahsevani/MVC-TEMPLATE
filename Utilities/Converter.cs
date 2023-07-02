using System.Drawing;
using System.Drawing.Imaging;
using Microsoft.AspNetCore.Http;

namespace Utilities;

public static class Converter
{

	public static byte[] Base64ToByteArray(this string? base64)
	{
		if (string.IsNullOrEmpty(base64) == true)
		{
			return new byte[0];
		}

		byte[] result = Convert.FromBase64String(base64);
		return result;
	}

	// **************************************************
	/// <summary>
	/// byte array to file
	/// </summary>
	/// <param name="data"></param>
	/// <param name="filePath"></param>
	public static void SaveByteArrayToFile(this byte[] data, string filePath)
	{
		using var stream = File.Create(filePath);
		stream.Write(data, 0, data.Length);
	}
	// **************************************************

	/// <summary>
	/// (Hellper) get;Bytes Array
	/// </summary>
	/// <param name="file">IFormFile</param>
	/// <returns>Byte[file.Length]</returns>
	// **************************************************
	public static byte[] IFormFileToByte(this IFormFile file)
	{
		var buffer = new byte[file.Length];

		file.OpenReadStream().Read(buffer: buffer, 0, buffer.Length);

		return buffer;
	}
	// **************************************************

	/// <summary>
	/// (Hellper) get;Bytes Array
	/// </summary>
	/// <param name="file">image</param>
	/// <returns>Byte[file.Length]</returns>
	// **************************************************
	public static byte[] ImageToByte(string fullAddress)
	{
		byte[] arrayByte = System.IO.File.ReadAllBytes(fullAddress);

		return arrayByte;
	}
	// **************************************************


	// **************************************************
	/// <summary>
	/// (Hellper) get;Bytes
	/// </summary>
	/// <param name="file">byte[]</param>
	/// <returns>Image</returns>
	public static Image ByteArrayToImage(this byte[] data)
	{
		Image image;
		using (MemoryStream ms = new MemoryStream(data))
		{
			image = Image.FromStream(ms);
		}
		return image;
	}
	// **************************************************

	// **************************************************
	public static string SaveImageInDirectory
		(this Image image, string folderAddress, string fileName = null)
	{
		var name = $"{Guid.NewGuid()}.Jpeg";

		if (string.IsNullOrEmpty(fileName) == false)
		{
			name = fileName;
		}

		var fullName = Path.Combine(folderAddress, name);
		try
		{
			if (!Directory.Exists(folderAddress))
			{
				Directory.CreateDirectory(folderAddress);
			}

			image.Save(fullName, ImageFormat.Jpeg);
			return name;
		}
		catch (Exception)
		{

			// Try HU's method: Convert it to a Bitmap first
			image = new Bitmap(image);
			image.Save(fullName, ImageFormat.Jpeg); // This is always successful

			return name;
		}
	}
	// **************************************************

	// **************************************************
	/// <summary>
	/// (Hellper) get;image with jpeg format and get;tumbnail image
	/// </summary>
	/// <param name="imageByte">byteImages[]</param>
	/// <returns></returns>
	public static (byte[] Image, byte[] Thumbnail)
		ChangeFormatImageToJpegWithThumbnail(this byte[] imageByte)
	{
		MemoryStream stream = new MemoryStream(imageByte);
		Image image = Image.FromStream(stream);
		Bitmap bitmapImage = new Bitmap(image);
		Bitmap bitmapImageThumbnail = new Bitmap(image, image.Width / 3, image.Height / 3);

		MemoryStream saveImageStream = new MemoryStream();
		MemoryStream saveThumbnailStream = new MemoryStream();


		bitmapImage.Save(saveImageStream, ImageFormat.Jpeg);
		bitmapImageThumbnail.Save(saveThumbnailStream, ImageFormat.Jpeg);

		byte[] resultImage = saveImageStream.ToArray();
		byte[] resultThumbnail = saveThumbnailStream.ToArray();

		return (resultImage, resultThumbnail);
	}
	// **************************************************

	// **************************************************
	/// <summary>
	/// (Hellper) Save Video File In Directory (wwwroot/media/...)
	/// </summary>
	/// <param name="videoFile">IFormFile</param>
	/// <param name="dir">"Address"</param>
	/// <returns>if -> it's OK = True</returns>
	public async static Task<(bool IsSaved, string NameVideo)>
		SaveFileVideoInDirAsync(this IFormFile videoFile, string dir)
	{
		// error
		if (videoFile == null)
		{
			return (false, string.Empty);
		}

		if (videoFile.Length <= 0)
		{
			return (false, string.Empty);
		}

		string ext = Path.GetExtension(videoFile.FileName);
		var exts = new List<string> { ".mp4", ".mvk" };
		if (!exts.Contains(ext))
		{
			return (false, string.Empty);
		}

		if (string.IsNullOrEmpty(dir))
		{
			return (false, string.Empty);
		}

		if (!videoFile.IsValidFileSize(1024))
		{
			return (false, string.Empty);
		}

		// It's OK
		string fileName = Guid.NewGuid().ToString() + ext;
		using (var fileStream = new FileStream(Path.Combine(dir, fileName), FileMode.Create))
		{
			await videoFile.CopyToAsync(fileStream);
		}

		return (true, fileName);
	}
	// **************************************************

	// **************************************************
	/// <summary>
	/// (Hellper) Save Video File In Directory (wwwroot/media/...)
	/// </summary>
	/// <param name="ImageFile">IFormFile</param>
	/// <param name="dir">"Address"</param>
	/// <returns>if -> it's OK = True</returns>
	public async static Task<(bool IsSaved, string NameImage)>
		SaveFileImageInDirAsync(this IFormFile videoFile, string dir)
	{
		// error
		if (videoFile == null)
		{
			return (false, string.Empty);
		}

		if (videoFile.Length <= 0)
		{
			return (false, string.Empty);
		}

		string ext = Path.GetExtension(videoFile.FileName);
		var exts = new List<string> { ".jpeg", ".jpg", ".gif", ".bmp", ".png" };
		if (!exts.Contains(ext))
		{
			return (false, string.Empty);
		}

		if (string.IsNullOrEmpty(dir))
		{
			return (false, string.Empty);
		}

		if (!videoFile.IsValidFileSize(1024))
		{
			return (false, string.Empty);
		}

		// It's OK
		string fileName = Guid.NewGuid().ToString() + ext;
		using (var fileStream = new FileStream(Path.Combine(dir, fileName), FileMode.Create))
		{
			await videoFile.CopyToAsync(fileStream);
		}

		return (true, fileName);
	}
	// **************************************************


	// **************************************************
	/// <summary>
	/// (Hellper) Save Video File In Directory (wwwroot/media/...)
	/// </summary>
	/// <param name="videoFile">IFormFile</param>
	/// <param name="dir">"Address"</param>
	/// <returns>if -> it's OK = True</returns>
	public async static Task<(bool IsSaved, string FileName)>
		SaveFileInDirAsync(this IFormFile postFile, string dir)
	{
		// error
		if (postFile == null)
		{
			return (false, string.Empty);
		}

		if (postFile.Length <= 0)
		{
			return (false, string.Empty);
		}

		string ext = Path.GetExtension(postFile.FileName);
		var exts = new List<string> { ".pptx", ".pdf", ".docx" };
		if (!exts.Contains(ext))
		{
			return (false, string.Empty);
		}

		if (string.IsNullOrEmpty(dir))
		{
			return (false, string.Empty);
		}

		if (!postFile.IsValidFileSize(1024))
		{
			return (false, string.Empty);
		}

		// It's OK
		string fileName = Guid.NewGuid().ToString() + ext;
		using (var fileStream = new FileStream(Path.Combine(dir, fileName), FileMode.Create))
		{
			await postFile.CopyToAsync(fileStream);
		}

		return (true, fileName);
	}
	// **************************************************

	// **************************************************
	// Tupple
	public static (string NameImage, string NameImageThumbnail)
		IFormFileToDirectoryWithThumbnail(this IFormFile fileImg, string directoryPath)
	{
		byte[] imageBytes = fileImg.IFormFileToByte();
		var images = imageBytes.ChangeFormatImageToJpegWithThumbnail();

		var img = images.Image.ByteArrayToImage();
		var nameImg = img.SaveImageInDirectory(directoryPath);

		var imgTh = images.Thumbnail.ByteArrayToImage();
		var nameImgTh = imgTh.SaveImageInDirectory(directoryPath);

		return (nameImg, nameImgTh);
	}

	public static string
		IFormFileToDirectory(this IFormFile fileImg, string directoryPath, string fileName = null)
	{
		byte[] imageBytes = fileImg.IFormFileToByte();
		var images = imageBytes.ChangeFormatImageToJpegWithThumbnail();

		var img = images.Image.ByteArrayToImage();
		var nameImg = img.SaveImageInDirectory(directoryPath, fileName);

		return nameImg;
	}
	// **************************************************
}
