using Domain;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using System.Linq;

namespace Infrastructure.JWT;

public static class JwtUtility
{
	static JwtUtility()
	{
	}

	#region Create Token JWT
	public static string GenerateJwtToken
		(Domain.User user, ApplicationSettings.JwtSettings settings)
	{
		// string[] roles = user.Roles
		byte[] key =
			System.Text.Encoding.ASCII.GetBytes(settings.SecretKey!);

		var symmetricSecurityKey =
			new Microsoft.IdentityModel.Tokens.SymmetricSecurityKey(key: key);

		var securityAlgoritm =
			Microsoft.IdentityModel.Tokens.SecurityAlgorithms.HmacSha256Signature;

		var signingCredentials =
			new Microsoft.IdentityModel.Tokens
			.SigningCredentials(key: symmetricSecurityKey, securityAlgoritm);

		var tokenDescriptor =
			new Microsoft.IdentityModel.Tokens.SecurityTokenDescriptor
			{
				Subject = new System.Security.Claims
					.ClaimsIdentity(new[]
					{
						//new System.Security.Claims.Claim
						//    (type: "firstname", value: string.IsNullOrWhiteSpace(user.FirstName) ? "": user.FirstName),

						//new System.Security.Claims.Claim
						//    (type: "lastname", value: string.IsNullOrWhiteSpace(user.LastName) ? "": user.LastName),
							
							//new System.Security.Claims.Claim
							//	(type: System.Security.Claims.ClaimTypes.Name, value: user.UserName),

							//new System.Security.Claims.Claim
							//	(type: System.Security.Claims.ClaimTypes.Email, value: user.Email),

						new System.Security.Claims.Claim
							(type: System.Security.Claims.ClaimTypes.NameIdentifier, value: user.Id.ToString()),
				}),

				Expires =
					System.DateTime.UtcNow.AddMinutes(value: settings.TokenExpiresInMinutes),

				SigningCredentials = signingCredentials
			};

		// *********************************************
		//if (attachment is not null)
		//{
		//	tokenDescriptor.Subject.AddClaim
		//		(new(type: "url", value: attachment.Url ?? string.Empty));

		//	tokenDescriptor.Subject.AddClaim(
		//		new(type: "filename", value: attachment.FileName));
		//}
		// *********************************************

		// *********************************************
		// add multiple roles => role: []
		//if (roleNames != null && roleNames.Count > 0)
		//{
		//	foreach (var item in roleNames)
		//	{
		//		tokenDescriptor
		//			.Subject.AddClaim
		//			(new System.Security.Claims
		//			.Claim(type: System.Security.Claims.ClaimTypes.Role, value: item));
		//	}
		//}
		// *********************************************

		// *********************************************
		// add multiple pages => page: []
		//if (pageNames != null && pageNames.Count > 0)
		//{
		//	foreach (var item in pageNames)
		//	{
		//		tokenDescriptor
		//			.Subject
		//			.AddClaim(new System.Security.Claims
		//			.Claim(type: nameof(Domain.Page).ToLower(), value: item));
		//	}
		//}
		// *********************************************
		var tokenHandler =
			new System.IdentityModel.Tokens.Jwt.JwtSecurityTokenHandler();

		Microsoft.IdentityModel.Tokens.SecurityToken securityToken =
			tokenHandler.CreateToken(tokenDescriptor: tokenDescriptor);

		string token =
			tokenHandler.WriteToken(token: securityToken);

		return token;
		// *********************************************
	}
	#endregion

	#region Attach User To Context By Token
	public static async System.Threading.Tasks.Task AttachUserToContextByToken(
		Microsoft.AspNetCore.Http.HttpContext context,
		Microsoft.AspNetCore.Identity.UserManager<Domain.User> userManager,
		string token, string secretKey)
	{
		try
		{
			byte[] key =
				System.Text.Encoding.ASCII.GetBytes(secretKey);

			var tokenHandler =
				new System.IdentityModel.Tokens.Jwt.JwtSecurityTokenHandler();

			tokenHandler.ValidateToken(token: token,
				validationParameters: new Microsoft.IdentityModel.Tokens.TokenValidationParameters
				{
					ValidateIssuer = false,
					ValidateAudience = false,
					ValidateIssuerSigningKey = true,

					IssuerSigningKey =
						new Microsoft.IdentityModel.Tokens.SymmetricSecurityKey(key: key),

					ClockSkew = System.TimeSpan.Zero,
				}, out Microsoft.IdentityModel.Tokens.SecurityToken validatedToken);

			var jwtToken =
				validatedToken as System.IdentityModel.Tokens.Jwt.JwtSecurityToken;

			if (jwtToken == null)
			{
				return;
			}

			System.Security.Claims.Claim? userIdClaime = jwtToken.Claims
				.Where(current => current.Type.ToLower() == "NameId".ToLower())
				.FirstOrDefault();

			if (userIdClaime == null)
			{
				return;
			}

			string userId =
				userIdClaime.Value.ToString();

			Domain.User foundedUser =
				await userManager.FindByIdAsync(userId);

			if (foundedUser == null)
			{
				return;
			}

			context.Items["User"] = foundedUser;
		}
		catch // (System.Exception ex)
		{
			// string errorMessage = ex.Message;
			// Install Logger and save error message...
		}
	}
	#endregion

	#region Get Token Detail
	public static TokenDetails GetTokenDetail(HttpContext context, string secretKey)
	{
		//try
		//{
		var result = new TokenDetails();

		byte[] key =
			System.Text.Encoding.ASCII.GetBytes(secretKey);

		var tokenHandler =
			new System.IdentityModel.Tokens.Jwt.JwtSecurityTokenHandler();

		// find token in context
		if (context.Request.Headers.ContainsKey("Authorization") &&
			context.Request.Headers["Authorization"][0]!.StartsWith("Bearer "))
		{
			// var token = context.HttpContext.Request.Headers["Authorization"][0]["Bearer ".Length..];
			var token = context.Request.Headers["Authorization"][0]!.Substring("Bearer ".Length);
			tokenHandler.ValidateToken
				(token: token,
					validationParameters: new Microsoft.IdentityModel.Tokens.TokenValidationParameters
					{
						ValidateIssuer = false,
						ValidateAudience = false,
						ValidateIssuerSigningKey = true,

						IssuerSigningKey =
							new Microsoft.IdentityModel.Tokens.SymmetricSecurityKey(key: key),

						ClockSkew = TimeSpan.Zero,
					}, out Microsoft.IdentityModel.Tokens.SecurityToken validatedToken);

			var jwtToken =
				validatedToken as System.IdentityModel.Tokens.Jwt.JwtSecurityToken;

			if (jwtToken == null)
			{
				return result;
			}

			if (jwtToken.Claims.Count() == 0)
			{
				return result;
			}

			var userId =
				jwtToken.Claims.First(x => x.Type.Equals("nameid")).Value;

			string customerId = jwtToken.Claims
				.First(x => x.Type
						.Equals(nameof(customerId).ToLower())).Value;

			result.RoleNames.AddRange(jwtToken.Claims
				.Where(x => x.Type.Equals(nameof(Domain.Role)
					.ToLower())).Select(x => x.Value).ToList());

			//result.PageHrefs.AddRange(jwtToken.Claims
			//	.Where(x => x.Type.Equals(nameof(Domain.Page)
			//		.ToLower())).Select(x => x.Value).ToList());

			result.UserId = Convert.ToInt32(userId);

			result.CustomerId = Convert.ToInt32(customerId);

			result.IsOwner =
				Convert.ToBoolean(jwtToken.Claims.First(x => x.Type.Equals("isowner")).Value);

			result.Token = token;

			result.TokenIsOk = true;
		}

		return result;
		//}
		//catch // (System.Exception ex)
		//{
		//    // string errorMessage = ex.Message;
		//    // Install Logger and save error message...
		//}
	}
	#endregion
}

