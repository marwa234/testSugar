using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace testSuger.Models;

public partial class Time
{
    public decimal PatientId { get; set; }

    public TimeSpan Breakfast { get; set; }

    public TimeSpan Lunch { get; set; }

    public TimeSpan Dinner { get; set; }
    [JsonIgnore]
    public virtual Patient Patient { get; set; } = null!;
}
