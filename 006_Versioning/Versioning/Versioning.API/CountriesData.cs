using Versioning.API.Models.Domain;

namespace Versioning.API
{
    public static class CountriesData
    {
        public static List<Country> Get()
        {
            var countries = new[]
            {
                new {Id = 1, Name = "USA"},
                new {Id = 2, Name = "RSA"},
                new {Id = 3, Name = "BRA"},
                new {Id = 4, Name = "IND"},
                new {Id = 5, Name = "GHA"},
                new {Id = 6, Name = "NIG"},
                new {Id = 7, Name = "ZIM"},
                new {Id = 8, Name = "LES"},
                new {Id = 9, Name = "NAM"},
                new {Id = 10, Name = "MEX"},
            };

            return countries.Select(c => new Country {Id = c.Id, Name = c.Name}).ToList();
        }
    }
}
