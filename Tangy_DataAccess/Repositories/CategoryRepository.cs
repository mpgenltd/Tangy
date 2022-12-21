using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Tangy_DataAccess.Data;
using Tangy_Models;

namespace Tangy_DataAccess.Repositories;

public class CategoryRepository : ICategoryRepository
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;
    
    public CategoryRepository(ApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }
    
    public async Task<CategoryDto> Create(CategoryDto dto)
    {
        var category = _mapper.Map<Category>(dto);
        category.CreatedDate = DateTime.UtcNow;

        await _context.Categories.AddAsync(category);
        await _context.SaveChangesAsync();

        return _mapper.Map<CategoryDto>(category);
    }

    public async Task<CategoryDto> Update(CategoryDto dto)
    {
        var category = await _context.Categories.FirstOrDefaultAsync(c => c.Id == dto.Id);

        if (category != null)
        {
            _mapper.Map(dto, category);
            _context.Update(category);
            await _context.SaveChangesAsync();

            return _mapper.Map<CategoryDto>(category);
        }

        return dto;
    }
    
    public async Task<int> Delete(int id)
    {
        var category = await _context.Categories.FirstOrDefaultAsync(c => c.Id == id);
        if (category != null)
        {
            _context.Categories.Remove(category);
            return await _context.SaveChangesAsync();
        }

        return 0;
    }

    public async Task<CategoryDto> Get(int id)
    {
        var category = await _context.Categories.FirstOrDefaultAsync(c => c.Id == id);

        if (category != null)
        {
            return _mapper.Map<CategoryDto>(category);
        }

        return null;
    }

    public async Task<IEnumerable<CategoryDto>> GetAll()
    {
        var categories = await _context.Categories.ToListAsync();
        
        return _mapper.Map<IEnumerable<CategoryDto>>(categories);
    }
}