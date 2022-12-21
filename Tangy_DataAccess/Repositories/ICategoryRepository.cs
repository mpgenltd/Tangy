using Tangy_Models;

namespace Tangy_DataAccess.Repositories;

public interface ICategoryRepository
{
    public Task<CategoryDto> Create(CategoryDto dto);
    public Task Update(CategoryDto dto);
    public Task<int> Delete(int id);
    public Task<CategoryDto> Get(int id);
    public Task<IEnumerable<CategoryDto>> GetAll();
}