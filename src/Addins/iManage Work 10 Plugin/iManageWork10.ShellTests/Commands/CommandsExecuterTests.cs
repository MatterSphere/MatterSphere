using iManageWork10.Shell.Commands;
using iManageWork10.Shell.RestAPI;
using NUnit.Framework;
using Rhino.Mocks;

namespace iManageWork10.ShellTests.Commands
{
    [TestFixture]
    class CommandsExecuterTests
    {
        private CommandsExecuter _commandsExecuter;
        private IRestApiClient _restApiClient;
        private IManageCommand<object> _command;

        [SetUp]
        public void SetUp()
        {
            _restApiClient = MockRepository.GenerateMock<IRestApiClient>();  
            _command = MockRepository.GenerateMock<IManageCommand<object>>();

            _commandsExecuter = new CommandsExecuter(_restApiClient);
        }

        [Test]
        public void Constructor_AssignsRestApiClient()
        {
            Assert.AreSame(_restApiClient, _commandsExecuter.RestApiClient);
        }

        [Test]
        public void Execute_CommandExecuteCalledWithAssignedRestApiClient()
        {
            _command.Expect(c => c.Execute(_restApiClient))
                .Return(new object());

            _commandsExecuter.Execute(_command);

            _command.AssertWasCalled(c => c.Execute(_restApiClient)); 
        }

        [Test]
        public void Execute_ReturnsResultFromCommandExecute()
        {
            var result = new object();
            _command.Expect(c => c.Execute(_restApiClient))
                .Return(result);

            var actualResult = _commandsExecuter.Execute(_command);

            Assert.AreSame(result, actualResult);
        }
    }
}
