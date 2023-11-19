using System.ComponentModel.DataAnnotations;
using System;

namespace TakeABreak.WebSite.Enums
{
    /// <summary>
    /// Enum for strings describing noise levels
    /// </summary>
    public enum NoiseLevel
    {
        [Display(Name = "Quiet (1-2)")]
        Quiet = 1,

        [Display(Name = "Moderate (3-4)")]
        Moderate = 3,

        [Display(Name = "Loud (5-6)")]
        Loud = 5,

        [Display(Name = "Very Loud (7-8)")]
        VeryLoud = 7,

        [Display(Name = "Extremely Loud (9-10)")]
        ExtremelyLoud = 9,
    }
}