using System.ComponentModel.DataAnnotations;
using System.Data.Entity;

namespace FindSomebody.Models
{
    /// <summary>
    /// Profile for a person.
    /// </summary>
    public class Person
    {
        /// <summary>
        /// Unique id of the person.
        /// </summary>
        public int ID { get; set; }

        /// <summary>
        /// Url for the persons picture.
        /// </summary>
        [DataType(DataType.ImageUrl)]
        public string Photo { get; set; }

        /// <summary>
        /// Name of the person.
        /// </summary>
        [Required]
        [StringLength(60, MinimumLength = 3)]
        public string Name { get; set; }

        /// <summary>
        /// Email address of the person.
        /// </summary>
        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        /// <summary>
        /// Home address.
        /// </summary>
        [StringLength(120)]
        [RegularExpression(@"^\s*\S+(?:\s+\S+){2,}")]
        public string Address { get; set; }

        /// <summary>
        /// Age of the person.
        /// </summary>
        public int? Age { get; set; }

        /// <summary>
        /// Intersts and hobbies a person has.
        /// </summary>
        public string Interests { get; set; }
    }

    /// <summary>
    /// Entity Framework person database context.
    /// </summary>
    public class PeopleDbContext : DbContext
    {
        /// <summary>
        /// A database set of persons.
        /// </summary>
        public virtual IDbSet<Person> People { get; set; }

        /// <summary>
        /// Mockable access to Entry.
        /// </summary>
        /// <param name="entity"></param>
        public virtual void SetModified(object entity)
        {
            Entry(entity).State = EntityState.Modified;
        }
    }
}