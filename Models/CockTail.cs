using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace mobie_app_api.Models
{
    public class CockTail
    {
        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage = "Vui lòng nhập trường này")]
        [Display(Name = "Tên sản phẩm")]
        public string? Name { get; set; }
        [Required(ErrorMessage = "Vui lòng nhập trường này")]
        [Display(Name = "Giá")]
        public double? Price { get; set; }

        [Display(Name = "Mô tả")]
        public string? Description { get; set; }

        [Display(Name = "Chọn hình ảnh")]
        public string? Image { get; set; }

    }
}