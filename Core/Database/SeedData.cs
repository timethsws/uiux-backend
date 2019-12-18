using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using Core.Entities;

namespace Core.Database
{
    public class SeedData
    {
        public SeedData()
        {
        }

        public static void SeedDatabase(AppDbContext dbContext)
        {
            var MySHA256 = SHA256.Create();

            List<ApplicationUser> Users = new List<ApplicationUser>
            {
                new ApplicationUser
                {
                    Id = Guid.Parse("4C6E72B2-87DC-4CDC-A855-A72498DC067B"),
                    Email ="john@doe.com",
                    UserName ="John Doe",
                    ProfilePicture = new Image
                    {
                        Id = Guid.NewGuid(),
                        Url = "/images/profile_m.png"
                    },
                    Gender = Gender.Male,
                    PasswordHash = Encoding.UTF8.GetString(MySHA256.ComputeHash(Encoding.UTF8.GetBytes("Qwerty1@" + "MySecret"))),
                },
                new ApplicationUser
                {
                    Id = Guid.Parse("65444AF4-841A-4B73-BE7F-C49CB7069C4B"),
                    Email ="jane@doe.com",
                    UserName ="Jane Doe",
                    ProfilePicture = new Image
                    {
                        Id = Guid.NewGuid(),
                        Url = "/images/profile_m.png"
                    },
                    Gender = Gender.Female,
                    PasswordHash = Encoding.UTF8.GetString(MySHA256.ComputeHash(Encoding.UTF8.GetBytes("Qwerty1@" + "MySecret"))),
                }
            };

            if (!dbContext.Users.Any())
            {
                dbContext.Users.AddRange(Users);
                dbContext.SaveChanges();

            }

            List<Place> places = new List<Place>
            {
                new Place
                {
                    Id = Guid.Parse("94EAB6AD-8062-4CE2-A982-3CA495652363"),
                    Name ="Tower Bridge",
                    LongDescription ="Tower Bridge is a combined bascule and suspension bridge in London, built between 1886 and 1894. The bridge crosses the River Thames close to the Tower of London and has become an iconic symbol of London. As a result, it is sometimes confused with London Bridge, about half a mile (0.8 km) upstream. Tower Bridge is one of five London bridges owned and maintained by the Bridge House Estates, a charitable trust overseen by the City of London Corporation. It is the only one of the trust's bridges not to connect the City of London directly to the Southwark bank, as its northern landfall is in Tower Hamlets.",
                    Image = new Image
                    {
                        Id = Guid.Parse("D00D120A-D563-442F-9ECE-358ED0B88ED7"),
                        Url = "https://upload.wikimedia.org/wikipedia/commons/6/63/Tower_Bridge_from_Shad_Thames.jpg"
                    }

                },
                new Place
                {
                    Id = Guid.Parse("D4A1AFB0-1968-41FD-9646-AEFFCB21E736"),
                    Name ="Big Ben",
                    LongDescription ="Big Ben is the nickname for the Great Bell of the striking clock at the north end of the Palace of Westminster in London[1] and is usually extended to refer to both the clock and the clock tower.[2][3] The official name of the tower in which Big Ben is located was originally the Clock Tower, but it was renamed Elizabeth Tower in 2012 to mark the Diamond Jubilee of Elizabeth II.The tower was designed by Augustus Pugin in a neo-Gothic style. When completed in 1859, its clock was the largest and most accurate four-faced striking and chiming clock in the world.[4] The tower stands 315 feet (96 m) tall, and the climb from ground level to the belfry is 334 steps. Its base is square, measuring 39 feet (12 m) on each side. Dials of the clock are 23 feet (7.0 m) in diameter. On 31 May 2009, celebrations were held to mark the tower's 150th anniversary.",
                    Image = new Image
                    {
                        Id = Guid.Parse("E3C7F467-9115-41BD-BA2B-10EC79609D55"),
                        Url = "https://images.theconversation.com/files/182776/original/file-20170821-27160-1kwep4u.jpg"
                    }
                },
                new Place
                {
                    Id = Guid.Parse("CA6FE982-3379-4B47-93ED-4923FF893ECD"),
                    Name ="Coca-Cola London Eye",
                    LongDescription ="Tower Bridge is a combined bascule and suspension bridge in London, built between 1886 and 1894. The bridge crosses the River Thames close to the Tower of London and has become an iconic symbol of London. As a result, it is sometimes confused with London Bridge, about half a mile (0.8 km) upstream. Tower Bridge is one of five London bridges owned and maintained by the Bridge House Estates, a charitable trust overseen by the City of London Corporation. It is the only one of the trust's bridges not to connect the City of London directly to the Southwark bank, as its northern landfall is in Tower Hamlets.",
                    Image = new Image
                    {
                        Id = Guid.Parse("C6136239-8852-419B-8399-1D88749B880D"),
                        Url = "https://upload.wikimedia.org/wikipedia/commons/d/d6/London-Eye-2009.JPG"
                    }
                },
                new Place
                {
                    Id = Guid.Parse("55B399EA-8264-4A18-AACE-3ED413B715EA"),
                    Name ="Tower of London",
                    LongDescription ="Tower Bridge is a combined bascule and suspension bridge in London, built between 1886 and 1894. The bridge crosses the River Thames close to the Tower of London and has become an iconic symbol of London. As a result, it is sometimes confused with London Bridge, about half a mile (0.8 km) upstream. Tower Bridge is one of five London bridges owned and maintained by the Bridge House Estates, a charitable trust overseen by the City of London Corporation. It is the only one of the trust's bridges not to connect the City of London directly to the Southwark bank, as its northern landfall is in Tower Hamlets.",
                    Image = new Image
                    {
                        Id = Guid.Parse("D2CC58D2-2554-4CEE-91B3-CAFF50EFF78B"),
                        Url = "https://lonelyplanetimages.imgix.net/a/g/hi/t/145074adb387be6f8f357d7dc3ae9e3c-tower-of-london.jpg"
                    }
                }
            };

            if (!dbContext.Places.Any())
            {
                dbContext.Places.AddRange(places);
                dbContext.SaveChanges();
            }

            List<Review> reviews = new List<Review>
            {
                new Review
                {
                    Id = Guid.Parse("302AC5EF-BAAF-4697-8612-4E78F4B10F67"),
                    Title="Awesome Place!!!",
                    Content = "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum.",
                    Image = new Image
                    {
                        Id = Guid.NewGuid(),
                        Url = "https://www.apexcartage.com/wp-content/uploads/revslider/rev_slider_example/placeholder-red.png"

                    },
                    Rating = Enums.Rating.Rate_3,
                    ReviewedOn = new DateTime(2019,10,12),
                    PlaceId = Guid.Parse("94EAB6AD-8062-4CE2-A982-3CA495652363"),
                    ReviewerId = Guid.Parse("4C6E72B2-87DC-4CDC-A855-A72498DC067B")
                },new Review
                {
                    Id = Guid.Parse("04536519-6BC2-4057-B888-849F131BC58D"),
                    Title="Will visit again !!!",
                    Content = "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum.",
                    Image = new Image
                    {
                        Id = Guid.NewGuid(),
                        Url = "https://www.apexcartage.com/wp-content/uploads/revslider/rev_slider_example/placeholder-red.png"

                    },
                    Rating = Enums.Rating.Rate_3,
                    ReviewedOn = new DateTime(2019,10,12),
                    PlaceId = Guid.Parse("94EAB6AD-8062-4CE2-A982-3CA495652363"),
                    ReviewerId = Guid.Parse("65444AF4-841A-4B73-BE7F-C49CB7069C4B")
                },

                new Review
                {
                    Id = Guid.Parse("F8B09215-71C6-46B1-8C8A-ACC7AEC8B207"),
                    Title="Breathtaking !!!",
                    Content = "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum.",
                    Image = new Image
                    {
                        Id = Guid.NewGuid(),
                        Url = "https://mywowo.net/media/images/cache/londra_houses_of_parliament_02_westminster_big_ben_jpg_1200_630_cover_85.jpg"

                    },
                    Rating = Enums.Rating.Rate_3,
                    ReviewedOn = new DateTime(2019,10,12),
                    PlaceId = Guid.Parse("D4A1AFB0-1968-41FD-9646-AEFFCB21E736"),
                    ReviewerId = Guid.Parse("4C6E72B2-87DC-4CDC-A855-A72498DC067B")
                },new Review
                {
                    Id = Guid.Parse("48ECD590-DE11-4A3D-A9ED-57D956A5924E"),
                    Title="Must Visit",
                    Content = "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum.",
                    Image = new Image
                    {
                        Id = Guid.NewGuid(),
                        Url = "https://lp-cms-production.imgix.net/news/2017/08/London.jpg"

                    },
                    Rating = Enums.Rating.Rate_3,
                    ReviewedOn = new DateTime(2019,10,12),
                    PlaceId = Guid.Parse("D4A1AFB0-1968-41FD-9646-AEFFCB21E736"),
                    ReviewerId = Guid.Parse("65444AF4-841A-4B73-BE7F-C49CB7069C4B")
                },
            };

            if (!dbContext.Reviews.Any())
            {
                dbContext.Reviews.AddRange(reviews);
                dbContext.SaveChanges();
            }

            var trainStations = new List<TrainStation>{
                new TrainStation{
                    Id = Guid.Parse("1aca7407-dee9-4ffb-9bf8-5bf9e2923bee"),
                    StationName = "Station 1",
                    StationCode ="Code1"
                },
                new TrainStation{
                    Id = Guid.Parse("742357d6-5036-4bc5-97ce-7f93ecc762b7"),
                    StationName = "Station 2",
                    StationCode ="Code2"
                },
                new TrainStation{
                    Id = Guid.Parse("10218c84-ef73-422f-9f6c-21c6e062f0e0"),
                    StationName = "Station 3",
                    StationCode ="Code3"
                },
                new TrainStation{
                    Id = Guid.Parse("d17bdf85-0a49-436b-b4bb-efe07edca99c"),
                    StationName = "Station 4",
                    StationCode ="Code4"
                }
            };

            if (!dbContext.Stations.Any())
            {
                dbContext.Stations.AddRange(trainStations);
                dbContext.SaveChanges();
            }


        }
    }
}
