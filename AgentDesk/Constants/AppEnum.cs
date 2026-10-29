using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgentDesk.Constants;

public static class AppEnum
{
    public enum Gender
    {
        Male,
        Female,
        Other
    }

    public enum MaritalStatus
    {
        Single,
        Married,
        Widowed,
        Divorced
    }

    public enum AgencyType
    {
        [Display(Name = "Life Insurers")]
        LifeInsurers,

        [Display(Name = "Standalone Health Insurers")]
        StandaloneHealthInsurers,

        [Display(Name = "Specialised Insurers")]
        SpecialisedInsurers,

        [Display(Name = "General Insurers")]
        GeneralInsurers
    }
}
