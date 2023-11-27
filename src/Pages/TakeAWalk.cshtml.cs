using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using TakeABreak.WebSite.Models;
using TakeABreak.WebSite.Services;

namespace TakeABreak.WebSite.Pages;

/// <summary>
/// The TakeAWalkModel class for the "Take A Walk" Razor Page.
/// It handles the loading of GeoJSON data and product information.
/// </summary>
public class TakeAWalkModel : PageModel
{
    // Logger 
    private readonly ILogger<TakeAWalkModel> _logger;
    
    // Map data 
    private readonly JsonFileMapModelService _mapService;
    
    // Product data 
    private readonly JsonFileProductService _productService;

    /// <summary>
    /// Stores GeoJSON data for the map.
    /// </summary>
    public string GeoJsonData { get; private set; }

    /// <summary>
    /// Stores a collection of ProductModel objects.
    /// </summary>
    public IEnumerable<ProductModel> Products { get; private set; }

    /// <summary>
    /// Initializes a new instance of the TakeAWalkModel class.
    /// </summary>
    /// <param name="logger">The logger instance.</param>
    /// <param name="mapService">The service for handling map data.</param>
    /// <param name="productService">The service for handling product data.</param>
    public TakeAWalkModel(ILogger<TakeAWalkModel> logger, JsonFileMapModelService mapService, JsonFileProductService productService) 
    {
        _logger = logger;
        _mapService = mapService;
        _productService = productService;
    }

    /// <summary>
    /// Called when the page is accessed via HTTP GET.
    /// Loads GeoJSON data and product information.
    /// </summary>
    public void OnGet()
    {
        // The geojson map data 
        GeoJsonData = _mapService.GetMapData(); 
        
        // The product information 
        Products = _productService.GetProducts(); 
    }
}