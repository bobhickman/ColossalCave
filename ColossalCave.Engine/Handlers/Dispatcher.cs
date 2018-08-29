using ColossalCave.Engine.Interfaces;
using ColossalCave.Engine.Utilities;
using Microsoft.Extensions.Logging;

namespace ColossalCave.Engine.Handlers
{
    public class Dispatcher : IActionHandler
    {
        private readonly ILogger _log;

        private readonly IControlHandler _controlHandler;
        private readonly IExamineHandler _examineHandler;
        private readonly IInventoryHandler _inventoryHandler;
        private readonly ILookAroundHandler _lookHandler;
        private readonly ILockHandler _lockHandler;
        private readonly IMagicHandler _magicHandler;
        private readonly IMoveDirectionHandler _moveDirHandler;
        private readonly IMoveFeatureHandler _moveFeatureHandler;
        private readonly IMoveLocationHandler _moveLocHandler;
        private readonly IOnOffHandler _onOffHandler;

        private readonly IResponseBuilder _responseBuilder;

        private AdventureContext _advContext;

        public Dispatcher(ILogger<Dispatcher> log,
            IControlHandler controlHandler,
            IExamineHandler examineHandler,
            IInventoryHandler inventoryHandler,
            ILockHandler lockHandler,
            ILookAroundHandler lookHandler,
            IMagicHandler magicHandler,
            IMoveDirectionHandler moveDirHandler,
            IMoveFeatureHandler moveFeatureHandler,
            IMoveLocationHandler moveLocHandler,
            IOnOffHandler onOffHandler,
            IResponseBuilder responseBuilder,
            AdventureContext context)
        {
            _log = log;
            _controlHandler = controlHandler;
            _examineHandler = examineHandler;
            _inventoryHandler = inventoryHandler;
            _lockHandler = lockHandler;
            _lookHandler = lookHandler;
            _magicHandler = magicHandler;
            _moveDirHandler = moveDirHandler;
            _moveFeatureHandler = moveFeatureHandler;
            _moveLocHandler = moveLocHandler;
            _onOffHandler = onOffHandler;
            _responseBuilder = responseBuilder;
            _advContext = context;
        }

        public void Handle()
        {
            if (_advContext.IntentName == "control")
                _controlHandler.Handle();
            else if (_advContext.IntentName == "inventory")
                _inventoryHandler.Handle();
            else if (_advContext.IntentName == "examination")
                _examineHandler.Handle();
            else if (_advContext.IntentName == "lock-unlock")
                _lockHandler.Handle();
            else if (_advContext.IntentName == "lookaround")
                _lookHandler.Handle();
            else if (_advContext.IntentName == "magic")
                _magicHandler.Handle();
            else if (_advContext.IntentName.EqualsNoCase("move-direction"))
                _moveDirHandler.Handle();
            else if (_advContext.IntentName.EqualsNoCase("move-location"))
                _moveLocHandler.Handle();
            else if (_advContext.IntentName.EqualsNoCase("move-feature"))
                _moveFeatureHandler.Handle();
            else if (_advContext.IntentName.EqualsNoCase("on-off"))
                _onOffHandler.Handle();

            _advContext.SpeechResponse = _responseBuilder.Speech;
            _advContext.TextResponse = _responseBuilder.Text;
        }
    }
}
