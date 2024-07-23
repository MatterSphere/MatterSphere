using System.Collections.Generic;

namespace FWBS.OMS.Design.CodeBuilder

{
    internal class Methods : List<EventMethodData>
    {
        public Methods()
        {

        }

        public Methods(IEnumerable<EventMethodData> data)
            : base(data)
        {

        }
    }

}
