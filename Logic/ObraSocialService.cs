using Dao;
using Logic.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logic
{
    public class ObraSocialService
    {
        private static readonly ObraSocialService _instance = new ObraSocialService();
        public static ObraSocialService Instance
        {
            get
            {
                return _instance;
            }
        }
        private ObraSocialService()
        {
        }

        public List<ObraSocial> GetAll()
        {
            using (DentalCareDBEntities context = new DentalCareDBEntities())
            {
                List<ObraSocial> obrasSociales = context.ObraSocial.ToList();

                if (!obrasSociales.Any())
                    throw new NoObraSocialFoundException();

                return obrasSociales;
            }
        }

    }
}
