using System;
using System.ComponentModel.DataAnnotations;

namespace BuildingShop.Models
{

    public class OrderMetadata
    {
        [Display(Name = "Клиент")]
        public Nullable<int> id_клиента { get; set; }

        [Display(Name = "Сотрудник")]
        public Nullable<int> id_сотрудника { get; set; }

        public bool Готовность { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public System.DateTime Дата { get; set; }
    }
    public class ProductCategoryMetadata
    {
        [RegularExpression(@"^[^\d]+$", ErrorMessage = "Поле не должно содержать цифры.")]
        [Required]
        public string Наименование { get; set; }
    }
    public class ClientMetadata
    {
        [Required]
        [Display(Name = "Номер телефона")]
        [StringLength(16, MinimumLength = 16, ErrorMessage = "Длина номера телефона должна быть 16 символов")]
        public string Номер_телефона { get; set; }

        [Required]
        [RegularExpression(@"^[^\d]+$", ErrorMessage = "Поле не должно содержать цифры.")]
        public string ФИО { get; set; }
    }
    public class EmployeeMetadata
    {
        [Required]
        [RegularExpression(@"^[^\d]+$", ErrorMessage = "Поле не должно содержать цифры.")]
        public string ФИО { get; set; }

        [Required]
        [Display(Name = "Номер телефона")]
        [StringLength(16, MinimumLength = 16, ErrorMessage = "Длина номера телефона должна быть 16 символов")]
        public string Номер_телефона { get; set; }
    }
    public class OrderItemMetadata
    {
        [Required]
        [Display(Name = "Товар")]
        public int id_товара { get; set; }
        [Display(Name = "Заказ")]
        public Nullable<int> id_заказа { get; set; }

        [Required]
        [Display(Name = "Количество")]
        [Range(1, 100)]
        [RegularExpression(@"^\d+$", ErrorMessage = "Поле должно содержать только цифры")]
        public int Кол_во { get; set; }
    }
    public class ProductMetadata
    {
        [Display(Name = "Категория")]
        public Nullable<int> id_категории { get; set; }
        [Required]
        public string Наименование { get; set; }

        [Required]
        [Display(Name = "Страна-производитель")]
        [RegularExpression(@"^[^\d]+$", ErrorMessage = "Поле не должно содержать цифры.")]
        public string Страна_производитель { get; set; }

        [Required]
        [Display(Name = "Производитель")]
        public string Производитель { get; set; }

        [Required]
        [Display(Name = "Цена")]
        [Range(0, 1_000_000)]
        [RegularExpression("^[0-9!@#$%^&*().]+$", ErrorMessage = "Поле должно содержать только цифры.")]
        [DecimalValidation]
        public decimal Цена { get; set; }

        [Display(Name = "Категория товара")]
        public virtual Категория_товаров Категория_товаров { get; set; }

        [Required]
        [Display(Name = "Количество")]
        [Range(0, 100)]
        [RegularExpression(@"^\d+$", ErrorMessage = "Поле должно содержать только цифры")]
        public int Количество { get; set; }
    }

}