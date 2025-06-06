using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWork.Repository.Repository
{
    public class EmployerRepository(SWorkDbContext context) : GenericRepository<Employer>(context), IEmployerRepository
    {
    }
}
