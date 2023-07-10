using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Infrastructure;

public static class HtmlHelpers : object
{
	static HtmlHelpers()
	{
	}

	public static IHtmlContent DisplayString(this IHtmlHelper html, string? value)
	{
		if (string.IsNullOrWhiteSpace(value: value))
		{
			return html.Raw
				(value: Constants.Format.NullValue);
		}

		return html.Raw(value: value);
	}

	public static IHtmlContent DisplayBoolean(this IHtmlHelper html, bool? value)
	{
		if (html is null)
		{
			throw new System
				.ArgumentNullException(paramName: nameof(html));
		}

		var div =
			new TagBuilder(tagName: "div");

		var input =
			new TagBuilder(tagName: "input");

		input.Attributes.Add
			(key: "type", value: "checkbox");

		input.Attributes.Add
			(key: "disabled", value: "disabled");

		if (value.HasValue && value.Value)
		{
			input.Attributes.Add
				(key: "checked", value: "checked");
		}

		div.InnerHtml.AppendHtml(content: input);

		return div;
	}
}
