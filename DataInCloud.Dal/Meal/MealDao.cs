using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataInCloud.Dal.Meal;
public class MealDao
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("name")]
    public string Name { get; set; }

    [Column("price")]
    public short Price { get; set; }

    [Column("is_available")]
    public bool IsAvailable { get; set; }
}