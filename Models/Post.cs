using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Http;

namespace ForumApp.Models
{
    public class Post
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        
        [Required(ErrorMessage = "Это поле обязательно для заполнения")]
        [Display(Name = "Текст публикации")]
        [DataType(DataType.Text)]
        [StringLength(100, MinimumLength = 10, ErrorMessage = "Длина публикации должна быть от 10 до 100 знаков")]
        public string Text { get; set; }
        
        [NotMapped]
        [Display(Name = "Фото публикации")]
        public IFormFile File { get; set; }
        
        public string Path { get; set; }

        public DateTime DatePublish { get; set; } = DateTime.Now;

        public string UserId { get; set; }
        public virtual User User { get; set; }
    }
}