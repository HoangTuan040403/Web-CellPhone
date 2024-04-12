using Doanphanmem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Doanphanmem.Pattern
{
    public class Category_pattern
    {

        private static Category_pattern _instance;
        private QL_CHDTEntities _dbContext;

        private Category_pattern()
        {
            _dbContext = new QL_CHDTEntities(); 
        }

        public static Category_pattern GetInstance()
        {
            if (_instance == null)
            {
                _instance = new Category_pattern();
            }
            return _instance;
        }

        public IEnumerable<PhanLoai> GetAllPhanLoais()
        {
            return _dbContext.PhanLoais.ToList();
        }

    }
}