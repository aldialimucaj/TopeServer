using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Nancy.Hosting;
using System.Web.Http.SelfHost;
using System.Web.Http.SelfHost.Channels;

namespace TopeServer.al.aldi.topeServer.control
{
    class TopeHttpsSelfHostConfiguration : HttpSelfHostConfiguration
    {
        public TopeHttpsSelfHostConfiguration(string baseAddress) : base(baseAddress) { }

        public TopeHttpsSelfHostConfiguration(Uri baseAddress) : base(baseAddress) { }

        protected override System.ServiceModel.Channels.BindingParameterCollection OnConfigureBinding(HttpBinding httpBinding)
        {
            httpBinding.Security.Mode = HttpBindingSecurityMode.Transport;
            return base.OnConfigureBinding(httpBinding);
        }
    }
}
