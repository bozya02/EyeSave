using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EyeSave.DB
{
    public partial class EyeSaveEntities 
    {
        private static EyeSaveEntities _context;

        public static EyeSaveEntities GetContext()
        {
            if (_context == null)
                _context = new EyeSaveEntities();

            return _context;
        }
    }
}
