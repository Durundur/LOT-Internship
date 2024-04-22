namespace LOT_TASK.Models
{
    public class FlightModel : BaseModel
    {
        public Guid Id { get; set; }
        public string FlightNumber { get; set; }
        public DateTime DepartureDate { get; set; }
        public string DepartureLocation { get; set; }
        public string ArrivalLocation { get; set; }
        public AircraftType AircraftType { get; set; }
    }

    public enum AircraftType
    {
        Embraer,
        Boeing,
        Airbus
    }
}
