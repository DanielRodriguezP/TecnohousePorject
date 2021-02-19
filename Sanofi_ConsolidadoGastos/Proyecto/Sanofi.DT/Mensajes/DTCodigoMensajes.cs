//####################################################################
// DEPENDÊNCIAS:    SanofiXC
// AUTOR:           Juan Esteban Parra Correa
// Descrição:       Arquitectura inicial aplicación
// DATA:            16/08/2020
//####################################################################
namespace Sanofi.DT.Mensajes
{
    public class DTCodigoMensajes
    {
        /// <summary>
        /// Se ha presentado un error. Por favor comunicarse con el administrador.
        /// </summary>
        public const int MENSAJE001 = 1;
        /// <summary>
        /// Los nombres de los campos del archivo no coinciden con el formato definido, por favor revise la plantilla e intente de nuevo..
        /// </summary>
        public const int MENSAJE002 = 2;
        /// <summary>
        /// La plantilla no se encuentra disponible, sin embargo puede realizar la carga con las columnas:Identificacíon Selas, IdentificaciónReceptor, Tipo identificación, NombreReceptor, CódigoMunicipio, Municipio, IdTipoReceptor, Tipo receptor, CódigoSociedad, Sociedad científica, Especialidad, Identificacion Reemplazante, Consentimiento Informado.
        /// </summary>
        public const int MENSAJE003 = 3;
        /// <summary>
        /// El archivo no contiene información. Por favor validar
        /// </summary>
        public const int MENSAJE004 = 4;
        /// <summary>
        ///"La plantilla no se encuentra disponible, sin embargo puede realizar la carga con las columnas: Identificaci\u00F3n receptor, Identificaci\u00F3n Selas, Tipop alimento,ID Evento, Nombre evento, Fecha transferencia, Valor transferencia, Indicador Receptor, Tipo tercero, Eliminar. ",
        /// </summary>
        public const int MENSAJE005 = 5;
        /// <summary>
        /// "Debe seleccionar un archivo con formato xls o xlsx"
        /// </summary>
        public const int MENSAJE006 = 6;
        /// <summary>
        /// "¿Realmente desea eliminar registros?"
        /// </summary>
        public const int MENSAJE007 = 7;
        /// <summary>
        /// Se presentó un error al momento de guardar la Información
        /// </summary>
        public const int MENSAJE008 = 8;
        /// <summary>
        /// El archivo no contine registros válidos. Por favor verifique la información.
        /// </summary>
        public const int MENSAJE009 = 9;

        /// <summary>
        ///La plantilla no se encuentra disponible, sin embargo puede realizar la carga
        /// con las columnas: Identificación receptor, OneKey ID, Nit agenci, Id Evento, Nombre evento, Fecha transferencia, Valor tiquete,
        ///Valor alojamiento, Valor traslados, Valor tarjeta de asistencias, Valor por visa, Valor inscripción, 
        ///Indicador Receptor, Tipo tercero, Eliminar.
        /// </summary>
        /// 

        public const int MENSAJE010 = 10;
        /// <summary>
        /// No se encontrarón coincidencias.
        /// </summary>
        public const int MENSAJE011 = 11;
        /// <summary>
        /// La plantilla no se encuentra disponible, sin embargo puede realizar la carga con las columnas: Identificación receptor, ID evento, Nombre evento, Fecha transferencia, 
        /// Valor transferencia, Indicador Receptor, TipoTercero, Eliminar
        /// </summary>
        public const int MENSAJE012 = 12;

        ///<summary>
        ///La plantilla no se encuentra disponible, sin embargo puede realizar la carga con las columnas: Identificación receptor, IdEvento, Nombre evento,
        ///Fecha transferencia, Valor transferencia, Indicador Receptor, Tipo tercero, Eliminar
        ///<summary>

        /// </summary>
        public const int MENSAJE013 = 13;


        ///<summary>
        //La plantilla no se encuentra disponible, sin embargo puede realizar la carga con las columnas: Identificaci\u00F3n receptor,Identificaci\u00F3n Selas,Fecha transferencia, Valor transferencia,NroDocSAP,
        //Indicador Receptor, Tipo tercero, Eliminar.
        ///<summary>
        
        public const int MENSAJE014 = 14;

        /// <summary>
        /// La plantilla no se encuentra disponible, sin embargo puede realizar la carga con las columnas: Identificación receptor, OneKey ID, Tipo identificación, Nombre Receptor, Código Municipio, Municipio Residencia, IdTipoReceptor, Tipo Receptor, Código sociedad, Sociedad científica, Especialidad, Identificación Reemplazante, Consentimiento Informado
        /// </summary>
        public const int MENSAJE016 = 16;

        /// <summary>
        /// La plantilla no se encuentra disponible, sin embargo puede realizar la carga con las columnas: Identificación receptor, OneKey ID, Fecha transferencia, Valor transferencia, Contrato SAP, Indicador Receptor, Tipo tercero , Eliminar
        /// </summary>
        public const int MENSAJE017 = 17;

        /// <summary>
        /// La plantilla no se encuentra disponible, sin embargo puede realizar la carga con las columnas: Código SAP, Producto, IUM, CUM y Código SAP de reemplazo
        /// </summary>
        public const int MENSAJE018 = 18;
        /// <summary>
        /// La plantilla no se encuentra disponible, sin embargo puede realizar la carga con las columnas: Identificaci\u00F3n receptor, Identificaci\u00F3n Selas, Fecha transferencia, Valor transferencia, Indicador Receptor, Tipo tercero,NroDocSAP,Cod promomat,Material, Eliminar.
        /// </summary>
        public const int MENSAJE019 = 19;
        /// <summary>
        ///  @¿Realmente desea remplazar los registros?
        /// </summary>
        public const int MENSAJE020 = 20;

        /// <summary>
        /// El usuario no existe en la Base de Datos.
        /// </summary>
        public const int MENSAJE021 = 21;

        /// <summary>
        /// El formato del mes de los gastos no concuerda con la estipulada (DD-YYYY).
        /// </summary>
        public const int MENSAJE022 = 22;
        /// <summary>
        /// Se genero el error al momento de generar el archivo excel.
        /// </summary>
        public const int MENSAJE023 = 23;

        /// <summary>
        /// No hay registros que cumplan con los criterios seleccionados.
        /// </summary>
        public const int MENSAJE024 = 24;
    }
}