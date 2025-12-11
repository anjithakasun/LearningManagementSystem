using LearningManagementSystem.Data.LMSModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearningManagementSystem.Data.OtherModels
{
    public class QuestionViewModel
    {
        public TrainingQuestion Question { get; set; }
        public List<TrainingQuestionAnswer> Answers { get; set; }
        public List<TrainingQuestionCorrectAnswer> CorrectAnswer { get; set; }
    }
}
