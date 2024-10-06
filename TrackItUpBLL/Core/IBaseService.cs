using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrackItUpBLL.Core
{
    public interface IBaseService
    {
        Task<ServiceResult> GetAll();
        Task<ServiceResult> GetById(int id);
    }
}
