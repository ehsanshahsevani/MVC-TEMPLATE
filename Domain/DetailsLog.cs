using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Domain;

public class DetailsLog : Base.BaseEntity
{
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
	public DetailsLog() : base()
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
	{
	}

	public int UserId { get; set; }

	public int RecordId { get; set; }

	public string? Token_JWT { get; set; }

	public string? RequestPath { get; set; }
	public string? HttpReferrer { get; set; }

	public string? RemoteIP { get; set; }
	public string? PortIP { get; set; }

	public string StateChangeWorker { get; set; }
	public string JsonField { get; set; }

	public string? NameSpace { get; set; }

	public string TypeName { get; set; }

	public string DebugView { get; set; }

	public T Clone<T>()
	{
		return (T)this.MemberwiseClone();
	}

	[JsonIgnore]
	[NotMapped]
	public object? EntityTracker { get; set; }
}
