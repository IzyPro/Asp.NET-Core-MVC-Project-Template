using System;
using System.Threading.Tasks;
using MiniSwitch.Enums;
using MiniSwitch.Models;
using Microsoft.Extensions.Configuration;

namespace MiniSwitch.Services
{
    public interface IMailTemplateServices
    {
        Task<EmailTemplate> GetTemplateAsync(EmailTypesEnum type);
    }

    public class MailTemplateServices : IMailTemplateServices
    {
        private IConfiguration _configuration;
        public MailTemplateServices(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<EmailTemplate> GetTemplateAsync(EmailTypesEnum type)
        {
            var response = new ResponseManager();

            if (type == EmailTypesEnum.ActivatedPlan)
            {
                var emailTemplate = new EmailTemplate
                {
                    Subject = $"{_configuration["AppSettings:AppName"]} - You're Activated!",
                    Content = "Hi {NAME}, <br /> It's great to have you on board. Lets make this money!.",
                };

                return emailTemplate;
            }
            else if (type == EmailTypesEnum.Birthday)
            {
                var emailTemplate = new EmailTemplate
                {
                    Subject = "Happy Birthday {NAME}!",
                    Content = "Hi {NAME}, <br /> We celebrate with you on this special day.",
                };
                return emailTemplate;
            }
            else if (type == EmailTypesEnum.ConfirmEmail)
            {
                var emailTemplate = new EmailTemplate
                {
                    Subject = $"{_configuration["AppSettings:AppName"]} - Confirm your Email",
                    Content = "Hi {NAME}, <br /> Click on the link below to confirm your email.<br /><a href='{LINK}'>Confirm Email</a>",
                };
                return emailTemplate;
            }
            else if (type == EmailTypesEnum.NewUserWelcomeMessage)
            {
                var emailTemplate = new EmailTemplate
                {
                    Subject = "Welcome {NAME}",
                    Content = "Hi {NAME}, <br /> Your profile has been created successfully. You can now proceed to activate the plans that work best for you. Let's make this money!",
                };
                return emailTemplate;
            }
            else if (type == EmailTypesEnum.PlanCompleted)
            {
                var emailTemplate = new EmailTemplate
                {
                    Subject = $"{_configuration["AppSettings:AppName"]} - ",
                    Content = "Hi {NAME}, <br /> Your plan has completed successfully. You can choose to reinvest by activating a plan.",
                };
                return emailTemplate;
            }
            else if (type == EmailTypesEnum.ResetPassword)
            {
                var emailTemplate = new EmailTemplate
                {
                    Subject = $"Reset your password",
                    Content = "Hi {NAME}, <br /> Click on the link below to reset your password.<br /><a href='{URL}'>Reset Password</a> <br />If you did not request this action, please ignore.",
                };
                return emailTemplate;
            }
            else
            {
                return null;
            }
        }
    }
}
