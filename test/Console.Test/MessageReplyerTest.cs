using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EnterpriseRegistration.Console;
using Xunit;
using FluentAssertions;

namespace Console.Test
{
    // This project can output the Class library as a NuGet Package.
    // To enable this option, right-click on the project and select the Properties menu item. In the Build tab select "Produce outputs on build".
    public class MessageReplyerTest
    {
        readonly MessageReplyer target;
        public MessageReplyerTest()
        {
            target = new MessageReplyer(null);
        }

        [Fact]
        public void GetPendingMessages_ShouldHaveNotNullReturn()
        {
            var actual = target.GetPendingMessages();
            actual.Should().NotBeNullOrEmpty();
        }
    }
}
