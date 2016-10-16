namespace FindSomebody.Migrations
{
    using Models;
    using System;
    using System.Data.Entity.Migrations;
    using System.IO;
    using System.Linq;

    /// <summary>
    /// People database configuration.
    /// </summary>
    internal sealed class Configuration : DbMigrationsConfiguration<PeopleDbContext>
    {
        /// <summary>
        /// Configuration settings.
        /// </summary>
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        /// <summary>
        /// Seed a couple fake profiles, and a large group of randomly generated profiles.
        /// </summary>
        /// <param name="context"></param>
        protected override void Seed(PeopleDbContext context)
        {
            context.People.AddOrUpdate(
              p => p.Email,
              new Person
              {
                  Name = "Andrew Peters",
                  Email = "andrewnoire@gmail.com",
                  Age = 48,
                  Address = "564 Waterson Ave.",
                  Interests = "Parasailing, classic films, biking"
              },
              new Person
              {
                  Name = "Brice Lambson",
                  Email = "yoloyachter@gmail.com",
                  Age = 34,
                  Address = "16 Topsbend Cove",
                  Interests = "Yachting, base jumping, kayaking"
              },
              new Person
              {
                  Name = "Rowan Miller",
                  Email = "playmaker@gmail.com",
                  Age = 24,
                  Address = "83 Lumbar St.",
                  Interests = "Reading, video games, wood carving"
              },
              new Person
              {
                  Name = "Bruce Chadwick",
                  Email = "brucebaritone@yahoo.com",
                  Age = 22,
                  Address = "99 Offpoint Dr.",
                  Interests = "Musical theatre, barbershop quartets, water polo"
              },
              new Person
              {
                  Name = "Madaline LaRouche",
                  Email = "madmaddy@yahoo.com",
                  Age = 52,
                  Address = "804 Appleseed Dr.",
                  Interests = "Cooking, fashion, interior design"
              },
              new Person
              {
                  Name = "Peter Sadive",
                  Email = "machinemind@gmail.com",
                  Age = 42,
                  Address = "112 Summer Glaive Ave.",
                  Interests = "Planes, trains, automobiles, rom coms"
              },
              new Person
              {
                  Name = "Stephanie Cane",
                  Email = "stephwins@gmail.com",
                  Age = 29,
                  Address = "33 Settled Meadows",
                  Interests = "Soccer, lacrosse, rock climbing"
              },
              new Person
              {
                  Name = "Chin Wong",
                  Email = "wongnumber@gmail.com",
                  Age = 19,
                  Address = "72 Crestpoint Rd.",
                  Interests = "Design, sketching, piano"
              });

            var firstNameSet = new[] { "Melissa", "Frank", "David", "Jesse", "Jacob", "Tyler", "Jason", "Amber", "Tobe", "Daniel", "Megan", "Summer" };
            var lastNameSet = new[] { "Smith", "Johnson", "Williams", "Jones", "Brown", "Miller", "White", "Jackson", "Lewis", "Lee", "Hall", "Allen" };
            var addressStreetName = new[] { "Waterson", "Main", "Trail", "MLK", "Riverwood", "Ironside", "Mountain", "Olympic" };
            var addressPostfix = new[] { "Ave.", "Dr.", "St.", "Blvd.", "Rd.", "Ln." };
            var interestSet = new[] { "Cars", "Videogames", "Sailling", "Base jumping", "Movies", "Mountain Biking", "Scuba", "Reading" };

            var photoDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\Uploads\Photos");
            var photos = Directory.EnumerateFiles(photoDirectory).Select(x => x.Substring(x.LastIndexOf('\\') + 1)).ToArray();

            var rand = new Random();
            for (int i = 0; i < 1000; ++i)
            {
                context.People.AddOrUpdate(
                    p => p.Email,
                    new Person
                    {
                        Name = firstNameSet[rand.Next(firstNameSet.Length)] + " " + lastNameSet[rand.Next(lastNameSet.Length)],
                        Email = i.ToString() + "@website.com",
                        Age = rand.Next(60) + 24,
                        Address = (rand.Next(999) + 1) + " " + addressStreetName[rand.Next(addressStreetName.Length)] + " " + addressPostfix[rand.Next(addressPostfix.Length)],
                        Interests = interestSet[rand.Next(interestSet.Length)] + ", " + interestSet[rand.Next(interestSet.Length)] + ", " + interestSet[rand.Next(interestSet.Length)],
                        Photo = "/Uploads/Photos/" + photos[rand.Next(photos.Length)]
                    });
            }
        }
    }
}