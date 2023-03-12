using System;
using System.Collections.Generic;
using System.Text;

namespace Internal.SSO.Repositories.Bases
{
    public interface IGenericRepository<TEntity, TPrimaryKey> where TEntity : class
    {
        TEntity Get(TPrimaryKey id);
        IEnumerable<TEntity> GetAll();
        TPrimaryKey Add(TEntity entity);
        void Delete(TPrimaryKey id);
        void Update(TEntity entity);
    }
}
