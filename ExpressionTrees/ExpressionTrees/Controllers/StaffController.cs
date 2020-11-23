using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper.QueryableExtensions;
using Domain;
using Domain.Dto;
using Domain.Entities;
using Domain.Specifications;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ExpressionTrees.Controllers
{
    [Route("api/staff")]
    [ApiController]
    public class StaffController : ControllerBase
    {
        private readonly IApplicationDbContext _context;

        public StaffController(IApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet("staff-all")]
        public async Task<List<Staff>> GetStaff()
        {
            return await _context.Staff
                .Where(StaffSpecification.IsActiveSpec 
                       & StaffSpecification.IsOfficialSpec)
                .ToListAsync();
        }

        [HttpGet("staff-filter")]
        public async Task<List<Staff>> FilterStaff([FromQuery] StaffDto filter)
        {
            return await _context.Staff
                .AutoFilter(filter)
                .ToListAsync();
        }


        [HttpGet("categories-highest")]
        public async Task<List<StaffCategory>> GetCategories()
        {
            return await _context.StaffCategory
                .Where(CategorySpecification.IsHigh)
                .ToListAsync();
        }

        [HttpGet("staff-with-highest-category")]
        public async Task<List<Staff>> HighestCategoryStaff()
        {
            return await _context.Staff
                .Where(x => x.StaffCategory, CategorySpecification.IsHigh)
                .ToListAsync();
        }

        //[HttpGet("staff-all")]
        //public async Task<List<Staff>> GetStaff()
        //{
        //    return await _context.Staff
        //        .Where(StaffSpecification.IsActiveSpec
        //               || StaffSpecification.IsOfficialSpec)
        //        .Where(CategorySpecification.IsHigh)
        //        .ToListAsync();
        //}


        //[HttpGet]
        //public async Task<List<Staff>> GetStaff()
        //{
        //    return await _context.Staff
        //        .Where(Staff.ToShowExpression)
        //        .ToListAsync();
        //}
    }
}
