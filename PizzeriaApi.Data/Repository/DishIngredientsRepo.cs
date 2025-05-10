using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PizzeriaApi.Data.DataModels;
using PizzeriaApi.Data.Interfaces;
using PizzeriaApi.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PizzeriaApi.Data.Repository
{
    public class DishIngredientsRepo:IDishIngredientsRepo
    {
        private readonly PizzeriaApiDBContext _dbContext;
        private readonly ILogger<DishIngredientsRepo> _logger;

        public DishIngredientsRepo(PizzeriaApiDBContext dbContext, ILogger<DishIngredientsRepo> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        
    }
}
