using Microsoft.EntityFrameworkCore;

namespace Order.API.Models
{
    [Owned]
    public class Address
    {
        public string Line { get; set; }
        public string Province { get; set; }
        public string District { get; set; }

        public Address(
            string line,
            string province,
            string district
            )
        {
            Line = line;
            Province = province;
            District = district;
        }

        public Address()
        {

        }
    }
}
