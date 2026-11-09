using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Caleb_Marks_Assignment_10._3.Models
{
    // Car class maps Cars table
    public class Car
    {
        // Primary key; app supplies each ID
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int CarId { get; set; }

        public string Vin { get; set; } = string.Empty;
        public string Make { get; set; } = string.Empty;
        public string Model { get; set; } = string.Empty;
        public int Year { get; set; }
        public double Price { get; set; }
    }
}
