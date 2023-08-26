using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace testSuger.Models;

public partial class PatientDoctor
{
    public int DrPtId { get; set; }

    public decimal PatientId { get; set; }

    public decimal DoctorId { get; set; }
    [JsonIgnore]
    public virtual Doctor Doctor { get; set; } = null!;
    [JsonIgnore]
    public virtual Patient Patient { get; set; } = null!;
}
