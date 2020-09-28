using System.ComponentModel.DataAnnotations;

namespace Infrastructure.Data
{
    public enum PermissionType
    {
        [Display(Name = "Назначать себе тренировку")]
        AssignTrainingToSelf = 1,

        [Display(Name = "Назначать тренировки")]
        AssignTrainings,

        [Display(Name = "Редактировать информацию о себе")]
        EditOwnProfile,

        [Display(Name = "Добавлять информацию о тренерах")]
        AddCoaches,

        [Display(Name = "Редактировать информацию о тренерах")]
        ManageCoaches,

        [Display(Name = "Добавлять информацию о залах")]
        AddHalls,

        [Display(Name = "Редактировать информацию о залах")]
        ManageHalls,

        [Display(Name = "Добавлять информацию о тренировках")]
        AddTrainings,

        [Display(Name = "Редактировать информацию о тренировках")]
        ManageTrainings,

        [Display(Name = "Добавлять информацию о клиентах")]
        AddClients,

        [Display(Name = "Редактировать информацию о клиентах")]
        ManageClients,

        [Display(Name = "Добавлять информацию о менеджерах")]
        AddManagers,

        [Display(Name = "Редактировать информацию о менеджерах")]
        ManageManagers,
    }
}
