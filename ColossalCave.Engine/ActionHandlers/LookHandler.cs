using ColossalCave.Engine.Interfaces;
using Microsoft.Extensions.Logging;

namespace ColossalCave.Engine.ActionHandlers
{
    public class LookHandler : BaseHandler, ILookHandler
    {
        public LookHandler(ILogger<LookHandler> log,
            IResponseBuilder responseBuilder,
            IMessageProvider messageProvider,
            ILocationProvider locationProvider,
            IMapHelper mapHelper,
            AdventureContext context)
            : base(log, responseBuilder, messageProvider, locationProvider, mapHelper, context)
        { }
            
        public override void Handle()
        {
            _log.LogInformation("Handling Look");
            var curLoc = _advContext.CurrentLocation;
            var lookStr = _advContext.GetParameterValue("visuals");
            var directionStr = _advContext.GetParameterValue("directions");
            if (directionStr != null)
            {
                _responseBuilder.AddToResponse(Mnemonic.CantGiveMoreDetail, 1);
                if (curLoc.IsLight)
                    _responseBuilder.AddToResponse(curLoc.Description);
                else
                    _responseBuilder.AddToResponse(Mnemonic.PitchDark);
            }
            else
            {
                if (curLoc.IsLight)
                    _responseBuilder.AddToResponse(curLoc.Description);
                else
                    _responseBuilder.AddToResponse(Mnemonic.PitchDark);
            }
        }
    }
}
