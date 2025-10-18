namespace comp_584_server.DTOs
{
    public class CountryPopulation
    {
        public int ID { get; set; }
        public required String name { get; set; }
        public required String iso2 { get; set; }
        public required String iso3 { get; set; }
        public decimal population { get; set; }


    }
}
