using Objects.Src;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Domain.Src
{
    public interface IRepository<TModel> where TModel : class, IDto
    {
        Task<TModel> AddAsync(TModel model);

        Task<TModel> UpdateAsync(TModel model);

        Task<ICollection<TModel>> GetAllAsync(Expression<Func<TModel, bool>> query = null);

        Task<TModel> FindByIdAsync(ulong id);

        Task DeleteAsync(ulong id);
    }
}
