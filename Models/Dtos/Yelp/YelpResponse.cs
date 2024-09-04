
namespace foodies_api.Models.Dtos.Yelp;

public class YelpResponse
{
    public List<BusinessDto>? Businesses { get; set; }
    public int Total { get; set; }
    public RegionDto? Region { get; set; }
    public List<ReviewDto> Reviews { get; set; }
}
