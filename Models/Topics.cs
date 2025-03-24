using System.ComponentModel.DataAnnotations;

namespace SubjectTopicApp.Models
{
    public class Topics
    {
            [Key]
            public int TopicID { get; set; }

            [Required]
            public string Topic { get; set; }
    }
}