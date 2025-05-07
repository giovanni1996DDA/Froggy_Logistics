using Dao;
using Logic.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logic
{
    public class TipoDocumentoService
    {
        private static TipoDocumentoService _instance = new TipoDocumentoService();

        public static TipoDocumentoService Instance
        {
            get
            {
                return _instance;
            }
        }
        private TipoDocumentoService()
        {
        }

        public List<TipoDocumento> Get()
        {
            using (DentalCareDBEntities context = new DentalCareDBEntities())
            {
                List<TipoDocumento> tiposDocumento = context.TipoDocumento.ToList();

                if (tiposDocumento == null)
                    throw new NoTipoDocumentoFoundException();

                return tiposDocumento;
            }
        }
    }
}
