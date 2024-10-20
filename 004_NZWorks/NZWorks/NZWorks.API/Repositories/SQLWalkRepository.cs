using Microsoft.EntityFrameworkCore;
using NZWorks.API.Data;
using NZWorks.API.Models.Domain;

namespace NZWorks.API.Repositories
{
    public class SQLWalkRepository : IWalkRepository
    {
        private readonly NZWalksDBContext _dbContext;

        public SQLWalkRepository(NZWalksDBContext dBContext)
        {
            _dbContext = dBContext;
        }
        public async Task<Walk> CreateAsync(Walk walk)
        {
            await _dbContext.AddAsync(walk);
            await _dbContext.SaveChangesAsync();
            return walk;
        }

        public async Task<Walk?> DeleteAsync(Guid id)
        {
            var existingWalk = await _dbContext.Walks.FirstOrDefaultAsync(x => x.Id == id);

            if (existingWalk == null)
            {
                return null;
            }

            _dbContext.Walks.Remove(existingWalk);
            await _dbContext.SaveChangesAsync();
            return existingWalk;
        }

        public async Task<List<Walk>> GetAllAsync(
            string? filterOn = null, string? filterQuery = null,
            string? sortBy = null, bool isAscending = true,
            int pageNumber = 1, int pageSize = 1000)
        {
            var walks = _dbContext.Walks.Include("Difficulty").Include("Region");

            // Filtering
            if(string.IsNullOrWhiteSpace(filterOn) == false && string.IsNullOrWhiteSpace(filterQuery) == false)
            {
                if(filterOn.Equals(nameof(Walk.Name), StringComparison.OrdinalIgnoreCase))
                {
                    walks = walks.Where(x => x.Name.Contains(filterQuery));
                }
            }

            // Sorting
            if(string.IsNullOrWhiteSpace(sortBy) == false)
            {
                if(sortBy.Equals(nameof(Walk.Name), StringComparison.OrdinalIgnoreCase))
                {
                    walks = isAscending ? walks.OrderBy(x => x.Name) : walks.OrderByDescending(x => x.Name);
                }
                else if (sortBy.Equals(nameof(Walk.LengthInks), StringComparison.OrdinalIgnoreCase))
                {
                    walks = isAscending ? walks.OrderBy(x => x.LengthInks) : walks.OrderByDescending(x => x.LengthInks);
                }
            }

            // Pagination
            var skipResults = (pageNumber - 1) * pageSize;
            return await walks.Skip(skipResults).Take(pageSize).ToListAsync();

            //return await _dbContext.Walks.Include("Difficulty").Include("Region").ToListAsync();
        }

        public async Task<Walk?> GetByIdAsync(Guid id)
        {
            return await _dbContext.Walks
                .Include("Difficulty")
                .Include("Region")
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<Walk?> UpdateAsync(Guid id, Walk walk)
        {
            var existingWalk = await _dbContext.Walks.FirstOrDefaultAsync(x => x.Id == id);

            if (existingWalk == null)
            {
                return null;
            }

            existingWalk.Name = walk.Name;
            existingWalk.Description = walk.Description;
            existingWalk.LengthInks = walk.LengthInks;
            existingWalk.DifficultyId = walk.DifficultyId;
            existingWalk.RegionId = walk.RegionId;

            await _dbContext.SaveChangesAsync();

            return existingWalk;
        }
    }
}
