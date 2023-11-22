using System;
using System.ComponentModel.DataAnnotations;

namespace YourNamespace
{
    /// <summary>
    /// Enum for Seattle neighborhoods with display names.
    /// </summary>
    public enum SeattleNeighborhoods
    {
        [Display(Name = "Ballard")]
        Ballard,

        [Display(Name = "Belltown")]
        Belltown,

        [Display(Name = "Capitol Hill")]
        CapitolHill,

        [Display(Name = "Downtown")]
        Downtown,

        [Display(Name = "First Hill")]
        FirstHill,

        [Display(Name = "Madison Park")]
        MadisonPark,

        [Display(Name = "Rainier Beach")]
        RainierBeach,

        [Display(Name = "University District")]
        UniversityDistrict,

        [Display(Name = "Waterfront")]
        Waterfront,

        [Display(Name = "West Seattle")]
        WestSeattle
    }
}