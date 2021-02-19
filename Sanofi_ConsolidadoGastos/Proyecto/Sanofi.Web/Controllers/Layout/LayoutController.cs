using Sanofi.BM.Layout;
using Sanofi.DT.General;
using Sanofi.DT.Usuarios;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.Web.Mvc;

namespace Sanofi.Web.Controllers.Layout
{
    public class LayoutController : Controller
    {
        // GET: Login
       
        public ActionResult Layout()
        {
            return View();
        }

      
        public ActionResult ConsultarRolModalidadUsuario(string _Usuario)
        {
            Session["UsuarioLogin"] = _Usuario;
            DTResultadoOperacionModel<List<DTMaestroUsuario>> _DTResultadoModel = new DTResultadoOperacionModel<List<DTMaestroUsuario>>();
            _DTResultadoModel = new BMLayout().ConsultarRolModalidadUsuario(_Usuario);
            var jey = Json(_DTResultadoModel, JsonRequestBehavior.AllowGet);
            return jey;

        }

      
        public ActionResult ConsultarUsuario()
        {

            DTResultadoOperacionModel<DTMaestroUsuario> _DTResultadoModel = new DTResultadoOperacionModel<DTMaestroUsuario>();
            _DTResultadoModel = new BMLayout().ConsultarUsuario(User.Identity.Name);
            return Json(_DTResultadoModel, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ConsultarVersion() {

            Assembly assembly = Assembly.GetExecutingAssembly();
            FileVersionInfo fvi = FileVersionInfo.GetVersionInfo(assembly.Location);
            string version = fvi.FileVersion;
            return Json(version, JsonRequestBehavior.AllowGet);

        }
    }
}