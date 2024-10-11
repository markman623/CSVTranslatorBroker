using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SourceCode.SmartObjects.Services.ServiceSDK;
using SourceCode.SmartObjects.Services.ServiceSDK.Objects;
using SourceCode.SmartObjects.Services.ServiceSDK.Attributes;
using SourceCode.SmartObjects.Services.ServiceSDK.Types;

namespace K2Translations
{
    public class K2TranslationsServiceBroker : ServiceAssemblyBase
    {
        public override string GetConfigSection()
        {
            try
            {
                this.Service.ServiceConfiguration.Add("APIKey", true, "Need a key");
                this.Service.ServiceConfiguration.Add("ServiceURL", true, "https://api.cognitive.microsofttranslator.com/");
                this.Service.ServiceConfiguration.Add("Region", false, "eastus");
            }
            catch (Exception ex){
                ServicePackage.ServiceMessages.Add(ex.Message, MessageSeverity.Error);
            }
            return base.GetConfigSection();
        }

        public override string DescribeSchema()
        {
            try
            {
                this.Service.ServiceObjects.Create(new ServiceObject(typeof(K2Translations)));
                this.Service.Name = "K2Translator";
                this.Service.MetaData.DisplayName = "K2Translator";
                this.Service.MetaData.Description = "A service to translate terms, and eventually K2 translation CSVSs.";
            }
            catch (Exception ex) 
            {
                ServicePackage.ServiceMessages.Add(ex.Message, MessageSeverity.Error);
                ServicePackage.IsSuccessful = false;
            }

            return base.DescribeSchema();
        }

        public override void Extend()
        {
            throw new NotImplementedException();
        }
    }
}
