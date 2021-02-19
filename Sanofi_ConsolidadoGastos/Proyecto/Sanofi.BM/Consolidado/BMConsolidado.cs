using Sanofi.DM.Consolidado;
using Sanofi.DT.Consolidado;
using Sanofi.DT.General;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sanofi.BM.Consolidado
{
    public class BMConsolidado
    {
        public List<DTConsolidadoExcel> ConsultarConsolidadoExcel(DTFiltros Dtos)
        {
            List<DTConsolidadoExcel> result = new List<DTConsolidadoExcel>();
            try
            {
                result = new DMConsolidado().ConsultarConsolidadoExcel(Dtos);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }
    }
}
