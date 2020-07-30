// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using OtripleS.Web.Api.Models.Assignments;
using Xunit;

namespace OtripleS.Web.Api.Tests.Unit.Services.AssignmentServiceTests
{
    public partial class AssignmentServiceTests
    {
        [Fact]
        public void ShouldRetrieveAllAssignments()
        {
            // given
            DateTimeOffset randomDateTime = GetRandomDateTime();
            IQueryable<Assignment> randomAssignments = CreateRandomAssignments(randomDateTime);
            IQueryable<Assignment> storageAssignments = randomAssignments;
            IQueryable<Assignment> expectedAssignments = storageAssignments;

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAllAssignments())
                    .Returns(storageAssignments);

            // when
            IQueryable<Assignment> actualAssignments =
                this.assignmentService.RetrieveAllAssignments();

            // then
            actualAssignments.Should().BeEquivalentTo(expectedAssignments);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Never);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAllAssignments(),
                    Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldRetrieveAssignmentById()
        {
            //given
            DateTimeOffset dateTime = GetRandomDateTime();
            Assignment randomAssignment = CreateRandomAssignment(dateTime);
            Guid inputAssignmentId = randomAssignment.Id;
            Assignment inputAssignment = randomAssignment;
            Assignment expectedAssignment = randomAssignment;

            this.storageBrokerMock.Setup(broker =>
                    broker.SelectAssignmentByIdAsync(inputAssignmentId))
                .ReturnsAsync(inputAssignment);

            //when 
            Assignment actualAssignment = await this.assignmentService.RetrieveAssignmentById(inputAssignmentId);

            //then
            actualAssignment.Should().BeEquivalentTo(expectedAssignment);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAssignmentByIdAsync(inputAssignmentId), Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}