using System.ComponentModel.DataAnnotations;

namespace BuildingShop.Models
{
    [MetadataType(typeof(OrderMetadata))]
    public partial class Заказ
    {

    }
    [MetadataType(typeof(ProductCategoryMetadata))]
    public partial class Категория_товаров
    {

    }
    [MetadataType(typeof(ClientMetadata))]
    public partial class Клиент
    {

    }
    [MetadataType(typeof(EmployeeMetadata))]
    public partial class Сотрудник
    {

    }
    [MetadataType(typeof(OrderItemMetadata))]
    public partial class Список_товаров
    {

    }
    [MetadataType(typeof(ProductMetadata))]
    public partial class Товар
    {

    }

}