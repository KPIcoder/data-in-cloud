using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataInCloud.Dal.Meal;
public class MealDao
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("name")]
    [MaxLength(256, ErrorMessage = "Must be within 1-256 chars")]
    public string Name { get; set; }

    [Column("price")]
    [Range(1, 1000, ErrorMessage = "Must be within 1-1000")]
    public short Price { get; set; }

    [Column("is_available")]
    public bool IsAvailable { get; set; }
}