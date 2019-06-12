namespace Pulse.Core.EmailTemplete
{
    using Model;
    using RazorEngine.Configuration;
    using RazorEngine.Templating;
    using System;

    public class SimpleEmailTemplate : IProcessEmailTemplate
    {
        private const string PATH_TEMPLATE = "EmailTemplate/Template.cshtml";

        private const string TEMPLATEKEY = "templateKey";

        public string GenerateEmailTemplate(object model)
        {
            return Generate((LoginModel)model);
        }

        private string Generate(LoginModel model)
        {
            var config = new TemplateServiceConfiguration();

            var service = RazorEngineService.Create(config);

            var template = System.IO.File.ReadAllText(AppDomain.CurrentDomain.BaseDirectory + PATH_TEMPLATE);

            var body = service.RunCompile(template, TEMPLATEKEY, typeof(LoginModel), model);

            return body;

        }
    }
}
