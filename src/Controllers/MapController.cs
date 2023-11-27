using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using TakeABreak.WebSite.Models;
using TakeABreak.WebSite.Services;

namespace TakeABreak.WebSite.Controllers
{
    /// <summary>
    /// Controller for managing map-related API endpoints.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class MapController : ControllerBase
    {
        private readonly JsonFileMapModelService _mapService;

        /// <summary>
        /// Initializes a new instance of the MapController class.
        /// </summary>
        /// <param name="mapService">The JsonFileMapService instance for map operations.</param>
        public MapController(JsonFileMapModelService mapService)
        {
            _mapService = mapService;
        }

        /// <summary>
        /// Gets GeoJSON data for map features.
        /// </summary>
        /// <returns>GeoJSON data for map features.</returns>
        [HttpGet("geojson")]
        public IActionResult GetGeoJSON()
        {
            // Call the GetMapFeatures method from the JsonFileMapService to retrieve GeoJSON data.
            var mapFeatures = _mapService.GetMapData();
            
            // Return the retrieved GeoJSON data as an HTTP response with a 200 OK status.
            return Ok(mapFeatures);
        }
    }
}


