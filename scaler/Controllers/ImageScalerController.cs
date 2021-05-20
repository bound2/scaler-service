using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

using scaler.Api.Request;
using scaler.Services;

namespace scaler.Controllers
{
    [ApiController]
    public class ImageScalerController : ControllerBase
    {

        private readonly ILogger<WeatherForecastController> _logger;
        private readonly ImageScalerService _imageScalerService;

        public ImageScalerController(
            ILogger<WeatherForecastController> logger,
            ImageScalerService imageScalerService
        )
        {
            _logger = logger;
            _imageScalerService = imageScalerService;
        }

        [HttpPost("/api/v1/scaler/image")]
        public void Post(ImageScalerRequest request)
        {
            _imageScalerService.Scale(request.Url);
        }
    }
}
