using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SubjectTopicApp.Data;
using SubjectTopicApp.Models;

namespace SubjectTopicApp.Controllers
{
    public class TopicsController : Controller
    {
        private ApplicationDbContext dbContext;

        public TopicsController(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public async Task<IActionResult> Index()
        {
            var topics = await dbContext.Topics.ToListAsync();
            return View(topics);
        }

        public async Task<IActionResult> GetSubtopics(int topicId)
        {
            var subtopics = await dbContext.SubTopics
                                           .Where(s => s.TopicID == topicId)
                                           .ToListAsync();
            ViewData["TopicID"] = topicId;
            return PartialView("_Subtopic", subtopics);
        }

        [HttpPost]
        public async Task<IActionResult> AddTopic(string topic)
        {
            if (string.IsNullOrWhiteSpace(topic))
            {
                return BadRequest("Topic name cannot be empty.");
            }

            var newTopic = new Topics { Topic = topic };
            dbContext.Topics.Add(newTopic);
            await dbContext.SaveChangesAsync();

            return Ok(); // Sends a success response (200)
        }

        [HttpPost]
        public async Task<IActionResult> DeleteTopic(int topicId)
        {
            var subtopics = dbContext.SubTopics.Where(s => s.TopicID == topicId);
            dbContext.SubTopics.RemoveRange(subtopics); // Delete related subtopics

            var topic = await dbContext.Topics.FindAsync(topicId);
            if (topic == null)
            {
                return NotFound();
            }

            dbContext.Topics.Remove(topic); // Delete the topic itself
            await dbContext.SaveChangesAsync();

            return Ok(); // Return success response
        }

        [HttpPut]
        public async Task<IActionResult> UpdateTopic([FromBody] TopicUpdateRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Topic))
            {
                return BadRequest("Topic name cannot be empty.");
            }

            var topic = await dbContext.Topics.FindAsync(request.TopicId);
            if (topic == null)
            {
                return NotFound();
            }

            topic.Topic = request.Topic;
            await dbContext.SaveChangesAsync();

            return Ok();
        }

        // Create a DTO (Data Transfer Object) for updating topics
        public class TopicUpdateRequest
        {
            public int TopicId { get; set; }
            public string Topic { get; set; }
        }

        [HttpPost]
        public IActionResult AddSubtopic(int topicId, string subtopic)
        {
            // Debugging: Check if method is called
            Console.WriteLine($"➡ AddSubtopic called with TopicID: {topicId}, Subtopic: {subtopic}");

            if (string.IsNullOrWhiteSpace(subtopic))
            {
                return BadRequest("⚠ Subtopic name cannot be empty.");
            }

            var newSubtopic = new SubTopics
            {
                TopicID = topicId, // Ensure correct TopicID
                SubTopic = subtopic
            };

            dbContext.SubTopics.Add(newSubtopic);
            int recordsAffected = dbContext.SaveChanges(); // Save changes

            // Debugging: Check if changes were saved
            Console.WriteLine($"✅ Subtopic Added. Records affected: {recordsAffected}");

            if (recordsAffected == 0)
            {
                return StatusCode(500, "⚠ Subtopic could not be saved.");
            }

            // Fetch updated subtopics list and return partial view
            var subtopics = dbContext.SubTopics.Where(s => s.TopicID == topicId).ToList();
            return PartialView("_Subtopic", subtopics);
        }



        [HttpPut]
        public async Task<IActionResult> UpdateSubtopic([FromBody] SubtopicUpdateRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.SubTopic))
            {
                return BadRequest("Subtopic name cannot be empty.");
            }

            var subtopic = await dbContext.SubTopics.FindAsync(request.SubTopicID);
            if (subtopic == null)
            {
                return NotFound();
            }

            subtopic.SubTopic = request.SubTopic;
            await dbContext.SaveChangesAsync();

            var subtopics = await dbContext.SubTopics.Where(s => s.TopicID == request.TopicID).ToListAsync();
            return PartialView("_Subtopic", subtopics);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteSubtopic(int subtopicId)
        {
            var subtopic = await dbContext.SubTopics.FindAsync(subtopicId);
            if (subtopic == null)
            {
                return NotFound();
            }

            var topicId = subtopic.TopicID; // Get the topic ID before deletion
            dbContext.SubTopics.Remove(subtopic);
            await dbContext.SaveChangesAsync();

            var subtopics = await dbContext.SubTopics.Where(s => s.TopicID == topicId).ToListAsync();
            return PartialView("_Subtopic", subtopics);
        }

        // DTO for updating subtopics
        public class SubtopicUpdateRequest
        {
            public int SubTopicID { get; set; }
            public string SubTopic { get; set; }
            public int TopicID { get; set; } // Keeps track of the selected topic
        }




    }
}