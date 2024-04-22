using LOT_TASK.Models;
using System.ComponentModel.DataAnnotations;

namespace LOT_TASK.Dtos
{
    public class FlightDto
    {
        public Guid? Id { get; set; }

        [Required(ErrorMessage = "Flight number is required.")]
        [StringLength(30, ErrorMessage = "Flight number must contain {2}-{1} characters.", MinimumLength = 3)]
        public string FlightNumber { get; set; }

        [Required(ErrorMessage = "Departure date is required.")]
        [DataType(DataType.DateTime, ErrorMessage = "Invalid date format.")]
        public DateTime DepartureDate { get; set; }

        [Required(ErrorMessage = "Departure location is required.")]
        [StringLength(50, ErrorMessage = "Departure location must contain {2}-{1} characters.", MinimumLength = 3)]
        public string DepartureLocation { get; set; }

        [Required(ErrorMessage = "Arrival location is required.")]
        [StringLength(50, ErrorMessage = "Arrival location must contain {2}-{1} characters.", MinimumLength = 3)]
        public string ArrivalLocation { get; set; }

        [Required(ErrorMessage = "Aircraft type is required.")]
        [EnumDataType(typeof(AircraftType), ErrorMessage = "Invalid aircraft type.")]
        public string AircraftType { get; set; }
    }
}
