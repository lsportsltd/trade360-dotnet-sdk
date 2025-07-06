using Trade360SDK.Common.Entities.Enums;
using Xunit;

namespace Trade360SDK.Common.Tests.Entities.Enums
{
    public class StatusDescriptionTests
    {
        [Fact]
        public void Enum_ShouldContainExpectedValues()
        {
            Assert.True(System.Enum.IsDefined(typeof(StatusDescription), StatusDescription.None));
            Assert.True(System.Enum.IsDefined(typeof(StatusDescription), StatusDescription.HT));
            Assert.True(System.Enum.IsDefined(typeof(StatusDescription), StatusDescription.OTHT));
            Assert.True(System.Enum.IsDefined(typeof(StatusDescription), StatusDescription.HomeRetired));
            Assert.True(System.Enum.IsDefined(typeof(StatusDescription), StatusDescription.AwayRetired));
            Assert.True(System.Enum.IsDefined(typeof(StatusDescription), StatusDescription.LostCoverage));
            Assert.True(System.Enum.IsDefined(typeof(StatusDescription), StatusDescription.MedicalTimeout));
            Assert.True(System.Enum.IsDefined(typeof(StatusDescription), StatusDescription.TimeoutHomeTeam));
            Assert.True(System.Enum.IsDefined(typeof(StatusDescription), StatusDescription.TimeoutAwayTeam));
            Assert.True(System.Enum.IsDefined(typeof(StatusDescription), StatusDescription.Timeout));
            Assert.True(System.Enum.IsDefined(typeof(StatusDescription), StatusDescription.HomeWalkover));
            Assert.True(System.Enum.IsDefined(typeof(StatusDescription), StatusDescription.AwayWalkover));
            Assert.True(System.Enum.IsDefined(typeof(StatusDescription), StatusDescription.MatchDrawn));
            Assert.True(System.Enum.IsDefined(typeof(StatusDescription), StatusDescription.BallMarkInspection));
            Assert.True(System.Enum.IsDefined(typeof(StatusDescription), StatusDescription.BallMarkInspectionCompleted));
            Assert.True(System.Enum.IsDefined(typeof(StatusDescription), StatusDescription.CorrectionMode));
            Assert.True(System.Enum.IsDefined(typeof(StatusDescription), StatusDescription.MedicalTreatment));
            Assert.True(System.Enum.IsDefined(typeof(StatusDescription), StatusDescription.TreatmentCompleted));
            Assert.True(System.Enum.IsDefined(typeof(StatusDescription), StatusDescription.PlayersOnCourt));
            Assert.True(System.Enum.IsDefined(typeof(StatusDescription), StatusDescription.TossStarted));
            Assert.True(System.Enum.IsDefined(typeof(StatusDescription), StatusDescription.TossSaved));
            Assert.True(System.Enum.IsDefined(typeof(StatusDescription), StatusDescription.UmpireOnCourt));
            Assert.True(System.Enum.IsDefined(typeof(StatusDescription), StatusDescription.UmpiringCancelled));
            Assert.True(System.Enum.IsDefined(typeof(StatusDescription), StatusDescription.WarmUpStarted));
            Assert.True(System.Enum.IsDefined(typeof(StatusDescription), StatusDescription.WarmUpCompleted));
            Assert.True(System.Enum.IsDefined(typeof(StatusDescription), StatusDescription.MedicalTimeoutCancelled));
            Assert.True(System.Enum.IsDefined(typeof(StatusDescription), StatusDescription.MedicalTimeoutCompleted));
            Assert.True(System.Enum.IsDefined(typeof(StatusDescription), StatusDescription.SwapServer));
            Assert.True(System.Enum.IsDefined(typeof(StatusDescription), StatusDescription.SwapReceiver));
            Assert.True(System.Enum.IsDefined(typeof(StatusDescription), StatusDescription.ChallengeStarted));
            Assert.True(System.Enum.IsDefined(typeof(StatusDescription), StatusDescription.ChallengeCompleted));
            Assert.True(System.Enum.IsDefined(typeof(StatusDescription), StatusDescription.ChallengeCancelled));
            Assert.True(System.Enum.IsDefined(typeof(StatusDescription), StatusDescription.ToiletBreakStarted));
            Assert.True(System.Enum.IsDefined(typeof(StatusDescription), StatusDescription.ToiletBreakCompleted));
            Assert.True(System.Enum.IsDefined(typeof(StatusDescription), StatusDescription.ToiletBreakCancelled));
            Assert.True(System.Enum.IsDefined(typeof(StatusDescription), StatusDescription.EvaluationStarted));
            Assert.True(System.Enum.IsDefined(typeof(StatusDescription), StatusDescription.TimePenalty));
            Assert.True(System.Enum.IsDefined(typeof(StatusDescription), StatusDescription.GamePenalty));
            Assert.True(System.Enum.IsDefined(typeof(StatusDescription), StatusDescription.CoachingStarted));
            Assert.True(System.Enum.IsDefined(typeof(StatusDescription), StatusDescription.CoachingCancelled));
            Assert.True(System.Enum.IsDefined(typeof(StatusDescription), StatusDescription.CoachingCompleted));
            Assert.True(System.Enum.IsDefined(typeof(StatusDescription), StatusDescription.Breaktime));
            Assert.True(System.Enum.IsDefined(typeof(StatusDescription), StatusDescription.PointStarted));
            Assert.True(System.Enum.IsDefined(typeof(StatusDescription), StatusDescription.PointScored));
            Assert.True(System.Enum.IsDefined(typeof(StatusDescription), StatusDescription.PointFault));
            Assert.True(System.Enum.IsDefined(typeof(StatusDescription), StatusDescription.PointReplayed));
        }
    }
} 