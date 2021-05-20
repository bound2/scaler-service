using System;
using System.ComponentModel.DataAnnotations;

namespace scaler.Api.Request
{
    public class ImageScalerRequest
    {
        [Required]
        public string Url { get; set; }
    }
}
