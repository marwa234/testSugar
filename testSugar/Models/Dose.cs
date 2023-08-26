using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace testSuger.Models;

public partial class Dose
{
    public int DoseId { get; set; }

    public decimal R1 { get; set; }

    public decimal R2 { get; set; }

    public int Dose1 { get; set; }

    public decimal PatientId { get; set; }
    [JsonIgnore]
    public virtual Patient Patient { get; set; } = null!;
}
