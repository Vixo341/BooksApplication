using BooksApplication.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace BooksApplication.DataAccess.Repository.IRepository
{
    public interface ICoverTypeRepository : IRepository<CoverType> 
    {

        void Update(CoverType obj);
    }
}
