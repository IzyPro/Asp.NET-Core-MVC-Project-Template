using System;
using System.ComponentModel.DataAnnotations;

namespace Template.Enums
{
    public enum EmailTypesEnum
    {
        NewUserWelcomeMessage = 1,
        ConfirmEmail,
        ResetPassword,
        ActivatedPlan,
        PlanCompleted,
        Birthday,
    }
}
