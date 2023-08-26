using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace testSuger.Models;

public partial class Doctor
{
    public decimal Id { get; set; }

    public string Name { get; set; } = null!;

    public DateTime Dob { get; set; }

    public decimal Age { get; set; }

    public bool Gender { get; set; }

    public string? Phone { get; set; }

    public string? Password { get; set; }
    [JsonIgnore]
    public virtual User IdNavigation { get; set; } = null!;
    [JsonIgnore]
    public virtual ICollection<PatientDoctor> PatientDoctors { get; set; } = new List<PatientDoctor>();
}
