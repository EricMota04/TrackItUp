using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrackItUpBLL.Core
{
    public interface IBaseService
    {
        ServiceResult GetAll();
        ServiceResult GetById(int id);
    }
}
