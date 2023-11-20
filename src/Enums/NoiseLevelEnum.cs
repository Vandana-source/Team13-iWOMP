using System;
using System.ComponentModel.DataAnnotations;

namespace TakeABreak.WebSite.Enums
{
    /// <summary>
    /// Enum for strings describing noise levels
    /// </summary>
    public enum NoiseLevel
    {
        [Display(Name = "Quiet")]
        Quiet = 1,

        [Display(Name = "Moderate")]
        Moderate = 3,

        [Display(Name = "Loud")]
        Loud = 5,

        [Display(Name = "Very Loud")]
        VeryLoud = 7,

        [Display(Name = "Extremely Loud")]
        ExtremelyLoud = 9,
    }
}