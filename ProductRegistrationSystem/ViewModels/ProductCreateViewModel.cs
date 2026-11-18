
using System.ComponentModel.DataAnnotations;

namespace ProductRegistrationSystem.ViewModels
{
    public class ProductCreateViewModel
    {
        [Required(ErrorMessage = "Название товара обязательно.")]
        [StringLength(150, MinimumLength = 2,
            ErrorMessage = "Название должно содержать от 2 до 150 символов.")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Описание обязательно.")]
        [StringLength(1000, MinimumLength = 10,
            ErrorMessage = "Описание должно содержать от 10 до 1000 символов.")]
        public string Description { get; set; } = string.Empty;

        [Required(ErrorMessage = "Категория обязательна.")]
        [StringLength(100, ErrorMessage = "Категория слишком длинная.")]
        public string Category { get; set; } = string.Empty;

        [Range(typeof(decimal), "1", "100000000",
            ErrorMessage = "Цена должна быть от 1 до 100000000.")]
        public decimal Price { get; set; }

        [Range(0, 1000000,
            ErrorMessage = "Количество должно быть от 0 до 1000000.")]
        public int StockQuantity { get; set; }

        [Url(ErrorMessage = "Введите корректный URL изображения.")]
        public string? ImageUrl { get; set; }
    }
}
