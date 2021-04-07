using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ImagesEF.Data
{
    public class ImagesRepository
    {
        private readonly string _connectionString;
        public ImagesRepository(string connectionString)
        {
            _connectionString = connectionString;       
        }
        public List<Image> GetImages()
        {
            using var context = new ImagesDBContext(_connectionString);
            return context.Images.OrderByDescending(i=>i.DateUploaded).ToList();
        }
        public void AddImage(Image i)
        {
            using var context = new ImagesDBContext(_connectionString);
            context.Images.Add(i);
            context.SaveChanges();
        }
        public Image GetById(int id)
        {
            using var context = new ImagesDBContext(_connectionString);
            return context.Images.FirstOrDefault(i => i.Id == id);
        }
        public int GetLikesOfImage(int id)
        {
            using var context = new ImagesDBContext(_connectionString);
            return GetById(id).Likes;
        }
        public void AddLike(int id)
        {
            using var context = new ImagesDBContext(_connectionString);
            context.Images.FirstOrDefault(i => i.Id == id).Likes++;
            context.SaveChanges();
        }
    }
}
