using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace testSuger.Models;

public partial class Read
{
    public int ReadId { get; set; }

    public decimal Value { get; set; }

    public DateTime Date { get; set; }

    public decimal PatientId { get; set; }
    [JsonIgnore]
    public virtual Patient Patient { get; set; } = null!;
}
