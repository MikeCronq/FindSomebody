using System.ComponentModel.DataAnnotations;
using System.Data.Entity;

namespace FindSomebody.Models
{
    /// <summary>
    /// Profile fields about a person.
    /// </summary>
    public class Person
    {
        /// <summary>
        /// Unique id of the person.
        /// </summary>
        public int ID { get; set; }

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
        public int Age { get; set; }

        /// <summary>
        /// Intersts and hobbies a person has.
        /// </summary>
        public string Interests { get; set; }

        //Missing Picture!
    }

    /// <summary>
    /// Entity Framework person database context.
    /// </summary>
    public class PersonDbContext : DbContext
    {
        /// <summary>
        /// A database set of persons.
        /// </summary>
        public DbSet<Person> People { get; set; }
    }
}