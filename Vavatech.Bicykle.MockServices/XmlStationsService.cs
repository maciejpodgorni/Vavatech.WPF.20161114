﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Vavatech.Bicycle.Interfaces;
using Vavatech.Bicycle.Models;

namespace Vavatech.Bicykle.MockServices
{
    public class XmlStationsService : IStationsService
    {
        private readonly string uri = "https://nextbike.net/maps/nextbike-official.xml";

        public XmlStationsService()
        {
        }

        public XmlStationsService(string uri)
        {
            this.uri = uri;
        }

        public void Add(Station item)
        {
            throw new NotSupportedException();
        }

        public IList<Station> Get()
        {
            throw new NotImplementedException();
        }

        public Station Get(int itemId)
        {
            throw new NotImplementedException();
        }

        public async Task<IList<Station>> GetAsync()
        {
            string country = "Poland";
            string city = "Warszawa";

            using (var client = new HttpClient())
            {
                var content = await client.GetStringAsync(uri);


                // Extract 
                var doc = XDocument.Parse(content)
                 .Root
                 .Descendants("country")
                 .Where(item => item.Attribute("country_name").Value == country)
                 .Descendants("city")
                 .Where(item => item.Attribute("name").Value == city)
                 .Descendants("place")
                 .Select(item => new
                 {
                     Id = item.Attribute("uid").Value,
                     Number = item.Attribute("number")?.Value,
                     Name = item.Attribute("name")?.Value,
                     Latitude = item.Attribute("lat").Value,
                     Longitude = item.Attribute("lng").Value,
                     Capacity = item.Attribute("bike_racks").Value,
                     City = item.Parent.Attribute("name").Value,
                 });


                // Mapper
                var stations = doc.Select(s => new Station
                {
                    StationId = int.Parse(s.Id, CultureInfo.InvariantCulture),
                    Number = s.Name,
                    Address = s.Name,
                    Capacity = short.Parse(s.Capacity, CultureInfo.InvariantCulture),
                    // City = s.City,
                    Location = new Location
                    {
                        Latitude = double.Parse(s.Latitude, CultureInfo.InvariantCulture),
                        Longitude = double.Parse(s.Longitude, CultureInfo.InvariantCulture),
                    },

                });


                return stations.ToList();

            }


        }

        public void Remove(int itemId)
        {
            throw new NotSupportedException();
        }

        public void Update(Station item)
        {
            throw new NotSupportedException();
        }
    }
}
