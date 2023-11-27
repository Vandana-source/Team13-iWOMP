using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using TakeABreak.WebSite.Services;

namespace TakeABreak.WebSite.Pages;

public class TakeAWalkModel : PageModel
{
    private readonly ILogger<TakeAWalkModel> _logger;
    private readonly JsonFileMapModelService _mapService;

    public string GeoJsonData { get; private set; } 

    public TakeAWalkModel(ILogger<TakeAWalkModel> logger, JsonFileMapModelService mapService) 
    {
        _logger = logger;
        _mapService = mapService; 
    }

    public void OnGet()
    {
        GeoJsonData = _mapService.GetMapData(); 
    }
}