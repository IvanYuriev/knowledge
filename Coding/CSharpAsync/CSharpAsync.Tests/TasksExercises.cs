using System;
using Xunit;

namespace CSharpAsync.Tests
{
    public class UnitTest1
    {
        [Fact]
        public void Test1()
        {
            var subject = GetSubject();

            subject.TaskContinuation();
        }

        private TasksExercises GetSubject()
        {
            return new TasksExercises();
        }
    }
}
