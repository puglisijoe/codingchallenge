﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Threading.Tasks;
using ContactListApp.Model;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace ContactListApp.DataAccess {
    public class ContactGenerator {
        private readonly string[] _cities = {
            "New York",
            "Los Angeles",
            "Chicago",
            "Houston",
            "Phoenix",
            "Philadelphia",
            "San Antonio",
            "San Diego",
            "Dallas",
            "San Jose",
            "Austin",
            "Jacksonville",
            "Fort Worth",
            "Columbus",
            "San Francisco",
            "Charlotte",
            "Indianapolis",
            "Seattle",
            "Denver",
            "Washington",
            "Boston",
            "El Paso",
            "Nashville",
            "Detroit",
            "Oklahoma City",
            "Portland",
            "Las Vegas",
            "Memphis",
            "Louisville",
            "Baltimore",
            "Milwaukee",
            "Albuquerque",
            "Tucson",
            "Fresno",
            "Mesa",
            "Sacramento",
            "Atlanta",
            "Kansas City",
            "Colorado Springs",
            "Miami",
            "Raleigh",
            "Omaha",
            "Long Beach",
            "Virginia Beach",
            "Oakland",
            "Minneapolis",
            "Tulsa",
            "Arlington",
            "Tampa",
            "New Orleans",
            "Wichita",
            "Cleveland",
            "Bakersfield",
            "Aurora",
            "Anaheim",
            "Honolulu",
            "Santa Ana",
            "Riverside",
            "Corpus Christi",
            "Lexington",
            "Stockton",
            "St. Paul",
            "Cincinnati",
            "Saint Louis",
            "Pittsburgh",
            "Greensboro",
            "Lincoln",
            "Anchorage",
            "Plano",
            "Orlando",
            "Irvine",
            "Newark",
            "Toledo",
            "Durham",
            "Chula Vista",
            "Fort Wayne",
            "Jersey City",
            "St. Petersburg",
            "Laredo",
            "Madison",
            "Chandler",
            "Buffalo",
            "Lubbock",
            "Scottsdale",
            "Reno",
            "Glendale",
            "Gilbert",
            "Winston-Salem",
            "North Las Vegas",
            "Norfolk",
            "Chesapeake",
            "Garland",
            "Irving",
            "Hialeah",
            "Fremont",
            "Boise",
            "Richmond",
            "San Bernardino",
            "Baton Rouge",
            "Spokane",
            "Des Moines",
            "Tacoma",
            "Grand Rapids",
            "Fontana",
            "Yonkers",
            "Augusta",
            "Irving",
            "Huntington Beach",
            "Modesto",
            "Montgomery",
            "Moreno Valley",
            "Glendale",
            "Aurora",
            "Columbus",
            "Shreveport",
            "Akron",
            "Little Rock",
            "Amarillo",
            "McKinney",
            "Grand Prairie",
            "Mobile",
            "Salt Lake City",
            "Huntsville",
            "Tallahassee",
            "Worcester",
            "Knoxville",
            "Tempe",
            "Concord",
            "Fayetteville",
            "Brownsville",
            "Jackson",
            "Cape Coral",
            "Salem",
            "Sioux Falls",
            "Peoria",
            "Springfield",
            "Eugene",
            "Corona",
            "Fort Collins",
            "Santa Clarita",
            "Garden Grove",
            "Oceanside",
            "Rancho Cucamonga",
            "Santa Rosa",
            "Chattanooga",
            "Vancouver",
            "Ontario",
            "Temecula",
            "Springfield",
            "Cary",
            "Pembroke Pines",
            "Pomona",
            "Paterson",
            "Joliet",
            "Rockford",
            "Torrence",
            "Bridgeport",
            "Escondido",
            "Sunnyvale",
            "Alexandria",
            "Kansas City",
            "Lakewood",
            "Palmdale",
            "Hollywood",
            "Clarksville",
            "Lancaster",
            "Salinas",
            "Naperville",
            "Mesquite",
            "Dayton",
            "Savannah",
            "Orange",
            "Fullerton",
            "Hampton",
            "McAllen",
            "Warren",
            "Bellevue",
            "West Valley City",
            "Columbia",
            "Sterling Heights",
            "New Haven",
            "Miramar",
            "Waco",
            "Thousand Oaks",
            "Cedar Rapids",
            "Charleston",
            "Visalia",
            "Topeka",
            "Elizabeth",
            "Gainesville",
            "Thornton",
            "Roseville",
            "Carrollton",
            "Coral Springs",
            "Stamford",
            "Sterling Heights",
            "Kent",
            "Columbus",
            "Surprise",
            "Denton",
            "Victorville",
            "Evansville",
            "Santa Clara",
            "Abilene",
            "Athens",
            "Vallejo",
            "Allentown",
            "Norman",
            "Beaumont",
            "Independence",
            "Murfreesboro",
            "Ann Arbor",
            "Springfield",
            "Berkeley",
            "Peoria",
            "Provo",
            "El Monte",
            "Columbia",
            "Lansing",
            "Fargo",
            "Downey",
            "Mesa",
            "Wilmington",
            "Inglewood",
            "Miami Gardens",
            "Carlsbad",
            "Westminster",
            "Rochester",
            "Odessa",
            "Manchester",
            "Elgin",
            "West Jordan",
            "Round Rock",
            "Clearwater",
            "Waterbury",
            "Gresham",
            "Fairfield",
            "Billings",
            "Lowell",
            "San Buenaventura",
            "Pueblo",
            "High Point",
            "West Covina",
            "Richmond",
            "Murrieta",
            "Cambridge",
            "Antioch",
            "Temecula",
            "Norwalk",
            "Centennial",
            "Everett",
            "Palm Bay",
            "Wichita Falls",
            "Green Bay",
            "Daly City",
            "Burbank",
            "Richardson",
            "Miramar",
            "North Charleston",
            "Santa Clara",
            "Rialto",
            "Boulder",
            "Carlsbad",
            "Westminster",
            "West Covina",
            "Gresham",
            "Everett",
            "Pueblo",
            "Fairfield",
            "Cambridge",
            "High Point",
            "Billings",
            "Lowell",
            "Ventura",
            "Murrieta",
            "Centennial",
            "Temecula",
            "Everett",
            "Norwalk",
            "Carlsbad",
            "Richardson",
            "Burbank",
            "North Charleston",
            "Rialto",
            "West Covina",
            "Clearwater",
            "Waterbury",
            "Odessa",
            "Manchester",
            "Elgin",
            "West Jordan",
            "Round Rock",
            "Westminster",
            "Fairfield",
            "Billings",
            "Lowell",
            "Ventura",
            "Pueblo",
            "High Point",
            "Centennial",
            "Everett",
            "Temecula",
            "Norwalk",
            "Cambridge",
            "Murrieta",
            "Clearwater",
            "Waterbury",
            "Westminster",
            "Odessa",
            "Manchester",
            "Elgin",
            "West Jordan",
            "Round Rock",
            "Billings",
            "Lowell",
            "Ventura",
            "Pueblo",
            "High Point",
            "Centennial",
            "Everett",
            "Temecula",
            "Norwalk",
            "Cambridge",
            "Murrieta",
            "Clearwater",
            "Waterbury",
            "Westminster",
            "Odessa",
            "Manchester",
            "Elgin",
            "West Jordan",
            "Round Rock",
            "Clearwater",
            "Waterbury",
            "Odessa",
            "Manchester",
            "Elgin",
            "West Jordan",
            "Round Rock",
            "Clearwater",
            "Waterbury",
            "Odessa",
            "Manchester",
            "Elgin",
            "West Jordan",
            "Round Rock",
            "Clearwater",
            "Waterbury",
            "Odessa",
            "Manchester",
            "Elgin",
            "West Jordan",
            "Round Rock",
            "Clearwater",
            "Waterbury",
            "Odessa",
            "Manchester",
            "Elgin",
            "West Jordan",
            "Round Rock",
            "Clearwater",
            "Waterbury",
            "Odessa",
            "Manchester",
            "Elgin",
            "West Jordan",
            "Round Rock",
            "Clearwater",
            "Water"
        };

        private readonly string[] _colors = {
            "Red",
            "Blue",
            "Green",
            "Yellow",
            "Orange",
            "Purple",
            "Pink",
            "Brown",
            "Black",
            "White",
            "Gray",
            "Silver",
            "Gold",
            "Maroon",
            "Navy",
            "Aqua",
            "Teal",
            "Lime",
            "Olive",
            "Beige",
            "Peach",
            "Turquoise",
            "Magenta",
            "Lavender",
            "Ivory",
            "Cyan",
            "Indigo",
            "Violet",
            "Tan",
            "Sky blue",
            "Rose",
            "Salmon",
            "Plum",
            "Periwinkle",
            "Pear",
            "Mulberry",
            "Mauve",
            "Lilac",
            "Khaki",
            "Jade",
            "Honeydew",
            "Coral",
            "Crimson",
            "Cobalt",
            "Chartreuse",
            "Charcoal",
            "Champagne",
            "Buff",
            "Burgundy"
        };

        private readonly string[] _directions = {
            "N",
            "NE",
            "E",
            "SE",
            "S",
            "SW",
            "W",
            "NW"
        };

        private readonly DbContextFactory<ContactContext> _factory;


        private readonly string[] _lastNames = {
            "Smith",
            "Johnson",
            "Williams",
            "Jones",
            "Brown",
            "Davis",
            "Miller",
            "Wilson",
            "Moore",
            "Taylor",
            "Anderson",
            "Thomas",
            "Jackson",
            "White",
            "Harris",
            "Martin",
            "Thompson",
            "Garcia",
            "Martinez",
            "Robinson",
            "Clark",
            "Rodriguez",
            "Lewis",
            "Lee",
            "Walker",
            "Hall",
            "Allen",
            "Young",
            "Hernandez",
            "King",
            "Wright",
            "Lopez",
            "Hill",
            "Scott",
            "Green",
            "Adams",
            "Baker",
            "Gonzalez",
            "Nelson",
            "Carter",
            "Mitchell",
            "Perez",
            "Roberts",
            "Turner",
            "Phillips",
            "Campbell",
            "Parker",
            "Evans",
            "Edwards",
            "Collins",
            "Stewart",
            "Sanchez",
            "Morris",
            "Rogers",
            "Reed",
            "Cook",
            "Morgan",
            "Bell",
            "Murphy",
            "Bailey",
            "Rivera",
            "Cooper",
            "Richardson",
            "Cox",
            "Howard",
            "Ward",
            "Torres",
            "Peterson",
            "Gray",
            "Ramirez",
            "James",
            "Watson",
            "Brooks",
            "Kelly",
            "Sanders",
            "Price",
            "Bennett",
            "Wood",
            "Barnes",
            "Ross",
            "Henderson",
            "Coleman",
            "Jenkins",
            "Perry",
            "Powell",
            "Long",
            "Patterson",
            "Hughes",
            "Flores",
            "Washington",
            "Butler",
            "Simmons",
            "Foster",
            "Gonzales",
            "Bryant",
            "Alexander",
            "Russell",
            "Griffin",
            "Diaz",
            "Hayes",
            "Myers",
            "Ford",
            "Hamilton",
            "Graham",
            "Sullivan",
            "Wallace",
            "Woods",
            "Cole",
            "West",
            "Jordan",
            "Owens",
            "Reynolds",
            "Fisher",
            "Ellis",
            "Harrison",
            "Gibson",
            "Mcdonald",
            "Cruz",
            "Marshall",
            "Ortiz",
            "Gomez",
            "Murray",
            "Freeman",
            "Wells",
            "Webb",
            "Simpson",
            "Stevens",
            "Tucker",
            "Porter",
            "Hunter",
            "Hicks",
            "Crawford",
            "Henry",
            "Boyd",
            "Mason",
            "Morales",
            "Kennedy",
            "Warren",
            "Dixon",
            "Ramos",
            "Reyes",
            "Burns",
            "Gordon",
            "Shaw",
            "Holmes",
            "Rice",
            "Robertson",
            "Hunt",
            "Black",
            "Daniels",
            "Palmer",
            "Mills",
            "Nichols",
            "Grant",
            "Knight",
            "Ferguson",
            "Rose",
            "Stone",
            "Hawkins",
            "Dunn",
            "Perkins",
            "Hudson",
            "Spencer",
            "Gardner",
            "Stephens",
            "Payne",
            "Pierce",
            "Berry",
            "Matthews",
            "Arnold",
            "Wagner",
            "Willis",
            "Ray",
            "Watkins",
            "Olson",
            "Carroll",
            "Duncan",
            "Snyder",
            "Hart",
            "Cunningham",
            "Bradley",
            "Lane",
            "Andrews",
            "Ruiz",
            "Harper",
            "Fox",
            "Riley",
            "Armstrong",
            "Carpenter",
            "Weaver",
            "Greene",
            "Lawrence",
            "Elliott",
            "Chavez",
            "Sims",
            "Austin",
            "Peters",
            "Kelley",
            "Franklin",
            "Lawson",
            "Fields",
            "Gutierrez",
            "Ryan",
            "Schmidt",
            "Carr",
            "Vasquez",
            "Castillo",
            "Wheeler",
            "Chapman",
            "Oliver",
            "Montgomery",
            "Richards",
            "Williamson",
            "Johnston",
            "Banks",
            "Meyer",
            "Bishop",
            "Mccoy",
            "Howell",
            "Alvarez",
            "Morrison",
            "Hansen",
            "Fernandez",
            "Garza",
            "Harvey",
            "Little",
            "Burton",
            "Stanley",
            "Nguyen",
            "George",
            "Jacobs",
            "Reid",
            "Kim",
            "Fuller",
            "Lynch",
            "Dean",
            "Gilbert",
            "Garrett",
            "Romero",
            "Welch",
            "Larson",
            "Frazier",
            "Burke",
            "Hanson",
            "Day",
            "Mendoza",
            "Moreno",
            "Bowman",
            "Medina",
            "Fowler",
            "Brewer",
            "Hoffman",
            "Carlson",
            "Silva",
            "Pearson",
            "Holland",
            "Douglas",
            "Fleming",
            "Jensen",
            "Vargas",
            "Byrd",
            "Davidson",
            "Hopkins",
            "May",
            "Terry",
            "Herrera",
            "Wade",
            "Soto",
            "Walters",
            "Curtis",
            "Neal",
            "Caldwell",
            "Lowe",
            "Jennings",
            "Barnett",
            "Graves",
            "Jimenez",
            "Horton",
            "Shelton",
            "Barrett",
            "Obrien",
            "Castro",
            "Sutton",
            "Gregory",
            "Mckinney",
            "Lucas",
            "Miles",
            "Craig",
            "Rodriquez",
            "Chambers",
            "Holt",
            "Lambert",
            "Fletcher",
            "Watts",
            "Bates",
            "Hale",
            "Rhodes",
            "Pena",
            "Beck",
            "Newman",
            "Haynes",
            "Mcdaniel",
            "Mendez",
            "Bush",
            "Vaughn",
            "Parks",
            "Dawson",
            "Santiago",
            "Norris",
            "Hardy",
            "Love",
            "Steele",
            "Curry",
            "Powers",
            "Schultz",
            "Barker",
            "Guzman",
            "Page",
            "Munoz",
            "Ball",
            "Keller",
            "Chandler",
            "Weber",
            "Leonard",
            "Walsh",
            "Lyons",
            "Ramsey",
            "Wolfe",
            "Schneider",
            "Mullins",
            "Benson",
            "Sharp",
            "Bowen",
            "Daniel",
            "Barber",
            "Cummings",
            "Hines",
            "Baldwin",
            "Griffith",
            "Valdez",
            "Hubbard",
            "Salazar",
            "Reeves",
            "Warner",
            "Stevenson",
            "Burgess",
            "Santos",
            "Tate",
            "Cross",
            "Garner",
            "Mann",
            "Mack",
            "Moss",
            "Thornton",
            "Dennis",
            "Mcgee",
            "Farmer",
            "Delgado",
            "Aguilar",
            "Vega",
            "Glover",
            "Manning",
            "Cohen",
            "Harmon",
            "Rodgers",
            "Robbins",
            "Newton",
            "Todd",
            "Blair",
            "Higgins",
            "Ingram",
            "Reese",
            "Cannon",
            "Strickland",
            "Townsend",
            "Potter",
            "Goodwin",
            "Walton",
            "Rowe",
            "Hampton",
            "Ortega",
            "Patton",
            "Swanson",
            "Joseph",
            "Francis",
            "Goodman",
            "Maldonado",
            "Yates",
            "Becker",
            "Erickson",
            "Hodges",
            "Rios",
            "Conner",
            "Adkins",
            "Webster",
            "Norman",
            "Malone",
            "Hammond",
            "Flowers",
            "Cobb",
            "Moody",
            "Quinn",
            "Blake",
            "Maxwell",
            "Pope",
            "Floyd",
            "Osborne",
            "Paul",
            "Mccarthy",
            "Guerrero",
            "Lindsey",
            "Estrada",
            "Sandoval",
            "Gibbs",
            "Tyler",
            "Gross",
            "Fitzgerald",
            "Stokes",
            "Doyle",
            "Sherman",
            "Sa"
        };

        private readonly Random _random = new Random();

        private readonly string[] _states = {
            "AL", "AK", "AZ", "AR", "CA", "CO", "CT", "DE", "FL", "GA", "HI", "ID", "IL", "IN", "IA", "KS", "KY", "LA",
            "ME", "MD", "MA", "MI", "MN", "MS", "MO", "MT", "NE", "NV", "NH", "NJ", "NM", "NY", "NC", "ND", "OH", "OK",
            "OR", "PA", "RI", "SC", "SD", "TN", "TX", "UT", "VT", "VA", "WA", "WV", "WI", "WY", "AB", "BC", "MB", "NB",
            "NL", "NS", "NT", "NU", "ON", "PE", "QC", "SK", "YT", "NSW", "VIC", "QLD", "SA", "WA", "TAS", "NT", "ACT",
            "BW", "BY", "BE", "BB", "HB", "HH", "HE", "MV", "NI", "NW", "RP", "SL", "SN", "ST", "SH", "TH"
        };

        private readonly string[] _streets = {
            "Adams", "Albert", "Ash", "Avenue", "Bay", "Beach", "Birch", "Bridge", "Broadway",
            "Cameron", "Cedar", "Chestnut", "Chicago", "Church", "Clark", "Cleveland", "Creek",
            "Douglas", "Dufferin", "Eighth", "Elm", "Elmwood", "Fifth", "First", "Fourth", "Franklin",
            "Fraser", "Front", "Grant", "Harrison", "High", "Hill", "Jackson", "Jefferson", "King",
            "Lake", "Larch", "Lee", "Lincoln", "Madison", "Main", "Maple", "Market", "Mitchell",
            "Mountain", "Ninth", "Oak", "Ontario", "Park", "Pine", "Poplar", "Queen", "River",
            "Second", "Seventh", "Sixth", "Spring", "Spruce", "Street", "Taylor", "Third", "Victoria",
            "Walnut", "Washington", "Water", "Willow", "Wilson", "Yonge"
        };


        private readonly string[] _streetSuffixes = {
            "Street", "Road", "Avenue", "Lane", "Drive", "Boulevard", "Court", "Place", "Circle", "Way",
            "Highway", "Trail", "Parkway", "Pike", "Boulevard West", "Boulevard East", "Junction", "Byway", "Plaza",
            "Terrace",
            "Ridge", "Heights", "Meadows", "Grove", "Estates", "Park", "Square", "Crossing", "Bypass", "Causeway",
            "Alley", "Walk", "Mall", "Street North", "Street South", "Street East", "Street West", "Highway East",
            "Highway West",
            "Ridge Road", "Valley Road", "Creek Road", "Mill Road", "Beach Road", "Lake Road", "Hilltop Drive",
            "Park Lane", "Garden Lane", "Bayside Drive"
        };

        private string[] commonNames = {
            "Emma", "Olivia", "Noah", "Liam", "Ava", "Sophia", "Isabella", "William",
            "James", "Charlotte", "Mia", "Abigail", "Elijah", "Mason", "Evelyn", "Lucas",
            "Benjamin", "Amelia", "Aiden", "Harper", "Jackson", "Elizabeth", "Michael", "Avery",
            "Emily", "David", "Sofia", "Matthew", "Evelyn", "Aiden", "Scarlett", "Daniel",
            "Chloe", "Anthony", "Madison", "Logan", "Ella", "Joseph", "Charlotte", "Olivia",
            "Alexander", "Luna", "Henry", "Abigail", "Andrew", "Evelyn", "Elizabeth", "Gabriel",
            "Eleanor", "Joshua", "Benjamin", "Amelia", "Christopher", "Harper"
        };


        public ContactGenerator(DbContextFactory<ContactContext> factory) {
            _factory = factory;
        }


        public static string SafeRandom(string[] arr) {
            if (arr == null || arr.Length == 0) throw new ArgumentException("The provided name list is empty.");

            // Use Random class to generate a random index
            Random random = new Random();
            int randomIndex = random.Next(arr.Length);

            // Return the name at the random index
            return arr[randomIndex];
        }

        private string RandomOne(string[] list) {
            int idx = _random.Next(list.Length - 1);
            return list[idx];
        }

        private Contact MakeContact() {
            Contact contact = new Contact {
                FirstName = SafeRandom(_lastNames),
                LastName = SafeRandom(_lastNames),
                Phone = $"({_random.Next(100, 999)})-{_random.Next(100, 999)}-{_random.Next(1000, 9999)}",
                Street = $"{_random.Next(1, 99999)} {_random.Next(1, 999)}" +
                         $" {RandomOne(_streets)} {RandomOne(_streetSuffixes)} {RandomOne(_directions)}",
                City = SafeRandom(_cities),
                State = SafeRandom(_states),
                ZipCode = $"{_random.Next(10000, 99999)}"
            };
            return contact;
        }

        public async Task CheckAndSeedDatabaseAsync(ClaimsPrincipal user) {
            using (ContactContext context = _factory.CreateDbContext()) {
                context.User = user;
                bool created = await context.Database.EnsureCreatedAsync();
                if (created) await SeedDatabaseWithContactCountOfAsync(context, 100);
            }
        }

        private async Task SeedDatabaseWithContactCountOfAsync(ContactContext context, int totalCount) {
            int count = 0;
            int i = 0;

            while (count < totalCount) {
                List<Contact> list = new List<Contact>();
                while (i++ < 100 && count++ < totalCount) list.Add(MakeContact());
                if (list.Count > 0) {
                    context.Contacts.AddRange(list);
                    await context.SaveChangesAsync();
                }

                i = 0;
            }
        }

        public static async Task<T> DoPostRequestJsonAsync<T>(string udid, string cmd, object postData)
            where T : class, new() {
            JObject serializableObject = CreateRequestJson(udid, cmd, postData);
            try {
                HttpClient httpClient = new HttpClient();
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage response =
                    await
                        httpClient.PostAsync("https://tools.usps.com/tools/app/ziplookup/zipByAddress",
                            new StringContent(JsonConvert.SerializeObject(serializableObject)));

                response.EnsureSuccessStatusCode();

                string content = await response.Content.ReadAsStringAsync();

                BaseResponseModel<T> deserializedObj = JsonConvert.DeserializeObject<BaseResponseModel<T>>(content);
                return deserializedObj.Body;
            }
            catch (Exception e) {
                Debug.WriteLine(e.Message);


                return null;
            }
        }

        private static JObject CreateRequestJson(string udid, string cmd, object obj) {
            return new JObject(
                new JProperty("from", udid),
                new JProperty("cmd", cmd),
                new JProperty("body", JObject.FromObject(obj)));
        }
    }

    public class BaseResponseModel<T> where T : class {
        public string Version { get; set; }
        public string Type { get; set; }
        public string From { get; set; }
        public string Cmd { get; set; }
        public T Body { get; set; }
    }
}