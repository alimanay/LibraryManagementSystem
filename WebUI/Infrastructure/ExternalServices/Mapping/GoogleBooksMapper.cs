using Entites.Models;
using Infrastructure.ExternalServices.DTOs;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.ExternalServices.Mapping
{
    public static class GoogleBooksMapper
    {
        public static Book ToBookEntity(Item item)
        {
            try
            {
                return new Book
                {
                    Title = item.VolumeInfo?.Title,
                    Description = item.VolumeInfo?.Description,
                    Author = item.VolumeInfo?.Authors.FirstOrDefault(),
                    Image = item.VolumeInfo?.ImageLinks?.Thumbnail,
                    ISBN = item.VolumeInfo.Industry?.Select(a => a.Identifier).FirstOrDefault(),
                    Type = item.VolumeInfo.Industry?.Select(b => b.Type).FirstOrDefault(),
                    IsActive = true,
                };

            }
            catch(Exception ex) {
                throw new Exception("HATA:" + ex);
            }
        }
    }
}
