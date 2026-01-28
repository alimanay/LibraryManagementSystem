using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Infrastructure.ExternalServices.DTOs
{
    public class GoogleBooksResponseDto
    {
        public List<Item>? Items { get; set; }
    }

    public class Item
    {
        public VolumeInfo? VolumeInfo { get; set; }
    }

    public class VolumeInfo
    {
        public string? Title { get; set; }
        public List<string>? Authors { get; set; }
        public string? Description { get; set; }
        public ImageLinksDto ImageLinks { get; set; }

        [JsonPropertyName("industryIdentifiers")]
        public List<IndustryIdentifiers>? Industry {  get; set; }
    }
    public class ImageLinksDto
    {
        public string Thumbnail { get; set; }
        public string SmallThumbnail { get; set; }
    }


    public class IndustryIdentifiers{
        [JsonPropertyName("type")]
        public string Type { get; set; }
        [JsonPropertyName("identifier")]
        public string Identifier {  get; set; }
    }
}
