using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace SubjectTopicApp.Models
{
    public class SubTopics
    {
        [Key]
        public int SubTopicID { get; set; }

        [Required]
        public string SubTopic { get; set; }

        [Required]
        public int TopicID { get; set; }
        [ForeignKey("TopicID")]
        public Topics Topic { get; set; }
    }
}