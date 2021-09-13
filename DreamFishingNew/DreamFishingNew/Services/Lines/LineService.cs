using DreamFishingNew.Data;
using DreamFishingNew.Data.Models;
using DreamFishingNew.Models.Lines;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace DreamFishingNew.Services.Lines
{
    
    public class LineService: ILineService
    {
        private ApplicationDbContext data;

        public LineService(ApplicationDbContext data)
        {
            this.data = data;
        }

        public void CreateLine(AddLineFormModel line, Brand brand)
        {
            var currentLine = new Line
            {
                Model = line.Model,
                BrandId = brand.Id,
                Image = line.Image,
                Price = line.Price,
                Length = line.Length,
                Weight = line.Weight,
                Size = line.Size,
                Description = line.Description,
                Quantity = line.Quantity
            };

            data.Lines.Add(currentLine);
            data.SaveChanges();
        }

        public void DecrementLineQuantity(Line currLine)
        {
            currLine.Quantity--;

            if (currLine.Quantity < 0)
            {
                currLine.Quantity = 0;
            }

            data.SaveChanges();
        }

        public void DeleteLine(Line line)
        {
            data.Lines.Remove(line);
            data.SaveChanges();
        }

        public void EditLine(int id, AddLineFormModel item)
        {
            var bait = data
                .Lines
                .Include("Brand")
                .Where(x => x.Id == id)
                .FirstOrDefault();

            bait.Model = item.Model;
            bait.Brand.Name = item.Brand;
            bait.Length = item.Length;
            bait.Description = item.Description;
            bait.Image = item.Image;
            bait.Size = item.Size;
            bait.Price = item.Price;
            bait.Quantity = item.Quantity;
            bait.Weight = item.Weight;

            data.SaveChanges();
        }

        public ICollection<Line> GetAllLines()
        {
            var linesQuery = data.Lines
                .Include("Brand")
                .ToList();

            return linesQuery;
        }

        public Brand GetLineBrandByName(AddLineFormModel line)
        {
            var brand = data
                .Brands
                .FirstOrDefault(x => x.Name.ToLower() == line.Brand.ToLower());

            return brand;
        }

        public ICollection<string> GetLineBrands()
        {
            var lineBrands = data
                .Lines
                .Select(x => x.Brand.Name)
                .Distinct()
                .ToList();

            return lineBrands;
        }

        public Line GetLineById(int id)
        {
            var line = data
                .Lines
                .Include("Brand")
                .FirstOrDefault(x => x.Id == id);

            return line;
        }

        public AddLineFormModel GetLineEditModel(int id)
        {
            var model = data.Lines
                .Where(x => x.Id == id)
                .Select(x => new AddLineFormModel 
                {              
                Model = x.Model,
                Brand = x.Brand.Name,
                Length = x.Length,
                Size = x.Size,
                Weight = x.Weight,
                Description = x.Description,
                Image = x.Image,
                Price = x.Price,
                Quantity = x.Quantity
                })
                .FirstOrDefault();

            return model;
        }

        public ICollection<Line> GetLinesByBrand(ICollection<Line> linesQuery, AllLinesQueryModel query)
        {
            linesQuery = linesQuery
                    .Where(x => x.Brand.Name.ToLower() == query.Brand.ToLower())
                    .ToList();

            return linesQuery;
        }

        public ICollection<LineListingViewModel> GetLinesByPage(ICollection<Line> linesQuery, AllLinesQueryModel query)
        {
            var linesByPage = linesQuery
                .Skip((query.currentPage -1) * AllLinesQueryModel.LinesPerPage)
                .Take(AllLinesQueryModel.LinesPerPage)
                .Select(x => new LineListingViewModel
                {
                    Id = x.Id,
                    Model = x.Model,
                    Brand = x.Brand.Name,
                    Image = x.Image,
                    Price = x.Price,
                })
                .ToList();

            return linesByPage;
        }

        public ICollection<Line> GetLinesBySearchTerm(ICollection<Line> linesQuery, AllLinesQueryModel query)
        {
            linesQuery = linesQuery
                    .Where(x => (x.Brand.Name + " " + x.Model).ToLower()
                    .Contains(query.SearchTerm.ToLower())||x.Description.ToLower()
                    .Contains(query.SearchTerm.ToLower()))
                    .ToList();

            return linesQuery;
        }

        public ICollection<Line> GetLinesBySortTerm(ICollection<Line> linesQuery, AllLinesQueryModel query)
        {
            linesQuery = query.Sorting switch
            {
                LineSorting.MinPrice => linesQuery.OrderBy(x => x.Price).ToList(),
                LineSorting.MaxPrice => linesQuery.OrderByDescending(x => x.Price).ToList(),
                LineSorting.BrandAndModel => linesQuery.OrderBy(x => x.Brand.Name).ThenBy(x => x.Model).ToList(),
                _ => linesQuery.OrderBy(x => x.Brand.Name).ThenBy(x => x.Model).ToList()
            };

            return linesQuery;
        }
    }
}
