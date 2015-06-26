using System.Web.UI;
using Common.Logging;
using Spring.Context;

namespace AG.Framework.Web.UI
{
    /// <summary>
    /// Pagina base para as demais p�ginas da aplica��o.
    /// Prov� alguns m�todos utilit�rios que devem ser utilizados pelas classes filhas.
    /// </summary>
    public abstract class AbstractBasePage : Page, IApplicationContextAware 
    {

        /// <summary>
        /// Innst�ncia do log
        /// </summary>
        protected static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private IApplicationContext applicationContext;

        #region IApplicationContextAware Members

        public IApplicationContext ApplicationContext
        {
            get
            {
                return applicationContext;
            }
            set
            {
                applicationContext = value;
            }
        }

        #endregion
    }
}
